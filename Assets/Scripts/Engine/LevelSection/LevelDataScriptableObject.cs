using Engine.GameSections;
using UnityEngine;

namespace Engine.LevelSection
{
    [CreateAssetMenu()]
    public class LevelDataScriptableObject : ScriptableObject
    {
        public GameplayMaestro.LevelType levelType;
        public ObstacleType[] obstacleTypes;
        public int enemyCount;
        public int sceneIndex;
        public void SpecialFunc()
        {
            
        }
    }
}
