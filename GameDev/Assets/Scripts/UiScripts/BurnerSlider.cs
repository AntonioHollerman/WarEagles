using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UiScripts
{
    public class BurnerSlider : MonoBehaviour
    {
        public Slider slider;
        public Burner burner;

        private void Awake()
        {
            StartCoroutine(WaitForValueChange());
        }

        private IEnumerator WaitForValueChange()
        {
            while (true)
            {
                float lastValue = slider.value;
                if (Mathf.Approximately(slider.value, 1))
                {
                    burner.SwapToCold();
                }

                if (Mathf.Approximately(slider.value, 2))
                {
                    burner.SwapToOff();
                }

                if (Mathf.Approximately(slider.value, 3))
                {
                    burner.SwapToHot();
                }

                yield return new WaitUntil(() => !Mathf.Approximately(lastValue, slider.value));
            }
        }
    }
}