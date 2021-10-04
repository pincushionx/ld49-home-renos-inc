using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD49
{
    public class LevelConfig
    {
        public int NumberOfStages { get { return 5; } }
        private SceneController scene;
        private StageController stage;
        private PartTypeManager partTypes;

        public void Init(StageController stage)
        {
            this.stage = stage;
            scene = stage.scene;
            partTypes = stage.partTypes;
        }

        public Stage GetStage(int stage)
        {
            if (stage == 1)
            {
                return Stage1();
            }
            if (stage == 2)
            {
                return Stage2();
            }
            if (stage == 3)
            {
                return Stage3();
            }
            if (stage == 4)
            {
                return Stage4();
            }
            if (stage == 5)
            {
                return Stage5();
            }
            if (stage == 900)
            {
                return StageLevelTutorial();
            }
            if (stage == 1000)
            {
                return StageLevelEdit();
            }

            Debug.LogError("The stage " + stage + " doesn't exist");
            return null;
        }


        public Stage StageLevelTutorial()
        {
            Part part;
            List<Floor> floors = new List<Floor>();
            Floor floor;
            Dictionary<IVector2, FloorTile> floorTiles;














            // Floor 0
            floorTiles = new Dictionary<IVector2, FloorTile>();

            for (int y = 0; y < 12; y++)
            {
                for (int x = 0; x < 12; x++)
                {
                    floorTiles.Add(new IVector2(x, y), new FloorTile(x, y));
                }
            }

            floor = new Floor();
            floor.floor = 0;
            floor.floorTiles = floorTiles;
            floor.height = 4;
            floor.parts = new HashSet<Part>();
            floor.Init();
            floors.Add(floor);


            part = new Part();
            part.Type = partTypes.GetPartType("2x4 Flowerpot");
            part.Position = new IVector3(12, 1, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(6, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x3 Door");
            part.Position = new IVector3(6, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(6, 4, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(7, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("3x4 SCubes");
            part.Position = new IVector3(8, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("3x4 SCubes");
            part.Position = new IVector3(4, 1, 12);
            part.Rotation = new IVector3(0, 2, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(12, 2, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(4, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(0, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(11, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(12, 4, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            // Floor 1
            floorTiles = new Dictionary<IVector2, FloorTile>();

            for (int y = 0; y < 12; y++)
            {
                for (int x = 0; x < 12; x++)
                {
                    floorTiles.Add(new IVector2(x, y), new FloorTile(x, y));
                }
            }

            floor = new Floor();
            floor.floor = 1;
            floor.floorTiles = floorTiles;
            floor.height = 4;
            floor.parts = new HashSet<Part>();
            floor.Init();
            floors.Add(floor);


            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(1, 1, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("3x4 SCubes");
            part.Position = new IVector3(1, 1, 8);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(11, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(6, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(0, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(6, 3, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(11, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(12, 1, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(12, 1, 8);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(11, 1, 4);
            part.Rotation = new IVector3(0, 1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(12, 3, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(0, 3, 5);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);









            Stage stage = new Stage();
            stage.timeLimit = 120;
            stage.decaySpeed = 0.2f;
            stage.floors = floors.ToArray();

            return stage;
        }

        public Stage StageLevelEdit()
        {
            List<Floor> floors = new List<Floor>();
            Part part;
            Floor floor;
            Dictionary<IVector2, FloorTile> floorTiles = new Dictionary<IVector2, FloorTile>();

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    floorTiles.Add(new IVector2(x, y), new FloorTile(x, y));
                }
            }


            floor = new Floor();
            floor.floor = 0;
            floor.floorTiles = floorTiles;
            floor.height = 4;
            floor.parts = new HashSet<Part>();
            floor.Init();
            floors.Add(floor);






            floorTiles = new Dictionary<IVector2, FloorTile>();

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    floorTiles.Add(new IVector2(x, y), new FloorTile(x, y));
                }
            }

            floor = new Floor();
            floor.floor = 1;
            floor.floorTiles = floorTiles;
            floor.height = 4;
            floor.parts = new HashSet<Part>();
            floor.Init();
            floors.Add(floor);

            floorTiles = new Dictionary<IVector2, FloorTile>();


            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    floorTiles.Add(new IVector2(x, y), new FloorTile(x, y));
                }
            }

            floor = new Floor();
            floor.floor = 2;
            floor.floorTiles = floorTiles;
            floor.height = 4;
            floor.parts = new HashSet<Part>();
            floor.Init();
            floors.Add(floor);



            Stage stage = new Stage();
            stage.timeLimit = 100000;
            stage.floors = floors.ToArray();

            return stage;
        }

        public Stage Stage1()
        {
            Floor floor;
            Part part;
            Dictionary<IVector2, FloorTile> floorTiles = new Dictionary<IVector2, FloorTile>();

            for (int y = 0; y < 12; y++)
            {
                for (int x = 0; x < 12; x++)
                {
                    floorTiles.Add(new IVector2(x, y), new FloorTile(x, y));
                }
            }


            floor = new Floor();
            floor.floor = 0;
            floor.floorTiles = floorTiles;
            floor.height = 4;
            floor.parts = new HashSet<Part>();
            floor.Init();









            part = new Part();
            part.Type = partTypes.GetPartType("2x3 Door");
            part.Position = new IVector3(6, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(7, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(11, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x4 Flowerpot");
            part.Position = new IVector3(12, 1, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(12, 2, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(6, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x4 Flowerpot");
            part.Position = new IVector3(0, 1, 6);
            part.Rotation = new IVector3(0, 1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(5, 1, 1);
            part.Rotation = new IVector3(0, 2, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(1, 2, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Hook");
            part.Position = new IVector3(11, 3, 7);
            part.Rotation = new IVector3(0, 1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(0, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(6, 3, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Hook");
            part.Position = new IVector3(12, 3, 5);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(0, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(1, 3, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(11, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Hook");
            part.Position = new IVector3(4, 2, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(4, 4, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(3, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(10, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(9, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("3x4 SCubes");
            part.Position = new IVector3(9, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(2, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(1, 1, 10);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(1, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(8, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(8, 2, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(9, 2, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(9, 4, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(2, 2, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);








            Stage stage = new Stage();
            stage.decaySpeed = 0.25f;
            stage.timeLimit = 120;
            stage.floors = new Floor[] { floor };

            return stage;
        }








        public Stage Stage2()
        {
            Part part;
            List<Floor> floors = new List<Floor>();
            Floor floor;
            Dictionary<IVector2, FloorTile> floorTiles;














            // Floor 0
            floorTiles = new Dictionary<IVector2, FloorTile>();

            for (int y = 0; y < 12; y++)
            {
                for (int x = 0; x < 12; x++)
                {
                    floorTiles.Add(new IVector2(x, y), new FloorTile(x, y));
                }
            }

            floor = new Floor();
            floor.floor = 0;
            floor.floorTiles = floorTiles;
            floor.height = 4;
            floor.parts = new HashSet<Part>();
            floor.Init();
            floors.Add(floor);


            part = new Part();
            part.Type = partTypes.GetPartType("2x4 Flowerpot");
            part.Position = new IVector3(12, 1, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(6, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x3 Door");
            part.Position = new IVector3(6, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(6, 4, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(7, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("3x4 SCubes");
            part.Position = new IVector3(8, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("3x4 SCubes");
            part.Position = new IVector3(4, 1, 12);
            part.Rotation = new IVector3(0, 2, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(12, 2, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(4, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(0, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(11, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(12, 4, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            // Floor 1
            floorTiles = new Dictionary<IVector2, FloorTile>();

            for (int y = 0; y < 12; y++)
            {
                for (int x = 0; x < 12; x++)
                {
                    floorTiles.Add(new IVector2(x, y), new FloorTile(x, y));
                }
            }

            floor = new Floor();
            floor.floor = 1;
            floor.floorTiles = floorTiles;
            floor.height = 4;
            floor.parts = new HashSet<Part>();
            floor.Init();
            floors.Add(floor);


            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(1, 1, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("3x4 SCubes");
            part.Position = new IVector3(1, 1, 8);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(11, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(6, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(0, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(6, 3, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(11, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(12, 1, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(12, 1, 8);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(11, 1, 4);
            part.Rotation = new IVector3(0, 1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(12, 3, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(0, 3, 5);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);









            Stage stage = new Stage();
            stage.timeLimit = 120;
            stage.decaySpeed = 0.5f;
            stage.floors = floors.ToArray();

            return stage;
        }









        public Stage Stage3()
        {
            Floor floor;
            Part part;
            Dictionary<IVector2, FloorTile> floorTiles = new Dictionary<IVector2, FloorTile>();

            for (int y = 0; y < 12; y++)
            {
                for (int x = 0; x < 12; x++)
                {
                    floorTiles.Add(new IVector2(x, y), new FloorTile(x, y));
                }
            }


            floor = new Floor();
            floor.floor = 0;
            floor.floorTiles = floorTiles;
            floor.height = 4;
            floor.parts = new HashSet<Part>();
            floor.Init();









            part = new Part();
            part.Type = partTypes.GetPartType("2x3 Door");
            part.Position = new IVector3(6, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(7, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(11, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x4 Flowerpot");
            part.Position = new IVector3(12, 1, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(12, 2, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(6, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x4 Flowerpot");
            part.Position = new IVector3(0, 1, 6);
            part.Rotation = new IVector3(0, 1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(5, 1, 1);
            part.Rotation = new IVector3(0, 2, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(1, 2, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Hook");
            part.Position = new IVector3(11, 3, 7);
            part.Rotation = new IVector3(0, 1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(0, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(6, 3, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Hook");
            part.Position = new IVector3(12, 3, 5);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(0, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(1, 3, 6);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(11, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Hook");
            part.Position = new IVector3(4, 2, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(4, 4, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(3, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(10, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(9, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("3x4 SCubes");
            part.Position = new IVector3(9, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(2, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(1, 1, 10);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(1, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(8, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(8, 2, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(9, 2, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(9, 4, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(2, 2, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);








            Stage stage = new Stage();
            stage.decaySpeed = 1f;
            stage.timeLimit = 90;
            stage.floors = new Floor[] { floor };

            return stage;
        }


        public Stage Stage4()
        {
            Floor floor;
            Part part;
            Dictionary<IVector2, FloorTile> floorTiles = new Dictionary<IVector2, FloorTile>();


            // Floor 0
            floorTiles = new Dictionary<IVector2, FloorTile>();

            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    floorTiles.Add(new IVector2(x, y), new FloorTile(x, y));
                }
            }
            for (int y = 10; y < 20; y++)
            {
                for (int x = 10; x < 16; x++)
                {
                    floorTiles.Add(new IVector2(x, y), new FloorTile(x, y));
                }
            }

            floor = new Floor();
            floor.floor = 0;
            floor.floorTiles = floorTiles;
            floor.height = 4;
            floor.parts = new HashSet<Part>();
            floor.Init();


            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(0, 1, 8);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(15, 1, 19);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(14, 1, 19);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(13, 2, 19);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(11, 1, 20);
            part.Rotation = new IVector3(0, 2, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Hook");
            part.Position = new IVector3(1, 1, 19);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(0, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Hook");
            part.Position = new IVector3(0, 1, 10);
            part.Rotation = new IVector3(0, 1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(0, 1, 9);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Hook");
            part.Position = new IVector3(3, 1, 20);
            part.Rotation = new IVector3(0, 2, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Hook");
            part.Position = new IVector3(1, 3, 10);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x4 Flowerpot");
            part.Position = new IVector3(10, 1, 7);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(11, 1, 19);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(2, 1, 19);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(3, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(12, 1, 19);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x3 Door");
            part.Position = new IVector3(5, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(10, 2, 7);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(5, 4, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(13, 4, 19);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x3 Door");
            part.Position = new IVector3(7, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(2, 3, 19);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(4, 1, 19);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(16, 1, 14);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(8, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(16, 3, 14);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(15, 1, 11);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);





            Stage stage = new Stage();
            stage.decaySpeed = 1.5f;
            stage.timeLimit = 90;
            stage.floors = new Floor[] { floor };

            return stage;
        }


        public Stage Stage5()
        {
            Part part;
            List<Floor> floors = new List<Floor>();
            Floor floor;
            Dictionary<IVector2, FloorTile> floorTiles;





            // Floor 0
            floorTiles = new Dictionary<IVector2, FloorTile>();

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    floorTiles.Add(new IVector2(x, y), new FloorTile(x, y));
                }
            }

            floor = new Floor();
            floor.floor = 0;
            floor.floorTiles = floorTiles;
            floor.height = 4;
            floor.parts = new HashSet<Part>();
            floor.Init();
            floors.Add(floor);


            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(8, 1, 15);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x3 Door");
            part.Position = new IVector3(7, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(7, 3, 15);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(7, 4, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Window");
            part.Position = new IVector3(9, 3, 15);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(16, 1, 8);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(1, 1, 8);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(16, 1, 10);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(15, 1, 6);
            part.Rotation = new IVector3(0, 1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x3 Door");
            part.Position = new IVector3(9, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(1, 1, 10);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(9, 4, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(0, 1, 6);
            part.Rotation = new IVector3(0, 1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(1, 3, 7);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(1, 3, 9);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(16, 3, 7);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x1 Cubes");
            part.Position = new IVector3(16, 3, 9);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            // Floor 1
            floorTiles = new Dictionary<IVector2, FloorTile>();

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    floorTiles.Add(new IVector2(x, y), new FloorTile(x, y));
                }
            }

            floor = new Floor();
            floor.floor = 1;
            floor.floorTiles = floorTiles;
            floor.height = 4;
            floor.parts = new HashSet<Part>();
            floor.Init();
            floors.Add(floor);


            part = new Part();
            part.Type = partTypes.GetPartType("3x4 SCubes");
            part.Position = new IVector3(10, 1, 15);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Hook");
            part.Position = new IVector3(12, 1, 16);
            part.Rotation = new IVector3(0, 2, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Hook");
            part.Position = new IVector3(4, 1, 15);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("3x4 SCubes");
            part.Position = new IVector3(6, 1, 16);
            part.Rotation = new IVector3(0, 2, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("3x4 SCubes");
            part.Position = new IVector3(10, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Hook");
            part.Position = new IVector3(12, 1, 1);
            part.Rotation = new IVector3(0, 2, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(10, 4, 15);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("2x2 Hook");
            part.Position = new IVector3(4, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("3x4 SCubes");
            part.Position = new IVector3(6, 1, 1);
            part.Rotation = new IVector3(0, 2, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(8, 1, 15);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(5, 4, 15);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(11, 4, 0);
            part.Rotation = new IVector3(0, -1, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(5, 4, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("4x2 Window");
            part.Position = new IVector3(8, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            // Floor 2
            floorTiles = new Dictionary<IVector2, FloorTile>();

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    floorTiles.Add(new IVector2(x, y), new FloorTile(x, y));
                }
            }

            floor = new Floor();
            floor.floor = 2;
            floor.floorTiles = floorTiles;
            floor.height = 4;
            floor.parts = new HashSet<Part>();
            floor.Init();
            floors.Add(floor);


            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(0, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(15, 1, 0);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(15, 1, 15);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 LPillar");
            part.Position = new IVector3(1, 1, 15);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(0, 1, 15);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(0, 2, 15);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x1");
            part.Position = new IVector3(0, 3, 15);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);

            part = new Part();
            part.Type = partTypes.GetPartType("1x4 Pillar");
            part.Position = new IVector3(7, 1, 15);
            part.Rotation = new IVector3(0, 0, 0);
            part.TimeBorn = scene.Time;
            floor.AddPart(part);







            Stage stage = new Stage();
            stage.timeLimit = 120;
            stage.decaySpeed = 1.5f;
            stage.floors = floors.ToArray();

            return stage;
        }
    }
}