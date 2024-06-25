using System.Collections.Generic;
using LevelEngine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Engine.LevelSection
{
    public class DataCreator : MonoBehaviour
    {
        public ObstacleDataObject[] obstacles;
        public List<ObstacleDataObject> obstacleDatas = new List<ObstacleDataObject>();
        [FormerlySerializedAs("levelGenerator")] public LevelMaker levelMaker;
        public List<ObstacleType> obstacleTypes;
        public int aimSpawnCount;
        public int escalatorSpawnCount;
        public bool isNone;
        public int limit;

        public void SpecialFunc()
        {
            
        }
        void Start()
        {
            aimSpawnCount = Random.Range(0, 100);
            if (aimSpawnCount >= 0 && aimSpawnCount < 33)
            {
                aimSpawnCount = 2;
            }
            else if (aimSpawnCount >= 33 && aimSpawnCount < 66)
            {
                aimSpawnCount = 3;
            }
            else if (aimSpawnCount >= 66 && aimSpawnCount < 101)
            {
                aimSpawnCount = 4;
            }

            escalatorSpawnCount = Random.Range(1, 3);
            int randomCircle = Random.Range(10, 16);
            if (randomCircle > 10 && randomCircle < 16)
            {
                randomCircle = 10;
            }

            obstacles[randomCircle].obstacleType = ObstacleType.Circle;
            obstacles[randomCircle].isNone = true;

            for (var i = 0; i < obstacles.Length; i++)
            {
                obstacles[i].spawnPoint = levelMaker.points[i];
                if (isNone)
                {
                    isNone = false;
                    obstacles[i].isNone = true;
                }
                else
                {
                    isNone = true;
                    if (!obstacles[i].isNone)
                    {
                        obstacleDatas.Add(obstacles[i]);
                    }
                }
            }

            for (int i = 0; i < aimSpawnCount; i++)
            {
                int randomIndex = Random.Range(0, 4);
                if (!obstacleDatas[randomIndex].isNone)
                {
                    obstacleDatas[randomIndex].isNone = true;
                    obstacleDatas[randomIndex].isAlreadyUsing = true;
                    obstacleDatas[randomIndex].obstacleType = ObstacleType.Aim;
                }
            }

            for (int i = 0; i < escalatorSpawnCount; i++)
            {
                int randomIndex = Random.Range(0, obstacleDatas.Count);
                if (!obstacleDatas[randomIndex].isNone)
                {
                    obstacleDatas[randomIndex].isNone = true;
                    obstacleDatas[randomIndex].isAlreadyUsing = true;
                    obstacleDatas[randomIndex].obstacleType = ObstacleType.Escalator;
                }
            }

            for (var i = 0; i < obstacleDatas.Count; i++)
            {
                if (!obstacleDatas[i].isNone)
                {
                    int randomRange = Random.Range(0, 100);
                    if (randomRange >= 0 && randomRange < 40)
                    {
                        obstacleDatas[i].obstacleType = ObstacleType.Gate;
                    }
                    else if (randomRange >= 40 && randomRange < 50)
                    {
                        obstacleDatas[i].obstacleType = ObstacleType.Gate;
                    }
                    else if (randomRange >= 50 && randomRange < 90)
                    {
                        obstacleDatas[i].obstacleType = ObstacleType.Gate;
                    }
                    else if (randomRange >= 90 && randomRange < 100)
                    {
                        obstacleDatas[i].obstacleType = ObstacleType.Obstacle;
                    }
                }
            }


            levelMaker.obstacles = obstacles;
        }
    }
}