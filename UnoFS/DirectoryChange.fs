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
                               Parents = tags.Add tag
                           })

let cd dir name =
    match dir with
    | Root root   -> root       |> cd' name Set.empty
    | Child child -> child.Root |> cd' name child.Parents