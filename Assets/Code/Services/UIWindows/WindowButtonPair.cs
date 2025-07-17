using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Services.UIWindows
{
    [Serializable]
    public struct WindowButtonPair
    {
        public Button Button;
        public GameObject Window;
    }
}