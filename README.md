# EF-Detached-Graph-Tracker
A library to support easier change tracking of a complex detached graph with EF-Core.
### What does this library do?
It handles the change tracking of a complex detached graph using pure vanilla EF-Core.
Let's begin by listing the three main problems EF-Core has with updating detached graphs.

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
* Another problem are associations where you don't want to persist changes to the entity inside the navigation property.
  <br/>
  In relation to the previous example consider that the client UI allows updates of ```Item``` on top of a page and just allows the user to associate ```Items``` in another part of the page.
  <br/>
  <br/>
  This problem is connected to the identity resolution problem. When identity resolution is performed automatically,
  it must somehow be determined which entity values in the graph to persist (track as ```Modified``` or ```Added```) and which to ignore (track as ```Ùnchanged```).
* The third problem is severing 1:many or many:many relationships by just removing items from a detached collection ont the client side.<br/>
  Another example makes this clear:
  ``` c#
  // First Request -> New Item will get Id 1
  RootNode
  └── List<Item>: {Id: 0, "This will be Added"}

  // Second Request -> New Item will get Id 2
  RootNode
  └── List<Item>: {Id: 1, "This will be Modified"}, {Id: 0, "This will be Added"}

  // Third Request -> Only the text of Item with Id 2 will change. Nothing else. No relationship gets severed.
  RootNode
  └── List<Item>: {Id: 2, "This will be Modified"}
  ```
