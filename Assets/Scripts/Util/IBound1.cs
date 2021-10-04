using System;

namespace Pincushion.LD49
{
    [Serializable]
    public struct IBound1
    {
        public int position;
        public int size;

        #region Properties

        public int Center
        {
            // note: since this is using integers, it is lossy
            get
            {
                return position + (size / 2);
            }
        }

        public int Length { get { return size; } }
        public bool Empty { get { return size == 0; } }

        public int Min { get { return position; } }
        public int Max { get { return position + size; } }

        public int MinX { get { return position; } }
        public int MaxX { get { return position + size; } }


        #endregion
        #region Constructors

        public IBound1(int position, int size)
        {
            this.position = position;
            this.size = size;
            CorrectSize();
        }

        private void CorrectSize()
        {
            if (size < 0)
            {
                position += size;
                size *= -1;
            }
        }

        #endregion

        /// <summary>
        /// Determines whether or not this IBound3 contains the given IVector3
        /// </summary>
        /// <param name="point">Point.</param>
        public bool Contains(int point)
        {
            if (point >= position && point < position + size)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether or not this IBound3 entirely contains the given IBound3
        /// </summary>
        /// <param name="bound">Bound.</param>
        public bool Contains(IBound1 bound)
        {
            int oppositex = position + size;
            int cmpOppositex = bound.position + bound.size - 1;

            if (bound.position >= position && bound.position < oppositex
             && cmpOppositex >= position && cmpOppositex < oppositex)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether or not this IBound3 overlaps with the given IBound3
        /// </summary>
        /// <param name="bound">Bound.</param>
        public bool Overlaps(IBound1 bound)
        {
            if (Math.Min(position + size, bound.position + bound.size) - Math.Max(position, bound.position) > 0)
            {
                return true;
            }
            return false;
        }

        public bool IsAdjacentTo(IBound1 bound)
        {
            int border = 0;

            int diff = Math.Min(position + size, bound.position + bound.size) - Math.Max(position, bound.position);
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
        public IBound1 GetOverlappedBound(IBound1 bound)
        {
            IBound1 overlap = new IBound1();

            overlap.position = Math.Max(position, bound.position);
            overlap.size = Math.Min(position + size, bound.position + bound.size) - overlap.position;

            return overlap;
        }

        /// <summary>
        /// Encapsulate the specified bound. Grow the bounds to include the given IBound3.
        /// </summary>
        /// <param name="bound">Bound.</param>
        public void Encapsulate(IBound1 bound)
        {
            int oppositepos = position + size;
            int boundoppositepos = bound.position + bound.size;

            position = Math.Min(position, bound.position);

            size = Math.Max(oppositepos, boundoppositepos) - position;
        }

        public IBound1[] Split(IBound1 bound)
        {
            if (Min < bound.Min && Max > bound.Max)
            {
                return new IBound1[] {
                    new IBound1(Min, bound.Min - Min),
                    new IBound1(bound.Max, Max - bound.Max)
                };
            }
            else if (Min < bound.Min && Max <= bound.Max)
            {
                return new IBound1[] {
                    new IBound1(Min, bound.Min - Min)
                };
            }
            else if (Min >= bound.Min && Max > bound.Max)
            {
                return new IBound1[] {
                    new IBound1(bound.Max, Max - bound.Max)
                };
            }
            return new IBound1[0];
        }

        public override string ToString()
        {
            return string.Format("[IBound3 pos:{0} size:{1}]", position.ToString(), size.ToString());
        }

        public IBound1 Clone()
        {
            return new IBound1(position, size);
        }

        public static bool operator ==(IBound1 a, IBound1 b)
        {
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
            {
                return object.ReferenceEquals(a, null) == object.ReferenceEquals(b, null);
            }
            return (a.position == b.position && a.size == b.size);
        }
        public static bool operator !=(IBound1 a, IBound1 b)
        {
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
            {
                return object.ReferenceEquals(a, null) != object.ReferenceEquals(b, null);
            }
            return (a.position != b.position || a.size != b.size);
        }

        public override int GetHashCode()
        {
            return size ^ position;
        }
        public override bool Equals(System.Object obj)
        {
            return (IBound1) obj == this;
        }
    }
}