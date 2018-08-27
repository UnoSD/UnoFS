module DirectoryChange

open Types
open System

let private getDirFromRoot root segment =
    root.Tags |>
    Map.tryFindKey (fun tag _ -> tag.Name = segment) |>
    Option.map (fun tag -> Child { Root = root; Tag = tag; Hierarchy = [] })

let private getParentDirFrom child =
    match child.Hierarchy with
    | []      -> Root child.Root
    | [x]     -> Child { Root = child.Root; Tag = x; Hierarchy = [] }
    | x :: xs -> Child { Root = child.Root; Tag = x; Hierarchy = xs }

let private hierarchyHas tag child =
    child.Hierarchy |> List.contains tag

let private getDirectoryFor tag child =
    {
        Root = child.Root
        Tag = tag
        Hierarchy = child.Tag :: child.Hierarchy
    } |>
    Child |>
    Some

let private getDirFromChild child segment =
    match child.Root.Tags |> Map.tryFindKey (fun tag _ -> tag.Name = segment) with
    | None                                    -> None
    | Some tag when child.Tag = tag           -> None
    | Some tag when child |> hierarchyHas tag -> None
    | Some tag                                -> child |> getDirectoryFor tag

let getRelativePath dir segment =
    match (dir, segment) with
    | (Root _     , ".."   ) -> None
    | (Root root  , segment) -> segment |> getDirFromRoot root
    | (Child child, ".."   ) -> getParentDirFrom child |> Some
    | (Child child, segment) -> segment |> getDirFromChild child

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