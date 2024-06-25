using Engine.GameSections;
using Engine.ObstacleDataSection.RedAimData;
using Engine.PlayerGunSection;
using UnityEngine;
using UnityEngine.Serialization;

namespace Engine.ObstacleDataSection.FRPGates
{
    public class EvolveGate : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private AimBoneFollower aimBoneStarter;
        [SerializeField] private int numberGate;
        [FormerlySerializedAs("gunController")] [SerializeField] private GunBehavior gunBehavior;

        public void SpecialFunc()
        {
            
        }
        private void Start()
        {
            gunBehavior = GunBehavior.gunBehavior;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameplayMaestro.Instance.PlaySound(AudioEngine.audioEngine.upgradeWeaponGate);
                gunBehavior.upgradeParticle.Play();
                other.transform.GetChild(0).GetComponent<GunBehavior>().Rotate();
                gunBehavior.AddPartGun(numberGate);
                GameplayMaestro.Instance.StartEvent($"+{numberGate} Evolve", Color.green);
                aimBoneStarter.OffAllObjects();
                /*DOVirtual.DelayedCall(0.5f, (() =>
            {
                if (transform.parent != null)
                {
                    if (transform.parent.gameObject.GetComponent<RandomGate>())
                    {
                        if (transform.parent.gameObject.GetComponent<RandomGate>().obstacleType == ObstacleType.Circle)
                        {
                            Destroy(transform.parent.gameObject);
                        }
                    }
                }
            }));*/
            }

            if (other.CompareTag("Arrow"))
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}