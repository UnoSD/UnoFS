open System
open Types

let (|StartsWith|_|) (value : string) (text : string) =
    if text.StartsWith(value) then
        Some (text.Substring(value.Length))
    else
        None

let mkdirCommand workingDir name =
    let dir = 
        DirectoryCreate.mkdir workingDir name
    
    printfn "%A" dir
    
    match workingDir with
    | Root _       -> Root dir.Root
    | Child oldDir -> Child { oldDir with Root = dir.Root }

let lsCommand workingDir =
    let dirs = 
        DirectoryList.ls workingDir
        
    printfn "%A" dirs
    
    workingDir

let cdCommand workingDir path =
    let dir =
        DirectoryChange.cd workingDir path
    
    printfn "%A" dir
    
    match dir with
    | Some dir -> dir
    | None     -> workingDir

let processCommand workingDir (command : string) =
    match command with
    | StartsWith "mkdir " rest -> mkdirCommand workingDir rest
    | "ls"                     -> lsCommand workingDir
    | StartsWith "cd " rest    -> cdCommand workingDir rest
    | _                        -> printfn "Invalid command"; workingDir

let rec replLoop workingDir =
    match Console.ReadLine() with
    | "exit"  -> ()
    | command -> processCommand workingDir command |>
                 replLoop

[<EntryPoint>]
let main _ =
    { Tags = Map.empty } |>
    Root |>
    replLoop 
    
    0