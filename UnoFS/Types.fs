module Types

type Content =
    byte[]
    
type File =
    {
        Name:    string
        Content: Content
    }

type Directory =
    {
        Name:        string
        Directories: Set<Directory>
        Parent:      Directory option
        Files:       File list
    }