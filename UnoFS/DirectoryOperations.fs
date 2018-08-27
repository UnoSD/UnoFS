module DirectoryOperations

open Types

let private tryFindTag name root =
    root.Children |>
    Seq.tryFind (fun tag -> tag.Name = name)
    
let private withTag dir root =
    { root with Children = root.Children.Add { Name = dir; Files = Set.empty } }
    
let private allContain content (tags : Set<Tag>) =
    tags |>
    Set.forall (fun tag -> tag.Files.Contains content)

let private filterBy tags contents =
     contents |>
     Set.filter (fun content -> tags |> allContain content)
    
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