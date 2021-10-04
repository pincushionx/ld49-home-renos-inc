using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD49
{
    public class FloorTileController : MonoBehaviour
    {
        public Material mainMaterial;
        public Material dimMaterial;
        public MeshRenderer meshRenderer;

        public bool IsInner = false;

        public void Dim(bool dim)
        {
            if (dim)
            {
                meshRenderer.material = dimMaterial;
            }
            else
            {
                meshRenderer.material = mainMaterial;
            }
        }
    }
}