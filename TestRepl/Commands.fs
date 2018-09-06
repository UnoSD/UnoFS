module Commands

open Types

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

let touchCommand workingDir name =
    let dir =
        FileCreate.touch workingDir name
        
    printfn "%A" dir
    
    match dir with
    | Some dir -> dir
    | None     -> workingDir

let processCommand workingDir (command : string) =
    match command with
    | StartsWith "mkdir " rest -> mkdirCommand workingDir rest
    | "ls"                     -> lsCommand workingDir
    | StartsWith "cd " rest    -> cdCommand workingDir rest
    | StartsWith "touch " rest -> touchCommand workingDir rest
    | _                        -> printfn "Invalid command"; workingDir