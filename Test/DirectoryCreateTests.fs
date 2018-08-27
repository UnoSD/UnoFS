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
        Tag       = tag
        Root      = { root with Tags = root.Tags |> Map.add tag Set.empty }
        Hierarchy = []
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
    
    Assert.True(directory.Hierarchy.IsEmpty)
    Assert.True(directory.Tag.Name = "tag")
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
    
    Assert.True(subDirectory.Hierarchy.Length = 1)
    Assert.True(subDirectory.Tag.Name = "subDirectory")
    Assert.True(subDirectory.Hierarchy = directory.Tag :: directory.Hierarchy)
    Assert.True(subDirectory.Root = expectedRoot)