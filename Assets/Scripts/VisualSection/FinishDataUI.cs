using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Engine.GameSections;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace UISection
{
    public class FinishDataUI : MonoBehaviour
    {
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private Transform coinParent;
        [SerializeField] private Transform spawnLocation;
        [SerializeField] private Transform endPosition;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI youWonCoinTexts;
        [SerializeField] private float duration;
        [SerializeField] private int coinAmount;
        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;
        public GameObject adError;

        [SerializeField] private Transform[] transforms;
        [SerializeField] private float speed;
        [SerializeField] private bool isWin;

        private bool arrowWork = true;
        private int plusMinus = 1;
        private List<GameObject> coins = new List<GameObject>();
        public TextMeshProUGUI extraMoney;

        [FormerlySerializedAs("arrowController")]
        public ArrowMultiplierUI arrowMultiplierUI;

        public int coin;
        public int targetMoney;
        private int oneCoin;
        public bool isMoneyTransfering;
        private Tween coinTween;
        private int winMoney;

        public void SpecialFunc()
        {
        }

        private void Start()
        {
            if (GameplayMaestro.Instance.isTutorial)
            {
                youWonCoinTexts.text = $"+50";
                coin = PlayerPrefs.GetInt("Coin", 350);
                coinText.text = coin.ToString();
            }
            else
            {
                youWonCoinTexts.text = $"+{GameplayMaestro.Instance.winMoney}";
                coin = PlayerPrefs.GetInt("Coin", 350);
                coinText.text = coin.ToString();
            }
        }

        public void OpenAdError()
        {
            adError.SetActive(true);
            var popup = adError.transform.GetChild(0);
            popup.transform.DOScale(1, 0.75f).SetEase(Ease.Linear);
            popup.transform.DOShakeScale(0.3f, 1, 5).SetEase(Ease.Linear).SetDelay(0.4f);
        }

        public void CloseAdError()
        {
            var popup = adError.transform.GetChild(0);
            popup.transform.DOScale(0, 0.5f).SetEase(Ease.Linear).OnComplete((() => { adError.SetActive(false); }));
        }

        private void Update()
        {
            if (GameplayMaestro.Instance.winMoney == 0)
            {
                GameplayMaestro.Instance.winMoney = 50;
            }

            if (extraMoney != null && arrowMultiplierUI != null)
            {
                extraMoney.text = $"+{arrowMultiplierUI.xNumber * GameplayMaestro.Instance.winMoney}";
            }
        }

        public void LoadShop(bool isWin)
        {
            if (arrowWork)
            {
                arrowWork = false;
                if (GameplayMaestro.Instance.isTutorial)
                {
                    PlayerPrefs.SetInt("battleTutorial", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("LGBI", GameplayMaestro.Instance.levelBGIndex);
                    PlayerPrefs.SetInt("BGLC", PlayerPrefs.GetInt("Level", 1));
                    PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
                    PlayerPrefs.Save();
                    PlayerPrefs.SetInt("LevelIndexState", GameplayMaestro.Instance.levelStateIndex + 1);
                }

                StartCoroutine(Coroutine(() =>
                {
                    if (AdManager.adManager != null)
                    {
                        Debug.Log("Work");
                        AdManager.adManager.ShowInterstitial((() => { SceneManager.LoadScene(3); }),
                            (() => { SceneManager.LoadScene(3); }));
                    }
                    else
                    {
                        Debug.Log("error");
                        SceneManager.LoadScene(3);
                    }
                    /*FalconMediation.ShowInterstitial((() =>
                        {
                            if (isWin)
                            {
                                FAdLog fAdLog = new FAdLog(AdType.Interstitial,
                                    "On Battle Win to Merge Weapon Scene Interstitial",
                                    GameplayMaestro.Instance.lvl);
                                fAdLog.Send();
                            }
                            else
                            {
                                FAdLog fAdLog = new FAdLog(AdType.Interstitial,
                                    "On Battle Lose to Merge Weapon Scene Interstitial",
                                    GameplayMaestro.Instance.lvl);
                                fAdLog.Send();
                            }

                        }),
                        (() => { SceneManager.LoadScene(3); }));  */
                    //SceneManager.LoadScene(3);
                }, false));
            }
        }


        public void Retry()
        {
            if (arrowWork)
            {
                arrowWork = false;
                if (isWin)
                {
                    PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1));
                    PlayerPrefs.SetInt("LevelRetry", 10);
                    PlayerPrefs.SetInt("LevelIndexState", GameplayMaestro.Instance.levelStateIndex);
                }

                PlayerPrefs.SetInt("Coin", coin);
                PlayerPrefs.Save();
                StartCoroutine(Coroutine((() =>
                {
                    if (AdManager.adManager != null)
                    {
                        AdManager.adManager.ShowInterstitial((() => { SceneManager.LoadScene(3); }),
                            (() => { SceneManager.LoadScene(3); }));
                    }
                    else
                    {
                        SceneManager.LoadScene(3);
                    }
                }), false));
            }
        }

        public void ShowRewardedWin()
        {
            AdManager.adManager.ShowRewarded((() => { OpenAdError(); }), (() =>
            {
                StartCoroutine(Coroutine((() =>
                {
                    if (GameplayMaestro.Instance.isTutorial)
                    {
                        PlayerPrefs.SetInt("battleTutorial", 1);
                    }

                    else
                    {
                        PlayerPrefs.SetInt("LGBI", GameplayMaestro.Instance.levelBGIndex);
                        PlayerPrefs.SetInt("BGLC", PlayerPrefs.GetInt("Level", 1));
                        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
                        PlayerPrefs.Save();
                        PlayerPrefs.SetInt("LevelIndexState", GameplayMaestro.Instance.levelStateIndex + 1);
                    }

                    winMoney = GameplayMaestro.Instance.winMoney * arrowMultiplierUI.xNumber;
                    int xNumber = coin + winMoney;
                    coin = xNumber;
                    /*FAdLog fAdLog = new FAdLog(AdType.Reward, "On Battle Win Panel X Money",
                        GameplayMaestro.Instance.lvl);
                    FResourceLog fResourceLog =
                        new FResourceLog(FlowType.Source, "win", "money", "win_level_get_x",
                            winMoney,
                            GameplayMaestro.Instance.lvl);
                    fResourceLog.Send();
                    fAdLog.Send();*/
                    PlayerPrefs.SetInt("Coin", coin);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene(3);
                }), true));
            }));
        }

        public void ShowRewardedLose()
        {
            AdManager.adManager.ShowRewarded(OpenAdError, (() =>
            {
                StartCoroutine(Coroutine((() =>
                {
                    if (GameplayMaestro.Instance.isTutorial)
                    {
                        PlayerPrefs.SetInt("battleTutorial", 1);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("LGBI", GameplayMaestro.Instance.levelBGIndex);
                        PlayerPrefs.SetInt("BGLC", PlayerPrefs.GetInt("Level", 1));
                        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
                        PlayerPrefs.Save();
                        PlayerPrefs.SetInt("LevelIndexState", GameplayMaestro.Instance.levelStateIndex + 1);
                    }

                    winMoney = GameplayMaestro.Instance.winMoney * arrowMultiplierUI.xNumber;
                    int xNumber = winMoney + coin;
                    coin = xNumber;
                    /*FAdLog fAdLog = new FAdLog(AdType.Reward, "On Battle Lose Panel X Money",
                        GameplayMaestro.Instance.lvl);
                    FResourceLog fResourceLog =
                        new FResourceLog(FlowType.Source, "lose", "money", "lose_level_get_x",
                            winMoney,
                            GameplayMaestro.Instance.lvl);
                    fResourceLog.Send();
                    fAdLog.Send();*/
                    PlayerPrefs.SetInt("Coin", coin);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene(3);
                }), true));
            }));
        }

        private IEnumerator Coroutine(Action onComplete, bool isRewarded)
        {
            isMoneyTransfering = true;
            if (arrowMultiplierUI != null)
            {
                arrowMultiplierUI.StopMove();
            }

            if (GameplayMaestro.Instance.isTutorial)
            {
                if (!isRewarded)
                {
                    targetMoney = coin + 50;
                    /*FResourceLog fResourceLog =
                        new FResourceLog(FlowType.Source, "finish", "money", "level_finish", 50,
                            GameplayMaestro.Instance.lvl);                    fResourceLog.Send();*/

                    coin = targetMoney;
                }
            }
            else
            {
                if (!isRewarded)
                {
                    targetMoney = coin + GameplayMaestro.Instance.winMoney;
                    /*FResourceLog fResourceLog =
                        new FResourceLog(FlowType.Source, "finish", "money", "level_finish",
                            GameplayMaestro.Instance.winMoney,
                            GameplayMaestro.Instance.lvl);
                    fResourceLog.Send();*/
                    coin = targetMoney;
                }
            }

            for (int i = 0; i < coinAmount; i++)
            {
                GameObject coinInstance = Instantiate(coinPrefab, transform);
                float xPosition = spawnLocation.position.x + Random.Range(minX, maxX);
                float yPosition = spawnLocation.position.y + Random.Range(minY, maxY);

                coinInstance.transform.position = new Vector3(xPosition, yPosition);
                coinInstance.transform.DOPunchPosition(new Vector3(0, 30, 0), Random.Range(0, .5f))
                    .SetEase(Ease.InOutElastic);
                coins.Add(coinInstance);
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(.1f);
            for (int i = 0; i < coins.Count; i++)
            {
                coins[i].transform.DOMove(endPosition.position, duration).SetEase(Ease.InBack).OnComplete(() =>
                {
                    /*coin += oneCoin;
                coinText.text = coin.ToString();*/
                    if (coinTween == null)
                    {
                        coinTween = endPosition.DOPunchScale(Vector3.one * 1.1f, 0.1f).SetEase(Ease.InOutElastic)
                            .OnComplete(() =>
                            {
                                coinTween = null;
                                coins[i].gameObject.SetActive(false);
                            });
                    }
                });
                yield return new WaitForSeconds(0.02f);
            }

            PlayerPrefs.SetInt("Coin", coin);
            PlayerPrefs.Save();
            yield return new WaitForSeconds(0.75f);
            isMoneyTransfering = false;
            onComplete();
        }

        public void ChangeButton(Transform activityButton)
        {
            if (activityButton.localScale != Vector3.one)
            {
                activityButton.localScale = Vector3.one;
            }
            else
            {
                activityButton.localScale = Vector3.zero;
            }
        }
    }
}