using Engine.GameSections;
using Engine.PlayerGunSection;
using UnityEngine;

namespace Engine.ObstacleDataSection.FRPGates
{
    public class EvolveAntiBulletShield : MonoBehaviour
    {
        private AudioEngine audioEngine;

        public void SpecialFunc()
        {
            
        }
        private void Start()
        {
            audioEngine = AudioEngine.audioEngine;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Arrow"))
            {
                GameplayMaestro.Instance.PlaySound(audioEngine.bulletHitAudioClip);
                other.gameObject.SetActive(false);
                GunBehavior.gunBehavior.arrowPooler.ReturnObjectToPool(other.gameObject.GetComponent<Bullet.Bullet>());
            }
        }
    }
}