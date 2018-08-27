module RegressionTest

open System
open Xunit
open Types
open DirectoryChange
open DirectoryCreate
open DirectoryList

[<Fact>]
let ``Regression test`` () =
    let root = Root { Tags = Map.empty }

    let a = mkdir root "a"
    
    Assert.True(a.Tag       = { Name = "a" })
    Assert.True(a.Hierarchy = [])
    Assert.True(a.Root      = { Tags = Map.empty |> Map.add { Name = "a"} Set.empty })
    
    let b = mkdir (Root a.Root) "b"

    Assert.True(b.Tag       = { Name = "b" })
    Assert.True(b.Hierarchy = [])
    Assert.True(b.Root      = { Tags = Map.empty |>
                                       Map.add { Name = "a"} Set.empty |>
                                       Map.add { Name = "b"} Set.empty })

    let c = mkdir (Root b.Root) "c"
    
    Assert.True(c.Root      = { Tags = Map.empty |>
                                       Map.add { Name = "a"} Set.empty |>
                                       Map.add { Name = "b"} Set.empty |>
                                       Map.add { Name = "c"} Set.empty })
    
    let list = ls (Root c.Root)
    
    Assert.True((list = [ { Name = "a" }; { Name = "b" }; { Name = "c" } ]))
    
    let a = cd (Root c.Root) "a"
    
    Assert.True(a.IsSome)
    
    let a = (match a.Value with | Child c -> c | _ -> failwith "Not child directory")
    
    Assert.True(a.Tag = { Name = "a" }) 
    Assert.True(a.Hierarchy = []) 
    Assert.True(a.Root      = { Tags = Map.empty |>
                                       Map.add { Name = "a"} Set.empty |>
                                       Map.add { Name = "b"} Set.empty |>
                                       Map.add { Name = "c"} Set.empty })
                                           
    let list = ls (Child a)
    
    Assert.True((list = [ { Name = "b" }; { Name = "c" } ]))
    
    let b = cd (Child a) "b"
    
    let b = (match b.Value with | Child c -> c | _ -> failwith "Not child directory")
    
    Assert.True(b.Hierarchy = [ { Name = "a" } ])
    
    let list = ls (Child b)
    
    Assert.True((list = [ { Name = "c" } ]))
        
    let c = cd (Child b) "c"
        
    let c = (match c.Value with | Child c -> c | _ -> failwith "Not child directory")
    
    Assert.True(c.Hierarchy = [ { Name = "b" }; { Name = "a" } ])
    
    let list = ls (Child c)
    
    Assert.True(list |> List.isEmpty)
        
    let c' = cd (Child c) "c"
    
    Assert.True(c'.IsNone)
    
    let b = cd (Child c) ".."
    
    let b = (match b.Value with | Child c -> c | _ -> failwith "Not child directory")
    
    Assert.True((b.Hierarchy = [ { Name = "a" } ]))
    
    let list = ls (Child b)
    
    Assert.True((list = [ { Name = "c" } ]))
    
    let a' = cd (Child b) "a"
    
    Assert.True(a'.IsNone)
    
    let a = cd (Child b) ".."
    
    Assert.True(a.IsSome)
    
    let root = cd a.Value ".."
    
    let root = (match root.Value with | Root r -> r | _ -> failwith "Not root directory")
    
    let list = ls (Root root)
    
    Assert.True((list = [ { Name = "a" }; { Name = "b" }; { Name = "c" } ]))