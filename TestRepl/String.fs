[<AutoOpen>]
module String

let (|StartsWith|_|) (value : string) (text : string) =
    if text.StartsWith(value) then
        text.Substring(value.Length) |> Some
    else
        None