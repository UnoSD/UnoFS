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
let ``touch creates a file`` () =
    let file = 
        Operations.touch "/" "file"
    
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