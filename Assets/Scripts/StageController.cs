using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD49
{
    public class StageController : MonoBehaviour
    {
        public GameObject floorTilePrefab;
        public GameObject innerFloorTilePrefab;
        public GameObject cube1x1Prefab;
        public GameObject cube2x2HookPrefab;
        public GameObject cube2x1Prefab;
        public GameObject door2x3Prefab;
        public GameObject flowerpot2x4Prefab;
        public GameObject lpillar1x4Prefab;
        public GameObject pillar1x4Prefab;
        public GameObject scubes3x4Prefab;
        public GameObject window2x2Prefab;
        public GameObject window4x2Prefab;

        public GameObject roof;

        public GameObject floatingTextDigitsPrefab;
        public GameObject floatingTextContainer;
        public GameObject highlightPrefab;

        public SceneController scene;

        [HideInInspector]
        public LevelConfig levels = new LevelConfig();

        [HideInInspector]
        public Stage stage;

        [HideInInspector]
        public PartTypeManager partTypes = new PartTypeManager();

        [HideInInspector]
        public int currentFloor = 0;

        public Part ActivePart { get; set; }
        public Floor ActiveFloor {
            get
            {
                return stage.floors[currentFloor];
            }
        }

        float cameraHeightAboveCenter = 10f;
        float cameraDistanceFromOuterBound = 5f;
        public int currentCameraDestinationIndex = 0; // should coincide with compensation rotations
        private GameObject cameraDestinationContainer;
        private GameObject[] cameraDestinations;
        private GameObject partQueueContainer;
        private GameObject[] floorContainers;
        private Dictionary<Part, PartController> parts = new Dictionary<Part, PartController>();
        private Dictionary<string, FloatingTextController> floatingText = new Dictionary<string, FloatingTextController>();

        public void LoadStage(int stageIndex)
        {
            // Clear stuff
            Clear();

            // Load the stage
            levels.Init(this);
            stage = levels.GetStage(stageIndex);

            // Draw the stage
            DrawStage();

            // Place the active part
                Part part = new Part();
            part.Type = scene.partQueue.GetRandomPartType();
            part.Position = new IVector3(2, 1, 4);
            AddActivePartToStage(part);
            SetActiveFloor(currentFloor);

            scene.overlay.SetNumberOfFloors(stage.floors.Length);
            MoveCamera(0);

            stage.timeStart = scene.Time;
            stage.loaded = true;
        }

        public void Clear()
        {
            if (stage == null)
            {
                return;
            }

            stage.loaded = false;

            currentCameraDestinationIndex = 0;
            currentFloor = 0;
            ActivePart = null;
            scene.partQueue.Reset();

            foreach (KeyValuePair<string, FloatingTextController> kv in floatingText)
            {
                SetFloatingText(kv.Key, "");
            }
            foreach (KeyValuePair<Part, PartController> kv in parts)
            {
                Destroy(kv.Value.gameObject);
            }
            parts.Clear();

            if (partQueueContainer != null)
            {
                Destroy(partQueueContainer);
                partQueueContainer = null;
            }

            if (floorContainers != null)
            {
                foreach (GameObject floorContainer in floorContainers)
                {
                    Destroy(floorContainer);
                }
                floorContainers = null;
            }

            stage = null;
        }

        private void DrawStage()
        {
            int currentHeight = 0;
            floorContainers = new GameObject[stage.floors.Length];

            // Draw the initial stage
            for (int floorIndex = 0; floorIndex < stage.floors.Length; floorIndex++)
            {
                Floor floor = stage.floors[floorIndex];

                GameObject floorContainer = new GameObject();
                floorContainer.transform.SetParent(transform);
                floorContainer.transform.localPosition = new Vector3(0, currentHeight, 0);
                floorContainer.name = "Floor";
                floorContainers[floorIndex] = floorContainer;

                GameObject floorTileContainer= new GameObject();
                floorTileContainer.transform.SetParent(floorContainer.transform);
                floorTileContainer.transform.localPosition = new Vector3(0, 0, 0);
                floorTileContainer.name = "Floor Tiles";

                GameObject partContainer = new GameObject();
                partContainer.transform.SetParent(floorContainer.transform);
                partContainer.transform.localPosition = new Vector3(0, 0, 0);
                partContainer.name = "Parts";

                // Draw the floor
                foreach (KeyValuePair<IVector2, FloorTile> kv in floor.floorTiles) 
                {
                    IVector2 floorTilePosition = kv.Key;
                    FloorTile floorTile = kv.Value;

                    GameObject floorTileGo = floorTile.isInner? Instantiate(innerFloorTilePrefab) : Instantiate(floorTilePrefab);

                    floorTileGo.transform.SetParent(floorTileContainer.transform);
                    floorTileGo.transform.localPosition = new Vector3(floorTilePosition.x, 0, floorTilePosition.y);
                }

                // Draw the initial parts
                foreach (Part part in floor.parts)
                {
                    //IVector3 partPosition = part.Position;
                    //part.Position = new IVector3(partPosition.x, partPosition.y, partPosition.z);
                    AddPartToStage(part, floorIndex);
                }

                currentHeight += floor.height + 1;
            }

            // Reposition / resize the roof
            roof.transform.localPosition = new Vector3(0, currentHeight, 0);
            roof.transform.localScale = new Vector3(stage.floors[0].outerBound.size.x / 10f, 1, stage.floors[0].outerBound.size.y / 10f);

            // Add the part queue container
            partQueueContainer = new GameObject();
            partQueueContainer.name = "Part Queue";
            partQueueContainer.transform.SetParent(transform);

            if (cameraDestinationContainer == null)
            {
                cameraDestinationContainer = new GameObject();
                cameraDestinationContainer.name = "Camera Destinations";
                cameraDestinationContainer.transform.SetParent(transform);

                cameraDestinations = new GameObject[4];
                cameraDestinations[0] = new GameObject();
                cameraDestinations[0].name = "Camera Destination 0";
                cameraDestinations[0].transform.SetParent(cameraDestinationContainer.transform);

                cameraDestinations[1] = new GameObject();
                cameraDestinations[1].name = "Camera Destination 1";
                cameraDestinations[1].transform.SetParent(cameraDestinationContainer.transform);

                cameraDestinations[2] = new GameObject();
                cameraDestinations[2].name = "Camera Destination 2";
                cameraDestinations[2].transform.SetParent(cameraDestinationContainer.transform);

                cameraDestinations[3] = new GameObject();
                cameraDestinations[3].name = "Camera Destination 3";
                cameraDestinations[3].transform.SetParent(cameraDestinationContainer.transform);

            }
            // No need to recreate, just update
            Floor floor0 = stage.floors[0];
            Vector3 centerPosition = new Vector3(floor0.outerBound.Center.x, GetFloorYPosition(currentFloor), floor0.outerBound.Center.y);
            cameraDestinations[0].transform.localPosition = new Vector3(floor0.outerBound.Center.x, cameraHeightAboveCenter, floor0.outerBound.MinY - cameraDistanceFromOuterBound);
            cameraDestinations[0].transform.LookAt(centerPosition);

            cameraDestinations[1].transform.localPosition = new Vector3(floor0.outerBound.MaxX + cameraDistanceFromOuterBound, cameraHeightAboveCenter, floor0.outerBound.Center.y);
            cameraDestinations[1].transform.LookAt(centerPosition);

            cameraDestinations[2].transform.localPosition = new Vector3(floor0.outerBound.Center.x, cameraHeightAboveCenter, floor0.outerBound.MaxY + cameraDistanceFromOuterBound);
            cameraDestinations[2].transform.LookAt(centerPosition);

            cameraDestinations[3].transform.localPosition = new Vector3(floor0.outerBound.MinX - cameraDistanceFromOuterBound, cameraHeightAboveCenter, floor0.outerBound.Center.y);
            cameraDestinations[3].transform.LookAt(centerPosition);

            scene.cameraController.transform.SetParent(cameraDestinations[currentCameraDestinationIndex].transform);
            scene.cameraController.transform.localPosition = Vector3.zero;
            scene.cameraController.transform.localRotation = Quaternion.identity;
            scene.cameraController.transform.localScale =  Vector3.one;

            RefreshQueue();
            RefreshSupport();
        }


        public Vector3 GetCenterOfActiveFloor()
        {
            IVector2 center = ActiveFloor.outerBound.Center;
            return new Vector3(center.x, GetFloorYPosition(currentFloor), center.y);
        }


        public void AddFloatingText(string name, Vector3 position, string initialText = "")
        {
            if (floatingText.ContainsKey(name))
            {
                SetFloatingText(name, initialText);
                floatingText[name].transform.localPosition = position;
                return;
            }
            
            GameObject go = Instantiate(floatingTextDigitsPrefab);
            FloatingTextController controller = go.GetComponent<FloatingTextController>();

            go.name = name;
            go.transform.SetParent(floatingTextContainer.transform);
            go.transform.localPosition = position;
            controller.SetText(initialText);

            floatingText.Add(name, controller);
        }
        public void SetFloatingText(string name, string text)
        {
            floatingText[name].SetText(text);
        }

        public void RefreshQueue()
        {
            if (partQueueContainer == null)
            {
                return;
            }

            float ypos = 0;// GetFloorYPosition(currentFloor);

            // TODO revisit if the board can be rotated
            partQueueContainer.transform.localPosition = new Vector3(ActiveFloor.outerBound.MaxX + 1, ypos, ActiveFloor.outerBound.MinY);

            // Just clear and recreate each time
            for (int i = partQueueContainer.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(partQueueContainer.transform.GetChild(i).gameObject);
            }

            if (scene.partQueue.nextPart != null) {
                GameObject partGo = Instantiate(GetPartPrefab(scene.partQueue.nextPart));
                partGo.transform.SetParent(partQueueContainer.transform);
                partGo.transform.localPosition = new Vector3(0,0,0);
            }
            if (scene.partQueue.afterNextPart != null)
            {
                GameObject partGo = Instantiate(GetPartPrefab(scene.partQueue.afterNextPart));
                partGo.transform.SetParent(partQueueContainer.transform);
                partGo.transform.localPosition = new Vector3(3, 0, 0);
            }
            else
            {
                // Draw floating text for floor
                string text = scene.partQueue.AfterNextPartTimeRemaining == 0 ? "" : Mathf.CeilToInt(scene.partQueue.AfterNextPartTimeRemaining).ToString();
                AddFloatingText("AfterNextPartTimer", partQueueContainer.transform.position + new Vector3(3, 1, 0), text);
            }
        }

        private GameObject GetPartContainer(int floor)
        {
            GameObject floorContainer = floorContainers[floor];
            return floorContainer.transform.GetChild(1).gameObject; // currently the second child
        }

        private int GetFloorYPosition(int floorIndex)
        {
            int currentHeight = 0;

            for (int i = 0; i < floorIndex; i++)
            {
                Floor floor = stage.floors[i];
                currentHeight += floor.height + 1;
            }

            return currentHeight;
        }

        public bool SetActiveFloor(int newfloor)
        {
            if (newfloor < 0 || newfloor > (stage.floors.Length-1))
            {
                return false;
            }
            int oldFloor = currentFloor;

            currentFloor = newfloor;
            ActivePart.Floor = currentFloor;

            GameObject partContainer = GetPartContainer(currentFloor);
            GameObject activePartGo = parts[ActivePart].gameObject;
            activePartGo.transform.SetParent(partContainer.transform);

            if (MoveActivePart(ActivePart.Position.ToVector2XZ()))
            {
                // If the part can move in its current place, move it
                DimOtherFloors();
                AdjustCameraHeight();
                return true;
            }

            currentFloor = oldFloor;
            ActivePart.Floor = currentFloor;
            DimOtherFloors();
            AdjustCameraHeight();

            // If the part doesn't fit, reset it
            Debug.LogError("TODO part couldn't move to new floor");

            return false;
        }

        private void DimOtherFloors()
        {
            for (int floorIndex = 0; floorIndex < stage.floors.Length; floorIndex++)
            {
                bool dim = floorIndex != currentFloor;

                Floor floor = stage.floors[floorIndex];

                GameObject floorContainer = floorContainers[floorIndex];
                Transform floorTilesTransform = floorContainer.transform.Find("Floor Tiles");
                for (int floorTileIndex = 0; floorTileIndex < floorTilesTransform.childCount; floorTileIndex++)
                {
                    GameObject floorTile = floorTilesTransform.GetChild(floorTileIndex).gameObject;
                    FloorTileController floorTileController = floorTile.GetComponent<FloorTileController>();

                    floorTileController.Dim(dim);
                }

                Transform partsTransform = floorContainer.transform.Find("Parts");
                for (int partIndex = 0; partIndex < partsTransform.childCount; partIndex++)
                {
                    GameObject part = partsTransform.GetChild(partIndex).gameObject;
                    PartController partController = part.GetComponent<PartController>();

                    if (partController == null)
                    {
                        Debug.LogError("Part " + part.name + " is missing its controller");
                    }
                    else
                    {
                        partController.Dim(dim);
                    }
                }
            }
        }


        public void MoveCamera(int destination)
        {
            if (!scene.cameraController.moving)
            {
                currentCameraDestinationIndex = destination;
                scene.cameraController.MoveCamera(cameraDestinations[currentCameraDestinationIndex]);
            }
        }


        IEnumerator MoveCameraContainerCoroutine()
        {
            float moveSpeed = .05f; // seconds per unit distance
            float progress = 0;

            float cameraYDest = GetFloorYPosition(currentFloor);
            float currentY = cameraDestinationContainer.transform.position.y;
            float distance = Mathf.Abs(cameraYDest - currentY);

            Vector3 initialPosition = cameraDestinationContainer.transform.position;
            Vector3 desiredPosition = new Vector3(initialPosition.x, cameraYDest, initialPosition.z);

            while (progress < 1)
            {
                yield return null;

                float frameProgress = Time.deltaTime / moveSpeed / distance;
                progress += frameProgress;

                cameraDestinationContainer.transform.position = Vector3.Slerp(initialPosition, desiredPosition, progress);
            }
        }

        private void AdjustCameraHeight()
        {
            StartCoroutine(MoveCameraContainerCoroutine());
        }

        public bool CommitActivePart()
        {
            if (!stage.floors[currentFloor].CanPlace(ActivePart))
            {
                // can't place it
                return false;
            }
            else
            {

                ActivePart.TimeBorn = scene.Time; // The part was just placed. It has been reborn!
                ActivePart.Floor = currentFloor;
                ActiveFloor.AddPart(ActivePart);

                // Highlight the part, play a sound
                IVector2[] supportColumns = ActiveFloor.GetCompleteSupportTiles(ActivePart);
                if (supportColumns.Length > 0)
                {
                    scene.Sound.PlaySound("yeah");

                    /*foreach (IVector2 supportColumn in supportColumns)
                    {
                        GameObject highlightGo = Instantiate(highlightPrefab);
                        HighlightController highlight = highlightGo.GetComponent<HighlightController>();

                        highlightGo.transform.SetParent(floorContainers[currentFloor].transform);
                        highlightGo.transform.localPosition = new Vector3(supportColumn.x, 1, supportColumn.y);

                        highlight.Init(scene, ActiveFloor.height);
                    }*/
                }


                // Get the new part
                PartType nextPartType = scene.partQueue.GetNextPart();

                Part part = new Part();
                part.Type = nextPartType;
                AddActivePartToStage(part);
                RefreshQueue();
                RefreshSupport();
            }
            return true;
        }
        public bool GetNewActivePart()
        {
            RemovePart(ActivePart);

            PartType nextPartType = scene.partQueue.GetNextPart();

            Part part = new Part();
            part.Type = nextPartType;
            AddActivePartToStage(part);
            RefreshQueue();

            return true;
        }

        public void RefreshSupport()
        {
            int currentHeight = 0;
            for (int floorIndex = 0; floorIndex < stage.floors.Length; floorIndex++)
            {
                Floor floor = stage.floors[floorIndex];
                for (int region = 0; region < floor.regions; region++)
                {
                    int support = floor.GetSupport(region);
                    //AddFloatingText("Floor" + floorIndex + "-" + region, new Vector3(floor.outerBound.MinX + 1.5f, currentHeight, floor.outerBound.MinY + 1.5f), support.ToString().PadLeft(2, '0'));
                    scene.overlay.UpdateStability(floorIndex, support);

                    if (support == 0)
                    {
                        scene.LoseCondition();
                        return;
                    }
                }
                currentHeight += floor.height + 1;
            }
        }

        public void AddActivePartToStage(Part part)
        {
            int floorIndex = currentFloor;

            if (ActiveFloor.newPartPointSet)
            {
                part.Position = new IVector3(ActiveFloor.newPartPoint.x, 1, ActiveFloor.newPartPoint.y);
            }
            else
            {
                part.Position = new IVector3(ActiveFloor.outerBound.Center.x, 1, ActiveFloor.outerBound.Center.y);
            }

            AddPartToStage(part, floorIndex);

            ActivePart = part;
        }
        public GameObject AddPartToStage(Part part, int floor)
        {
            int floorIndex = floor;

            GameObject partGo = Instantiate(GetPartPrefab(part.Type));
            GameObject partContainer = GetPartContainer(floorIndex);
            partGo.transform.SetParent(partContainer.transform);
            partGo.transform.localPosition = part.Position.ToVector3();
            partGo.transform.transform.localRotation = Quaternion.Euler(part.Rotation.ToVector3() * 90);

            PartController controller = partGo.GetComponent<PartController>();
            controller.Init(scene, part);
            
            parts.Add(part, controller);

            return partGo;
        }

        private GameObject GetPartPrefab(PartType partType)
        {
            string partTypeName = partType.name;

            if (partTypeName == "1x1")
            {
                return cube1x1Prefab;
            }
            else if (partTypeName == "2x2 Hook")
            {
                return cube2x2HookPrefab;
            }
            else if (partTypeName == "2x1 Cubes")
            {
                return cube2x1Prefab;
            }
            else if (partTypeName == "2x3 Door")
            {
                return door2x3Prefab;
            }
            else if (partTypeName == "2x4 Flowerpot")
            {
                return flowerpot2x4Prefab;
            }
            else if (partTypeName == "1x4 Pillar")
            {
                return pillar1x4Prefab;
            }
            else if (partTypeName == "1x4 LPillar")
            {
                return lpillar1x4Prefab;
            }
            else if (partTypeName == "3x4 SCubes")
            {
                return scubes3x4Prefab;
            }
            else if (partTypeName == "2x2 Window")
            {
                return window2x2Prefab;
            }
            else if (partTypeName == "4x2 Window")
            {
                return window4x2Prefab;
            }


            Debug.LogError("Prefab doesn't exist");
            return null;
        }

        public IVector3 GetCameraRotation()
        {
            return IUtil.InvertRotation(IUtil.SanitizeRotation(new IVector3(0, currentCameraDestinationIndex, 0)));
        }
        public bool MoveActivePart(IVector2 position)
        {
            if (ActivePart == null)
            {
                return false;
            }

            IVector3 oldPosition = ActivePart.Position;
            /*ActivePart.Position = new IVector3(position.x, 1, position.y);

            if (ActiveFloor.IsShapeAvailable(ActivePart))
            {
                // We can move there, move the game object
                parts[ActivePart].transform.localPosition = new Vector3(ActivePart.Position.x, ActivePart.Position.y, ActivePart.Position.z); // +1 since it site on top of the floor
                return true;
            }*/

            // try each spot from the ground up
            for (int y = 1; y <= ActiveFloor.height; y++)
            {
                ActivePart.Position = new IVector3(position.x, y, position.y);

                if (ActiveFloor.IsShapeAvailable(ActivePart))
                {
                    // We can move there, move the game object
                    parts[ActivePart].transform.localPosition = new Vector3(ActivePart.Position.x, ActivePart.Position.y, ActivePart.Position.z); // +1 since it site on top of the floor
                    return true;
                }
            }

            // We can't move there. Restore the position
            ActivePart.Position = oldPosition;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotation">IVector3, with 4 possible rotations per axis, 0-3</param>
        /// <returns></returns>
        public bool RotateActivePart(IVector3 rotation)
        {
            if (ActivePart == null)
            {
                return false;
            }

            IVector3 oldRotation = ActivePart.Rotation;

            ActivePart.Rotation = rotation;

            if (ActiveFloor.IsShapeAvailable(ActivePart))
            {
                // We can move there, move the game object
                parts[ActivePart].transform.localPosition = new Vector3(ActivePart.Position.x, ActivePart.Position.y, ActivePart.Position.z); // +1 since it site on top of the floor
                parts[ActivePart].transform.localRotation = Quaternion.Euler(ActivePart.Rotation.ToVector3() * 90);
                return true;
            }

            // We can't move there. Restore the position
            ActivePart.Rotation = oldRotation;
            return false;
        }

        public void RemovePart(Part part)
        {
            stage.floors[part.Floor].RemovePart(part);
            Destroy(parts[part].gameObject);
            parts.Remove(part);

            RefreshSupport();
        }
    }
}