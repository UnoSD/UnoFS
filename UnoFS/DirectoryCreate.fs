module DirectoryCreate

open Types
open DirectoryOperations
    
let private withTag dir root =
    { root with Children = root.Children.Add { Name = dir; Files = Set.empty } }
    
let private getOrCreateDir name tags root =
    match root |> tryFindTag name with
    | Some tag -> {
                      Root    = root
                      Name    = name
                      Files   = tag.Files |> filterBy tags
                      Parents = tags
                  }
    | None     -> {
                      Root    = root |> withTag name
                      Name    = name
                      Files   = Set.empty
                      Parents = tags
                  }
    
let mkdir parent name =
    match parent with 
    | Root root   -> root       |> getOrCreateDir name Set.empty
    | Child child -> child.Root |> getOrCreateDir name child.Parents