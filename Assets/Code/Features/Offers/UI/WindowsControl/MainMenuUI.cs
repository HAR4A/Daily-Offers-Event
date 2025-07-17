using System.Collections.Generic;
using Code.Services.UIWindows;
using UnityEngine;

namespace Code.Features.Offers.UI.WindowsControl
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private List<WindowButtonPair> _buttonWindowPairs;

        private void Start()
        {
            foreach (var pair in _buttonWindowPairs)
            {
                var windowToOpen = pair.Window;
                pair.Button.onClick.AddListener(() => windowToOpen.SetActive(true));
            }
        }
    }
}