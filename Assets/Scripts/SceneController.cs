using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Pincushion.LD49
{
    public class SceneController : MonoBehaviour
    {
        public OverlayController overlay;
        public StageController stage;
        public PartQueueController partQueue;
        public CameraController cameraController;

        private SoundController sound;
        private ControlsController controls;

        public SoundController Sound { get { return sound; } }
        public ControlsController Controls { get { return controls; } }
        public OverlayController Overlay { get { return overlay; } }

        private int currentLevel = 0;
        private float time;

        public bool editMode = false;

        private bool paused = false;
        public bool Paused {
            get
            {
                return paused;
            }
            set
            {
                paused = value;
            }
        }
        public float Time { get { return time; } }

        public float TimeLeftThisLevel { get { return time; } }

        private void Awake()
        {
            sound = GetComponentInChildren<SoundController>();

            controls = GetComponentInChildren<ControlsController>();
            controls.Init(this);

            partQueue = GetComponent<PartQueueController>();
            partQueue.Init(this);
        }

        private void Start()
        {
            if (editMode)
            {
                LoadLevel(1000);
            }
            else
            {
                LoadTutorial();
            }
        }

        private void Update()
        {
            if (editMode && Input.GetKeyDown(KeyCode.F))
            {
                WriteStage("Stage", stage.stage);
            }

            if (!editMode && !Paused && stage.stage != null && stage.stage.loaded)
            {
                Stage stageData = stage.stage;
                int timeElapsed = Mathf.CeilToInt(Time - stageData.timeStart);
                int timeLeft = stageData.timeLimit - timeElapsed;
                if (timeLeft < 0)
                {
                    // Level is done. Player wins
                    WinCondition();
                    timeLeft = 0;
                }



                overlay.UpdateTimer(timeLeft);
                time += UnityEngine.Time.deltaTime;
            }
        }

        public void NextLevel()
        {
            if (currentLevel < stage.levels.NumberOfStages)
            {
                currentLevel++;
                LoadLevel(currentLevel);
                Debug.Log("Loaded level:" + currentLevel);
                if (currentLevel == 1)
                {
                    // Decay 0.5, time 120s, 1f
                    overlay.ShowMessage("It looks like you've got the hang of things. It's time to work on more difficult tasks. This one decays twice as fast as the last, but only has one floor.");
                }
                if (currentLevel == 2)
                {
                    // Decay 0.5, time 120s, 2f
                    overlay.ShowMessage("That's pretty stable. C'mon we have more work to do! We've got another two floor house to fix up.");
                }
                if (currentLevel == 3)
                {
                    // Decay 1, time 90s, 1f
                    overlay.ShowMessage("This one's just one floor, but its got termites. It decays really fast. Let's get to it!");
                }
                if (currentLevel == 4)
                {
                    // Decay 1.5, time 90s, 1f
                    overlay.ShowMessage("This one's built horribly. Did we build it? Anyway, work fast!");
                }
                if (currentLevel == 5)
                {
                    // Decay 1.25, time 90s, 1f
                    overlay.ShowMessage("Your reputation precedes you. Our client asked specifically for your handywork! It's a tall one, get ready!");
                }

            }
            else
            {
                // End screen
                overlay.ShowMessage("That's all. Thanks for playing!");
            }
        }

        public void LoadLevel(int level)
        {
            stage.LoadStage(level);
        }

        public void WinCondition()
        {
            Paused = true;

            // Show... something
            overlay.ShowMessage("That's pretty stable. C'mon we have more work to do!");

            // move to next level
            NextLevel();

            Paused = false;
        }

        public void LoseCondition()
        {
            if (!editMode)
            {
                Paused = true;

                overlay.LoseConditionMessage();

                Paused = false;
            }
        }

        public void LoadTutorial()
        {
            Paused = true;

            // clear and restart the stage
            stage.LoadStage(900);

            // Resume the game
            Paused = false;
        }

        public void RestartLevel()
        {
            Paused = true;

            // clear and restart the stage
            stage.LoadStage(currentLevel);

            // Resume the game
            Paused = false;
        }



        public void WriteStage(string name, Stage stage)
        {
            string contents = "";
            /*                floor{floorIndex} = new Floor();
                floor{floorIndex}.floor = {floorIndex};
                floor{floorIndex}.floorTiles = floorTiles;
                floor{floorIndex}.height = 2;
                floor{floorIndex}.parts = new HashSet<Part>();*/

            for (int floorIndex = 0; floorIndex < stage.floors.Length; floorIndex++)
            {
                Floor floor = stage.floors[floorIndex];

                contents += $@"
            // Floor {floorIndex}
            floorTiles = new Dictionary<IVector2, FloorTile>();

            for (int y = 0; y < 12; y++)
            {{
                for (int x = 0; x < 12; x++)
                {{
                    floorTiles.Add(new IVector2(x, y), new FloorTile(x, y));
                }}
            }}

            floor = new Floor();
            floor.floor = {floorIndex};
            floor.floorTiles = floorTiles;
            floor.height = {floor.height};
            floor.parts = new HashSet<Part>();
            floor.Init();
            floors.Add(floor);

                    ";

               int partIndex = 0;
                foreach (Part part in floor.parts)
                {
                    contents += $@"
                        part = new Part();
                        part.Type = partTypes.GetPartType(""{part.Type.name}"");
                        part.Position = new IVector3({part.Position.x}, {part.Position.y}, {part.Position.z});
                        part.Rotation = new IVector3({part.Rotation.x}, {part.Rotation.y}, {part.Rotation.z});
                        part.TimeBorn = scene.Time;
                        floor.AddPart(part);
                    ";
                    partIndex++;
                }
            }

            string path = Path.Combine(Application.streamingAssetsPath + Path.DirectorySeparatorChar + "Levels", name) + ".txt";

            if (File.Exists(path))
            {
                int suffix = 0;
                while (File.Exists(path))
                {
                    path = Path.Combine(Application.streamingAssetsPath + Path.DirectorySeparatorChar + "Levels", name + suffix.ToString().PadLeft(2, '0')) + ".txt";
                    suffix++;
                }
            }
            File.WriteAllText(path, contents);
        }
    }
}