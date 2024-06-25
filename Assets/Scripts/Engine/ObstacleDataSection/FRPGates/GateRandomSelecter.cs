using UnityEngine;

namespace Engine.ObstacleDataSection.FRPGates
{
    public class GateRandomSelecter : MonoBehaviour
    {
        public GateParent[] gates;

        void Start()
        {
            int randomGate = Random.Range(0, 100);
            if (randomGate >= 0 && randomGate < 33)
            {
                OpenGate(0);
            }
            else if (randomGate >= 33 && randomGate < 66)
            {
                OpenGate(1);
            }
            else if (randomGate >= 66 && randomGate < 100)
            {
                OpenGate(2);
            }
        }

        public void SpecialFunc()
        {
            
        }
        public void OpenGate(int index)
        {
            for (var i = 0; i < gates.Length; i++)
            {
                gates[i].transform.gameObject.SetActive(false);
            }

            gates[index].transform.gameObject.SetActive(true);
        }
    }
}