using System;
using UnityEngine;

namespace Pincushion.LD49
{
	[Serializable]
	public struct IVector2
	{
		public int x,y;

		public IVector2(int x, int y)
	    {
	        this.x = x;
	        this.y = y;
	    }

		public IVector2(Vector2 vec)
		{
			this.x = (int) Math.Floor(vec.x);
			this.y = (int) Math.Floor(vec.y);
		}

		public static IVector2 operator +(IVector2 a, IVector2 b)
	    {
			return new IVector2(a.x + b.x, a.y + b.y);
	    }

		public static IVector2 operator +(IVector2 a, int b)
		{
			return new IVector2(a.x + b, a.y + b);
		}

		public static IVector2 operator -(IVector2 a, IVector2 b)
	    {
			return new IVector2(a.x - b.x, a.y - b.y);
	    }

		public static IVector2 operator -(IVector2 a, int b)
		{
			return new IVector2(a.x - b, a.y - b);
		}

		public static IVector2 operator /(IVector2 v, int divisor)
		{
			return new IVector2((int) v.x/divisor, (int) v.y/divisor);
		}

		public static IVector2 operator *(IVector2 v, IVector2 multiplier)
		{
			return new IVector2(multiplier.x * v.x, multiplier.y * v.y);
		}
		public static IVector2 operator *(IVector2 v, int multiplier)
		{
			return new IVector2(multiplier * v.x, multiplier * v.y);
		}

		public static Vector3 operator *(float multiplier, IVector2 v)
	    {
	        return new Vector3(multiplier * v.x, multiplier * v.y);
	    }

		public static Vector3 operator *(IVector2 v, float multiplier)
	    {
	        return new Vector3(multiplier * v.x, multiplier * v.y);
	    }
			
		public static bool operator >=(IVector2 a, IVector2 b) {
			return a.x >= b.x && a.y >= b.y;
		}

		public static bool operator <=(IVector2 a, IVector2 b) {
			return a.x <= b.x && a.y <= b.y;
		}


		public static bool operator >(IVector2 a, IVector2 b) {
			return a.x > b.x && a.y > b.y;
		}
		public static bool operator <(IVector2 a, IVector2 b) {
			return a.x < b.x && a.y < b.y;
		}

		public static bool operator ==(IVector2 a, IVector2 b) {
			if(object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) {
				return object.ReferenceEquals(a, null) == object.ReferenceEquals(b, null);
			}
			return a.x == b.x && a.y == b.y;
		}
		public static bool operator !=(IVector2 a, IVector2 b) {
			if(object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) {
				return object.ReferenceEquals(a, null) != object.ReferenceEquals(b, null);
			}
			return a.x != b.x || a.y != b.y;
		}

        public float DistanceTo(IVector2 v)
        {
            return (float) Math.Sqrt(Math.Pow(x - v.x, 2) + Math.Pow(y - v.y, 2));
        }

        public override bool Equals(System.Object obj) {
			return (IVector2) obj == this;
		}

		public override int GetHashCode ()
		{
			return x ^ y;
		}

		public Vector2 ToVector2() {
			return new Vector2 ((float) x, (float) y);
		}
			
		public IVector2 Clone() {
			return new IVector2 (x, y);
		}
		public override string ToString ()
		{
			return string.Format ("[IVector2 {0},{1}]", x, y);
		}
	}
}