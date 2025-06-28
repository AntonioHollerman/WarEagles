using System;
using AtomBehaviour;
using UnityEngine;

namespace Managers
{
    public class TemperatureManager : MonoBehaviour
    {
        public float f;
        private void Update()
        {
            Atom.SpeedMultiplier = (1 / 57f) * (f - 0.5f) - 273;
            Atom.ChanceToBreak = (30 / 57f) * (f - 10) - 273;
            Atom.ChanceToBond = (40 / 57f) * (f - 50) - 273;
        }
    }
}