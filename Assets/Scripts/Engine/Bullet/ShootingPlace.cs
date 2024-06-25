using DG.Tweening;
using Engine.Oponent;
using Engine.PlayerGunSection;
using UnityEngine;

namespace Engine.Bullet
{
    public class ShootingPlace : MonoBehaviour
    {
        private GunBehavior gunBehavior;

        void Start()
        {
            gunBehavior = GunBehavior.gunBehavior;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Zombie"))
            {
                if (other.gameObject.GetComponent<EnemyController>())
                {
                    if (gunBehavior.reBoot)
                    {
                        DOVirtual.DelayedCall(0.5f, (() => { gunBehavior.reBoot = false; }));
                    }
                }
            }
        }
        public void SpecialFunc()
        {
            
        }
    }
}