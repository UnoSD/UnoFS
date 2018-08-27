open System
open Commands
open Types

let rec replLoop workingDir =
    match ReadLine.Read("> ") with
    | "exit"  -> ()
    | command -> processCommand workingDir command |>
                 replLoop

[<EntryPoint>]
let main _ =
    ReadLineSetup.setup()

    { Tags = Map.empty } |>
    Root |>
    replLoop 
    
    0