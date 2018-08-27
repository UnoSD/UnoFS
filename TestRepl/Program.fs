open System
open Commands
open Types

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