module DirectoryList

open Types

let ls' (root : RootDirectory) excludeTags =
    root.Tags |>
    Map.toSeq |>
    Seq.map fst |>
    Seq.filter (fun tag -> excludeTags |> List.contains tag |> not) |>
    Seq.toList

let ls dir =
    match dir with
    | Root root   -> ls' root       []
    | Child child -> ls' child.Root (child.Tag :: child.Hierarchy)