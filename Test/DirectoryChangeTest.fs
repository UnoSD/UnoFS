module DirectoryChangeTest

open System
open Xunit
open Types
    
[<Fact>]
let ``cd enters a directory`` () =
    let root =
        Root
            {
                Children = Set.empty.Add { Name = "directory"; Files = Set.empty }
                Files = []
            }

    let directory = 
        DirectoryChange.cd root "directory"
    
    Assert.True(directory.IsSome)

[<Fact>]
let ``cd does not enter a non existing directory`` () =
    let root =
        Root
            {
                Children = Set.empty
                Files = []
            }

    let directory = 
        DirectoryChange.cd root "directory"
    
    Assert.True(directory.IsNone)