using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD49
{
    public class Stage
    {
        public float decaySpeed = 0.25f;
        public int timeLimit;
        public float timeStart = 0;
        public Floor[] floors;
        public bool loaded = false;
    }

    public class Floor
    {
        public bool newPartPointSet = false;
        public IVector2 newPartPoint;
        public int floor = 0;
        public int regions = 1;
        public IBound2 outerBound;
        public HashSet<Part> parts;
        public int height;
        public Dictionary<IVector2, FloorTile> floorTiles;
        //public Partitioner floorPartitioner;
        public Octree<OctreeStageCell> tree = new Octree<OctreeStageCell>(); // false is available space, true is reserved

        /*public Floor(int height, Dictionary<IVector2, FloorTile> floorTiles)
        {
            partitionersByHeight = new Partitioner[height];
            for (int i = 0; i < height; i++)
            {
                Partitioner partitioner = partitionersByHeight[i] = new Partitioner();

                foreach (KeyValuePair<IVector2, FloorTile> kv in floorTiles)
                {
                    FloorTile floorTile = kv.Value;
                    partitioner.AddAvailableSpace(new IBound2(floorTile.position, new IVector2(1,1)));
                }

                partitioner.SimplifyAvailable();
            }
        }*/

        public bool CanPlace(Part part)
        {
            if (!IsShapeAvailable(part))
            {
                Debug.LogError("Shape not available");
                return false;
            }

            foreach (IBound3 partBound in part.ShapeOnFloor)
            {
                for (int x = partBound.MinX; x < partBound.MaxX; x++)
                {
                    for (int z = partBound.MinZ; z < partBound.MaxZ; z++)
                    {
                        IVector2 position = new IVector2(x, z);
                        if (floorTiles.ContainsKey(position) && !floorTiles[position].isInner)
                        {
                            // requires one outer tile
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        // if the part complets a support, get all parts
        public Part[] GetAdjacentSupportParts(Part part)
        {
            List<Part> adjacentParts = new List<Part>();
            HashSet<OctreeStageCell> cells = new HashSet<OctreeStageCell>();
            foreach (IBound3 partBound in part.ShapeOnFloor)
            {
                for (int x = partBound.MinX; x < partBound.MaxX; x++)
                {
                    for (int y = partBound.MinY; y < partBound.MaxY; y++)
                    {
                        IBound3 bound = new IBound3(x, 1, y, 1, height, 1);
                        if (tree.IsAreaFull(bound))
                        {
                            cells.Clear();
                            tree.GetCells(bound, cells);

                            foreach (OctreeStageCell cell in cells)
                            {
                                if (cell.part != null)
                                {
                                    adjacentParts.Add(cell.part);
                                }
                            }
                        }
                    }
                }
             }
                
            return adjacentParts.ToArray();
        }
        public IVector2[] GetCompleteSupportTiles(Part part)
        {
            List<IVector2> completeTiles = new List<IVector2>();
            foreach (IBound3 partBound in part.ShapeOnFloor)
            {
                for (int x = partBound.MinX; x < partBound.MaxX; x++)
                {
                    for (int y = partBound.MinY; y < partBound.MaxY; y++)
                    {
                        IBound3 bound = new IBound3(x, 1, y, 1, height, 1);
                        if (tree.IsAreaFull(bound))
                        {
                            completeTiles.Add(new IVector2(x, y));
                        }
                    }
                }
            }

            return completeTiles.ToArray();
        }

        public int GetSupport(int region)
        {
            int support = 0;
            HashSet<OctreeStageCell> cells = new HashSet<OctreeStageCell>();
            foreach (KeyValuePair<IVector2, FloorTile> kv in floorTiles)
            {
                IVector2 position = kv.Key;
                FloorTile floorTile = kv.Value;

                if (floorTile.region == region)
                {
                    IBound3 bound = new IBound3(position.x, 1, position.y, 1, height, 1);
                    if (tree.IsAreaFull(bound))
                    {
                        support++;
                        /*cells.Clear();
                        tree.GetCells(bound, cells);

                        foreach (OctreeStageCell cell in cells)
                        {
                            if (cell.part != null)
                            {*/
                                // determine which bound matched, and get the number of spots taken
                                /*foreach (IBound3 partShapeBound in cell.part.ShapeOnFloor)
                                {
                                    IBound3 overlap = partShapeBound.GetOverlappedBound(bound);

                                    if (overlap.size.y > 0)
                                    {
                                        support += cell.part.Type.strength * overlap.size.y;
                                    }
                                }*/
                                /*support += cell.part.Type.strength;
                            }
                        }*/
                    }
                }
            }
            return support;
        }

        public void AddPart(Part part)
        {
            part.IsPlaced = true;
            parts.Add(part);
            part.Floor = floor;

            foreach (IBound3 bound in part.ShapeOnFloor)
            {
                OctreeStageCell cell = new OctreeStageCell();
                cell.part = part;
                tree.SetCell(bound, cell);
            }

        }

        public void RemovePart(Part part)
        {
            parts.Remove(part);
            
            foreach (IBound3 bound in part.ShapeOnFloor)
            {
                tree.RemoveNodes(bound);
            }
        }

        /*public void AddFloorTile(FloorTile floorTile)
        {
            floorTiles.Add(position, floorTile);
        }*/

        public void Init()
        {
            // Build a shape that surrounds all of the given shapes
            IBound2[] floorTileBounds = new IBound2[floorTiles.Count];
            int i = 0;
            foreach (KeyValuePair<IVector2, FloorTile> kv in floorTiles)
            {
                FloorTile floorTile = kv.Value;

                IBound2 bound = new IBound2(floorTile.position.x, floorTile.position.y, 1, 1);
                floorTileBounds[i++] = bound;
                if (outerBound.Empty)
                {
                    outerBound = bound;
                }
                else
                {
                    outerBound.Encapsulate(bound);
                }
            }

            // Enlarge the bound,to ensure that outer walls are captured
            outerBound = IUtil.EnlargeIBound(outerBound, 1);

            // Create a partitioner to create the shape that surrounds the given shapes
            Partitioner partitioner = new Partitioner();
            partitioner.AddAvailableSpace(outerBound);
            partitioner.Reserve(new Partition("", floorTileBounds));
            partitioner.SimplifyAvailable();

            OctreeStageCell cell = new OctreeStageCell();
            //tree.SetCell(new IBound3(outerBound.position.x, -1, outerBound.position.y, 1, 1, 1), cell);
            tree.SetCell(new IBound3(outerBound.position.x, height+1, outerBound.position.y, outerBound.size.x, 1, outerBound.size.y), cell); // height + 1 will be the ceiling
            foreach (IBound2 bound in partitioner.available)
            {
                tree.SetCell(new IBound3(bound.position.x, 1, bound.position.y, bound.size.x, height+1, bound.size.y), cell); // height + 1 to include the floor
            }
            
            // Add the floor cells
            foreach (KeyValuePair<IVector2, FloorTile> kv in floorTiles)
            {
                FloorTile floorTile = kv.Value;
                cell = new OctreeStageCell();
                cell.floorTile = floorTile;
                tree.SetCell(new IBound3(floorTile.position.x, 0, floorTile.position.y, 1, 1, 1), cell);
            }

            // Mark inner tiles
            foreach (KeyValuePair<IVector2, FloorTile> kv in floorTiles)
            {
                FloorTile floorTile = kv.Value;

                if (tree.IsAreaFull(new IBound3(floorTile.position.x - 1, 0, floorTile.position.y - 1, 3, 1, 3)))
                {
                    floorTile.isInner = true;
                }
            }

        }

        public bool IsShapeAvailable(Part part)
        {
            IBound3[] partShape = part.ShapeOnFloor;

            foreach (IBound3 partBound in partShape)
            {
                if (tree.ContainsCell(partBound))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class OctreeStageCell
    {
        public Part part;
        public FloorTile floorTile;
    }

    public class Part
    {
        public static int counter = 1;
        public int id = 0;
        public Part()
        {
            id = counter++;
        }

        public float DecayTime { get; set; }
        public bool IsPlaced { get; set; }
        public int Floor { get; set; }
        public float TimeBorn { get; set; }
        public PartType Type { get; set; }
        public IVector3 Position { get; set; }
        public IVector3 Rotation { get; set; }
        public IBound2[] Shape {
            get
            {
                return Type.shape;
            }
        } // at identity
        public IBound3[] ShapeOnFloor {
            get
            {
                IBound3[] shapeOnFloor = new IBound3[Shape.Length];
                IBound2[] shape = Shape;
                for (int i = 0; i < Shape.Length; i++)
                {
                    IBound2 bound = shape[i];
                    shapeOnFloor[i] = IUtil.Rotate(new IBound3(bound.position.x, bound.position.y, 0,bound.size.x, bound.size.y, 1), Rotation);
                    shapeOnFloor[i].position += Position;
                }
                return shapeOnFloor;
            }
        }
    }

    public class FloorTile
    {
        public int region = 0;
        public IVector2 position;
        public bool isInner = false;

        public FloorTile(int x, int y)
        {
            position.x = x;
            position.y = y;
        }
    }

    public class PartType
    {
        public string name;
        public IBound2[] shape;
        public float timeModifier = 1; // speed time? Does it affect time to live?
        public float strengthModifier = 1; // Affects the strength of a given column
        public int strength = 1;
        public float timeToLive = 30;
        public float warningTime = 10;

        public PartType(string name, IBound2[] shape)
        {
            this.name = name;
            this.shape = shape;
        }
    }
    public class PartTypeManager
    {
        public PartType[] types;
        public Dictionary<string, PartType> typesByName;

        public PartTypeManager()
        {
            types = new PartType[]
            {
                new PartType("1x1", new IBound2[] { new IBound2(0, 0, 1, 1) }),
                new PartType("2x2 Hook", new IBound2[] {
                    new IBound2(-1, 0, 1, 2),
                    new IBound2(0, 1, 1, 1)
                }),
                new PartType("2x1 Cubes", new IBound2[] { new IBound2(-1, 0, 2, 1) }),
                new PartType("2x3 Door", new IBound2[] { new IBound2(-1, 0, 2, 3) }),
                new PartType("2x4 Flowerpot", new IBound2[] {
                    new IBound2(-2, 0, 4, 1),
                    new IBound2(-2, 1, 1, 1),
                    new IBound2( 1, 1, 1, 1),
                }),
                new PartType("1x4 Pillar", new IBound2[] { new IBound2(0, 0, 1, 4) }),
                new PartType("1x4 LPillar", new IBound2[] {
                    new IBound2(0, 0, 1, 4),
                    new IBound2(-1, 3, 1, 1)
                }),
                new PartType("3x4 SCubes", new IBound2[] {
                    new IBound2(0, 0, 2, 1),
                    new IBound2(0, 1, 1, 2),
                    new IBound2(-2, 2, 1, 2),
                }),
                new PartType("2x2 Window", new IBound2[] { new IBound2(-1, 0, 2, 2) }),
                new PartType("4x2 Window", new IBound2[] { new IBound2(-2, 0, 4, 2) })
            };

            typesByName = new Dictionary<string, PartType>();
            foreach (PartType type in types)
            {
                typesByName.Add(type.name, type);
            }
        }

        public PartType GetPartType(string name)
        {
            return typesByName[name];
        }
    }
}