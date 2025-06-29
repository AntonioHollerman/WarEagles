using UnityEngine;

namespace UiScripts
{
    public class Burner : MonoBehaviour
    {
        public GameObject offGo;
        public GameObject hotGo;
        public GameObject coldGo;

        public void SwapToOff()
        {
            offGo.SetActive(true);
            hotGo.SetActive(false);
            coldGo.SetActive(false);
        }

        public void SwapToHot()
        {
            offGo.SetActive(false);
            hotGo.SetActive(true);
            coldGo.SetActive(false);
        }

        public void SwapToCold()
        {
            offGo.SetActive(false);
            hotGo.SetActive(false);
            coldGo.SetActive(true);
        }
    }
}