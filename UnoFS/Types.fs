module Types

type Content =
    byte[]
    
type File =
    {
        Name:    string
        Content: Content
    }
    
type RootDirectory =
    {
        Name:        string
        Directories: Set<ChildDirectory>
        Files:       File list
    }

and ChildDirectory =
    {
        Name:        string
        Directories: Set<ChildDirectory>
        Files:       File list
        Parent:      Directory
    }

and Directory =
    | RootDirectory of RootDirectory
    | ChildDirectory of ChildDirectory