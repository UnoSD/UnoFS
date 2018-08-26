module Types

type Content =
    byte[]
    
type File =
    {
        Name:    string
        Content: Content
    }
    
// With this circular dependency and immutability
// it's impossible to make it work.
// It's a dog chasing its own tail,
// If I create a new dir, then I need to set
// its parent to the old parent and I need to add
// the child to the parent, but I also need the
// child to have the parent with the child alredy
// and its child needs to have the parent already
// and so on...
type DirectoryContent =
    {
        Children: Set<ChildDirectory>
        Files:    File list
    }
    
and RootDirectory =
    {
        Content: DirectoryContent
    }

and ChildDirectory =
    {
        Name:    string
        Parent:  Directory
        Content: DirectoryContent
    }

and Directory =
    | RootDirectory of RootDirectory
    | ChildDirectory of ChildDirectory