module DirectoryCreateTests

open System
open Xunit
open Types

let root =
    {
        Tags = Map.empty
    }

let root' =
    Root root

let tag =
    { Name = "tag" }
    
let directory =
    {
        Tags = [ tag ]
        Root = { root with Tags = root.Tags |> Map.add tag Set.empty }
    }

let directory' = 
    Child directory
            
[<Fact>]
let ``mkdir creates a directory under root`` () =
    let directory = 
        DirectoryCreate.mkdir root' "tag"
    
    let expectedRoot =
        {
            root with
                Tags =
                    root.Tags |>
                    Map.add { Name = "tag" } Set.empty
        }
    
    Assert.True(directory.Tags.Length = 1)
    Assert.True(directory.Tags.Head.Name = "tag")
    Assert.True(directory.Root = expectedRoot)
    
[<Fact>]
let ``mkdir creates a directory under another directory`` () =
    let subDirectory = 
        DirectoryCreate.mkdir directory' "subDirectory"
    
    let expectedRoot =
        {
            directory.Root with
                Tags = 
                    directory.Root.Tags |>
                    Map.add { Name = "subDirectory" } Set.empty
        }
    
    Assert.True(subDirectory.Tags.Length = 2)
    Assert.True(subDirectory.Tags.Head.Name = "subDirectory")
    Assert.True(subDirectory.Tags.Tail = directory.Tags)
    Assert.True(subDirectory.Root = expectedRoot)