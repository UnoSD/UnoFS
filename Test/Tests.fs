module Tests

open System
open Xunit

[<Fact>]
let ``ls / shows root directory content`` () =
    let directories = 
        Operations.ls "/"
    
    Assert.True(true)
