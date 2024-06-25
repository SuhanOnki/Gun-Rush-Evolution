using Engine.PlayerGunSection;
using UnityEngine;

namespace Engine.ObstacleDataSection.GamePlayTrigger
{
    public class AttackChanger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Gun"))
            {
                GunBehavior.AttackChange?.Invoke();
                transform.GetComponent<Collider>().enabled = false;
            }
        }
        public void SpecialFunc()
        {
            
        }
    }
}
