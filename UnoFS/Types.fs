module Types

type File =
    {
        Name:     string
        Location: unit
    }
    
type Tag =
    {
        Name: string
    }
    
type RootDirectory =
    {
        Tags: Map<Tag, Set<File>>
    }

type ChildDirectory =
    {
        Tags: Tag list (* should be an ordered set *)
        Root: RootDirectory
    }
   
type Directory =
    | Root of RootDirectory
    | Child of ChildDirectory