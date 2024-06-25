using System;
using DG.Tweening;
/*using Falcon.FalconAnalytics.Scripts.Enum;
using Falcon.FalconAnalytics.Scripts.Models.Messages.PreDefines;
using Falcon.FalconMediation.Core;*/
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Engine.MergeGamePlaySection
{
    public class Table : MonoBehaviour
    {
        public static Table Instance { get; private set; }
        private int price;

        public static UnityEvent SavingGrid = new UnityEvent();
        public Cell[] inventories;
        [HideInInspector] public int countPrefab;
        public TextMeshProUGUI priceText;
        public Button buyButton;
        public Button buyRewardedButton;
        public int perUprgradePriceIncreaseValue;
        public GameObject freeSpareObject;
        private MergeGamePlayState mergeGamePlayState;
        public GameObject adErrorPanel;
        [SerializeField] private GameObject[] spawnObjects;
        public bool canMerge;
        public bool isPressed;
        public int maxTouch;
        public int valueTouch;
        [SerializeField] private TextMeshProUGUI coinText;

        private int coin;

        private void Awake()
        {
            Instance = this;
        }

        public void SpecialFunc()
        {
        }

        private void Start()
        {
            mergeGamePlayState = MergeGamePlayState.Instance;
            maxTouch = 1;
            countPrefab = 4;
            if (!mergeGamePlayState.isTutorial)
            {
                coin = PlayerPrefs.GetInt("Coin", 350);
                price = PlayerPrefs.GetInt("Price", 100);
                UploadSavingGrid();
            }
            else
            {
                coin = 350;
                price = 100;
            }

            priceText.text = price.ToString();
            coinText.text = coin.ToString();

            SavingGrid.AddListener(SavingGrid_Invoke);
            if (!MergeGamePlayState.Instance.isTutorial)
            {
                int randomSpawn = Random.Range(0, 100);
                if (randomSpawn >= 0 && randomSpawn < 50)
                {
                    int randomPlace = Random.Range(0, inventories.Length);
                    if (inventories[randomPlace].transform.childCount != 0)
                    {
                        for (var i = 0; i < inventories.Length; i++)
                        {
                            if (inventories[i].transform.childCount == 0)
                            {
                                randomPlace = i;

                                break;
                            }
                        }
                    }

                    if (inventories[randomPlace].transform.childCount == 0)
                    {
                        int randomLevelType = 0;
                        int randomLevel = Random.Range(0, 100);
                        if (randomLevel >= 0 && randomLevel < 50)
                        {
                            randomLevelType = 2;
                        }
                        else if (randomLevel >= 50 && randomLevel < 100)
                        {
                            randomLevelType = 3;
                        }

                        if (randomLevelType != 0)
                        {
                            freeSpareObject = Instantiate(
                                spawnObjects[randomLevelType],
                                inventories[randomPlace].transform);
                            freeSpareObject.gameObject.GetComponent<CellObject>().isRewarded = true;
                            freeSpareObject.transform.localPosition = new Vector3(
                                freeSpareObject.transform.localPosition.x,
                                freeSpareObject.transform.localPosition.y,
                                freeSpareObject.transform.localPosition.z + 0.05f);
                        }
                    }
                }
            }

            UpdateButtonStatus();
        }

        public void ShowRewardedForBuyFree()
        {
            AdManager.adManager.ShowRewarded(OpenAdError, (() =>
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(0).transform.childCount == 0)
                    {
                        var obj = Instantiate(spawnObjects[0], transform.GetChild(i).transform);
                        obj.transform.localScale = Vector3.zero;
                        SavingGrid?.Invoke();
                        obj.transform.DOScale(1, 0.3f).SetEase(Ease.Linear);
                        obj.transform.localPosition = new Vector3(obj.transform.localPosition.x,
                            obj.transform.localPosition.y, obj.transform.localPosition.z + 0.05f);
                        SaveBuyButtonData((() => { priceText.text = price.ToString(); }));
                        break;
                    }
                }
            }));
            /*
            FalconMediation.ShowRewardedVideo((() =>
            {*/
            /*FAdLog fAdLog = new FAdLog(AdType.Reward, "On Merge Buy Free Spare Part",
                MergeGamePlayState.Instance.lvl);
            fAdLog.Send();*/
            /*
        }), (() => { OpenAdError(); }));*/
        }

        public void OpenAdError()
        {
            adErrorPanel.SetActive(true);
            var popup = adErrorPanel.transform.GetChild(0);
            popup.transform.DOScale(1, 0.75f).SetEase(Ease.Linear);
            popup.transform.DOShakeScale(0.3f, 1, 5).SetEase(Ease.Linear).SetDelay(0.4f);
        }

        public void CloseAdError()
        {
            var popup = adErrorPanel.transform.GetChild(0);
            popup.transform.DOScale(0, 0.5f).SetEase(Ease.Linear).OnComplete((() =>
            {
                adErrorPanel.SetActive(false);
            }));
        }

        public void UpdateButtonStatus()
        {
            if (price > coin)
            {
                buyRewardedButton.gameObject.SetActive(true);
                buyButton.gameObject.SetActive(false);
            }
            else
            {
                buyButton.gameObject.SetActive(true);
                buyRewardedButton.gameObject.SetActive(false);
            }
        }

        public void OnClick()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).childCount == 0)
                    {
                        Instantiate(spawnObjects[spawnObjects.Length - 1], transform.GetChild(i).transform);
                        SavingGrid?.Invoke();
                        return;
                    }
                }
            }
            else
            {
                if (mergeGamePlayState.isTutorial)
                {
                    if (valueTouch >= maxTouch)
                    {
                        return;
                    }
                }

                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).childCount == 0 && coin >= price)
                    {
                        if (mergeGamePlayState.isTutorial)
                        {
                            valueTouch++;
                            if (maxTouch == 2 && valueTouch == 1)
                            {
                                canMerge = true;
                            }
                        }

                        var obj = Instantiate(spawnObjects[0], transform.GetChild(i).transform);
                        obj.transform.localScale = Vector3.zero;
                        SavingGrid?.Invoke();
                        obj.transform.DOScale(1, 0.3f).SetEase(Ease.Linear);
                        coin -= price;
                        /*FResourceLog fReSinkLog =
                            new FResourceLog(FlowType.Sink, "buy", "money", "buy_item", price);
                        fReSinkLog.Send();*/
                        price += perUprgradePriceIncreaseValue;
                        coinText.text = coin.ToString();
                        if (!mergeGamePlayState.isTutorial)
                        {
                            UpdateButtonStatus();
                        }

                        obj.transform.localPosition = new Vector3(obj.transform.localPosition.x,
                            obj.transform.localPosition.y, obj.transform.localPosition.z + 0.05f);
                        SaveBuyButtonData((() => { priceText.text = price.ToString(); }));
                        return;
                    }
                }
            }
        }

        private void SaveBuyButtonData(Action onComplete)
        {
            PlayerPrefs.SetInt("Price", price);
            PlayerPrefs.SetInt("Coin", coin);
            PlayerPrefs.Save();
            onComplete();
        }

        public void OnClickBattle()
        {
            if (PlayerPrefs.GetInt("tutorialInMergeIsFinished", 1) == 0)
            {
                PlayerPrefs.SetInt("tutorialInMergeIsFinished", 1);
                PlayerPrefs.Save();
            }

            if (mergeGamePlayState.isTutorial)
            {
                return;
            }

            AdManager.adManager.ShowInterstitial((() => { SceneManager.LoadScene(1); }),
                (() => { SceneManager.LoadScene(1); }));
        }

        private void UploadSavingGrid()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (PlayerPrefs.GetInt($"DraggableItemLevel{i}", 0) != 0)
                {
                    Transform tr = Instantiate(spawnObjects[PlayerPrefs.GetInt($"DraggableItemLevel{i}") - 1],
                        transform.GetChild(i).transform).transform;
                    tr.localScale = Vector3.zero;
                    tr.DOScale(1, 0.3f).SetEase(Ease.Linear);
                    tr.localPosition = new Vector3(tr.localPosition.x, tr.localPosition.y,
                        tr.localPosition.z + 0.05f);
                    SelectableObjectItem selectableObjectItemItem = tr.GetChild(0).GetComponent<SelectableObjectItem>();
                    selectableObjectItemItem.level = PlayerPrefs.GetInt($"DraggableItemLevel{i}", 1);
                }
            }
        }

        private void SavingGrid_Invoke()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).childCount == 1)
                {
                    if (transform.GetChild(i).gameObject.GetComponent<CellObject>() &&
                        transform.GetChild(i).gameObject.GetComponent<CellObject>().isRewarded &&
                        !transform.GetChild(i).gameObject.GetComponent<CellObject>().isWATCHED)
                    {
                        return;
                    }


                    SelectableObjectItem selectableObjectItemItem =
                        transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<SelectableObjectItem>();
                    if (selectableObjectItemItem.transform.parent.gameObject.GetComponent<CellObject>())
                    {
                        var item = selectableObjectItemItem.transform.parent.gameObject.GetComponent<CellObject>();
                        if (item.isRewarded && item.isWATCHED)
                        {
                            PlayerPrefs.SetInt($"DraggableItemLevel{i}", selectableObjectItemItem.level);
                        }
                        else if (!item.isRewarded && !item.isWATCHED)
                        {
                            PlayerPrefs.SetInt($"DraggableItemLevel{i}", selectableObjectItemItem.level);
                        }
                    }
                    else
                    {
                        PlayerPrefs.SetInt($"DraggableItemLevel{i}", selectableObjectItemItem.level);
                    }
                }
                else
                {
                    PlayerPrefs.SetInt($"DraggableItemLevel{i}", 0);
                }
            }

            PlayerPrefs.Save();
        }

        public SelectableObjectItem CreatePrefab_Invoke(int levelGun, Transform tr)
        {
            return Instantiate(spawnObjects[levelGun], tr).transform.GetChild(0).GetComponent<SelectableObjectItem>();
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