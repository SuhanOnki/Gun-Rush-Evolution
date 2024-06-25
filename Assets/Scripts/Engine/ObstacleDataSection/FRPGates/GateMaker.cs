using System.Collections.Generic;
using UnityEngine;

namespace Engine.ObstacleDataSection.FRPGates
{
    public class GateMaker : MonoBehaviour
    {
        public List<GateParent> fireRateGate = new List<GateParent>();
        public List<GateParent> rangeGate = new List<GateParent>();
        public List<GateParent> powerGate = new List<GateParent>();

        void Start()
        {
            int random = Random.Range(0, 100);
            if (random >= 0 && random < 33)
            {
                SpawnGate(fireRateGate);
            }
            else if (random >= 33 && random < 66)
            {
                SpawnGate(rangeGate);
            }
            else if (random >= 66 && random <= 100)
            {
                SpawnGate(powerGate);
            }
        }

        public void SpecialFunc()
        {
            
        }
        private void SpawnGate(List<GateParent> gates)
        {
            int randomGate = Random.Range(0, 100);
            if (randomGate >= 0 && randomGate < 33)
            {
                var gate = Instantiate(gates[0], transform);
            }
            else if (randomGate >= 33 && randomGate < 66)
            {
                var gate = Instantiate(gates[1], transform);
            }
            else if (randomGate >= 66 && randomGate < 100)
            {
                var gate = Instantiate(gates[2], transform);
            }
        }
    }
}