open System
open Types

let mkdirCommand current name =
    let dir = 
        DirectoryCreate.mkdir current name
    
    printfn "%A" dir
    
    match current with
    | Root _ -> Root dir.Root
    | Child parent -> Child { parent with Root = dir.Root }

let cdCommand current name =
    let dir = 
        DirectoryChange.cd current name
        
    printfn "%A" dir
    
    match dir with
    | Some dir -> Child dir
    | None -> current

let getFirstParent dir =
    match dir with
    | Root _ -> None
    | Child dir -> dir.Parents |>
                   Seq.head |>
                   (fun tag -> tag.Name) |>
                   Some

let error () =
    failwith "TODO"

// TODO: Change from string to type
// I.E. mkdir current { .. } instead of string
//      cd current root.Children...

let processCommand current (command : string) =
    match command with
    | command when command.StartsWith("mkdir ") -> mkdirCommand current (command.Substring("mkdir ".Length))
    | command when command.StartsWith("cd ..") -> cdCommand current (getFirstParent current |> Option.defaultWith error)
    | command when command.StartsWith("cd ") -> cdCommand current (command.Substring("cd ".Length))
    | _ -> printfn "Invalid command"; current

let rec replLoop current =
    match Console.ReadLine() with
    | "exit"  -> ()
    | command -> processCommand current command |>
                 replLoop

[<EntryPoint>]
let main _ =
    {
        Children = Set.empty
        Files = []
    } |>
    Root |>
    replLoop 
    
    0