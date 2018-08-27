module DirectoryChange

open Types
open System

let getRelativePath dir segment =
    match (dir, segment) with
    | (Root _, "..")     -> None
    | (Root r, segment)  -> r.Tags |>
                            Map.tryFindKey (fun tag _ -> tag.Name = segment) |>
                            Option.map (fun tag -> Child { Root = r; Tag = tag; Hierarchy = [] })
    | (Child c, "..")    -> match c.Hierarchy with
                            | []      -> Root c.Root |> Some
                            | [x]     -> Child { Root = c.Root; Tag = x; Hierarchy = [] } |> Some
                            | x :: xs -> Child { Root = c.Root; Tag = x; Hierarchy = xs } |> Some
    | (Child c, segment) -> match c.Root.Tags |> Map.tryFindKey (fun tag _ -> tag.Name = segment) with
                            | None                                           ->
                                None
                            | Some tag when c.Tag = tag ->
                                None
                            | Some tag when c.Hierarchy |> List.contains tag ->
                                None
                            | Some tag                                       ->
                                Child { Root = c.Root; Tag = tag; Hierarchy = c.Tag :: c.Hierarchy } |>
                                Some

let private pathFolder current segment =
    match current with
    | Some current -> getRelativePath current segment
    | None -> None

let private getSegments (path : string) =
    path.Split('/' |> Array.singleton, StringSplitOptions.RemoveEmptyEntries)

let private cd' path current =
    path |>
    getSegments |>
    Array.fold pathFolder (Some current)
    
let private getBasePath workingDir (path : string) =
    if path.StartsWith("/") then
        match workingDir with
        | Root root   -> Root root
        | Child child -> Root child.Root
    else 
        workingDir
    
let cd workingDir path =
    path |>
    getBasePath workingDir |>
    cd' path