﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD49
{
    public class StartScreenController : MonoBehaviour
    {
        public void StartGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    }
}