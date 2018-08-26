module DirectoryOperations

open Types

let private tryFindDir name directories =
    directories |>
    Seq.tryPick (fun x -> if x.Name = name then
                              Some x
                          else
                              None)

let private getContent =
    function
    | RootDirectory  dir -> dir.Content
    | ChildDirectory dir -> dir.Content
    
let private getChildren dir =
    dir |>
    getContent |>
    (fun d -> d.Children)
    
let private getOrCreateDir name parent =
    match parent |> getChildren |> tryFindDir name with
    | Some dir -> dir
    | None     -> {
                      Name = name
                      Content =
                          {
                              Children = Set.empty
                              Files = []
                          }
                      Parent = parent
                  }
    
let private withChild' dir parent dirContent =
    {
        dirContent with
            Children = 
                dirContent.Children.Add { dir with Parent = parent }
    }

let private withChild dir parent =
    parent |>
    getContent |>
    withChild' dir parent
    
let private moveDir destination dir =
    match destination with 
    | RootDirectory root ->
        RootDirectory
            { 
                root with 
                    Content = destination |> withChild dir
            }
    | ChildDirectory child ->
        ChildDirectory
            { 
                child with 
                    Content = destination |> withChild dir
            }
    
let mkdir parent name =
    parent |>
    getOrCreateDir name |>
    moveDir parent

let cd dir name =
    dir |> 
    getChildren |>
    Seq.tryPick (function | d when d.Name = name -> Some d
                          | _                    -> None)