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

// Path cases:
// /              -> TODO: MISSING!
// /a
// /a/b
// a
// a/b
// ..
// ../a
// ../..
// ../../a
// xxxx//xxx///xx -> RemoveEmptyEntries

let cd current (name : string) =
    let segments = 
        name.Split('/' |> Array.singleton, StringSplitOptions.RemoveEmptyEntries) |>
        List.ofArray

    let folder current segment =
        match current with
        | Some current -> getRelativePath current segment
        | None -> None

    segments |>
    List.fold folder (Some current)