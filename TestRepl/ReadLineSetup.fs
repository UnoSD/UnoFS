module ReadLineSetup

open System

let private commands =
    [| "ls"; "exit"; "mkdir "; "cd " |]

type Handler () =
    interface IAutoCompleteHandler with 
        member __.GetSuggestions(text, index) =
            commands |>
            Array.filter (fun command -> command.StartsWith(text))
        member __.Separators
            with get() = [||]
            and set(c) = ()

let setup () =
    ReadLine.HistoryEnabled <- true
    Handler () |> ReadLine.set_AutoCompletionHandler