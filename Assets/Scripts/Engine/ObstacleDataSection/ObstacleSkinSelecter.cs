using Engine.Obstacle;
using UnityEngine;

namespace Engine.ObstacleDataSection
{
    public class ObstacleSkinSelecter : MonoBehaviour
    {
        public ObstalceSkinData[] obstacle;
        public ObstacleType obstacleType;
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

            for (var i = 0; i < obstacle.Length; i++)
            {
                if (obstacle[i].obstacleType == obstacleType)
                {
                    obstacle[i].gameObject.SetActive(true);
                }
            }
        }
    }
}