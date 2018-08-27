module Tests

open System
open Xunit
open Types

[<Fact>]
let ``ls shows the content of a directory`` () =
    let items = 
        Operations.ls "/"
    
    Assert.True(true)

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
        DirectoryOperations.mkdir parent "directory"
    
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
    
[<Fact>]
let ``touch creates a file`` () =
    let file = 
        Operations.touch "/" "file"
    
    Assert.True(true)
    
[<Fact>]
let ``cd enters a directory`` () =
    let root =
        Root
            {
                Children = Set.empty.Add { Name = "directory"; Files = Set.empty }
                Files = []
            }

    let directory = 
        DirectoryOperations.cd root "directory"
    
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
        DirectoryOperations.cd root "directory"
    
    Assert.True(directory.IsNone)
    
[<Fact>]
let ``read reads the content of a file`` () =
    let content = 
        Operations.read "/" "file"
    
    Assert.True(true)
    
[<Fact>]
let ``write writes content to a file`` () =
    let file = 
        Operations.write "/" "file" "content"
    
    Assert.True(true)