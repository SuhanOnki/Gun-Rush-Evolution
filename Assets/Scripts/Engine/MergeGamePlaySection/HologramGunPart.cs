using UnityEngine;
using UnityEngine.Serialization;

namespace Engine.MergeGamePlaySection
{
    public class HologramGunPart : MonoBehaviour
    {
        [FormerlySerializedAs("levelUI")] public GunPartLevelVisualizer gunPartLevelVisualizer;
        public int index;
        public int gunLevel;
        public GameObject originGunPart;
        public int level;
        public Sprite[] part;

        private void OnEnable()
        {
            level = PlayerPrefs.GetInt($"GunPartLevel: {name}", 0);
            if (gunPartLevelVisualizer.level >= 4)
            {
                return;
            }

            if (!GunPartHolder.instance.closestPlace.Contains(gameObject))
            {
                GunPartHolder.instance.closestPlace.Add(gameObject);
            }
        }
        public void SpecialFunc()
        {
            
        }
    }
}