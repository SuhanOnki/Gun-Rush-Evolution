using Engine.Oponent;
using UnityEngine;

namespace Engine.ObstacleDataSection.GamePlayTrigger
{
    public class AntiEnemySensor : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Zombie"))
            {
                var zombie = other.gameObject.GetComponent<EnemyController>();
                zombie.canCollide = true;
            }
        }
        public void SpecialFunc()
        {
            
        }
    }
}