using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pincushion.LD49
{
    public class FloatingTextController : MonoBehaviour
    {
        public TextMeshPro text;

        private void Awake()
        {
            text = GetComponent<TextMeshPro>();
        }

        public void SetText(string s)
        {
            text.text = s;
        }
    }
}