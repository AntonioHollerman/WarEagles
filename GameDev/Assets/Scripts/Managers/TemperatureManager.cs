using System;
using AtomBehaviour;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class TemperatureManager : MonoBehaviour
    {
        public TextMeshProUGUI tempDisplay;
        public Slider tempSlider;
        public float tempChangePerSecond;
        
        public static float K = 300f;
        
        
        private void Update()
        {
            tempDisplay.text = $"Temp: {(int) K}K";
            Atom.SpeedMultiplier = (1 / 57f) * (K - 273) + 0.5f;
            Atom.ChanceToBreak = (30 / 57f) * (K - 273) + 10;
            Atom.ChanceToBond = (40 / 57f) * (K - 273) + 50;

            if (Mathf.Approximately(tempSlider.value, 1))
            {
                K = Mathf.Clamp(K - tempChangePerSecond * Time.deltaTime, 273, 330);
            }

            if (Mathf.Approximately(tempSlider.value, 3))
            {
                K = Mathf.Clamp(K + tempChangePerSecond * Time.deltaTime, 273, 330);
            }
        }
    }
}