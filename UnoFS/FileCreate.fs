module FileCreate

open Types

let merge left right =
    right |>
    Map.fold (fun acc key value -> acc |> Map.add key value) left

let touch' child name =
    {
        child with 
            Root = {
                       child.Root with
                           Tags = child.Root.Tags |>
                                  Map.filter (fun tag _ -> (child.Tag :: child.Hierarchy) |>
                                                           List.contains tag) |>
                                  Map.map (fun _ files -> files |> Set.add { Name = name; Location = () }) |>
                                  merge child.Root.Tags
                   }
    }

let touch parent name =
    match parent with
    | Root root -> None
    | Child child -> touch' child name |>
                     Child |>
                     Some