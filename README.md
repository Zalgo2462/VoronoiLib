# VoronoiLib
C# implementation of Fortune's Algorithm. Includes MonoGame example.
Unlike several implemenations of Fortune's Algorithm, this implementation guarantees O(n ln(n)) performance by way of a specialized Red Black Tree (Credit: Raymond Hill).

# Use
```
var points = new List<FortuneSite> {
  new FortuneSite(100, 200),
  new FortuneSite(500, 200),
  new FortuneSite(300, 300)
}
//FortunesAlgorithm.Run(points, min x, min y, max x, max y)
LinkedList<VEdge> = FortunesAlgorithm.Run(points, 0, 0, 800, 800);

//VEdge.Start is a VPoint with location VEdge.Start.X and VEdge.End.Y
//VEdge.End is the ending point for the edge
//FortuneSite.Neighbors contains the site's neighbors in the Delaunay Triangulation
```
#MonoGame Example:
##Voronoi Cells in Red. Delaunay Triangulation in Yellow
![Voronoi](http://i.imgur.com/MuXTyL7.png)
Implementation inspired by:
Ivan Kuckir's project (MIT) @ http://blog.ivank.net/fortunes-algorithm-and-implementation.html
Raymond Hill's project (MIT) @ https://github.com/gorhill/Javascript-Voronoi 

Feel free to use the code under MIT license. However, if you find the code useful, feel free to send me a message or make a link back to the repo.

