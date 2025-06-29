using Managers;
using TMPro;
using UnityEngine;

namespace UiScripts
{
    public class NaCountUi : MonoBehaviour
    {
        public TextMeshProUGUI countText;
        public void UpButton()
        {
            SpawnManager.Instance.AddSodium();
            countText.text = $"{SpawnManager.Instance.sodiumList.Count}";
        }

        public void DownButton()
        {
            SpawnManager.Instance.RemoveSodium();
            countText.text = $"{SpawnManager.Instance.sodiumList.Count}";
        }
    }
}