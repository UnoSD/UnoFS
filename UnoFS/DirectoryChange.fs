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

let private isParentOf child name =
    child.Parents |> Set.exists (fun p -> p.Name = name)

let private getTagsBefore name (tags : Set<Tag>) =
    tags |>
    Seq.takeWhile (fun tag -> tag.Name <> name) |>
    Set.ofSeq

let cd dir name =
    match dir with
    | Child child when child.Name = name        -> None
    | Child child when name |> isParentOf child -> child.Root |> cd' name (child.Parents |> getTagsBefore name)
    | Child child                               -> child.Root |> cd' name (getTags child)
    | Root root                                 -> root       |> cd' name Set.empty