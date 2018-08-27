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
        Tag:       Tag
        Root:      RootDirectory
        Hierarchy: Tag list (* should be an ordered set *)
    }
   
type Directory =
    | Root of RootDirectory
    | Child of ChildDirectory