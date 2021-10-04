using System;

namespace Pincushion.LD49
{
    [Serializable]
    public struct IBound3
    {
        public IVector3 position;
        public IVector3 size;

        #region Properties

        public IVector3 Center
        {
            // note: since this is using integers, it is lossy
            get
            {
                return new IVector3(
                    position.x + (size.x / 2),
                    position.y + (size.y / 2),
                    position.z + (size.z / 2)
                );
            }
        }

        public int Volume { get { return size.x * size.y * size.z; } }
        public bool Empty { get { return size.x == 0; } }

        public IVector3 Min { get { return position; } }
        public IVector3 Max { get { return position + size; } }

        public int MinX { get { return position.x; } }
        public int MinY { get { return position.y; } }
        public int MinZ { get { return position.z; } }

        public int MaxX { get { return position.x + size.x; } }
        public int MaxY { get { return position.y + size.y; } }
        public int MaxZ { get { return position.z + size.z; } }

        #endregion
        #region Constructors

        public IBound3(IVector3 position, IVector3 size)
        {
            this.position = position.Clone();
            this.size = size.Clone();
            CorrectSize();
        }

        public IBound3(int x, int y, int z, int sizeX, int sizeY, int sizeZ)
        {
            position = new IVector3(x, y, z);
            size = new IVector3(sizeX, sizeY, sizeZ);
            CorrectSize();
        }

        private void CorrectSize()
        {
            if (size.x < 0)
            {
                position.x += size.x;
                size.x *= -1;
            }
            if (size.y < 0)
            {
                position.y += size.y;
                size.y *= -1;
            }
            if (size.z < 0)
            {
                position.z += size.z;
                size.z *= -1;
            }
        }

        #endregion

        /// <summary>
        /// Determines whether or not this IBound3 contains the given IVector3
        /// </summary>
        /// <param name="point">Point.</param>
        public bool Contains(IVector3 point)
        {
            if (point.x >= position.x && point.x < position.x + size.x
             && point.y >= position.y && point.y < position.y + size.y
             && point.z >= position.z && point.z < position.z + size.z)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether or not this IBound3 entirely contains the given IBound3
        /// </summary>
        /// <param name="bound">Bound.</param>
        public bool Contains(IBound3 bound)
        {
            int oppositex = position.x + size.x;
            int oppositey = position.y + size.y;
            int oppositez = position.z + size.z;

            int cmpOppositex = bound.position.x + bound.size.x - 1;
            int cmpOppositey = bound.position.y + bound.size.y - 1;
            int cmpOppositez = bound.position.z + bound.size.z - 1;

            if (bound.position.x >= position.x && bound.position.x < oppositex
             && bound.position.y >= position.y && bound.position.y < oppositey
             && bound.position.z >= position.z && bound.position.z < oppositez

             && cmpOppositex >= position.x && cmpOppositex < oppositex
             && cmpOppositey >= position.y && cmpOppositey < oppositey
             && cmpOppositez >= position.z && cmpOppositez < oppositez)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether or not this IBound3 overlaps with the given IBound3
        /// </summary>
        /// <param name="bound">Bound.</param>
        public bool Overlaps(IBound3 bound)
        {
            if (Math.Min(position.x + size.x, bound.position.x + bound.size.x) - Math.Max(position.x, bound.position.x) > 0
             && Math.Min(position.y + size.y, bound.position.y + bound.size.y) - Math.Max(position.y, bound.position.y) > 0
             && Math.Min(position.z + size.z, bound.position.z + bound.size.z) - Math.Max(position.z, bound.position.z) > 0)
            {
                return true;
            }
            return false;
        }

        public bool IsAdjacentTo(IBound3 bound)
        {
            int border = 0;

            int diff = Math.Min(position.x + size.x, bound.position.x + bound.size.x) - Math.Max(position.x, bound.position.x);
            if (diff == 0)
            {
                border++;
            }
            else if (diff < 0)
            {
                // not within
                return false;
            }
            diff = Math.Min(position.y + size.y, bound.position.y + bound.size.y) - Math.Max(position.y, bound.position.y);
            if (diff == 0)
            {
                border++;
            }
            else if (diff < 0)
            {
                // not within
                return false;
            }

            diff = Math.Min(position.z + size.z, bound.position.z + bound.size.z) - Math.Max(position.z, bound.position.z);
            if (diff == 0)
            {
                border++;
            }
            else if (diff < 0)
            {
                // not within
                return false;
            }

            // exactly one face can be touching
            return border == 1;
        }

        /// <summary>
        /// Gets the overlapped bound.
        /// </summary>
        /// <returns>The overlapped bound.</returns>
        /// <param name="bound">Bound.</param>
        public IBound3 GetOverlappedBound(IBound3 bound)
        {
            IBound3 overlap = new IBound3();

            overlap.position.x = Math.Max(position.x, bound.position.x);
            overlap.position.y = Math.Max(position.y, bound.position.y);
            overlap.position.z = Math.Max(position.z, bound.position.z);

            overlap.size.x = Math.Min(position.x + size.x, bound.position.x + bound.size.x) - overlap.position.x;
            overlap.size.y = Math.Min(position.y + size.y, bound.position.y + bound.size.y) - overlap.position.y;
            overlap.size.z = Math.Min(position.z + size.z, bound.position.z + bound.size.z) - overlap.position.z;

            return overlap;
        }

        // not used
        public void GetOverlappedBound(ref IBound3 overlap)
        {
            int maxX = overlap.position.x + overlap.size.x;
            int maxY = overlap.position.y + overlap.size.y;
            int maxZ = overlap.position.z + overlap.size.z;

            overlap.position.x = Math.Max(position.x, overlap.position.x);
            overlap.position.y = Math.Max(position.y, overlap.position.y);
            overlap.position.z = Math.Max(position.z, overlap.position.z);

            overlap.size.x = Math.Min(position.x + size.x, maxX) - overlap.position.x;
            overlap.size.y = Math.Min(position.y + size.y, maxY) - overlap.position.y;
            overlap.size.z = Math.Min(position.z + size.z, maxZ) - overlap.position.z;
        }

        /// <summary>
        /// Encapsulate the specified bound. Grow the bounds to include the given IBound3.
        /// </summary>
        /// <param name="bound">Bound.</param>
        public void Encapsulate(IBound3 bound)
        {
            IVector3 oppositepos = position + size;
            IVector3 boundoppositepos = bound.position + bound.size;

            position.x = Math.Min(position.x, bound.position.x);
            position.y = Math.Min(position.y, bound.position.y);
            position.z = Math.Min(position.z, bound.position.z);

            size.x = Math.Max(oppositepos.x, boundoppositepos.x) - position.x;
            size.y = Math.Max(oppositepos.y, boundoppositepos.y) - position.y;
            size.z = Math.Max(oppositepos.z, boundoppositepos.z) - position.z;
        }

        public void CopyTo(IBound3 copy)
        {
            copy.position.x = position.x;
            copy.position.y = position.y;
            copy.position.z = position.z;

            copy.size.x = size.x;
            copy.size.y = size.y;
            copy.size.z = size.z;
        }
        public override string ToString()
        {
            return string.Format("[IBound3 pos:{0} size:{1}]", position.ToString(), size.ToString());
        }

        public IBound2 ToIBound2XZ()
        {
            return new IBound2(position.x, position.z, size.x, size.z);
        }
        public IBound3 Clone()
        {
            return new IBound3(position, size);
        }

        public static bool operator ==(IBound3 a, IBound3 b)
        {
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
            {
                return object.ReferenceEquals(a, null) == object.ReferenceEquals(b, null);
            }
            return (a.position.x == b.position.x && a.position.y == b.position.y && a.position.z == b.position.z
                && a.size.x == b.size.x && a.size.y == b.size.y && a.size.z == b.size.z);
        }
        public static bool operator !=(IBound3 a, IBound3 b)
        {
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
            {
                return object.ReferenceEquals(a, null) != object.ReferenceEquals(b, null);
            }
            return (a.position.x != b.position.x || a.position.y != b.position.y || a.position.z != b.position.z
                || a.size.x != b.size.x || a.size.y != b.size.y || a.size.z != b.size.z);
        }

        public override int GetHashCode()
        {
            return size.x ^ size.y ^ size.z ^ position.x ^ position.y ^ position.z;
        }
        public override bool Equals(System.Object obj)
        {
            return (IBound3) obj == this;
        }
    }
}