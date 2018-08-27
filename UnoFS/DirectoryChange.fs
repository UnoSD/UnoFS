module DirectoryChange

open Types
open DirectoryOperations

let private cd' name tags root =
    root |>
    tryFindTag name |>
    Option.map (fun tag -> {
                               Root    = root
                               Name    = tag.Name
                               Files   = tag.Files |> filterBy tags
                               Parents = tags
                           })

let private getTags child =
    child.Root |>
    tryFindTag child.Name |>
    Option.map child.Parents.Add |>
    Option.defaultWith (fun _ -> failwith "Dangling child")

let isCurrentOrParentOf child name =
    child.Name = name ||
    child.Parents |> Set.exists (fun p -> p.Name = name)

let cd dir name =
    match dir with
    | Child child when name |> isCurrentOrParentOf child -> None
    | Child child                                        -> child.Root |> cd' name (getTags child)
    | Root root                                          -> root       |> cd' name Set.empty