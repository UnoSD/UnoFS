module RegressionTest

open System
open Xunit
open Types
open DirectoryChange
open DirectoryCreate
open DirectoryList

type ChildDirectory with 
    member this.directory     = Child this
    member this.rootDirectory = Root  this.Root

type RootDirectory with 
    member this.directory = Root this

let rootHas tagNames child =
    tagNames |>
    List.map (fun name -> ({ Name = name }, Set.empty)) |>
    Map.ofList |>
    (fun tags -> { Tags = tags }) |>
    (=) child.Root |>
    Assert.True

let has tagNames tags =
    tagNames |>
    List.map (fun name -> { Name = name }) |>
    (=) tags |>
    Assert.True

let hierarchyHas tagNames child =
    child.Hierarchy |> has tagNames

let tagIs name child =
    child.Tag.Name = name |>
    Assert.True

let exists dir =
    match dir with 
    | Some (Child _)
    | Some (Root _) -> Assert.True(true)
    | _             -> Assert.True(false)

let asChild =
    function
    | Some (Child c) -> c
    | _              -> failwith "Not a child directory"

let asRoot =
    function
    | Some (Root c) -> c
    | _             -> failwith "Not a root directory"
    
type Option<'a> with
    member this.toChild =
        match box this with
        | :? Option<Directory> as y -> y |> asChild
        | _                         -> failwith "Not an option of directory"
    member this.toRoot =
        match box this with
        | :? Option<Directory> as y -> y |> asRoot
        | _                         -> failwith "Not an option of directory"
    member this.toDirectory =
        match box this with
        | :? Option<Directory> as y -> y.Value
        | _                         -> failwith "Not an option of directory"
        
let isNone (option : 'a option) =
    Assert.True(option.IsNone)

let isSome (option : 'a option) =
    Assert.True(option.IsSome)

let root =
    Root { Tags = Map.empty }

[<Fact>]
let ``Regression test`` () =
    let a = mkdir root "a"
    
    a |> tagIs        "a"
    a |> rootHas      ["a"]
    a |> hierarchyHas []
    
    let b = mkdir a.rootDirectory "b"

    b |> tagIs        "b"
    b |> rootHas      ["a"; "b"]
    b |> hierarchyHas []

    let c = mkdir b.rootDirectory "c"
    
    c |> rootHas ["a"; "b"; "c"]
    
    let list = ls c.rootDirectory
    
    list |> has ["a"; "b"; "c"]
    
    let a = cd c.rootDirectory "a"
    
    a |> exists
    
    let a = a.toChild
    
    a |> tagIs        "a"
    a |> rootHas      ["a"; "b"; "c"]
    a |> hierarchyHas []
                                           
    let list = ls a.directory
    
    list |> has ["b"; "c"]
    
    let b = cd a.directory "b"
    
    let b = b.toChild
    
    b |> hierarchyHas ["a"]
    
    let list = ls b.directory
    
    list |> has ["c"]
        
    let c = cd b.directory "c"
        
    let c = c.toChild
    
    c |> hierarchyHas ["b"; "a"]
    
    let list = ls c.directory
    
    list |> has []
        
    let c' = cd c.directory "c"
    
    c' |> isNone
    
    let b = cd c.directory ".."
    
    let b = b.toChild
    
    b |> hierarchyHas ["a"]
    
    let list = ls b.directory
    
    list |> has ["c"]
    
    let a' = cd b.directory "a"
    
    a' |> isNone
    
    let a = cd b.directory ".."
    
    a |> isSome
    
    let root = cd a.toDirectory ".."
    
    let root = root.toRoot
    
    let list = ls root.directory
    
    list |> has ["a"; "b"; "c"]    

    let a = cd root.directory "b/c/a"
    
    let a = a.toChild
    
    a |> tagIs "a"
    a |> hierarchyHas ["c"; "b"]
    
    let a = cd a.directory "../../a"
    
    let a = a.toChild
    
    a |> tagIs        "a"
    a |> hierarchyHas ["b"]