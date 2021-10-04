using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD49
{
    public class Partition : IPartition
    {
        public string name;
        public IBound2[] shape;

        public string Name { get { return name; } }
        public IBound2[] Shape
        {
            get { return shape; }
            set { shape = value; }
        }

        public Partition(string name, IBound2[] shape)
        {
            this.name = name;
            this.shape = shape;
        }

        public bool Overlaps(IBound2[] compareShape)
        {
            for (int i = 0; i < compareShape.Length; i++)
            {
                if (Overlaps(compareShape[i]))
                {
                    return true;
                }
            }
            return false;
        }
        public bool Overlaps(IBound2 bound)
        {
            for (int i = 0; i < shape.Length; i++)
            {
                if (shape[i].Overlaps(bound))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsAdjacentTo(IBound2 bound)
        {
            for (int i = 0; i < shape.Length; i++)
            {
                if (shape[i].IsAdjacentTo(bound))
                {
                    return true;
                }
            }
            return false;
        }

        public void Simplify()
        {
            shape = ShapeUtil.SimplifyShape(shape);
        }

        public override string ToString()
        {
            string shapeString = "";

            foreach (IBound2 bound in Shape)
            {
                shapeString += bound.ToString();
            }

            return name + " " + shapeString;
        }
    }
}