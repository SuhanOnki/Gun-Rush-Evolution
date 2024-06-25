using DG.Tweening;
using Engine.GameSections;
using Engine.ObstacleDataSection.FinishObjects;
using Engine.PlayerGunSection;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Engine.ObstacleDataSection
{
    public class StoneMoneyObstacle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private float count;
        [SerializeField] private GameObject particle;
        [FormerlySerializedAs("moneyOn")] [SerializeField] private StoneMoney stoneMoneyOn;
        [SerializeField] private GameObject[] gameObjectObstacles;
        [FormerlySerializedAs("xGround")] public StoneMultiplierGround stoneMultiplierGround;
        public bool isNormalObstacle;
        public bool isManual;

        private float count2;

        public void SpecialFunc()
        {
            
        }
        private void Start()
        {
            if (isManual)
            {
                countText.text = count.ToString();
                count2 = count;
                return;
            }

            count = Random.Range(100, 200);
            countText.text = count.ToString();
            count2 = count;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Arrow"))
            {
                GameplayMaestro.Instance.PlaySound(AudioEngine.audioEngine.bulletHitAudioClip);
                particle.GetComponent<ParticleSystem>().Play();
                transform.DOShakeScale(0.15f, 0.5f);
                count -= GameplayMaestro.Instance.powerArrow;
                if (count <= count2 / 2)
                {
                    gameObjectObstacles[0].SetActive(false);
                    gameObjectObstacles[1].SetActive(true);
                }

                if (count <= 0)
                {
                    stoneMoneyOn.Work();
                    gameObject.SetActive(false);
                }

                countText.text = count.ToString();
                other.gameObject.SetActive(false);
                GunBehavior.gunBehavior.arrowPooler.ReturnObjectToPool(other.gameObject.GetComponent<Bullet.Bullet>());
            }

            if (other.CompareTag("Player"))
            {
                if (isNormalObstacle)
                {
                    return;
                }

                GunMovement.gunMovement.mainCamera.SetActive(true);
                GunBehavior.AttackChange?.Invoke();
                GunBehavior.gunBehavior.animator.speed = 0;
                GameplayMaestro.Instance.EndGame(true);
                GunMovement.gunMovement.enabled = false;
                DOVirtual.DelayedCall(1, (() =>
                {
                    var stoneIndex = ActivateRecord(out var distance);
                    PlayerPrefs.SetInt($"scoreIndex", stoneIndex);
                    PlayerPrefs.Save();
                }));
            }
        }

        public int ActivateRecord(out float distance)
        {
            var gameManager = GameplayMaestro.Instance;
            var stoneIndex = gameManager.stones.IndexOf(gameObject);
            distance = (gameManager.stoneFinishBegin.position - transform.position).magnitude * 10;
            gameManager.bestScoreObjectIndex = stoneIndex;
            gameManager.bestScoreObject.gameObject.SetActive(true);

            var targetPosition = new Vector3(transform.position.x - 2f,
                transform.position.y + 0.8f, transform.position.z + 1);
            if (PlayerPrefs.GetInt($"score") < distance)
            {
                float dis = distance;
                gameManager.bestScoreObject.transform.DOMove(targetPosition, 0.75f).SetEase(Ease.Linear).OnComplete((() =>
                {
                    if (!gameManager.isTutorial)
                    {
                        PlayerPrefs.SetFloat($"BestPosition X", gameManager.bestScoreObject.transform.localPosition.x);
                        PlayerPrefs.SetFloat($"BestPosition Y", gameManager.bestScoreObject.transform.localPosition.y);
                        PlayerPrefs.SetFloat($"BestPosition Z", gameManager.bestScoreObject.transform.localPosition.z);
                        PlayerPrefs.SetInt($"score", (int)dis);
                        PlayerPrefs.Save();
                    }
                }));
                gameManager.bestScoreText.text = "The best score: " + distance.ToString("F1") + "M";
            }


            return stoneIndex;
        }
    }
}