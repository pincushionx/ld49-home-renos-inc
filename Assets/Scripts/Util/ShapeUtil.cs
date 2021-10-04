using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pincushion.LD49
{
    /// <summary>
    /// Utils for manipulating and analyzing IBound2[] shapes
    /// </summary>
    public static class ShapeUtil
    {
        #region Analysis

        public static int GetArea(IEnumerable<IBound2> shape)
        {
            int area = 0;
            foreach (IBound2 bound in shape)
            {
                area += bound.Area;
            }
            return area;
        }
        public static IBound2 GetEncapsulatingBound(IEnumerable<IBound2> shape)
        {
            IBound2 encapsulatingBound = new IBound2();
            foreach (IBound2 bound in shape)
            {
                if (encapsulatingBound.Empty)
                {
                    encapsulatingBound = bound;
                }
                else
                {
                    encapsulatingBound.Encapsulate(bound);
                }
            }
            return encapsulatingBound;
        }

        /// <summary>
        /// Gets the distance between the two given shapes.
        /// </summary>
        /// <param name="shapeA"></param>
        /// <param name="shapeB"></param>
        /// <returns></returns>
        public static float GetDistance(IEnumerable<IBound2> shapeA, IEnumerable<IBound2> shapeB)
        {
            float minDistance = float.MaxValue;
            
            foreach (IBound2 boundA in shapeA)
            {
                foreach (IBound2 boundB in shapeB)
                {
                    float distance = boundA.Distance(boundB);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                    }
                }
            }

            return minDistance;
        }

        public static bool IsAdjacent(IEnumerable<IBound2> shapeA, IEnumerable<IBound2> shapeB)
        {
            foreach (IBound2 boundA in shapeA)
            {
                foreach (IBound2 boundB in shapeB)
                {
                    if (boundA.IsAdjacentTo(boundB))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the adjacent shape from shapeA, in the given thickness.
        /// </summary>
        /// <param name="shapeA"></param>
        /// <param name="shapeB"></param>
        /// <returns></returns>
        public static List<IBound2> GetAdjacentShape(IEnumerable<IBound2> shapeA, IEnumerable<IBound2> shapeB, int thickness)
        {
            List<IBound2> shape = new List<IBound2>();
            foreach (IBound2 boundA in shapeA)
            {
                foreach (IBound2 boundB in shapeB)
                {
                    if (boundA.IsAdjacentTo(boundB))
                    {
                        IBound2 enlargedBoundB = IUtil.EnlargeIBound(boundB, thickness);
                        IBound2 overlappedBound = boundA.GetOverlappedBound(enlargedBoundB);
                        shape.Add(overlappedBound);
                    }
                }
            }
            return shape;
        }

        public static bool Overlaps(IEnumerable<IBound2> shapeA, IEnumerable<IBound2> shapeB)
        {
            foreach (IBound2 boundA in shapeA)
            {
                foreach (IBound2 boundB in shapeB)
                {
                    if (boundA.Overlaps(boundB))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion
        #region Split
        /// <summary>
        /// Slices the shape in the given direction
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="direction">False is in the X direction. True is in the Y direction</param>
        /// <returns></returns>
        public static List<IBound2>[] ShapeToGrid(IEnumerable<IBound2> shape, bool direction)
        {
            return direction? ShapeToGridY(shape) : ShapeToGridX(shape);
        }

        /// <summary>
        /// Slices the given shape, slicing Y, moving X left to right.
        /// </summary>
        /// <param name="shape"></param>
        /// <returns>Array of lists of chopped shapes. The array slots are the Y slices, the lists are the bounds that fit within the slice</returns>
        public static List<IBound2>[] ShapeToGridX(IEnumerable<IBound2> shape)
        {
            // Slice Y, moving X left to right
            // Build sliced partitions
            List<int> tiers = new List<int>();
            List<IBound2>[] boundByTier;

            // First, get the starting X positions
            foreach (IBound2 bound in shape)
            {
                if (!tiers.Contains(bound.MinX))
                {
                    tiers.Add(bound.MinX);
                }
                if (!tiers.Contains(bound.MaxX))
                {
                    tiers.Add(bound.MaxX);
                }
            }

            // check if the given shape was empty
            if (tiers.Count == 0)
            {
                return null;
            }

            tiers.Sort();
            boundByTier = new List<IBound2>[tiers.Count - 1];

            // For each tier, get the available area
            for (int tierIndex = 0; tierIndex < tiers.Count - 1; tierIndex++)
            {
                IBound1 tierBound = new IBound1(tiers[tierIndex], tiers[tierIndex + 1] - tiers[tierIndex]);
                List<IBound2> tierBounds = new List<IBound2>();
                boundByTier[tierIndex] = tierBounds;

                foreach (IBound2 bound in shape)
                {
                    IBound2 boundForShapeTier = bound;
                    IBound1 boundX = boundForShapeTier.ToIBound1X();
                    if (boundX.Overlaps(tierBound))
                    {
                        IBound1 overlap = boundX.GetOverlappedBound(tierBound);
                        boundForShapeTier.position.x = overlap.position;
                        boundForShapeTier.size.x = overlap.size;

                        tierBounds.Add(boundForShapeTier);
                    }
                }
                tierBounds.Sort(IBound2.CompareByPosY);
            }

            return boundByTier;
        }
        /// <summary>
        /// Slices the given shape, slicing X, moving Y bottom to top.
        /// </summary>
        /// <param name="shape"></param>
        /// <returns>Array of lists of chopped shapes. The array slots are the X slices, the lists are the bounds that fit within the slice</returns>
        public static List<IBound2>[] ShapeToGridY(IEnumerable<IBound2> shape)
        {
            // Slice Y, moving X left to right
            // Build sliced partitions
            List<int> tiers = new List<int>();
            List<IBound2>[] boundByTier;

            // First, get the starting X positions
            foreach (IBound2 bound in shape)
            {
                if (!tiers.Contains(bound.MinY))
                {
                    tiers.Add(bound.MinY);
                }
                if (!tiers.Contains(bound.MaxY))
                {
                    tiers.Add(bound.MaxY);
                }
            }

            // check if the given shape was empty
            if (tiers.Count == 0)
            {
                return null;
            }

            tiers.Sort();
            boundByTier = new List<IBound2>[tiers.Count - 1];

            // For each tier, get the available area
            for (int tierIndex = 0; tierIndex < tiers.Count - 1; tierIndex++)
            {
                IBound1 tierBound = new IBound1(tiers[tierIndex], tiers[tierIndex + 1] - tiers[tierIndex]);
                List<IBound2> tierBounds = new List<IBound2>();
                boundByTier[tierIndex] = tierBounds;

                foreach (IBound2 bound in shape)
                {
                    IBound2 boundForShapeTier = bound;
                    IBound1 boundX = boundForShapeTier.ToIBound1Y();
                    if (boundX.Overlaps(tierBound))
                    {
                        IBound1 overlap = boundX.GetOverlappedBound(tierBound);
                        boundForShapeTier.position.y = overlap.position;
                        boundForShapeTier.size.y = overlap.size;
                        tierBounds.Add(boundForShapeTier);
                    }
                }
                tierBounds.Sort(IBound2.CompareByPosX);
            }

            return boundByTier;
        }

        /// <summary>
        /// Attempts to simplify a shape by slicing it by all X-stops (starts and ends on the x axis) and merging the slices, 
        /// and then it does the same with Y stops
        /// </summary>
        /// <param name="shape"></param>
        /// <returns>The simplified shape</returns>
        public static IBound2[] SimplifyShape(IEnumerable<IBound2> shape)
        {
            List<IBound2>[] grid = ShapeToGrid(shape, false); // X direction
            if (grid == null)
            {
                // The shape was empty, return an empty array
                return new IBound2[0];
            }

            IBound2[] simplifiedShape = SimplifyGridToShape(grid);
            grid = ShapeToGrid(simplifiedShape, true); // Y direction

            return SimplifyGridToShape(grid);
        }

        /// <summary>
        /// This is a helper function to SimplifyShape
        /// </summary>
        /// <returns></returns>
        private static IBound2[] SimplifyGridToShape(List<IBound2>[] grid)
        {
            List<IBound2> simplifiedShape = new List<IBound2>();
            for (int gridIndex = 0; gridIndex < grid.Length; gridIndex++)
            {
                List<IBound2> tier = grid[gridIndex];
                List<IBound2> simplifiedTier = new List<IBound2>();
                IBound2 accumulatedCell = new IBound2();
                for (int tierIndex = 0; tierIndex < tier.Count; tierIndex++)
                {
                    IBound2 cell = tier[tierIndex];

                    if (accumulatedCell.Empty)
                    {
                        accumulatedCell = cell;
                    }
                    else if (accumulatedCell.IsAdjacentTo(cell))
                    {
                        // merge the cells
                        accumulatedCell.Encapsulate(cell);
                    }
                    else
                    {
                        // Add the accumulated cell, and start another
                        simplifiedTier.Add(accumulatedCell);
                        accumulatedCell = cell;
                    }
                }
                if (!accumulatedCell.Empty)
                {
                    simplifiedTier.Add(accumulatedCell);
                }
                simplifiedShape.AddRange(simplifiedTier);
            }

            return simplifiedShape.ToArray();
        }

        /// <summary>
        /// Splits a shape on the x axis.
        /// </summary>
        /// <param name="sectionShape">the single shape to divide</param>
        /// <param name="divisions">target number of shapes</param>
        /// <param name="wallThickness">Width of gap between divisions</param>
        /// <returns></returns>
        public static IBound2[][] Split(IBound2[] sectionShape, int divisions, int wallThickness)
        {
            // create even-sized divisions
            int[] divisionList = new int[divisions];
            for (int i = 0; i < divisions; i++)
            {
                divisionList[i] = 1;
            }

            return SplitX(sectionShape, divisionList, wallThickness);
        }
        public static IBound2[][] SplitX(IEnumerable<IBound2> sectionShape, int[] partitionsByWeight, int wallThickness)
        {
            return Split(sectionShape, partitionsByWeight, wallThickness, false);
        }
        public static IBound2[][] SplitY(IEnumerable<IBound2> sectionShape, int[] partitionsByWeight, int wallThickness)
        {
            return Split(sectionShape, partitionsByWeight, wallThickness, true);
        }
        public static IBound2[][] Split(IEnumerable<IBound2> sectionShape, int[] partitionsByWeight, int wallThickness, bool direction)
        {
            int area = GetArea(sectionShape);
            int divisions = partitionsByWeight.Length;

            // correct the partition area
            int totalPartitionsByArea = 0;
            for (int partitionsByAreaIndex = 0; partitionsByAreaIndex < divisions; partitionsByAreaIndex++)
            {
                totalPartitionsByArea += partitionsByWeight[partitionsByAreaIndex];
            }
            float partitionFactor = area / (float)totalPartitionsByArea; // use this against partitionsByArea to determine partition area

            int totalRoundedArea = 0;
            int[] partitionsByArea = new int[divisions];
            for (int partitionsByAreaIndex = 0; partitionsByAreaIndex < divisions; partitionsByAreaIndex++)
            {
                partitionsByArea[partitionsByAreaIndex] = Mathf.FloorToInt(partitionsByWeight[partitionsByAreaIndex] * partitionFactor);
                totalRoundedArea += partitionsByArea[partitionsByAreaIndex];
            }

            // add the remaining area to the last partition
            partitionsByArea[partitionsByArea.Length - 1] += area - totalRoundedArea;


            IBound2[][] partitions = new IBound2[divisions][];
            int currentPartitionIndex = 0;

            // Slice Y, moving X left to right
            // Build sliced partitions
            List<IBound2>[] grid = ShapeToGrid(sectionShape, direction);
            List<IBound2> currentPartition = new List<IBound2>();

            int remainingMargin = 0; // if a tier width is less than the wall thickness
            int accumulatedArea = 0;
            for (int tierIndex = 0; tierIndex < grid.Length;)
            {
                IBound2[] tierShape = grid[tierIndex].ToArray();
                int tierWidth = direction? tierShape[0].size.y : tierShape[0].size.x;

                // Resize the tier to exclude the wallThickness
                if (tierWidth < remainingMargin)
                {
                    // the entire tier is eaten up by wall, so just exclude it
                    remainingMargin -= tierWidth;
                    tierIndex++;
                    continue;
                }
                if (remainingMargin > 0)
                {
                    // eat the margin from the beginning of the tier
                    for (int cellIndex = 0; cellIndex < tierShape.Length; cellIndex++)
                    {
                        IBound2 currentBound = tierShape[cellIndex];

                        if (direction)
                        {
                            currentBound.position.y += remainingMargin;
                            currentBound.size.y -= remainingMargin;
                        }
                        else
                        {
                            currentBound.position.x += remainingMargin;
                            currentBound.size.x -= remainingMargin;
                        }

                        // Set the remaining bound to the current tier
                        grid[tierIndex][cellIndex] = currentBound;
                    }

                    // Remove the remaining margin until the next partition
                    remainingMargin = 0;
                    continue;
                }

                int tierArea = GetArea(tierShape);
                int accumulatedAreaPlusTier = accumulatedArea + tierArea;

                if (tierArea == 0)
                {
                    tierIndex++;
                    continue;
                }

                int areaPerPartition = Mathf.FloorToInt(partitionsByArea[currentPartitionIndex]);

                if (accumulatedAreaPlusTier > areaPerPartition)
                {
                    // Divide this tier, add the partition
                    int neededTierArea = areaPerPartition - accumulatedArea;
                    float resizeFactor = (float)neededTierArea / tierArea;
                    int newWidth = Mathf.CeilToInt(tierWidth * resizeFactor);

                    // separate the used part from the tier
                    for (int cellIndex = 0; cellIndex < tierShape.Length; cellIndex++)
                    {
                        IBound2 currentBound = tierShape[cellIndex];
                        IBound2 remainingBound = tierShape[cellIndex];

                        if (direction)
                        {
                            currentBound.size.y = newWidth;
                            remainingBound.position.y += newWidth;
                            remainingBound.size.y -= newWidth;
                        }
                        else
                        {
                            currentBound.size.x = newWidth;
                            remainingBound.position.x += newWidth;
                            remainingBound.size.x -= newWidth;
                        }

                        // Add the current bound to the current partition
                        currentPartition.Add(currentBound);
                        accumulatedArea += currentBound.Area;

                        // Set the remaining bound to the current tier
                        grid[tierIndex][cellIndex] = remainingBound;
                    }

                    // Don't go to the next tier
                }
                else
                {
                    // Add the shape to the current partition. Remove any empty bounds
                    for (int cellIndex = 0; cellIndex < tierShape.Length; cellIndex++)
                    {
                        IBound2 currentBound = tierShape[cellIndex];

                        // Add the current bound to the current partition
                        if (currentBound.Area > 0)
                        {
                            currentPartition.Add(currentBound);
                        }
                    }
                    //currentPartition.AddRange(tierShape);
                    accumulatedArea = accumulatedAreaPlusTier;
                    tierIndex++;
                }

                // The shape meets the criteria, save it and start the next shape
                if (accumulatedArea >= areaPerPartition || tierIndex == grid.Length)
                {
                    partitions[currentPartitionIndex] = currentPartition.ToArray();
                    currentPartitionIndex++;

                    remainingMargin = wallThickness; // reset wall thickness to add a wall between partitions
                    accumulatedArea = 0;
                    currentPartition.Clear();
                }
            }
            return partitions;
        }

        /// <summary>
        /// Splits the given shape using the squarify algorithm
        /// </summary>
        /// <param name="sectionShape"></param>
        /// <param name="partitionsByArea"></param>
        /// <param name="wallThickness"></param>
        /// <returns></returns>
        public static IBound2[][] Split(IBound2[] sectionShape, int[] partitionsByArea, int wallThickness)
        {
            List <int> sorted = new List<int>(partitionsByArea);
            //sorted.Sort();
            //sorted.Reverse();
            IBound2[][] partitions = Squarify(sorted.ToArray(), sectionShape, wallThickness, false);
            return partitions;
        }

        /// <summary>
        /// A strategy for the Split function. Based on squarified treemaps, but here, there's no tree
        /// TODO remove recursion, since there's no tree
        ///  https://www.hindawi.com/journals/ijcgt/2010/624817/
        ///  https://github.com/imranghory/treemap-squared/blob/7bdc84a99379f46759c148aa9db41a6735d5a584/treemap-squarify.js#L71
        ///  https://github.com/nicopolyptic/treemap/blob/master/src/main/ts/squarifier.ts
        /// </summary>
        /// <param name="partitionsByArea"></param>
        /// <param name="unclaimedShape"></param>
        /// <param name="wallThickness"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private static IBound2[][] Squarify(int[] partitionsByArea, IBound2[] unclaimedShape, int wallThickness, bool direction)
        {
            IBound2[][] stepPartitions = direction? SplitY(unclaimedShape, partitionsByArea, 0) : SplitX(unclaimedShape, partitionsByArea, 0);

            List<IBound2[]> partitions = new List<IBound2[]>();
            List<int> currentRow = new List<int>();
            List<IBound2> currentRowShape = new List<IBound2>();
            int partitionIndex = 0;
            for (; partitionIndex < partitionsByArea.Length; partitionIndex++)
            {
                if (stepPartitions[partitionIndex].Length == 0)
                {
                    Debug.Log("Step partition has no shapes (from splitY)");
                    continue;
                }

                if (currentRow.Count == 0)
                {
                    currentRow.Add(partitionsByArea[partitionIndex]);
                    currentRowShape.AddRange(stepPartitions[partitionIndex]);
                }
                else if (improvesRatio(currentRow, currentRowShape, partitionsByArea[partitionIndex], stepPartitions[partitionIndex], direction)) //partitionIndex % 2 == 1)// 
                {
                    currentRow.Add(partitionsByArea[partitionIndex]);
                    currentRowShape.AddRange(stepPartitions[partitionIndex]);
                }
                else
                {
                    break;
                }
            }

            // add the remaining stuff
            if (currentRow.Count > 0)
            {
                // Relayout and store the current row
                // flip the direction to layout
                IBound2[][] currentRowLayout;// = !direction ? SplitY(currentRowShape, currentRow.ToArray(), wallThickness) : SplitX(currentRowShape, currentRow.ToArray(), wallThickness);

                // First, add walls by reducing the current row's shapes by wallThickness
                if (partitionIndex < partitionsByArea.Length)
                {
                    IBound2 currentRowBound = GetEncapsulatingBound(currentRowShape);
                    int max = direction? currentRowBound.MaxY : currentRowBound.MaxX;

                    for (int i = currentRowShape.Count - 1; i >= 0; i--)
                    {
                        IBound2 bound = currentRowShape[i];
                        int maxBound = direction ? bound.MaxY : bound.MaxX;

                        if (maxBound == max)
                        {
                            if (direction)
                            {
                                bound.size.y -= wallThickness;
                            }
                            else
                            {
                                bound.size.x -= wallThickness;
                            }
                            currentRowShape[i] = bound;

                            if (bound.Empty)
                            {
                                currentRowShape.RemoveAt(i);
                            }
                        }
                    }
                }

                // Relayout the row divisions
                currentRowLayout = Split(currentRowShape, currentRow.ToArray(), wallThickness, !direction);
                partitions.AddRange(currentRowLayout);
            }

            // Recurse with the remaining area
            if (partitionIndex < partitionsByArea.Length)
            {
                List<IBound2> remainingUnclaimedShape = new List<IBound2>();
                for (int i = partitionIndex; i < stepPartitions.Length; i++)
                {
                    remainingUnclaimedShape.AddRange(stepPartitions[i]);
                }
                int[] remainingPartitionsByArea = CopyArray(partitionsByArea, partitionIndex, partitionsByArea.Length);

                // TODO remove recursion. It isn't necessary
                IBound2[][] remainingRowLayout = Squarify(remainingPartitionsByArea, remainingUnclaimedShape.ToArray(), wallThickness, !direction);
                partitions.AddRange(remainingRowLayout);
            }

            // Prune null partitions. This isn't ideal, but may occur of the shape is too small to fit
            // TODO A better solution may be to take wall thickness from a larger partition.
            for (int i = partitions.Count - 1; i >= 0; i--)
            {
                if (partitions[i] == null)
                {
                    partitions.RemoveAt(i);
                }
            }

            return partitions.ToArray();
        }

        /// <summary>
        /// Attempts to find the best ratio for a row. This is a helper function to Squarify
        /// </summary>
        /// <param name="rowWeights"></param>
        /// <param name="rowShape"></param>
        /// <param name="additionalWeight"></param>
        /// <param name="additionalShape"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private static bool improvesRatio(IEnumerable<int> rowWeights, IEnumerable<IBound2> rowShape, int additionalWeight, IEnumerable<IBound2> additionalShape, bool direction)
        {
            if (rowShape == null)
            {
                return true;
            }

            IBound2 currentRowBound = GetEncapsulatingBound(rowShape);

            int totalWeight = 0;
            foreach (int rowWeight in rowWeights)
            {
                totalWeight += rowWeight;
            }

            int width = direction? currentRowBound.size.y : currentRowBound.size.x;
            int height = direction ? currentRowBound.size.x : currentRowBound.size.y;

            float currentRowRatio = 0;// float.MaxValue;
            foreach (int rowWeight in rowWeights)
            {
                float factor = (height * (rowWeight / (float)totalWeight));
                float ratio = (factor < width)? width / factor : factor / width;

                if (ratio > currentRowRatio)
                {
                    currentRowRatio = ratio;
                }
            }

            IBound2 additionalShapeBound = GetEncapsulatingBound(additionalShape);
            additionalShapeBound.Encapsulate(currentRowBound);

            width = direction ? additionalShapeBound.size.y : additionalShapeBound.size.x;
            height = direction ? additionalShapeBound.size.x : additionalShapeBound.size.y;

            int nextTotalWeight = totalWeight + additionalWeight;
            float nextRowRatio = 0;// float.MaxValue;
            List<int> nextWeights = new List<int>(rowWeights);
            nextWeights.Add(additionalWeight);
            foreach (int nextWeight in nextWeights)
            {
                float factor = (height * (nextWeight / (float)nextTotalWeight));
                float ratio = (factor < width) ? width / factor : factor / width;
                if (ratio > nextRowRatio)
                {
                    nextRowRatio = ratio;
                }
            }

            return currentRowRatio >= nextRowRatio;
        }

        #endregion

        private static T[] CopyArray<T>(T[] source, int from, int to)
        {
            int length = to - from;
            T[] copy = new T[length];

            for (int i = 0; i < length; i++) {
                copy[i] = source[from + i];
            }

            return copy;
        }
        private static T[] CloneAndAddToArray<T>(T[] source, T add)
        {
            T[] copy = new T[source.Length + 1];
            source.CopyTo(copy, 0);
            copy[source.Length] = add;
            return copy;
        }
        private static IBound2[] MergeShapes(IEnumerable<IEnumerable<IBound2>> shapes)
        {
            List<IBound2> merged = new List<IBound2>();
            foreach (IEnumerable<IBound2> shape in shapes)
            {
                merged.AddRange(shape);
            }
            return merged.ToArray();
        }

        /// <summary>
        /// Returns a shape that surrounds the given bound.
        /// </summary>
        /// <param name="partitionBound"></param>
        /// <param name="bound"></param>
        public static List<IBound2> GetNonOverlappingShape(IBound2 referenceBound, IBound2 bound)
        {
            //[clone]
            //[]hit[]
            //[clone]

            List<IBound2> sliced = new List<IBound2>();
            IBound2 overlap = referenceBound.GetOverlappedBound(bound);

            IVector2 overlapTopcorner = overlap.position + overlap.size;
            IVector2 cellTopcorner = referenceBound.position + referenceBound.size;
            if (overlap.position.x != referenceBound.position.x)
            {
                // left. x-gap, y-overlap
                IBound2 cellBound = new IBound2(
                    new IVector2(referenceBound.position.x, overlap.position.y),
                    new IVector2(overlap.position.x - referenceBound.position.x, overlap.size.y));
                if (cellBound.size.x > 0 && cellBound.size.y > 0)
                {
                    sliced.Add(cellBound);
                }
            }
            if (overlap.position.y != referenceBound.position.y)
            {
                // bottom. full x, y-gap
                IBound2 cellBound = new IBound2(
                    new IVector2(referenceBound.position.x, referenceBound.position.y),
                    new IVector2(referenceBound.size.x, overlap.position.y - referenceBound.position.y));
                if (cellBound.size.x > 0 && cellBound.size.y > 0)
                {
                    sliced.Add(cellBound);
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
                    sliced.Add(cellBound);
                }
            }
            if (overlapTopcorner.y != cellTopcorner.y)
            {
                // top. full x, y-gap
                IBound2 cellBound = new IBound2(
                    new IVector2(referenceBound.position.x, overlapTopcorner.y),
                    new IVector2(referenceBound.size.x, cellTopcorner.y - overlapTopcorner.y));
                if (cellBound.size.x > 0 && cellBound.size.y > 0)
                {
                    sliced.Add(cellBound);
                }
            }

            return sliced;
        }

    }
}