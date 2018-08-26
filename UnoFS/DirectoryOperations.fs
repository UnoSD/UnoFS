module DirectoryOperations

open Types

let private tryFindDir parent name =
    match parent with 
    | RootDirectory parent ->
        parent.Directories |>
            Seq.tryPick (fun x -> if x.Name = name then
                                      Some x
                                  else
                                      None)
    | ChildDirectory parent ->
        parent.Directories |>
            Seq.tryPick (fun x -> if x.Name = name then
                                      Some x
                                  else
                                      None)
    
let private getOrCreateDir name parent =
    match tryFindDir parent name with
    | Some dir -> dir
    | None     -> {
                      Name = name
                      Directories = Set.empty
                      Files = []
                      Parent = parent
                  }
    
let private moveDir destination dir =
    match destination with 
    | RootDirectory root ->
        RootDirectory
            { 
                root with 
                    Directories = 
                        root.Directories.Add dir
            }
    | ChildDirectory child ->
        ChildDirectory
            { 
                child with 
                    Directories = 
                        child.Directories.Add dir
            }
    
let mkdir parent name =
    parent |>
    getOrCreateDir name |>
    moveDir parent

let cd dir name =
    match dir with 
    | RootDirectory dir ->
        dir.Directories |>
        Seq.tryPick (function | d when d.Name = name -> Some d
                              | _                    -> None)
    | ChildDirectory dir ->
        dir.Directories |>
        Seq.tryPick (function | d when d.Name = name -> Some d
                              | _                    -> None)