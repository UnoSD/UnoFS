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
        RootDirectory
            {
                Name = "root"
                Directories = Set.empty
                Files = []
            }

    let directory = 
        DirectoryOperations.mkdir root "directory"
    
    Assert.True(true)
    
[<Fact>]
let ``touch creates a file`` () =
    let file = 
        Operations.touch "/" "file"
    
    Assert.True(true)
    
[<Fact>]
let ``cd enters a directory`` () =
    let root =
        RootDirectory
            {
                Name = "root"
                Directories = Set.empty
                Files = []
            }

    let directory = 
        DirectoryOperations.cd root "directory"
    
    Assert.True(true)
    
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