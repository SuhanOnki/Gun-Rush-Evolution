using UnityEngine;
using Random = UnityEngine.Random;

namespace Engine.ObstacleDataSection
{
    public class BaseObstacle : MonoBehaviour
    {
        public Transform[] positions;
        public bool isManual;
        
        public void SpecialFunc()
        {
            
        }
        private void Start()
        {
            if (isManual)
            {
                return; 
            }

            if (positions.Length > 0)
            {
            
                int randomPosition = Random.Range(0, positions.Length);
                if (positions[randomPosition] != null)
                {
                    transform.position = positions[randomPosition].position;
                
                }
            }
        }
    }
}