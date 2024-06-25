using UnityEngine;
using UnityEngine.Serialization;

namespace Engine.ObstacleDataSection.FRPGates
{
    public class WallGate : MonoBehaviour
    {
        public bool explode;
        public GameObject[] bricks;
        [FormerlySerializedAs("gate")] public global::Engine.ObstacleDataSection.FRPGates.GateParent gateParent;
        
        public void SpecialFunc()
        {
            
        }
    }
}