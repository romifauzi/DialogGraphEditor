using System;
using UnityEngine;
using UnityEngine.UI;

namespace DialogGraph.Examples
{
    public class ButtonOption : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] TMPro.TMP_Text text;

        public void SetupResponse(Action action, string message)
        {
            text.SetText(message);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate { action?.Invoke(); });
            gameObject.SetActive(true);
        }
    }
}