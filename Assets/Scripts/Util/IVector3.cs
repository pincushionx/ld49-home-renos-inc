using System;
using UnityEngine;

namespace Pincushion.LD49
{
	[Serializable]
	public struct IVector3
	{
		public int x,y,z;

        public static readonly IVector3 zero = new IVector3(0, 0, 0);
        public static readonly IVector3 one = new IVector3(1, 1, 1);

        public static readonly IVector3 up = new IVector3(0, 1, 0);
        public static readonly IVector3 down = new IVector3(0, -1, 0);
        public static readonly IVector3 left = new IVector3(-1, 0, 0);
        public static readonly IVector3 right = new IVector3(1, 0, 0);
        public static readonly IVector3 front = new IVector3(0, 0, 1);
        public static readonly IVector3 back = new IVector3(0, 0, -1);

        public IVector3(int x, int y, int z)
	    {
	        this.x = x;
	        this.y = y;
	        this.z = z;
	    }

		public IVector3(Vector3 vec)
		{
			this.x = (int) Math.Floor(vec.x);
			this.y = (int) Math.Floor(vec.y);
			this.z = (int) Math.Floor(vec.z);
		}

        /// <summary>
        /// Pseudo-operator. Used to add without allocating another class.
        /// </summary>
        /// <param name="b">IVector3 to add</param>
        public void add(IVector3 b)
        {
            x += b.x;
            y += b.y;
            z += b.z;
        }

        /// <summary>
        /// Pseudo-operator. Used to subtract without allocating another class.
        /// </summary>
        /// <param name="b">IVector3 to subtract</param>
        public void subtract(IVector3 b)
        {
            x -= b.x;
            y -= b.y;
            z -= b.z;
        }

		public static IVector3 operator +(IVector3 a, IVector3 b)
	    {
			return new IVector3(a.x + b.x, a.y + b.y, a.z + b.z);
	    }

		public static IVector3 operator +(IVector3 a, int b)
		{
			return new IVector3(a.x + b, a.y + b, a.z + b);
		}

        public static IVector3 operator -(IVector3 a, IVector3 b)
	    {
			return new IVector3(a.x - b.x, a.y - b.y, a.z - b.z);
	    }

		public static IVector3 operator -(IVector3 a, int b)
		{
			return new IVector3(a.x - b, a.y - b, a.z - b);
		}

		public static IVector3 operator /(IVector3 v, int divisor)
		{
			return new IVector3((int) v.x/divisor, (int) v.y/divisor, (int) v.z/divisor);
		}

		public static IVector3 operator *(IVector3 v, IVector3 multiplier)
		{
			return new IVector3(multiplier.x * v.x, multiplier.y * v.y, multiplier.z * v.z);
		}
		public static IVector3 operator *(IVector3 v, int multiplier)
		{
			return new IVector3(multiplier * v.x, multiplier * v.y, multiplier * v.z);
		}

		public static Vector3 operator *(float multiplier, IVector3 v)
	    {
	        return new Vector3(multiplier * v.x, multiplier * v.y, multiplier * v.z);
	    }

		public static Vector3 operator *(IVector3 v, float multiplier)
	    {
	        return new Vector3(multiplier * v.x, multiplier * v.y, multiplier * v.z);
	    }
			
		public static bool operator >=(IVector3 a, IVector3 b) {
			return a.x >= b.x && a.y >= b.y && a.z >= b.z;
		}

		public static bool operator <=(IVector3 a, IVector3 b) {
			return a.x <= b.x && a.y <= b.y && a.z <= b.z;
		}


		public static bool operator >(IVector3 a, IVector3 b) {
			return a.x > b.x && a.y > b.y && a.z > b.z;
		}
		public static bool operator <(IVector3 a, IVector3 b) {
			return a.x < b.x && a.y < b.y && a.z < b.z;
		}

		public static bool operator ==(IVector3 a, IVector3 b) {
			if(object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) {
				return object.ReferenceEquals(a, null) == object.ReferenceEquals(b, null);
			}
			return a.x == b.x && a.y == b.y && a.z == b.z;
		}
		public static bool operator !=(IVector3 a, IVector3 b) {
			if(object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) {
				return object.ReferenceEquals(a, null) != object.ReferenceEquals(b, null);
			}
			return a.x != b.x || a.y != b.y || a.z != b.z;
		}

        public float DistanceTo(IVector3 v)
        {
            return (float) Math.Sqrt(Math.Pow(x - v.x, 2) + Math.Pow(y - v.y, 2) + Math.Pow(z - v.z, 2));
        }

        public override bool Equals(System.Object obj) {
			return (IVector3) obj == this;
		}

		public override int GetHashCode ()
		{
			return x ^ y ^ z;
		}

		public Vector3 ToVector3() {
			return new Vector3 ((float) x, (float) y, (float) z);
		}
		public IVector2 ToVector2XZ()
		{
			return new IVector2(x, z);
		}

		public void CopyTo(IVector3 copy)
        {
            copy.x = x;
            copy.y = y;
            copy.z = z;
        }
		public IVector3 Clone() {
			return new IVector3 (x, y, z);
		}
		public override string ToString ()
		{
			return string.Format ("[IVector3 {0},{1},{2}]", x, y, z);
		}
	}
}