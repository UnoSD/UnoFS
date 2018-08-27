module DirectoryCreateTests

open System
open Xunit
open Types

[<Fact>]
let ``mkdir creates a directory`` () =
    let root =
        {
            Children = Set.empty
            Files = []
        }
    let parent =
        Root root

    let directory = 
        DirectoryCreate.mkdir parent "directory"
    
    let expectedRoot =
        {
            root with
                Children =
                    root.Children.Add {
                                          Name = "directory"
                                          Files = Set.empty
                                      }
        }
    
    Assert.True(directory.Name = "directory")
    Assert.True(directory.Root = expectedRoot)
    Assert.True(directory.Files = Set.empty)
    Assert.True(directory.Parents = Set.empty)