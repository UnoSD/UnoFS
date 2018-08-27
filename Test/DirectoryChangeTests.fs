module DirectoryChangeTests

open System
open Xunit
open Types
    
[<Fact>]
let ``cd enters a directory`` () =
    let root =
        Root
            {
                Tags = Map.empty |> Map.add { Name = "tag" } Set.empty
            }

    let directory = 
        DirectoryChange.cd root "tag"
    
    Assert.True(directory.IsSome)

[<Fact>]
let ``cd does not enter a non existing directory`` () =
    let root =
        Root
            {
                Tags = Map.empty
            }

    let directory = 
        DirectoryChange.cd root "tag"
    
    Assert.True(directory.IsNone)