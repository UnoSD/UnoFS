module Types

type Content =
    {
        Name:     string
        Location: unit
    }
    
type Tag =
    {
        Name:  string
        Files: Set<Content>
    }
    
type File =
    {
        Tags:    Set<Tag>
        Content: Content
    }

type RootDirectory =
    {
        Files:    File list
        Children: Set<Tag>
    }

type ChildDirectory =
    {
        Name:    string
        Files:   Set<Content>
        Parents: Set<Tag>
        Root:    RootDirectory
    }
   
type Directory =
    | Root of RootDirectory
    | Child of ChildDirectory