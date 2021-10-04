using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD49
{
    public interface IPartition
    {
        string Name { get; }
        IBound2[] Shape { get; set; }

        bool Overlaps(IBound2[] shape);
        bool Overlaps(IBound2 shape);
        void Simplify();
    }
}