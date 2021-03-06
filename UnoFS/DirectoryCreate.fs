module DirectoryCreate

open Types
    
let toDirectory root parentTags tag =
    {
        Tag       = tag
        Hierarchy = parentTags |> List.except (tag |> List.singleton)
        Root      = {
                        root with
                            Tags = root.Tags |>
                                   Map.add tag Set.empty
                    }
    }
    
let mkdir parent name =
    match parent with 
    | Root root   -> { Name = name } |> toDirectory root       []
    | Child child -> { Name = name } |> toDirectory child.Root (child.Tag :: child.Hierarchy)