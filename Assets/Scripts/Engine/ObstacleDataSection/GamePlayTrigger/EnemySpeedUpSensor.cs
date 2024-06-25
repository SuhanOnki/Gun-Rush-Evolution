using Engine.Oponent;
using UnityEngine;

namespace Engine.ObstacleDataSection.GamePlayTrigger
{
    public class EnemySpeedUpSensor : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Zombie"))
            {
                var zombie = other.gameObject.GetComponent<EnemyController>();
                zombie.speed = zombie.RunSpeed;
                zombie.AnimationsOff("Run");
                zombie.animator.SetBool("Run", true);
            }
        }
        public void SpecialFunc()
        {
            
        }
    }
}