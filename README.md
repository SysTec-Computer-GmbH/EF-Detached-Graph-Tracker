# EF-Detached-Graph-Tracker
A library to support easier change tracking of a complex detached graph with EF-Core.
### What does this library do?
It handles the change tracking of a complex detached graph using pure vanilla EF-Core.
Let's begin explaining what this library does, by listing the three main problems EF-Core has with updating detached graphs.

### EF-Core Detached Graph Problems
* Identity Resolution: When attaching a graph to the change tracker, the caller needs to assure that the graph contains only one element of the same type and the same key.<br/>
  See following example:
``` c#
RootNode
├── Item: {Id: 1, Text: "This text should be persisted"}
└── List<Item>: {Id: 1, Text "This text should not be persisted"}, {...}

// Throws Item with Id 1 can not be tracked because another instance with the same key is already tracked...
dbContext.Attach(RootNode);
```
* Another problem are associations. When speaking of associations this 
