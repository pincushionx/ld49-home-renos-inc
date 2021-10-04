
using System.Collections.Generic;

namespace Pincushion.LD49
{
    /// <summary>
    /// Manages available quads in 2d space.
    /// Needs to be able to provide open space adjacent to a given partition for further partitioning or merging.
    /// </summary>
    public class Partitioner
    {
        public int innerWallThickness = 0;
        public int outerWallThickness = 0;

        public List<IPartition> reserved;
        public List<IBound2> available;

        public Partitioner()
        {
            reserved = new List<IPartition>();
            available = new List<IBound2>();
        }


        public void AddAvailableSpace(IBound2[] shape)
        {
            // Do it in reverse, so if there are overlaps, the first shape gets precedence
            for (int i = shape.Length - 1; i >= 0; i--)
            {
                AddAvailableSpace(shape[i]);
            }
        }
        public virtual bool AddAvailableSpace(IBound2 bound)
        {
            if (MakeRoom(bound, false))
            {
                available.Add(bound);
                return true;
            }
            return false;
        }

        public virtual bool Reserve(IPartition partition)
        {
            // TODO this must now account for walls

            foreach (IBound2 bound in partition.Shape)
            {
                if (!MakeRoom(bound, true))
                {
                    // can't make room
                    return false;
                }
            }

            reserved.Add(partition);

            return true;
        }


        public List<IBound2> GetAvailableWithinBound(IBound2 bound)
        {
            List<IBound2> availableShape = new List<IBound2>();

            foreach (IBound2 availableBound in available)
            {
                if (bound.Overlaps(availableBound))
                {
                    IBound2 overlap = bound.GetOverlappedBound(availableBound);
                    if (!overlap.Empty)
                    {
                        availableShape.Add(overlap);
                    }
                }
            }

            return availableShape;
        }

        /// <summary>
        /// Gets available bounds, groupped as contiguous shapes.
        /// </summary>
        /// <returns></returns>
        public List<IBound2>[] GetAvailableShapes()
        {
            return null;
        }


        private bool OverlapsReserved(IBound2 bound)
        {
            IBound2 paddedBound = IUtil.EnlargeIBound(bound, innerWallThickness);

            for (int i = 0; i < reserved.Count; i++)
            {
                if (reserved[i].Overlaps(paddedBound))
                {
                    return true;
                }
            }
            return false;
        }

        private bool MakeRoom(IBound2 bound, bool usePadding)
        {
            // Make sure the room is available
            if (OverlapsReserved(bound))
            {
                return false;
            }

            IBound2 paddedBound = usePadding? IUtil.EnlargeIBound(bound, innerWallThickness) : bound;

            // Slice any available room that is overlapped.
            for (int i = 0; i < available.Count; i++)
            {
                if (available[i].Overlaps(paddedBound))
                {
                    SliceBound(available[i], paddedBound);
                    i--; // Retry this step, since the slice may have updated
                }
            }
            return true;
        }

        /// <summary>
        /// Removes the available bound and replaces it with bounds that surround the given bound
        /// </summary>
        /// <param name="partitionBound"></param>
        /// <param name="bound"></param>
        private void SliceBound(IBound2 availableBound, IBound2 bound)
        {
            //[clone]
            //[]hit[]
            //[clone]

            available.Remove(availableBound);

            IBound2 overlap = availableBound.GetOverlappedBound(bound);

            IVector2 overlapTopcorner = overlap.position + overlap.size;
            IVector2 cellTopcorner = availableBound.position + availableBound.size;
            if (overlap.position.x != availableBound.position.x)
            {
                // left. x-gap, y-overlap
                IBound2 cellBound = new IBound2(
                    new IVector2(availableBound.position.x, overlap.position.y),
                    new IVector2(overlap.position.x - availableBound.position.x, overlap.size.y));
                if (cellBound.size.x > 0 && cellBound.size.y > 0)
                {
                    available.Add(cellBound);
                }
            }
            if (overlap.position.y != availableBound.position.y)
            {
                // bottom. full x, y-gap
                IBound2 cellBound = new IBound2(
                    new IVector2(availableBound.position.x, availableBound.position.y),
                    new IVector2(availableBound.size.x, overlap.position.y - availableBound.position.y));
                if (cellBound.size.x > 0 && cellBound.size.y > 0)
                {
                    available.Add(cellBound);
                }
            }
            if (overlapTopcorner.x != cellTopcorner.x)
            {
                // right. x-gap, y-overlap
                IBound2 cellBound = new IBound2(
                    new IVector2(overlapTopcorner.x, overlap.position.y),
                    new IVector2(cellTopcorner.x - overlapTopcorner.x, overlap.size.y));
                if (cellBound.size.x > 0 && cellBound.size.y > 0)
                {
                    available.Add(cellBound);
                }
            }
            if (overlapTopcorner.y != cellTopcorner.y)
            {
                // top. full x, y-gap
                IBound2 cellBound = new IBound2(
                    new IVector2(availableBound.position.x, overlapTopcorner.y),
                    new IVector2(availableBound.size.x, cellTopcorner.y - overlapTopcorner.y));
                if (cellBound.size.x > 0 && cellBound.size.y > 0)
                {
                    available.Add(cellBound);
                }
            }
        }

        public void Simplify()
        {
            SimplifyAvailable();
            SimplifyReserved();
        }

        public void SimplifyAvailable()
        {
            IBound2[] simplifiedShape = ShapeUtil.SimplifyShape(available);
            available.Clear();
            available.AddRange(simplifiedShape);
        }
        public void SimplifyReserved()
        {
            foreach (IPartition partition in reserved)
            {
                partition.Simplify();
            }
        }

        public void Merge(Partitioner partitioner)
        {
            foreach (IBound2 bound in partitioner.available)
            {
                AddAvailableSpace(bound);
            }
            foreach (IPartition partition in partitioner.reserved)
            {
                Reserve(partition);
            }
        }
        public Partitioner Clone()
        {
            Partitioner partitioner = new Partitioner();

            partitioner.reserved = new List<IPartition>(reserved);
            partitioner.available = new List<IBound2>(available);

            return partitioner;
        }
        public Partitioner CloneAvailable()
        {
            Partitioner partitioner = new Partitioner();
            partitioner.available = new List<IBound2>(available);
            return partitioner;
        }


        #region Force insert

        public virtual bool OverwriteShape(IPartition partition)
        {
            foreach (IBound2 bound in partition.Shape)
            {
                if (!MakeRoomOverwrite(bound))
                {
                    // can't make room
                    return false;
                }
            }

            reserved.Add(partition);

            return true;
        }


        
        private bool MakeRoomOverwrite(IBound2 bound)
        {
            IBound2 paddedBound = IUtil.EnlargeIBound(bound, innerWallThickness);

            // Make sure the room is available
            // Slice reserved bounds if necessary
            for (int i = 0; i < reserved.Count; i++)
            {
                IBound2[] reservedShape = reserved[i].Shape;
                List<IBound2> newShape = new List<IBound2>(); //
                bool useNewShape = false;
                for (int j = 0; j < reservedShape.Length; j++)
                {
                    IBound2 reservedBound = reservedShape[j];
                    if (reservedBound.Overlaps(paddedBound))
                    {
                        //SliceReservedBound(reserved[i], j, paddedBound);
                        List<IBound2> slicedShape = ShapeUtil.GetNonOverlappingShape(reservedBound, paddedBound);
                        if (slicedShape.Count > 0)
                        {
                            newShape.AddRange(slicedShape);
                        }
                        useNewShape = true;
                    }
                    else
                    {
                        // doesn't overlap, add it as-is to to the new shape
                        newShape.Add(reservedBound);
                    }
                }
                if (useNewShape)
                {
                    reserved[i].Shape = newShape.ToArray();
                }
            }

            // Slice any available room that is overlapped.
            for (int i = 0; i < available.Count; i++)
            {
                if (available[i].Overlaps(paddedBound))
                {
                    SliceBound(available[i], paddedBound);
                    i--; // Retry this step, since the slice may have updated
                }
            }
            return true;
        }

        #endregion
    }
}