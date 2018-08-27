module DirectoryOperations

open Types

let tryFindTag name root =
    root.Children |>
    Seq.tryFind (fun tag -> tag.Name = name)
    
let private allContain content (tags : Set<Tag>) =
    tags |>
    Set.forall (fun tag -> tag.Files.Contains content)

let filterBy tags contents =
     contents |>
     Set.filter (fun content -> tags |> allContain content)
    
