using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD49
{
    public class PartController : MonoBehaviour
    {
        private SceneController scene;
        public Part part;

        public Material mainMaterial;
        public Material dimMaterial;
        public MeshRenderer meshRenderer;
        public Color original;

        public bool dimmed = false;
        public bool warning = false;
        public float hightlighting = 0f; // duration left

        public float initialTimeToLive = 30;
        public float warningTime = 15;

        public bool colourCoroutineRunning = false;
        private Coroutine colourCoroutine;

        public void Init(SceneController scene, Part part)
        {
            this.part = part;
            this.scene = scene;

            original = meshRenderer.material.color;

            initialTimeToLive = Random.Range(15, 60) / scene.stage.stage.decaySpeed;
            warningTime = 15f / scene.stage.stage.decaySpeed;
        }

        // Update is called once per frame
        void Update()
        {
            if (scene != null && !scene.Paused && !scene.editMode) // using a null scene for off-stage parts
            {
                if (part.IsPlaced)
                {
                    float timeLived = scene.Time - part.TimeBorn;
                    float timeLeft = initialTimeToLive - timeLived;
                    if (timeLeft <= 0)
                    {
                        // remove the part
                        scene.stage.RemovePart(part);
                    }
                    else if (!warning && timeLeft <= warningTime)
                    {
                        // start flashing / warn the player
                        // Change the material?
                        Warning(true);
                    }
                }
            }
        }

        IEnumerator ColourCoroutine()
        {
            float highlightStartTime = 0f;
            while (true) // this will be stopped externally
            {
                if (hightlighting > 0f)
                {
                    Color32 originalColour = original;
                    Color32 destinationColour = new Color(0xFF, 0xFF, 0x88, 0xFF);

                    meshRenderer.material.color = Color32.Lerp(originalColour, destinationColour, Mathf.PingPong(Time.time, 1));

                    if (highlightStartTime == 0)
                    {
                        highlightStartTime = scene.Time;
                    }
                    hightlighting -= scene.Time - highlightStartTime;
                }
                else if (warning)
                {
                    // warning transitions between original and full red.
                    Color32 originalColour = original;
                    Color32 destinationColour = new Color(0xFF, 0, 0, 0x99);

                    if (dimmed)
                    {
                        originalColour = GetDimmedColour(originalColour);
                        destinationColour = GetDimmedColour(destinationColour);
                    }

                    meshRenderer.material.color = Color32.Lerp(originalColour, destinationColour, Mathf.PingPong(Time.time, 1));
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        /// <summary>
        /// Quick sin
        /// https://gamedev.stackexchange.com/questions/4779/is-there-a-faster-sine-function
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public float Sin(float x)
        {
            const float B = 4 / Mathf.PI;
            const float C = -4 / (Mathf.PI * Mathf.PI);

            return -(B * x + C * x * ((x < 0) ? -x : x));
        }

        public void ShortHighlight()
        {
            hightlighting = 2f;
            UpdateColourCoroutine();
        }

        public void Warning(bool on)
        {
            if (on == warning)
            {
                return;
            }
            warning = on;
            UpdateColourCoroutine();
        }
        private void UpdateColourCoroutine()
        {
            if (warning || hightlighting > 0f)
            {
                if (!colourCoroutineRunning)
                {
                    colourCoroutineRunning = true;
                    colourCoroutine = StartCoroutine(ColourCoroutine());
                }
            }
            else
            {
                if (colourCoroutineRunning)
                {
                    colourCoroutineRunning = false;
                    StopCoroutine(colourCoroutine);
                }
                if (dimmed)
                {
                    Dim(true);
                }
                else // no effects. restore the original colour
                {
                    meshRenderer.material.color = original;
                }
            }
        }
      
        public void Dim(bool dim)
        {
            if (dim)
            {
                meshRenderer.material.color = GetDimmedColour(original);
            }
            else
            {
                meshRenderer.material.color = original;
            }
        }

        public Color GetDimmedColour(Color originalColour)
        {
            Color color = originalColour;
            color.a *= .25f;
            return color;
        }
    }
}