using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Engine.LevelSection;
using Engine.ObstacleDataSection.FinishObjects;
using Engine.ObstacleDataSection.FRPGates;
using Engine.ObstacleDataSection.GamePlayTrigger;
using Engine.Oponent;
using Engine.PlayerGunSection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Engine.GameSections
{
    public class GameplayMaestro : MonoBehaviour
    {
        public static GameplayMaestro Instance { get; private set; }

        public State playerState;
        public int powerArrow;
        public float rateGateArrow;
        public float speedPlayer;
        public Material[] colors;
        public GameObject target;
        public List<StoneMultiplierGround> xGrounds;
        public float defaultSpeed;
        public int colorIndex;
        public float minX;
        public float maxX;
        public bool playerFinishHalfRoad;
        public int countArrow;
        public int amountZombies;

        [HideInInspector] public float getGateTime;

        // public FResourceLog fResourceLog;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject finishPanel;
        public Image[] bg;
        [SerializeField] private GameObject startingPanel;
        public Material red;
        [SerializeField] private GameObject runningPanel;
        [SerializeField] private GameObject finishZonePanel;
        [SerializeField] private TextMeshProUGUI coin;
        [SerializeField] private TextMeshProUGUI level;
        public LevelType levelType;
        [SerializeField] private GameObject[] levelGameObjects;
        [SerializeField] private GameObject[] zombiesPrefab;
        public GameObject[] zombieBoss;
        public GameObject KillZone;
        public GameObject bestScoreObject;
        public TextMeshProUGUI bestScoreText;
        public float bestScore;
        public int bestScoreObjectIndex;
        public List<GameObject> stones;
        public int plusMoney;
        public int money;
        public GameObject stoneZone;
        public GameObject ground;
        public TextMeshProUGUI eventText;
        public Material[] plus;
        public Material[] minus;
        public GameObject endGround;
        public bool isLoaded;
        public Transform stoneFinishBegin;
        public GameObject stoneFinishGround;
        public int levelStateIndex;
        public int upgradeZombieTimes;
        public int lvl;
        public int winMoney;
        public int countZombies;
        private Vector3 emitPoint;
        private TimeSpan timeSpan;
        private DateTime startLevelTime;
        public GameObject[] zombies;
        public bool haveBoss;
        public LevelDataScriptableObject[] levelDatas;
        private int perZombieLevel;
        public float rangeGateArrow;
        public GameObject spawnPoint;

        public TextMeshProUGUI minusText;

        //private FLevelLog fLevelLog;
        [FormerlySerializedAs("currentLevelData")] public LevelDataScriptableObject currentLevelDataScriptableObject;
        public int levelBGIndex;
        public LevelObstacleData[] levelObstacles;
        public Material whiteMaterial;
        public GameObject[] env;
        public bool isTutorial;
        public GameObject moneyTarget;
        public Sprite[] bgImages;
        public GameObject[] envFogs;
        [FormerlySerializedAs("soundManager")] public AudioEngine audioEngine;
        public bool canZombieSoundEffect;
        public bool updateMoney;
        public bool isInsterstitalShowed;
        public Transform fogParent;
        public Transform envParent;

        private void Awake()
        {
            Instance = this;
            Application.targetFrameRate = 60;

            lvl = PlayerPrefs.GetInt("Level", 1);
            SelectEnv(lvl);
        }

        public void SpecialFunc()
        {
            
        }
        public void StartEvent(string text, Color color)
        {
            eventText.transform.localScale = Vector3.one;
            eventText.color = color;
            eventText.transform.gameObject.SetActive(true);
            eventText.text = text;
            eventText.transform.DOLocalMoveY(450, 1f).SetEase(Ease.Linear).OnComplete((() =>
            {
                eventText.transform.gameObject.SetActive(false);
                eventText.transform.localPosition = Vector3.zero;
            }));
            eventText.DOFade(0f, 0.75f).SetEase(Ease.Linear).SetDelay(0.25f);
            eventText.transform.DOScale(0.6f, 1f).SetEase(Ease.Linear);
        }

        public void PlaySound(AudioClip audioClip)
        {
            if (audioEngine == null)
            {
                audioEngine = AudioEngine.audioEngine;
            }

            if (audioEngine.isMusicPlay)
            {
                if (audioEngine.gunAudio.clip != audioClip)
                {
                    audioEngine.gunAudio.clip = audioClip;
                }

                audioEngine.gunAudio.Play();
            }
            else
            {
                audioEngine.gunAudio.Stop();
            }
        }

        private void SelectEnv(int level)
        {
            int levelBGindex = level;
            levelBGIndex = PlayerPrefs.GetInt($"LGBI", 0);
            if (levelBGindex % 5 == 0)
            {
                if (PlayerPrefs.GetInt("BGLC", 0) == level)
                {
                    return;
                }

                levelBGIndex++;
                if (levelBGIndex >= env.Length)
                {
                    levelBGIndex = 0;

                    /*env[levelBGIndex].SetActive(true);
                envFogs[levelBGIndex].SetActive(true); */
                    var fog = Instantiate(envFogs[levelBGIndex], fogParent);
                    fog.transform.localPosition = Vector3.zero;
                    Instantiate(env[levelBGIndex], envParent);
                }
                else
                {
                    /*env[levelBGIndex].SetActive(true);
                envFogs[levelBGIndex].SetActive(true); */
                    var fog = Instantiate(envFogs[levelBGIndex], fogParent);
                    fog.transform.localPosition = Vector3.zero;
                    Instantiate(env[levelBGIndex], envParent);
                }
            }
            else
            {
                /*env[levelBGIndex].SetActive(true);
            envFogs[levelBGIndex].SetActive(true); */
                var fog = Instantiate(envFogs[levelBGIndex], fogParent);
                fog.transform.localPosition = Vector3.zero;
                Instantiate(env[levelBGIndex], envParent);

                PlayerPrefs.SetInt("LGBI", levelBGIndex);
                PlayerPrefs.Save();
            }
        }

        private void Start()
        {
            defaultSpeed = speedPlayer;
            playerState = State.Wait;
            coin.text = PlayerPrefs.GetInt("Coin", 350).ToString();
            money = PlayerPrefs.GetInt("Coin", 350);
            bestScoreObjectIndex = PlayerPrefs.GetInt($"scoreIndex", 0);
            bestScore = PlayerPrefs.GetInt("score", 0);
            Vector3 bestScorePosition = new Vector3(PlayerPrefs.GetFloat($"BestPosition X", 0),
                PlayerPrefs.GetFloat($"BestPosition Y", 0), PlayerPrefs.GetFloat($"BestPosition Z", 0));
            levelStateIndex = PlayerPrefs.GetInt("LevelIndexState", 0);
            DOVirtual.DelayedCall(4, (() =>
            {
                if (!isLoaded)
                {
                    isLoaded = true;
                }
            }));
            if (levelStateIndex == 3)
            {
                levelType = LevelType.KillZombie;
                countZombies = Random.Range(9, 20);
            }
            else if (levelStateIndex == 5)
            {
                levelType = LevelType.KillZombie;
                countZombies = Random.Range(5, 9);
                levelStateIndex = 0;
                haveBoss = true;
                perZombieLevel = PlayerPrefs.GetInt($"ZombieLevel", 1);
                upgradeZombieTimes = PlayerPrefs.GetInt($"UpgradeZombie", 1);
            }
            else
            {
                levelType = LevelType.Stone;
            }

            if (bestScore > 0)
            {
                if (bestScorePosition != Vector3.zero)
                {
                    bestScoreObject.transform.localPosition = bestScorePosition;
                }

                bestScoreText.text = $"The best score: {bestScore}M";
            }
            else
            {
                bestScoreObject.gameObject.SetActive(false);
            }

            if (levelType == LevelType.KillZombie)
            {
                KillZone.SetActive(true);
                stoneFinishGround.SetActive(false);
            }
            else if (levelType == LevelType.Stone)
            {
                stoneFinishGround.SetActive(true);
                KillZone.SetActive(false);
            }

            if (!isTutorial)
            {
                level.text = $"LEVEL {lvl}";
            }

            if (levelType == LevelType.KillZombie)
            {
                zombies = new GameObject[countZombies];

                CreateZombies();
            }

            PowerGateParent.OnPowerGateTrigger.AddListener(OnPowerGateTrigger_Invoke);
            FireGateParent.OnFireRateGateTrigger.AddListener(OnFireRateGateTrigger_Invoke);
            RangeGateParent.OnRangeGateTrigger.AddListener(OnRangeGateTrigger_Invoke);
            LastPoint.PlayerWentFinish.AddListener(PlayerWentFinish_Invoke);
            AdManager.adManager.LoadBanner();
            /*FalconMediation.ShowBanner();
            FAdLog fAdLog = new FAdLog(AdType.Interstitial, "On Battle Show Banner", PlayerPrefs.GetInt("Level", 1));
            fAdLog.Send();*/
        }


        public void PlusCoin(int value)
        {
            winMoney += value;
        }

        public void MinusCoin(int value)
        {
            plusMoney = money;
            minusText.gameObject.SetActive(true);
            minusText.text = $"-{value.ToString()}";
            minusText.DOFade(1, 0.5f).SetEase(Ease.Linear);
            DOVirtual.DelayedCall(0.5f, (() =>
            {
                plusMoney -= value;
                if (plusMoney < 0)
                {
                    plusMoney = 0;
                }

                minusText.DOFade(0, 0.75f);
                updateMoney = true;
            }));
        }

        private void Update()
        {
            if (updateMoney)
            {
                money = (int)Mathf.MoveTowards(money, plusMoney, Time.deltaTime / 1.25f);
                coin.text = money.ToString();
                if (money == plusMoney)
                {
                    updateMoney = false;
                }
            }
        }

        private void CreateZombies()
        {
            bool upgrade = false;
            if (perZombieLevel >= 3)
            {
                upgrade = true;
                perZombieLevel = 0;
            }

            for (int i = 0; i < countZombies; i++)
            {
                if (haveBoss && i == 0)
                {
                    haveBoss = false;
                    int randomBoss = Random.Range(0, zombieBoss.Length);
                    zombies[i] = Instantiate(zombieBoss[randomBoss]);
                    if (upgrade)
                    {
                        zombies[i].gameObject.GetComponent<EnemyController>().health += 10 * upgradeZombieTimes;
                    }

                    zombies[i].transform.position = new Vector3(zombies[i].transform.position.x, 0.04709917f,
                        zombies[i].transform.position.z);
                    zombies[i].SetActive(false);
                    continue;
                }

                int random = Random.Range(0, zombiesPrefab.Length);
                zombies[i] = Instantiate(zombiesPrefab[random]);
                zombies[i].transform.position = new Vector3(zombies[i].transform.position.x, 0.04709917f,
                    zombies[i].transform.position.z);
                if (upgrade)
                {
                    zombies[i].gameObject.GetComponent<EnemyController>().health += 10 * upgradeZombieTimes;
                }

                zombies[i].SetActive(false);
                PlayerPrefs.SetInt("ZombieLevel", perZombieLevel);
                upgradeZombieTimes++;
                PlayerPrefs.SetInt("UpgradeZombie", upgradeZombieTimes);
                PlayerPrefs.Save();
            }
        }

        private void OnPowerGateTrigger_Invoke(int power)
        {
            powerArrow += power;
            if (powerArrow < 1) powerArrow = 1;
        }

        private void OnFireRateGateTrigger_Invoke(float power)
        {
            rateGateArrow += power;
            GunBehavior.gunBehavior.animationSpeed = rateGateArrow;
            float gunControllerAnimationSpeed = GunBehavior.gunBehavior.animationSpeed / 100;
            GunBehavior.gunBehavior.animator.SetFloat("Speed",
                Mathf.Abs(gunControllerAnimationSpeed));

            if (rateGateArrow < 45) rateGateArrow = 45;
        }

        private void OnRangeGateTrigger_Invoke(float power)
        {
            rangeGateArrow += power / 10;
            if (rangeGateArrow < 1) rangeGateArrow = 1;
        }

        private IEnumerator Finish()
        {
            yield return new WaitForSeconds(2f);
            runningPanel.SetActive(false);
            string levelDifficulty = "normal";
            //finishZonePanel.SetActive(false);
            if (levelType == LevelType.KillZombie)
            {
                levelDifficulty = "hard";
            }
            else if (levelType == LevelType.Stone)
            {
                levelDifficulty = "normal";
            }

            /*
            LevelStatus levelStatus = LevelStatus.Fail;
            timeSpan = DateTime.Now - startLevelTime;*/

            if (playerState == State.Win)
            {
                /*if (PlayerPrefs.GetInt("LevelRetry") == 10)
                {
                    levelStatus = LevelStatus.ReplayPass;
                    PlayerPrefs.SetInt("LevelRetry", 0);
                    PlayerPrefs.Save();
                }
                else
                {
                    levelStatus = LevelStatus.Pass;
                }

                FLevelLog fLevelLog = new FLevelLog(lvl, levelDifficulty, levelStatus, timeSpan, 0);*/
                finishPanel.SetActive(true);
            }
            else
            {
                /*if (PlayerPrefs.GetInt("LevelRetry") == 10)
                {
                    levelStatus = LevelStatus.ReplayPass;
                }
                else
                {
                    levelStatus = LevelStatus.Fail;
                }

                FLevelLog fLevelLog = new FLevelLog(lvl, levelDifficulty, levelStatus, timeSpan, 0);*/
                gameOverPanel.SetActive(true);
            }
        }

        public void EndGame(bool isWin)
        {
            if (isWin)
            {
                playerState = State.Win;
            }
            else
            {
                AudioEngine.audioEngine.gunAudio.Stop();
                GameStateEvents.PlayerStateLose?.Invoke();
                playerState = State.Lose;
            }

            StartCoroutine(Finish());
        }

        private void PlayerWentFinish_Invoke()
        {
            if (levelType == LevelType.KillZombie)
            {
                SwipeSensor.swipeSensor.swipeDetectionValue /= 2;
                playerState = State.Finish;
                finishZonePanel.SetActive(true);
                PlaySound(audioEngine.zombieHordeSound);
                audioEngine.gunAudio.loop = true;
                StartCoroutine(SpawnCoroutine());
            }
        }

        public void StartGame()
        {
            runningPanel.SetActive(true);
            startingPanel.SetActive(false);
            GunBehavior.AttackChange?.Invoke();
            playerState = State.Start;
            startLevelTime = DateTime.Now;
        }

        IEnumerator SpawnCoroutine()
        {
            for (int i = 0; i < countZombies; i++)
            {
                if (playerState == State.Finish)
                {
                    amountZombies++;
                    zombies[i].SetActive(true);
                    zombies[i].transform.SetParent(spawnPoint.transform);
                    emitPoint = transform.localPosition + new Vector3(0, 0.045f, 7);
                    emitPoint.x = Random.Range(minX, maxX);
                    zombies[i].transform.localPosition = new Vector3(emitPoint.x, emitPoint.y, 0);
                }

                yield return new WaitForSeconds(1);
            }

            if (finishZonePanel.activeSelf)
            {
                finishZonePanel.SetActive(false);
            }
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnClickGun()
        {
            SceneManager.LoadScene(3);
        }

        public enum State
        {
            Wait,
            Start,
            Finish,
            Lose,
            Win,
            End
        }

        public enum LevelType
        {
            None,
            Stone,
            KillZombie,
        }
    }
}