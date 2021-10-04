using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD49
{
    public class HighlightController : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        public Color32 destinationColour;

        private float lifetime = 2f;
        private Color original;
        private float startTime;
        private SceneController scene;

        public void Init(SceneController scene, int height)
        {
            this.scene = scene;
            transform.localScale = new Vector3(1, height, 1);
            original = meshRenderer.material.color;

            startTime = scene.Time;
        }

        private void Update()
        {
            Color32 originalColour = original;

            meshRenderer.material.color = Color32.Lerp(originalColour, destinationColour, Mathf.PingPong(scene.Time, 1));

            if ((scene.Time - startTime) > lifetime)
            {
                Destroy(gameObject);
            }
        }
    }
}