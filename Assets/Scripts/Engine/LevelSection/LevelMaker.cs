using System.Collections;
using Engine.LevelSection;
using Engine.ObstacleDataSection.FRPGates;
using UnityEngine;

namespace LevelEngine
{
    public class LevelMaker : MonoBehaviour
    {
        [Header("Prefabs")] public ObstacleDataObject[] obstacles;

        public GateRandomGenerator[] prefabs;
        [Header("Distance")] public Transform beginPlatform;
        public Transform endPlatform;
        [Header("SpawnPoints")] public Transform[] points;

        public void SpecialFunc()
        {
            
        }
        IEnumerator Start()
        {
            yield return null;
            int generation = 0;
            for (var i = 0; i < obstacles.Length; i++)
            {
                obstacles[i].spawnPoint = points[i];
                if (obstacles[i].obstacleType == ObstacleType.Gate)
                {
                    var obstacle = Instantiate(prefabs[0], points[i]);
                    generation++;
                    obstacle.transform.position = new Vector3(obstacles[i].spawnPoint.position.x,
                        obstacle.transform.position.y, obstacles[i].spawnPoint.position.z);
                    obstacle.obstacleType = obstacles[i].obstacleType;
                }
                else if (obstacles[i].obstacleType == ObstacleType.Aim)
                {
                    var obstacle = Instantiate(prefabs[2], points[i]);
                    generation++;
                    obstacle.obstacleType = obstacles[i].obstacleType;
                    obstacle.transform.position = obstacles[i].spawnPoint.position;
                }
                else if (obstacles[i].obstacleType == ObstacleType.Circle)
                {
                    var obstacle = Instantiate(prefabs[3], points[i]);
                    generation++;
                    obstacle.obstacleType = obstacles[i].obstacleType;
                    obstacle.transform.position = obstacles[i].spawnPoint.position;
                }
                else if (obstacles[i].obstacleType == ObstacleType.Escalator)
                {
                    var obstacle = Instantiate(prefabs[4], points[i]);
                    generation++;
                    obstacle.transform.position = obstacles[i].spawnPoint.position;
                    obstacle.obstacleType = obstacles[i].obstacleType;
                }
                else if (obstacles[i].obstacleType == ObstacleType.Obstacle)
                {
                    var obstacle = Instantiate(prefabs[1], points[i]);
                    generation++;
                    obstacle.transform.position = obstacles[i].spawnPoint.position;
                    obstacle.obstacleType = obstacles[i].obstacleType;
                }

                if (generation > 100)
                {
                    generation -= 100;
                    yield return null;
                }
            }
        }
    }
}

public enum ObstacleType
{
    None,
    Gate,
    Aim,
    Stone,
    Circle,
    Obstacle,
    Escalator
}

public enum TypeVariant
{
    First,
    Second,
    Third
}