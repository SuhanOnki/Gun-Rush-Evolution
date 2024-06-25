using Engine.GameSections;
using Engine.PlayerGunSection;
using UnityEngine;

namespace Engine.ObstacleDataSection.GamePlayTrigger
{
    public class AttackStopTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Gun"))
            {
                GunBehavior.AttackChange?.Invoke();
                GameplayMaestro.Instance.EndGame(true);
            }
        }
        public void SpecialFunc()
        {
            
        }
    }
}
