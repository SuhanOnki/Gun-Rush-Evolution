using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Engine.LevelSection
{
    [Serializable]
    public class ObstacleDataObject
    {
        public ObstacleType obstacleType;
        public Transform spawnPoint;
        [FormerlySerializedAs("obstacleExtraInfo")] public ObstacleSecrets obstacleSecrets;
        public bool isNone;
        public Vector3 spawnPosition;
        public bool isAlreadyUsing;
        public void SpecialFunc()
        {
            
        }
    }
}