using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD49
{
    public class PartQueueController : MonoBehaviour
    {
        private SceneController scene;
        private StageController stage;

        public PartType nextPart;
        public PartType afterNextPart;

        private float lastPartTime = 0;
        private readonly float afterNextPartLoadTime = 5f;

        public float AfterNextPartTimeRemaining
        {
            get
            {
                float timeRemaining =  afterNextPartLoadTime - (scene.Time - lastPartTime);
                return timeRemaining < 0 ? 0 : timeRemaining;
            }
        }

        public void Init(SceneController scene)
        {
            this.scene = scene;
            stage = scene.stage;

            nextPart = GetRandomPartType();
            afterNextPart = GetRandomPartType();
        }

        public void Reset()
        {
            // not sure if anything is needed
        }

        private void Update()
        {
            if (!scene.Paused && scene.stage.stage != null)
            {
                if (afterNextPart == null && AfterNextPartTimeRemaining == 0)
                {
                    stage.RefreshQueue(); // to clear the timer
                    afterNextPart = GetRandomPartType();
                    stage.RefreshQueue();
                }
                else if (AfterNextPartTimeRemaining > 0)
                {
                    stage.RefreshQueue();
                }
            }
        }

        public PartType GetRandomPartType()
        {
            int partIndex = Random.Range(0, stage.partTypes.types.Length);
            return stage.partTypes.types[partIndex];
        }

        public PartType GetNextPart()
        {
            PartType part = nextPart;

            nextPart = afterNextPart == null ? GetRandomPartType() : afterNextPart;
            afterNextPart = null;
            lastPartTime = scene.Time;

            return part;
        }

        public bool ReturnPart(Part part)
        {
            if (AfterNextPartTimeRemaining > 0)
            {
                afterNextPart = nextPart;
                nextPart = part.Type;
            }

            // Time has elapsed. Can't undo.
            return false;
        }
    }
}