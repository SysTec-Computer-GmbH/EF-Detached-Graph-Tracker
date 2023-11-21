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
### How does this library solve the problems?
* Identity Resolution & Associations: You can set the ```[UpdateAssociationOnly]``` attribute over any navigation property. <br/>
  It makes sure entities in nodes annotated with this attribute are kept in the unchanged state (only relationships / FKs are updated).
  If an entity with a temporary key value (e.g. Id = 0, Id = -123) is being tracked over a navigation property annotated with this attribute, an exception will be thrown.
  However you can configure the behavior to keep the entity in a detached state instead of throwing an exception using the parameter in the attribute constructor. See usage for further details.
* 1:many / many:many missing list items: The library loads the original values of a collection navigation from the database and then removes all items that are not present in the current collection but in the original DB collection.
  Depending on the configuration of the relationship and the delete behavior, cascading actions may occur.

### How to use this library?
1. Install the nuget package ```SysTec.EF.ChangeTracking.DetachedGraphTracker```
2. Create a new instance of ```DetachedGraphTracker``` and make sure one instance is used per request (for example register it as a scoped service in a Web-API application)
3. Call ```graphTrackerInstance.TrackGraphAsync(rootNode);``` with the root of your graph.
4. After the call every node in the graph that has been traversed using the ```DbContext.ChangeTracker.TrackGraph(object root,  Action<EntityEntryGraphNode> callback)``` method is tracked in a ```Modified```, ```Unchanged```, ```Added``` or ```Deleted``` state. <br/>
   The states depend on the attributes set in the model classes (refer to "Attributes" for details). <br/>
   When no attributes are set, the node is tracked depending on its key value. If the key is set (e. g. Id = 42) the state is set to ```Modified``` otherwise the entity is tracked in an ```Added``` state.

### How does the library work (in a more detailed way)
1. After the call to ```graphTrackerInstance.TrackGraphAsync(rootNode)``` graph traversal is performed using the ```ChangeTracker.TrackGraph()``` method.
2. In the callback it is first determined if the entity is already present in the change tracker.
   1. If it is, and the current node is not annotated with an ```[UpdateAssociationOnly]``` attribute, an exception will be thrown, because in this case multiple nodes of the same type with the same key value are present in the graph and are not annotated with an ```[UpdateAssociationOnly]``` attribute.<br/>
   2. If it´s not, and the node is not annotated with an ```[UpdateAssociationOnly]``` attribute, the state will be set depending on the key value of the entity (Id > 0 -> ```Modified```, otherwise ```Added```) <br/>
   3. If it´s not, and the node is annotated with an ```[UpdateAssociationOnly]``` attribute, the entity will be kept in a detached state and stored in a list to be handled later .
3. After the graph traversal is finished, Identity Resolution is now performed for the yet detached association entities (2.iii).
   1. It an entity of the same type and key as the association is already present in the change tracker, the association gets "replaced" with the entity from the change tracker because only entities that are not annotated with the ```[UpdateAssociationOnly]``` attribute are present in the change tracker (2.ii)
   2. If no entity of the same type and key as the association is present in the change tracker and the key value of the association is set (e. g. Id > 0), it will be tracked in an ``Ùnchanged``` state.
   3. If no entity of the same type and key as the association is present in the change tracker and the key value of the association is not set (e. g. Id < 0) the entity will remain in the ```Detached``` state or an exception will be thrown, depending on the parameter passed to the ```[UpdateAssociationOnly]``` attribute.
