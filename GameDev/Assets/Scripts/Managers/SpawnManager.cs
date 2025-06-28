using System.Collections.Generic;
using AtomBehaviour;
using UnityEngine;

namespace Managers
{
    public class SpawnManager : MonoBehaviour
    {
        public GameObject chloridePrefab;
        public GameObject sodiumPrefab;

        public float xMin;
        public float xMax;
        public float yMin;
        public float yMax;
        
        public List<Atom> chlorideList = new List<Atom>();
        public List<Atom> sodiumList = new List<Atom>();

        public void AddChloride()
        {
            Vector3 pos = new Vector3(
                Random.Range(xMin, xMax),
                Random.Range(yMin, yMax),
                0);
            
            chlorideList.Add(
                Instantiate(chloridePrefab, pos, chloridePrefab.transform.rotation).GetComponent<Atom>()
             );
        }

        public void AddSodium()
        {
            Vector3 pos = new Vector3(
                Random.Range(xMin, xMax),
                Random.Range(yMin, yMax),
                0);
            
            sodiumList.Add(
                Instantiate(sodiumPrefab, pos, sodiumPrefab.transform.rotation).GetComponent<Atom>()
            );
        }

        public void RemoveChloride()
        {
            if (chlorideList.Count == 0)
            {
                return;
            }

            Atom chloride = chlorideList[0];
            chlorideList.RemoveAt(0);

            if (chloride.otherAtom != null)
            {
                
            }
            Destroy(chloride.gameObject);
        }

        public void RemoveSodium()
        {
            if (sodiumList.Count == 0)
            {
                return;
            }
        }
    }
}