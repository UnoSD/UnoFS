module DirectoryChange

open Types
open System

let getRelativePath dir segment =
    match (dir, segment) with
    | (Root _, "..")     -> None
    | (Root r, segment)  -> r.Tags |>
                            Map.tryFindKey (fun tag _ -> tag.Name = segment) |>
                            Option.map (fun tag -> Child { Root = r; Tags = [tag] })
    | (Child c, "..")    -> match c.Tags with
                            | []      -> failwith "TODO, child with no tags shouldn't be representable"
                            | [x]     -> Root c.Root |> Some
                            | x :: xs -> Child { Root = c.Root; Tags = xs } |> Some
    | (Child c, segment) -> match c.Root.Tags |> Map.tryFindKey (fun tag _ -> tag.Name = segment) with
                            | None                                      -> None
                            | Some tag when c.Tags |> List.contains tag -> None
                            | Some tag                                  -> Child { Root = c.Root; Tags = tag :: c.Tags } |> Some

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