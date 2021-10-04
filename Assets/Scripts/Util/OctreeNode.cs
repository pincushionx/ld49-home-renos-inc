using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD49
{
    public class OctreeNode<TCell> {
        public IBound3 bounds;
        public TCell cell = default(TCell); // contains node data, if any
        protected OctreeNode<TCell>[] children = null;

        private static readonly IEqualityComparer<TCell> cellComparer = EqualityComparer<TCell>.Default;

        public OctreeNode(IBound3 bound) {
            bounds = bound;
        }

        /// <summary>
        /// Adds a voxel at the specified position. Currently traverses until the node size is 1, then stores the cell.
        /// In the future, will look into having leaves with size > 1
        /// </summary>
        /// <param name="cell">Cell.</param>
        public virtual void SetCell(IBound3 cellBounds, TCell cell) {
            // does the cell entirely own this node
            if (cellBounds.Contains(bounds))
            {
                this.cell = cell;
            }
            else if (cellBounds.Overlaps(bounds))
            {
                if (children == null)
                {
                    children = new OctreeNode<TCell>[8];
                }

                int[] indices = GetChildIndices(cellBounds);
                for (int i = 0; i < indices.Length; i++)
                {
                    int index = indices[i];
                    if (children[index] == null)
                    {
                        IBound3 childBounds = GetChildBounds(index);
                        children[index] = new OctreeNode<TCell>(childBounds);

                    }
                    children[index].SetCell(cellBounds, cell);
                }
            }
        }

        /// <summary>
        /// Gets the node at the specified position. Not used.
        /// </summary>
        /// <returns>The cell. Null if not found.</returns>
        /// <param name="position">Position.</param>
        public OctreeNode<TCell> GetNode(IVector3 position)
        {
            if (!cellComparer.Equals(cell, default(TCell))) // null check
            {
                if (bounds.Contains(position))
                {
                    return this;
                }
            }
            else if (children != null)
            {
                int index = GetChildIndex(position);
                if (children[index] != null)
                {
                    return children[index].GetNode(position);
                }
            }

            return null;
        }
        public OctreeNode<TCell> GetNode(IBound3 bound)
        {
            if (bounds.Equals(bound))
            {
                return this;
            }
            else if (children != null)
            {
                int[] indices = GetChildIndices(bound);
                for (int i = 0; i < indices.Length; i++)
                {
                    int index = indices[i];
                    if (children[index] != null)
                    {
                        return children[index].GetNode(bound);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets nodes at a certain depth. Used for rendering. Nodes found before the depth will be returned as well.
        /// </summary>
        /// <returns>The nodes.</returns>
        /// <param name="bound">Bounds for the nodes to retrieve.</param>
        public void GetLeafNodes(IBound3 bound, ICollection<OctreeNode<TCell>> nodes)
        {
            if (cellComparer.Equals(cell, default(TCell))) // null check
            {
                if (children != null)
                {
                    for (int i = 0; i < children.Length; i++)
                    {
                        if (children[i] != null)
                        {
                            IBound3 childBound = children[i].bounds;
                            IBound3 overlapBound = childBound.GetOverlappedBound(bound);

                            // if it overlaps the requested bounds, return the child's nodes
                            if (overlapBound.size > IVector3.zero)
                            {
                                children[i].GetLeafNodes(overlapBound, nodes);
                            }
                        }
                    }
                }
            }
            else
            {
                // we found a cell, add this node
                nodes.Add(this);
            }
        }

        /// <summary>
        /// Gets the first node found. Used for cellular automation, assuming that all nodes are contiguous.
        /// </summary>
        /// <returns>A node, or null if none are found</returns>
        public OctreeNode<TCell> GetAnyLeafNode()
        {
            if (cellComparer.Equals(cell, default(TCell))) // null check
            {
                if (children != null)
                {
                    for (int i = 0; i < children.Length; i++)
                    {
                        if (children[i] != null)
                        {
                            OctreeNode<TCell> node = children[i].GetAnyLeafNode();
                            if (node != null)
                            {
                                return node;
                            }
                        }
                    }
                }
            }
            else
            {
                // we found a cell, add this node
                return this;
            }
            return null;
        }

        /// <summary>
        /// Gets the cell at the specified position
        /// </summary>
        /// <returns>The cell. Null if not found.</returns>
        /// <param name="position">Position.</param>
        public TCell GetCell(IVector3 position) {
			if (!cellComparer.Equals(cell, default(TCell))) // null check
            {
				if (bounds.Contains(position)) {
					return cell;
				}
			}
			else if (children != null) {
				int index = GetChildIndex (position);
				if (children [index] != null) {
					return children [index].GetCell (position);
				}
			}

			return default(TCell);
		}

        public bool HasCell(IVector3 position)
        {
            if (!cellComparer.Equals(cell, default(TCell))) // null check
            {
                if (bounds.Contains(position))
                {
                    return true;
                }
            }
            else if (children != null)
            {
                int index = GetChildIndex(position);
                if (children[index] != null)
                {
                    return children[index].HasCell(position);
                }
            }

            return false;
        }
        public bool HasCell(IVector3 position, TCell compareCell)
        {
            if (cellComparer.Equals(cell, compareCell)) // null check
            {
                if (bounds.Contains(position))
                {
                    return true;
                }
            }
            else if (children != null)
            {
                int index = GetChildIndex(position);
                if (children[index] != null)
                {
                    return children[index].HasCell(position, compareCell);
                }
            }

            return false;
        }

        public void RemoveNodes(IBound3 removeBounds)
        {
            if (removeBounds.Contains(bounds))
            {
                children = null;
                cell = default(TCell);
            }
            else if (children != null)
            {

                int[] indices = GetChildIndices(removeBounds);
                bool hasChildren = false;
                for (int i = 0; i < indices.Length; i++)
                {
                    int index = indices[i];
                    if (children[index] != null)
                    {
                        children[index].RemoveNodes(removeBounds);

                        // remove any empty children
                        // there may be a performance gain if empty children are kept - this would require an isEmpty function
                        if (cellComparer.Equals(children[index].cell, default(TCell)) && children[index].children == null)
                        {
                            children[index] = null;
                        }
                    }
                }

                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i] != null)
                    {
                        hasChildren = true;
                        break;
                    }
                }
                if (!hasChildren)
                {
                    children = null;
                }
            }

        }
    
        /// <summary>
        /// Gets the index of the child. From 0-7.
        /// </summary>
        /// <returns>The child index.</returns>
        /// <param name="childPosition">Child position.</param>
        protected virtual int GetChildIndex(IVector3 childPosition) {
            int index = 0;

            // may need to do a size=1 check here
            index += (childPosition.x >= (bounds.position.x + bounds.size.x / 2)) ? 1 : 0;
            index += (childPosition.y >= (bounds.position.y + bounds.size.y / 2)) ? 2 : 0;
            index += (childPosition.z >= (bounds.position.z + bounds.size.z / 2)) ? 4 : 0;

			return index;
		}

		/// <summary>
		/// Gets the entire bounds that a child node is resposible for.
		/// </summary>
		/// <returns>The child bounds.</returns>
		/// <param name="index">The child's index. It is a value between 0 and 7.</param>
		private IBound3 GetChildBounds(int index) {
			IBound3 bound = new IBound3 (bounds.position.x, bounds.position.y, bounds.position.z, bounds.size.x/2, bounds.size.y/2, bounds.size.z/2);
			bound.position.x += ((index & 1) > 0)? bound.size.x : 0;
			bound.position.y += ((index & 2) > 0)? bound.size.y : 0;
			bound.position.z += ((index & 4) > 0)? bound.size.z : 0;
			return bound;
		}

        /// <summary>
        /// Gets all child node indices that the given bound covers
        /// </summary>
        /// <returns>The child indices.</returns>
        /// <param name="childBound">Child bound.</param>
        protected virtual int[] GetChildIndices(IBound3 childBound)
        {
            List<int> indices = new List<int>();
            int index;

            IVector3 childPosition = childBound.position.Clone();

            // 0,0,0
            index = GetChildIndex(childPosition);
            if (!indices.Contains(index))
            {
                indices.Add(index);
            }

            // 0,0,1
            childPosition = childBound.position;
            childPosition.z += childBound.size.z - 1;
            index = GetChildIndex(childPosition);
            if (!indices.Contains(index))
            {
                indices.Add(index);
            }

            // 0,1,0
            childPosition = childBound.position;
            childPosition.y += childBound.size.y - 1;
            index = GetChildIndex(childPosition);
            if (!indices.Contains(index))
            {
                indices.Add(index);
            }

            // 0,1,1
            childPosition = childBound.position;
            childPosition.y += childBound.size.y - 1;
            childPosition.z += childBound.size.z - 1;
            index = GetChildIndex(childPosition);
            if (!indices.Contains(index))
            {
                indices.Add(index);
            }

            // 1,0,0
            childPosition = childBound.position;
            childPosition.x += childBound.size.x - 1;
            index = GetChildIndex(childPosition);
            if (!indices.Contains(index))
            {
                indices.Add(index);
            }

            // 1,0,1
            childPosition = childBound.position;
            childPosition.x += childBound.size.x - 1;
            childPosition.z += childBound.size.z - 1;
            index = GetChildIndex(childPosition);
            if (!indices.Contains(index))
            {
                indices.Add(index);
            }

            // 1,1,0
            childPosition = childBound.position;
            childPosition.x += childBound.size.x - 1;
            childPosition.y += childBound.size.y - 1;
            index = GetChildIndex(childPosition);
            if (!indices.Contains(index))
            {
                indices.Add(index);
            }

            // 1,1,1
            childPosition = childBound.position;
            childPosition.x += childBound.size.x - 1;
            childPosition.y += childBound.size.y - 1;
            childPosition.z += childBound.size.z - 1;
            index = GetChildIndex(childPosition);
            if (!indices.Contains(index))
            {
                indices.Add(index);
            }

            return indices.ToArray();
        }

        /// <summary>
        /// Gets the bounds this node is responsible for.
        /// </summary>
        /// <returns>The bounds.</returns>
        public IBound3 GetBounds() {
			return bounds;
		}

		/// <summary>
		/// Gets an entire child node. Used when merging
		/// </summary>
		/// <param name="node">Node.</param>
		public OctreeNode<TCell>[] GetChildren() {
			return children;
		}

		/// <summary>
		/// Recursively gets voxel cells. Used for serialization
		/// </summary>
		/// <returns>The nodes.</returns>
		/// <param name="depth">Depth relative to this level.</param>
		/// <param name="bound">Bounds for the nodes to retrieve.</param>
		public void GetCells(HashSet<TCell> cells) {
			// get this node's cells, if any
			if (!cellComparer.Equals(cell, default(TCell))) {
				cells.Add (this.cell);
			}
			// get child node's cells, if any
			if (children != null) {
				for (int i = 0; i < children.Length; i++) {
					if (children [i] != null) {
						children [i].GetCells (cells);						
					}
				}
			}
		}

        /// <summary>
        /// Get all cells within the given bounds
        /// </summary>
        /// <param name="bound"></param>
        /// <param name="cells"></param>
        public void GetCellsWithinBounds(IBound3 bound, HashSet<TCell> cells) {
			if (cellComparer.Equals(cell, default(TCell))) {
				if (children != null) {
					for (int i = 0; i < children.Length; i++) {
						if (children [i] != null) {
                            IBound3 childBound = children[i].bounds;

                            // if it overlaps the requested bounds, return the child's nodes
                            if (childBound.Overlaps(bound)) {
								children [i].GetCellsWithinBounds (bound, cells);
							}
						}
					}
				}
			} else if (bounds.Overlaps(bound)) {
                // the node overlaps the requested bound
                cells.Add(cell);
			}
		}

        public bool ContainsCell(IBound3 bound)
        {
            if (!cellComparer.Equals(cell, default(TCell)))
            {
                // This overlaps check makes the funciton work.
                // TODO find a better way to solve this
                return this.bounds.Overlaps(bound);
            }
            else if (children != null)
            {
                int[] indices = GetChildIndices(bound);
                for (int i = 0; i < indices.Length; i++)
                {
                    int index = indices[i];
                    if (children[index] != null)
                    {
                        if (children[index].ContainsCell(bound))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
		/// Merge the specified node. This function assumes that the merging node uses the same grid.
		/// </summary>
		/// <param name="node">Node.</param>
		public void Merge(OctreeNode<TCell> node)
        {
            if (node.bounds.size == bounds.size)
            {
                if (!cellComparer.Equals(cell, default(TCell)))
                {
                    cell = node.cell;
                    children = null;
                }
                else
                {
                    OctreeNode<TCell>[] mergeChildren = node.GetChildren();
                    if (mergeChildren != null)
                    {
                        if (children == null)
                        {
                            children = new OctreeNode<TCell>[8];
                        }
                        for (int i = 0; i < 8; i++)
                        {
                            if (mergeChildren[i] != null)
                            {
                                if (children[i] == null)
                                {
                                    children[i] = mergeChildren[i];
                                }
                                else
                                {
                                    children[i].Merge(mergeChildren[i]);
                                }
                            }
                        }
                    }
                }
            }
            else if (node.bounds.size < bounds.size)
            {
                // go deeper, until the size is the same or 1
                if (children == null)
                {
                    children = new OctreeNode<TCell>[8];
                }
                int index = GetChildIndex(node.bounds.position);
                if (children[index] == null)
                {
                    IBound3 childBounds = GetChildBounds(index);
                    children[index] = new OctreeNode<TCell>(childBounds);
                }

                // merge the node
                children[index].Merge(node);
            }
        }





        /// <summary>
        /// Returns true if the given IBound3 is completely populated
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        public bool IsAreaFull(IBound3 bound)
        {
            if (!cellComparer.Equals(cell, default(TCell)))
            {
                return true;
            }
            else if (children != null)
            {
                int[] indices = GetChildIndices(bound);

                for (int i = 0; i < indices.Length; i++)
                {
                    int index = indices[i];
                    if (children[index] != null)
                    {
                        if (!children[index].IsAreaFull(bound))
                        {
                            // if any child isn't full the area isn't full
                            return false;
                        }
                    }
                    else
                    {
                        // null child means it isn't full
                        return false;
                    }
                }

                // all the children within bounds were full, so this node is full
                return true;
            }

            // null cell and child means it isn't full
            return false;
        }
        /// <summary>
        /// Returns true if the given IBound3 is completely populated with the given cell
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        public bool IsAreaFull(IBound3 bound, TCell compareCell)
        {
            if (cellComparer.Equals(cell, compareCell))
            {
                return true;
            }
            else if (children != null)
            {
                int[] indices = GetChildIndices(bound);

                for (int i = 0; i < indices.Length; i++)
                {
                    int index = indices[i];
                    if (children[index] != null)
                    {
                        if (!children[index].IsAreaFull(bound, compareCell))
                        {
                            // if any child isn't full the area isn't full
                            return false;
                        }
                    }
                    else
                    {
                        // null child means it isn't full
                        return false;
                    }
                }

                // all the children within bounds were full, so this node is full
                return true;
            }

            // null cell and child means it isn't full
            return false;
        }

        /// <summary>
        /// Finds the nearest cell in the provided direction
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        /*public OctreeNode<TCell> Raycast(IVector3 position, IVector3 direction)
        {
            if (!cellComparer.Equals(cell, default(TCell)))
            {
                return this;
            }
            else if (children != null)
            {
                int[] indices = GetChildIndices(position);

                for (int i = 0; i < indices.Length; i++)
                {
                    int index = indices[i];
                    if (children[index] != null)
                    {
                        if (!children[index].IsAreaFull(bound))
                        {
                            // if any child isn't full the area isn't full
                            return false;
                        }
                    }
                    else
                    {
                        // null child means it isn't full
                        return false;
                    }
                }

                // all the children within bounds were full, so this node is full
                return true;
            }

            // null cell and child means it isn't full
            return false;
        }*/



        /*public TCell GetShapes(IVector3 position, Dictionary<TCell, List<IBound3>> shapes)
        {
            if (!cellComparer.Equals(cell, default(TCell))) // null check
            {
                if (bounds.Contains(position))
                {
                    return cell;
                }
            }
            else if (children != null)
            {
                int index = GetChildIndex(position);
                if (children[index] != null)
                {
                    return children[index].GetCell(position);
                }
            }

            return default(TCell);
        }*/
    }
}
