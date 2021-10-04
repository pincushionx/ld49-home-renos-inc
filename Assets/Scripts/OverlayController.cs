using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pincushion.LD49
{
    public class OverlayController : MonoBehaviour
    {
        public SceneController scene;

        // Menu controllers
        private PauseMenuController pauseMenu;

        // Child components 
        public Dictionary<string, Button> buttons = new Dictionary<string, Button>();
        public Dictionary<string, Text> texts = new Dictionary<string, Text>();
        public Dictionary<string, GameObject> messages = new Dictionary<string, GameObject>();

        public GameObject messagePanel;

        public GameObject floor1Panel;
        public GameObject floor2Panel;
        public GameObject floor3Panel;


        private void Awake()
        {
            // Initialize menu controllers
            pauseMenu = GetComponentInChildren<PauseMenuController>(true);
            pauseMenu.Init(this);
            pauseMenu.gameObject.SetActive(false);

            // Index the buttons
            Button[] buttonComponents = GetComponentsInChildren<Button>(true);
            foreach (Button buttonComponent in buttonComponents) {
                if (!buttons.ContainsKey(buttonComponent.name))
                {
                    buttons.Add(buttonComponent.name, buttonComponent);
                }
            }

            // Index the text
            Text[] textComponents = GetComponentsInChildren<Text>(true);
            foreach (Text textComponent in textComponents)
            {
                if (!texts.ContainsKey(textComponent.name))
                {
                    texts.Add(textComponent.name, textComponent);
                }
            }

            // Index the messages
            messagePanel.SetActive(true);
            for (int i = 0; i < messagePanel.transform.childCount; i++)
            {
                GameObject messageGo = messagePanel.transform.GetChild(i).gameObject;
                messages.Add(messageGo.name, messageGo);

                // Add event to the ok button
                Button messageButton = messageGo.GetComponentInChildren<Button>(true);
                messageButton.onClick.AddListener(() => MessageOkClicked(messageGo));

                messageGo.SetActive(false);
            }
           

            // Disable buttons that need disabling
            //buttons["ExitEditModeButton"].gameObject.SetActive(false);
        }

        private void Start()
        {
            // Add Events
            buttons["ShowTutorial"].onClick.AddListener(() => ShowTutorial());
            buttons["RestartLevel"].onClick.AddListener(() => scene.RestartLevel());
            buttons["SkipTutorial"].onClick.AddListener(() => scene.LoadLevel(1));
            buttons["LevelTest"].onClick.AddListener(() => scene.WinCondition());



            RefreshLayout();
        }

        public void RefreshLayout()
        {
            // Force the vertical layout groups to update
            foreach (var layoutGroup in GetComponentsInChildren<LayoutGroup>())
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
            }
        }

        public void UpdateTimer(int timeleft)
        {
            texts["TimeLeft"].text = timeleft.ToString().PadLeft(2, '0');
        }

        public void ShowPauseMenu(bool show)
        {
            pauseMenu.gameObject.SetActive(show);
        }
        public bool IsPauseMenuShown()
        {
            return pauseMenu.gameObject.activeSelf;
        }

        public void ShowTutorial()
        {
            messages["Tut0"].SetActive(true);
            scene.LoadTutorial();
            scene.Paused = true;
        }
        public void ShowControls()
        {
            messages["Controls"].SetActive(true);
            scene.Paused = true;
        }

        public void SetNumberOfFloors(int floors)
        {
            floor3Panel.SetActive(floors > 2);
            floor2Panel.SetActive(floors > 1);
            floor1Panel.SetActive(true);
        }

        public void UpdateStability(int floor, int stability)
        {
            string textName = "Floor" + (floor + 1) + "Stability";
            if (texts.ContainsKey(textName))
            {
                texts[textName].text = stability.ToString().PadLeft(2, '0');
            }
            else
            {
                Debug.LogError("Text " + textName + "doesn't exist");
            }
        }



        public void MessageOkClicked(GameObject messagePanel)
        {
            if (messagePanel.name == "Tut0")
            {
                messagePanel.SetActive(false);
                messages["Tut1"].SetActive(true);
            }
            else if (messagePanel.name == "Tut1")
            {
                messagePanel.SetActive(false);
                messages["Tut2"].SetActive(true);
            }
            else if (messagePanel.name == "Tut2")
            {
                messagePanel.SetActive(false);
                messages["Tut3"].SetActive(true);
            }
            else if (messagePanel.name == "Tut3")
            {
                messagePanel.SetActive(false);
                messages["Tut4"].SetActive(true);
            }
            else // messages
            {
                messagePanel.SetActive(false);
                scene.Paused = false;
            }
        }

        public void MessageError(string message)
        {
            texts["MessageShortText"].text = message;
            messages["MessageShort"].SetActive(true);
            scene.Paused = true;
        }
        public void ShowMessage(string message)
        {
            texts["MessageShortText"].text = message;
            messages["MessageShort"].SetActive(true);
            scene.Paused = true;
        }
        public void LoseConditionMessage()
        {
            string message = "Oh no! The building is destroyed!";
            texts["LoseConditionText"].text = message;
            messages["LoseCondition"].SetActive(true);
            scene.Paused = true;
        }
    }
}