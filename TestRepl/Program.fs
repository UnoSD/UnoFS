open System
open Commands
open Types

let commands =
    [| "ls"; "mkdir "; "cd " |]

type Handler () =
    interface IAutoCompleteHandler with 
        member __.GetSuggestions(text, index) =
            commands |>
            Array.filter (fun command -> command.StartsWith(text))
        member __.Separators
            with get() = [||]
            and set(c) = ()

ReadLine.HistoryEnabled <- true
Handler () |> ReadLine.set_AutoCompletionHandler

let rec replLoop workingDir =
    match ReadLine.Read("> ") with
    | "exit"  -> ()
    | command -> processCommand workingDir command |>
                 replLoop

[<EntryPoint>]
let main _ =
    { Tags = Map.empty } |>
    Root |>
    replLoop 
    
    0