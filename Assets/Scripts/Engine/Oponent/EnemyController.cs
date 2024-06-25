using System.Collections;
using DG.Tweening;
using Engine.GameSections;
using Engine.ObstacleDataSection.FinishObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Engine.Oponent
{
    public class EnemyController : MonoBehaviour
    {
        public float health;
        public ZombieState state;

        public float RunSpeed;
        public float WalkSpeed;
        public float speed;
        [SerializeField] private SkinnedMeshRenderer meshRenderer;
        [SerializeField] private ParticleSystem particle;
        public GameObject[] skins;
        public GameObject currentSkin;
        private bool firstBlood;
        public VitalityBar vitalityBar;
        public ParticleSystem blood;

        private Collider boxCollider;
        private GameplayMaestro gameplayMaestro;
        public Animator animator;
        public bool canCollide;
        public SkinnedMeshRenderer[] parts;
        private AudioSource audioSource;
        public GameObject moneyPrefab;

        public void SpecialFunc()
        {
            
        }
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            gameplayMaestro = GameplayMaestro.Instance;
            if (skins.Length > 0)
            {
                int randomSkin = Random.Range(0, skins.Length);
                currentSkin = skins[randomSkin];
                currentSkin.SetActive(true);
            }

            boxCollider = GetComponent<Collider>();
            animator = GetComponent<Animator>();
            AnimationsOff("Walk");
            if (vitalityBar.health == health)
            {
                vitalityBar.gameObject.SetActive(false);
            }

            animator.SetBool("Walk", true);
            state = ZombieState.Walk;
            speed = WalkSpeed;
            int randomPlay = Random.Range(0, 100);
            if (randomPlay >= 0 && randomPlay < 50)
            {
                audioSource.enabled = true;
            }
            else
            {
                audioSource.enabled = false;
            }

            GameStateEvents.PlayerStateLose.AddListener(PlayerStateLose_Invoke);
        }

        void Update()
        {
            if (state == ZombieState.Walk)
            {
                transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
            }


            if (gameplayMaestro.canZombieSoundEffect)
            {
                audioSource.enabled = true;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Gun"))
            {
                gameplayMaestro.EndGame(false);
            }
        }

        public void CreateMoney(Vector3 pos)
        {
            GameObject currentMoney = Instantiate(moneyPrefab, pos + Vector3.up * 0.2f, Quaternion.identity);
            currentMoney.transform.DORotate(new Vector3(0, 0, 0), 1);
            currentMoney.transform
                .DOJump(
                    new Vector3(Random.Range(-0.00001f, 0.000001f), 0, Random.Range(-0.000001f, 0.000001f)).normalized +
                    currentMoney.transform.position,
                    0.3f, Random.Range(2, 3), 0.8f)
                .OnComplete(() => currentMoney.GetComponent<MoneyJumper>().canMove = true);
        }

        private void ArrowDegdi()
        {
            if (state == ZombieState.Death)
            {
                return;
            }

            blood.Play();
            /*
        Material materialThis = meshRenderer.material;
        meshRenderer.material = gameManager.whiteMaterial;

        if (currentSkin != null)
        {
            SkinnedMeshRenderer component = currentSkin.GetComponent<SkinnedMeshRenderer>();
            Material skinMaterial = component.material;
            component.material = gameManager.whiteMaterial;
            DOVirtual.DelayedCall(0.05f, (() => { component.material = skinMaterial; }));
        }

        if (parts.Length > 0)
        {
            for (var i = 0; i < parts.Length; i++)
            {
                Material materiala = meshRenderer.material;
                parts[i].material = gameManager.whiteMaterial;
                DOVirtual.DelayedCall(0.05f, (() => { parts[i].material = materiala; }));
            }
        }

        DOVirtual.DelayedCall(0.05f, (() => { meshRenderer.material = materialThis; }));*/
            /*
        for (int i = 0; i < meshRenderer.materials.Length; i++)
        {
            meshRenderer.materials[i].DOColor(gameManager.red.color, 0.5f).SetEase(Ease.Linear);
        }*/

            /*DOVirtual.DelayedCall(0.2f, (() =>
        {
            for (var i = 0; i < meshRenderer.materials.Length; i++)
            {
                meshRenderer.materials[i].DOColor(materialThis[i].color, 0.5f).SetEase(Ease.Linear);
            }
        }));*/
        }

        public void ArrowCollide()
        {
            if (!firstBlood)
            {
                firstBlood = true;
                AnimationsOff("Run");
                speed = RunSpeed;
                animator.SetBool("Run", true);
            }

            ArrowDegdi();
        }

        public void MoveUp()
        {
            var targetPosition = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
            transform.DORotate(targetPosition, 0.5f).SetEase(Ease.Linear);
        }

        public void DeathEffect()
        {
            particle.transform.SetParent(transform.parent.parent);
            gameObject.SetActive(false);
            particle.Play();
            CreateMoney(transform.position);
        }

        public void Death()
        {
            //boxCollider.enabled = false;
            state = ZombieState.Death;
            audioSource.Stop();
            AnimationsOff("Die");
            animator.SetBool("Die", true);
            StartCoroutine(DeathCoroutine());
        }

        private IEnumerator DeathCoroutine()
        {
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
        }

        private void PlayerStateLose_Invoke()
        {
            state = ZombieState.Idle;
            AnimationsOff("Idle");
            audioSource.Stop();
            audioSource.enabled = false;
            animator.SetBool("Idle", true);
        }

        public void AnimationsOff(string name)
        {
            string[] namesAnimator = new string[] { "Run", "Walk", "Die", "Idle" };
            for (int i = 0; i < namesAnimator.Length; i++)
            {
                if (name != namesAnimator[i])
                {
                    animator.SetBool(namesAnimator[i], false);
                }
            }
        }

        public enum ZombieState
        {
            Idle,
            Walk,
            Attack,
            Death
        }
    }
}