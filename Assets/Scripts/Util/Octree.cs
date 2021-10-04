using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD49 {
	/// <summary>
	/// The voxel octree stores all voxels for a particular object. It assumes that all objects are the same size.
	/// </summary>
	public class Octree<TCell> {
		private OctreeNode<TCell> root;
		private int levels;

        // a scale of 1 represents the smallest cell
        // a scale of 16 represents 1 meter per cell

        // bounds are built and cached in GetBounds(). they are set to null when invalidated
        private IBound3 bounds;
        private bool boundsValid = false;

        public Octree() {
            levels = 4;
			IBound3 initialBounds = GetBounds();
			root = new OctreeNode<TCell>(initialBounds);
		}
		
		/// <summary>
		/// Gets the potential depth of this voxel manager
		/// </summary>
		/// <returns>The depth.</returns>
		public int GetDepth() {
			return levels;
		}
 
        /// <summary>
        /// Sets a cell at the specified position
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="cell">Cell.</param>
        virtual public void SetCell(IBound3 cellBounds, TCell cell) {
            Grow(cellBounds);
            root.SetCell (cellBounds, cell);
		}

		public void RemoveNodes(IBound3 bound) {
			root.RemoveNodes(bound);
        }

		/// <summary>
		/// Gets the cell at the specified position
		/// </summary>
		/// <returns>The cell. Null if not found.</returns>
		/// <param name="position">Position.</param>
		public TCell GetCell(IVector3 position) {
			return root.GetCell (position);
		}

        /// <summary>
        /// Determines whether or not the requested cell is populated. If compare cell isn't null/default, it'll only search for that same cell.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="compareCell"></param>
        /// <returns></returns>
        public bool HasCell(IVector3 position, TCell compareCell)
        {
            if (EqualityComparer<TCell>.Default.Equals(compareCell, default(TCell)))
            {
                return !EqualityComparer<TCell>.Default.Equals(root.GetCell(position), default(TCell));
            }
            return root.HasCell(position, compareCell);
        }


        /// <summary>
        /// Finds the nearest cell in the provided direction
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public IVector3 Raycast(IVector3 position, IVector3 direction)
        {
            return Raycast(position, direction, int.MaxValue);
        }
        public IVector3 Raycast(IVector3 position, IVector3 direction, float distance)
        {
            IBound3 bound = GetBounds();

            //TODO implement in the nodes
            while (bound.Contains(position) && !root.HasCell(position) && distance-- > 0)
            {
                position += direction;
            }
            return position;
        }

        /// <summary>
        /// Gets all cells in the tree
        /// </summary>
        /// <returns>The cells.</returns>
        /// <param name="bounds">Bounds.</param>
        public TCell[] GetCells()
        {
            return GetCells(GetBounds());
        }

        /// <summary>
        /// Gets the cells within the provided bounds
        /// </summary>
        /// <returns>The cells.</returns>
        /// <param name="bounds">Bounds.</param>
        public TCell[] GetCells(IBound3 bounds) {
            HashSet<TCell> cells = new HashSet<TCell> ();
			root.GetCellsWithinBounds (bounds, cells);

            // convert and return the cells
            TCell[] arr = new TCell[cells.Count];
            cells.CopyTo(arr);
			return arr;
		}

        /// <summary>
        /// Gets the cells within the provided bounds
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="cells"></param>
        public void GetCells(IBound3 bounds, HashSet<TCell> cells)
        {
            root.GetCellsWithinBounds(bounds, cells);
        }

        public bool ContainsCell(IBound3 bound)
        {
            return root.ContainsCell(bound);
        }

        /// <summary>
        /// Gets the first node found. Used for cellular automation, assuming that all nodes are contiguous.
        /// </summary>
        /// <returns>A node, or null if none are found</returns>
        public OctreeNode<TCell> GetAnyLeafNode()
        {
            return root.GetAnyLeafNode();
        }

        public bool IsAreaFull(IBound3 bound)
        {
            return root.IsAreaFull(bound);
        }
        public bool IsAreaFull(IBound3 bound, TCell compareCell)
        {
            if (EqualityComparer<TCell>.Default.Equals(compareCell, default(TCell)))
            {
                return root.IsAreaFull(bound);
            }
            return root.IsAreaFull(bound, compareCell);
        }

        /// <summary>
        /// Merges another Octree into this one
        /// </summary>
        /// <param name="tree">Other Octree</param>
        public void Merge(Octree<TCell> tree)
        {
            Grow(tree.GetBounds());

            // start with the children for easy merging. 
            OctreeNode<TCell>[] children = tree.GetRootNode().GetChildren();
            if (children != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (children[i] != null)
                    {
                        root.Merge(children[i]);
                    }
                }
            }
        }
        /// <summary>
        /// Merges another Octree into this one using the provided translations
        /// </summary>
        /// <param name="tree">Other Octree</param>
        public void Merge(Octree<TCell> tree, IVector3 position, IVector3 rotation)
        {
            rotation = IUtil.SanitizeRotation(rotation);

            // get all of the nodes into a list
            List<OctreeNode<TCell>> oldNodes = new List<OctreeNode<TCell>>();
            tree.GetRootNode().GetLeafNodes(tree.GetBounds(), oldNodes);

            // Rotate and add the cells back into the tree
            foreach (OctreeNode<TCell> node in oldNodes)
            {
                IBound3 nodeBound = IUtil.Rotate(node.bounds, rotation);
                nodeBound.position += position;
                SetCell(nodeBound, node.cell);
            }
        }

        /// <summary>
        /// Gets the size of the octree in cell space
        /// </summary>
        /// <returns>The size.</returns>
        private IVector3 GetSize() {
			int levelsMinusOne = levels - 1;
			IVector3 size = new IVector3 (1,1,1);
			size.x <<= levelsMinusOne; 
			size.y <<= levelsMinusOne;
			size.z <<= levelsMinusOne;
			return size;
		}

		/// <summary>
		/// Gets the bounds for the entire octree in cell space
		/// </summary>
		/// <returns>The bounds.</returns>
		public IBound3 GetBounds() {
            if (!boundsValid)
            {
                IVector3 size = GetSize();
                bounds = new IBound3(size / 2 * -1, size);
                boundsValid = true;
            }
			return bounds;
		}

		/// <summary>
		/// Grows the octree one level
		/// </summary>
		private void Grow() {
			// increment the level
			levels++;

            // invalidate the bounds
            boundsValid = false;

            // save the root's children. they'll be merged to the new root
            OctreeNode<TCell>[] children = root.GetChildren ();

			// create a new root, with the new bounds
			root = new OctreeNode<TCell>(GetBounds());

			// merge the old root's children with the new root
			if (children != null) {
				for (int i = 0; i < 8; i++) {
					if (children [i] != null) {
						root.Merge (children [i]);
					}
				}
			}
		}

        /// <summary>
        /// Grows to contain the given bound
        /// </summary>
        /// <param name="bound"></param>
        public void Grow(IBound3 bound)
        {
            IBound3 bounds = root.GetBounds();

            // contains may equal the cell bounds, also ensure the bounds are bigger than the cell
            while (!bounds.Contains(bound) || bounds.size <= bound.size)
            {
                Grow();
                bounds = root.GetBounds();
            }
        }

        public OctreeNode<TCell> GetRootNode() {
			return root;
		}

        /// <summary>
		/// Rotates all cells on the 0,0 axis.
		/// </summary>
		/// <param name="rotation">The rotation as an IVector3. Each 1 refers to a rotation of 90 degrees on any axis.</param>
		public void Rotate(IVector3 rotation)
        {
            rotation = IUtil.SanitizeRotation(rotation);

            // get all of the nodes into a list
            List<OctreeNode<TCell>> oldNodes = new List<OctreeNode<TCell>>();
            root.GetLeafNodes(GetBounds(), oldNodes);

            // clear the tree
            RemoveNodes(GetBounds());

            // Rotate and add the cells back into the tree
            foreach (OctreeNode<TCell> node in oldNodes)
            {
                node.bounds = IUtil.Rotate(node.bounds, rotation);
                SetCell(node.bounds, node.cell);
            }
        }
        /// <summary>
		/// Rotates all cells on the 0,0 axis.
		/// </summary>
		/// <param name="rotation">The rotation as an IVector3. Each 1 refers to a rotation of 90 degrees on any axis.</param>
		public void Transform(IVector3 position, IVector3 rotation)
        {
            rotation = IUtil.SanitizeRotation(rotation);

            // get all of the nodes into a list
            List<OctreeNode<TCell>> oldNodes = new List<OctreeNode<TCell>>();
            root.GetLeafNodes(GetBounds(), oldNodes);

            // clear the tree
            RemoveNodes(GetBounds());

            // Rotate and add the cells back into the tree
            foreach (OctreeNode<TCell> node in oldNodes)
            {
                node.bounds = IUtil.Rotate(node.bounds, rotation);

                IUtil.Rotate(node.bounds, rotation);
                node.bounds.position += position;
                SetCell(node.bounds, node.cell);
            }
        }

        public bool Overlaps<TCellOther>(Octree<TCellOther> other)
        {
            List<OctreeNode<TCellOther>> otherNodes = new List<OctreeNode<TCellOther>>();
            other.GetRootNode().GetLeafNodes(other.GetBounds(), otherNodes);

            foreach (OctreeNode<TCellOther> otherNode in otherNodes)
            {
                if (root.ContainsCell(otherNode.bounds))
                {
                    return true;
                }
            }
            return false;
        }
        public OctreeNode<TCell>[] GetOverlappedNodes<TCellOther>(Octree<TCellOther> other)
        {
            List<OctreeNode<TCellOther>> otherNodes = new List<OctreeNode<TCellOther>>();
            other.GetRootNode().GetLeafNodes(other.GetBounds(), otherNodes);

            List<OctreeNode<TCell>> overlappedNodes = new List<OctreeNode<TCell>>();

            foreach (OctreeNode<TCellOther> otherNode in otherNodes)
            {
                root.GetLeafNodes(otherNode.bounds, overlappedNodes);
            }

            return overlappedNodes.ToArray();
        }
        public Octree<TCell> Clone()
        {
            Octree<TCell> tree = new Octree<TCell>();
            //tree.Merge(this);

            HashSet<OctreeNode<TCell>> nodes = new HashSet<OctreeNode<TCell>>();
            GetRootNode().GetLeafNodes(GetBounds(), nodes);

            foreach (OctreeNode<TCell> node in nodes)
            {
                tree.SetCell(node.bounds, node.cell);
            }

            return tree;
        }
    }
}