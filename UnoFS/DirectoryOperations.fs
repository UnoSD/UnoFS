module DirectoryOperations

open Types
    
let private tryFindDir parent name =
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
                      Parent = Some parent
                  }
    
let private moveDir destination dir =
    { 
        destination with 
            Directories = 
                destination.Directories.Add dir
    }    
    
let mkdir parent name =
    parent |>
    getOrCreateDir name |>
    moveDir parent

let cd dir name =
    dir.Directories |>
    Seq.tryPick (function | d when d.Name = name -> Some d
                          | _                    -> None)