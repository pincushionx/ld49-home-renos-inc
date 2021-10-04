using System;

namespace Pincushion.LD49
{
    [Serializable]
    public struct IBound2
    {
        public IVector2 position;
        public IVector2 size;

        #region Properties

        public IVector2 Center
        {
            // note: since this is using integers, it is lossy
            get
            {
                return new IVector2(
                    position.x + (size.x / 2),
                    position.y + (size.y / 2)
                );
            }
        }
        public float Ratio
        {
            get
            {
                if (size.x == 0 || size.y == 0) return float.MaxValue;

                float ratio = size.x / (float) size.y;
                if (ratio >= 1f) return ratio;
                return size.y / (float) size.x;
            }
        }

        public int Area { get { return size.x * size.y; } }
        public bool Empty { get { return size.x == 0; } }

        public IVector2 Min { get { return position; } }
        public IVector2 Max { get { return position + size; } }

        public int MinX { get { return position.x; } }
        public int MinY { get { return position.y; } }

        public int MaxX { get { return position.x + size.x; } }
        public int MaxY { get { return position.y + size.y; } }

        #endregion
        #region Constructors

        public IBound2(IVector2 position, IVector2 size)
        {
            this.position = position.Clone();
            this.size = size.Clone();
            CorrectSize();
        }

        public IBound2(int x, int y,int sizeX, int sizeY)
        {
            position = new IVector2(x, y);
            size = new IVector2(sizeX, sizeY);
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
        }

        #endregion

        /// <summary>
        /// Determines whether or not this IBound3 contains the given IVector3
        /// </summary>
        /// <param name="point">Point.</param>
        public bool Contains(IVector2 point)
        {
            if (point.x >= position.x && point.x < position.x + size.x
             && point.y >= position.y && point.y < position.y + size.y)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether or not this IBound3 entirely contains the given IBound3
        /// </summary>
        /// <param name="bound">Bound.</param>
        public bool Contains(IBound2 bound)
        {
            int oppositex = position.x + size.x;
            int oppositey = position.y + size.y;

            int cmpOppositex = bound.position.x + bound.size.x - 1;
            int cmpOppositey = bound.position.y + bound.size.y - 1;

            if (bound.position.x >= position.x && bound.position.x < oppositex
             && bound.position.y >= position.y && bound.position.y < oppositey

             && cmpOppositex >= position.x && cmpOppositex < oppositex
             && cmpOppositey >= position.y && cmpOppositey < oppositey)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether or not this IBound3 overlaps with the given IBound3
        /// </summary>
        /// <param name="bound">Bound.</param>
        public bool Overlaps(IBound2 bound)
        {
            if (Math.Min(position.x + size.x, bound.position.x + bound.size.x) - Math.Max(position.x, bound.position.x) > 0
             && Math.Min(position.y + size.y, bound.position.y + bound.size.y) - Math.Max(position.y, bound.position.y) > 0)
            {
                return true;
            }
            return false;
        }

        public bool IsAdjacentTo(IBound2 bound)
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
            // exactly one face can be touching
            return border == 1;
        }

        /// <summary>
        /// Gets the overlapped bound.
        /// </summary>
        /// <returns>The overlapped bound.</returns>
        /// <param name="bound">Bound.</param>
        public IBound2 GetOverlappedBound(IBound2 bound)
        {
            IBound2 overlap = new IBound2();

            overlap.position.x = Math.Max(position.x, bound.position.x);
            overlap.position.y = Math.Max(position.y, bound.position.y);

            overlap.size.x = Math.Min(position.x + size.x, bound.position.x + bound.size.x) - overlap.position.x;
            overlap.size.y = Math.Min(position.y + size.y, bound.position.y + bound.size.y) - overlap.position.y;

            return overlap;
        }

        public float Distance(IBound2 bound)
        {
            bool left = bound.MaxX < MinX;
            bool right = MaxX < bound.MinX;
            bool bottom = bound.MaxY < MinY;
            bool top = MaxY < bound.MinY;

            float x = (left) ? MinX - bound.MaxX : bound.MinX - MaxX;
            float y = (bottom) ? MinY - bound.MaxY : bound.MinY - MaxY;

            if (top && left
             || bottom && left
             || bottom && right
             || top && right)
            {
                return (float) Math.Sqrt(x * x + y * y);
            }
            else if (left || right || top || bottom)
            {
                return Math.Abs(x + y);
            }
            // intersect
            return 0;
        }

        /// <summary>
        /// Encapsulate the specified bound. Grow the bounds to include the given IBound3.
        /// </summary>
        /// <param name="bound">Bound.</param>
        public void Encapsulate(IBound2 bound)
        {
            IVector2 oppositepos = position + size;
            IVector2 boundoppositepos = bound.position + bound.size;

            position.x = Math.Min(position.x, bound.position.x);
            position.y = Math.Min(position.y, bound.position.y);

            size.x = Math.Max(oppositepos.x, boundoppositepos.x) - position.x;
            size.y = Math.Max(oppositepos.y, boundoppositepos.y) - position.y;
        }

        public override string ToString()
        {
            return string.Format("[IBound2 pos:{0} size:{1}]", position.ToString(), size.ToString());
        }
        public IBound1 ToIBound1X()
        {
            return new IBound1(position.x, size.x);
        }
        public IBound1 ToIBound1Y()
        {
            return new IBound1(position.y, size.y);
        }





        public static int CompareByPosX(IBound2 a, IBound2 b)
        {
            if (a.position.x < b.position.x)
            {
                return -1;
            }
            if (a.position.x > b.position.x)
            {
                return 1;
            }
            return 0;
        }

        public static int CompareByPosY(IBound2 a, IBound2 b)
        {
            if (a.position.y < b.position.y)
            {
                return -1;
            }
            if (a.position.y > b.position.y)
            {
                return 1;
            }
            return 0;
        }





        public IBound2 Clone()
        {
            return new IBound2(position, size);
        }

        public static bool operator ==(IBound2 a, IBound2 b)
        {
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
            {
                return object.ReferenceEquals(a, null) == object.ReferenceEquals(b, null);
            }
            return (a.position.x == b.position.x && a.position.y == b.position.y
                && a.size.x == b.size.x && a.size.y == b.size.y);
        }
        public static bool operator !=(IBound2 a, IBound2 b)
        {
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
            {
                return object.ReferenceEquals(a, null) != object.ReferenceEquals(b, null);
            }
            return (a.position.x != b.position.x || a.position.y != b.position.y
                || a.size.x != b.size.x || a.size.y != b.size.y);
        }

        public override int GetHashCode()
        {
            return size.x ^ size.y ^ position.x ^ position.y;
        }
        public override bool Equals(System.Object obj)
        {
            return (IBound2) obj == this;
        }
    }
}