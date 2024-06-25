using Engine.ObstacleDataSection;
using UnityEngine;

namespace Engine.Obstacle
{
    public class ObstalceSkinData : MonoBehaviour
    {
        public BaseObstacle[] obstacles;
        public ObstacleType obstacleType;
        public bool isManual;

        public void SpecialFunc()
        {
            
        }
        void Start()
        {
            if (isManual)
            {
                return;
            }
            int randomObstacle = Random.Range(0, obstacles.Length);
            obstacles[randomObstacle].gameObject.SetActive(true);
        }
    }
}