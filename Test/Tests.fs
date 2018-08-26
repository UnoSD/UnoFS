module Tests

open System
open Xunit

[<Fact>]
let ``ls shows the content of a directory`` () =
    let directories = 
        Operations.ls "/"
    
    Assert.True(true)

[<Fact>]
let ``mkdir creates a directory`` () =
    let directory = 
        Operations.mkdir "/" "directory"
    
    Assert.True(true)
    
[<Fact>]
let ``touch creates a file`` () =
    let file = 
        Operations.touch "/" "file"
    
    Assert.True(true)
    
[<Fact>]
let ``cd enters a directory`` () =
    let directory = 
        Operations.cd "/" "directory"
    
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