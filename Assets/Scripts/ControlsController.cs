using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD49
{
    public class ControlsController : MonoBehaviour
    {
        private SceneController scene;
        private StageController stage;

        void Update()
        {
            // Pause
            // TODO add a better pause key. Esc is bad for webgl
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!scene.Paused)
                {
                    scene.overlay.ShowPauseMenu(true);
                }
                scene.Paused = !scene.Paused;
            }

            // Gameplay
            if (!scene.Paused)
            {
                bool unsuccessfulMove = false;

                if (stage.ActivePart != null)
                {
                    // Move part up
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        IVector3 position = stage.ActivePart.Position;
                        IVector3 move = IUtil.Rotate(new IVector3(0, 0, 1), stage.GetCameraRotation());
                        unsuccessfulMove = !stage.MoveActivePart(new IVector2(position.x + move.x, position.z + move.z));
                    }
                    // Move part down
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        IVector3 position = stage.ActivePart.Position;
                        IVector3 move = IUtil.Rotate(new IVector3(0, 0, -1), stage.GetCameraRotation());
                        unsuccessfulMove = !stage.MoveActivePart(new IVector2(position.x + move.x, position.z + move.z));
                    }
                    // Move part left
                    else if (Input.GetKeyDown(KeyCode.A))
                    {
                        IVector3 position = stage.ActivePart.Position;
                        IVector3 move = IUtil.Rotate(new IVector3(-1, 0, 0), stage.GetCameraRotation());
                        unsuccessfulMove = !stage.MoveActivePart(new IVector2(position.x + move.x, position.z + move.z));
                    }
                    // Move part right
                    else if (Input.GetKeyDown(KeyCode.D))
                    {
                        IVector3 position = stage.ActivePart.Position;
                        IVector3 move = IUtil.Rotate(new IVector3(1, 0, 0), stage.GetCameraRotation());
                        unsuccessfulMove = !stage.MoveActivePart(new IVector2(position.x + move.x, position.z + move.z));
                    }
                    // Rotate part left
                    else if (Input.GetKeyDown(KeyCode.Q))
                    {
                        IVector3 rotation = stage.ActivePart.Rotation;
                        rotation.y = rotation.y + 1;
                        rotation = IUtil.SanitizeRotation(rotation);
                        unsuccessfulMove = !stage.RotateActivePart(rotation);
                    }
                    // Rotate part right
                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        IVector3 rotation = stage.ActivePart.Rotation;
                        rotation.y = rotation.y - 1;
                        rotation = IUtil.SanitizeRotation(rotation);
                        unsuccessfulMove = !stage.RotateActivePart(rotation);
                    }
                }
                // Move up floor
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    unsuccessfulMove = !stage.SetActiveFloor(stage.currentFloor + 1);
                }
                // Move down floor
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    unsuccessfulMove = !stage.SetActiveFloor(stage.currentFloor - 1);
                }
                // Rotate around house left 
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    int index = stage.currentCameraDestinationIndex == 0? 3 : stage.currentCameraDestinationIndex - 1;
                    stage.MoveCamera(index);
                }
                // Rotate around house right
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    int index = stage.currentCameraDestinationIndex == 3 ? 0 : stage.currentCameraDestinationIndex + 1;
                    stage.MoveCamera(index);
                }
                // Next part
                else if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    unsuccessfulMove = !stage.GetNewActivePart();
                }
                // Commit
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    unsuccessfulMove = !stage.CommitActivePart();
                }

                if (unsuccessfulMove)
                {
                    if (Random.Range(0, 1) == 0)
                    {
                        scene.Sound.PlaySound("nope");
                    }
                    else
                    {
                        scene.Sound.PlaySound("nah-ah");
                    }
                }
            }
        }

        public void Init(SceneController scene)
        {
            this.scene = scene;
            stage = scene.stage;
        }
    }
}