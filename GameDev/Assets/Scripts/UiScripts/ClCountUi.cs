using Managers;
using TMPro;
using UnityEngine;

namespace UiScripts
{
    public class ClCountUi : MonoBehaviour
    {
        public TextMeshProUGUI countText;
        public void UpButton()
        {
            SpawnManager.Instance.AddChloride();
            countText.text = $"{SpawnManager.Instance.chlorideList.Count}";
        }

        public void DownButton()
        {
            SpawnManager.Instance.RemoveChloride();
            countText.text = $"{SpawnManager.Instance.chlorideList.Count}";
        }
    }
}