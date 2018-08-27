module DirectoryCreateTests

open System
open Xunit
open Types

let root =
    {
        Children = Set.empty
        Files = []
    }

let root' =
    Root root

let directory =
    {
        Name = "directory"
        Root = { root with Children = root.Children.Add { Name = "directory"; Files = Set.empty } }
        Files = Set.empty
        Parents = Set.empty
    }

let directory' = 
    Child directory
            
[<Fact>]
let ``mkdir creates a directory under root`` () =
    let directory = 
        DirectoryCreate.mkdir root' "directory"
    
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
let ``mkdir creates a directory under another directory`` () =
    let subDirectory = 
        DirectoryCreate.mkdir directory' "subDirectory"
    
    let expectedRoot =
        {
            directory.Root with
                Children =
                    directory.Root.Children.Add
                        {
                            Name = "subDirectory"
                            Files = Set.empty
                        }
        }
    
    Assert.True(subDirectory.Name = "subDirectory")
    Assert.True(subDirectory.Root = expectedRoot)
    Assert.True(subDirectory.Files = Set.empty)
    Assert.True(subDirectory.Parents = Set.empty.Add { Name = directory.Name; Files = Set.empty })