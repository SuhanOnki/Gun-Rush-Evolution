using DG.Tweening;
using Engine.GameSections;
using Engine.ObstacleDataSection.FinishObjects;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Engine.PlayerGunSection
{
    public class GunMovement : MonoBehaviour
    {
        public GameObject mainCamera;
        [FormerlySerializedAs("gunsRotationMove")] [SerializeField] private GunRotator gunRotator;
        public GameObject moneyParent;

        [FormerlySerializedAs("gameManager")] public GameplayMaestro gameplayMaestro;
        public static GunMovement gunMovement;
        public bool isCollide;
        private float defaultSpeed;
        private GunBehavior gunBehavior;
        public Vector3 moveSpeed;
        private float roadLength;
        private bool one;
        private float time;

        private void Awake()
        {
            gunMovement = this;
        }

        public void SpecialFunc()
        {
            
        }
        private void Start()
        {
            one = false;
            gunBehavior = GunBehavior.gunBehavior;
            gameplayMaestro = GameplayMaestro.Instance;
            SwipeSensor.SwipeEvent += OnSwipe;
        }

        private void Update()
        {
            if (gameplayMaestro.playerState == GameplayMaestro.State.Start ||
                (gameplayMaestro.playerState == GameplayMaestro.State.Finish &&
                 gameplayMaestro.levelType == GameplayMaestro.LevelType.Stone))
            {
                if (gameplayMaestro.playerState != GameplayMaestro.State.Win && gameplayMaestro.playerState != GameplayMaestro.State.Lose)
                {
                    transform.position += new Vector3(0, 0, gameplayMaestro.speedPlayer * Time.deltaTime);
                }
            }
        }

        private void OnSwipe(float x)
        {
            if (!one && (Mathf.Abs(x) > .005f || Input.mousePosition.y < Screen.height / 2))
            {
                if (gameplayMaestro.isLoaded)
                {
                    one = true;
                    gameplayMaestro.StartGame();
                }
            }


            if (gameplayMaestro.playerState == GameplayMaestro.State.Start ||
                (gameplayMaestro.playerState == GameplayMaestro.State.Finish))
            {
                if (gameplayMaestro.playerState != GameplayMaestro.State.Win || gameplayMaestro.playerState != GameplayMaestro.State.Lose)
                {
                    gunRotator.Rotate(x);
                    x += transform.position.x;
                    if (x < gameplayMaestro.minX)
                    {
                        x = gameplayMaestro.minX;
                    }
                    else if (x > gameplayMaestro.maxX)
                    {
                        x = gameplayMaestro.maxX;
                    }

                    //transform.position = new Vector3 (x, 0, transform.position.z);
                    transform.DOMoveX(x, 0.1f).SetEase(Ease.Linear);
                }
            }
        }

        public void CreateMoney(Vector3 pos)
        {
            for (int i = 0; i < Random.Range(2, 5); i++)
            {
                var money = gunBehavior.moneyPooler.GetObject().gameObject.GetComponent<StoneMoney>();
                money.transform.position = gunBehavior.transform.position;
                money.transform.SetParent(moneyParent.transform);
                money.transform.DORotate(new Vector3(0, 0, 180), 1);
                pos = new Vector3(0, 0, pos.z);
                Vector3 endValue =
                    new Vector3(Random.Range(gameplayMaestro.minX, gameplayMaestro.maxX), 0.2f, Random.Range(1.5f, 3.5f)) + pos;
                money.transform
                    .DOJump(
                        endValue,
                        0.3f, Random.Range(2, 3), 0.8f).OnComplete((() => { money.work = true; }));
                DOVirtual.DelayedCall(3, (() =>
                {
                    if (money != null)
                    {
                        money.transform.DOScale(0.0001f, 0.75f).SetEase(Ease.Linear).OnComplete((() =>
                        {
                            gunBehavior.moneyPooler.ReturnObjectToPool(money);
                        }));
                    }
                }));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle"))
            {
                if (Time.time - time > 1.5f)
                {
                    transform.DOMoveZ(transform.position.z - 1.5f, 0.3f).SetEase(Ease.Linear);
                    if (gunBehavior.moneyPooler._availableObjectsPool.Count > 0)
                    {
                        CreateMoney(gunBehavior.transform.position);
                    }

                    time = Time.time;
                }
            }

            if (other.CompareTag("MoneyObstacle"))
            {
                // mainCamera.SetActive(true);
            }
        }
    }
}