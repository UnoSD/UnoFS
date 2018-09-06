module DirectoryList

open Types

let rec findFilesWithAllTags list files =
    match list with 
    | []           -> files
    | (_, f) :: xs -> files |>
                      Set.intersect f |>
                      findFilesWithAllTags xs

let private ls' root currentTags =
    root.Tags |>
    Map.toList |>
    List.groupBy (fun (tag, files) -> currentTags |> List.contains tag) |>
    List.collect (fun (areCurrent, map) -> match areCurrent with 
                                           | false -> map |>
                                                      List.map fst |>
                                                      List.map TagContent
                                           | true  -> map.Head |>
                                                      snd |>
                                                      findFilesWithAllTags map.Tail |>
                                                      Set.map (fun file -> FileContent file) |>
                                                      Set.toList)

let ls dir =
    match dir with
    | Root root   -> ls' root       []
    | Child child -> ls' child.Root (child.Tag :: child.Hierarchy)