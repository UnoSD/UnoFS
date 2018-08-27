# UnoFS

UnoFS is a purely functional semantic file system implemented in F#.

It will be integrated with Mono.Fuse to be used as storage, but it supports different application as it will have no direct dependency.

After FUSE it will support Azure Blobs.

It will support different backing storage, first LiteDB then maybe cloud storage.

# REPL

There is a small project TestRepl to simplify testing, it is a conole application that support textual commands.

```
mkdir a
mkdir b
mkdir c
cd a
cd b
ls // Outputs [{Name = "c";}]
cd ..
ls // Outputs [{Name = "b";}; {Name = "c";}]
cd ..
cd c
ls // Outputs [{Name = "a";}; {Name = "b";}]
cd ..

// Any order

cd a/b/c
ls // Outputs []
cd ../../..
cd c/a
ls // Outputs [{Name = "b";}]

// You get the gist...
```
