using Engine.GameSections;
using Engine.ObjectCreatorDatas;
using Engine.ObstacleDataSection.RedAimData;
using Engine.Oponent;
using Engine.PlayerGunSection;
using UnityEngine;
using UnityEngine.Serialization;

namespace Engine.Bullet
{
    public class Bullet : PoolableObjectData
    {
        [SerializeField] private GameObject particle;
        public GameObject zombieHit;

        private GameplayMaestro gameplayMaestro;
        private AudioEngine audioEngine;

        [FormerlySerializedAs("gunController")]
        public GunBehavior gunBehavior;

        private void Start()
        {
            audioEngine = AudioEngine.audioEngine;
            gameplayMaestro = GameplayMaestro.Instance;
        }

        public void SpecialFunc()
        {
        }

        void Update()
        {
            if (gameplayMaestro.transform.position.z + gameplayMaestro.rangeGateArrow < transform.position.z)
            {
                gameObject.SetActive(false);
                GunBehavior.gunBehavior.arrowPooler.ReturnObjectToPool(this);
                return;
            }

            //transform.position += new Vector3(0, 0, gameManager.rateGateArrow * Time.deltaTime);
            transform.position += (transform.forward * 10 * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Zombie"))
            {
                gameObject.SetActive(false);
                GunBehavior.gunBehavior.arrowPooler.ReturnObjectToPool(this);

                Instantiate(zombieHit, transform.position, Quaternion.identity);
                zombieHit.gameObject.GetComponent<ParticleSystem>().Play();
                EnemyController zombie = other.gameObject.GetComponent<EnemyController>();
                if (!zombie.canCollide) return;
                zombie.ArrowCollide();
                zombie.health -= gameplayMaestro.powerArrow;
                zombie.vitalityBar.TakeDamage(gameplayMaestro.powerArrow);

                if (zombie.health <= 0 && zombie.state == EnemyController.ZombieState.Walk)
                {
                    zombie.DeathEffect();
                    // zombie.Death();
                    gameplayMaestro.amountZombies--;
                    if (gameplayMaestro.amountZombies < 1)
                    {
                        gameplayMaestro.EndGame(true);
                    }
                }
            }

            if (other.gameObject.CompareTag("AimBones"))
            {
                GameplayMaestro.Instance.PlaySound(audioEngine.bulletHitAudioClip);
                gameObject.SetActive(false);
                gunBehavior.arrowPooler.ReturnObjectToPool(this);

                Instantiate(particle, transform.position, Quaternion.identity);
                AimAnimatedItem aimAnimatedItem = other.gameObject.GetComponent<AimAnimatedItem>();
                aimAnimatedItem.Animate();
            }
        }
    }
}