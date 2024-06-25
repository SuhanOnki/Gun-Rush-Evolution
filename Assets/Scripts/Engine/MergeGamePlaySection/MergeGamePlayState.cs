using System.Collections.Generic;
using DG.Tweening;
using Engine.GameSections;
using Engine.PlayerGunSection;
//using Falcon.FalconMediation.Core;
using NINESOFT.TUTORIAL_SYSTEM;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Engine.MergeGamePlaySection
{
    public class MergeGamePlayState : MonoBehaviour
    {
        private const string SavedGunParts = "GunPartsLevel";

        public static MergeGamePlayState Instance { get; private set; }
        public GameObject center;
        public static UnityEvent SavingGun = new UnityEvent();
        public int gunLevel;
        public float fireRate;
        public float fireRange;
        public float power;

        public GunPartData[] guns;
        [SerializeField] private GameObject[] gunParts;
        public Sprite[] gunPartLevelsSprite;
        [SerializeField] private int[] gunPartsCountEveryGun;
        [SerializeField] private TextMeshProUGUI levelText;
        [FormerlySerializedAs("grid")] public global::Engine.MergeGamePlaySection.Table table;
        public Image gun;
        public GameObject newItemPanel;
        public TextMeshProUGUI gunLevelText;
        public TextMeshProUGUI fireRateText;

        public TextMeshProUGUI fireRangeText;

        //public FResourceLog fReSinkLog;
        public int lvl;
        public TextMeshProUGUI nextGunLevelText;
        public TextMeshProUGUI nextFireRateText;
        public TextMeshProUGUI nextFireRangeText;
        public bool preventShowNewItem;
        public GameObject[] offPanel;
        public bool isTutorial;
        private int indexGunParts;
        public TutorialManager tutorialManager;
        [FormerlySerializedAs("soundManager")] public AudioEngine audioEngine;

        private void Awake()
        {
            Instance = this;
        }

        public void SpecialFunc()
        {
        }

        private void Start()
        {
            lvl = PlayerPrefs.GetInt("Level", 1);
            levelText.text = $"LEVEL {lvl}";
            gunLevel = PlayerPrefs.GetInt("GunLevel", 1);
            fireRate = float.Parse(PlayerPrefs.GetString("FireRate", "1"));
            fireRange = float.Parse(PlayerPrefs.GetString("FireRange", "1"));
            var number = PlayerPrefs.GetInt("battleTutorial", 0);
            if (number == 1)
            {
                PlayerPrefs.SetInt("tutorialInMergeIsFinished", 0);
                isTutorial = true;
                PlayerPrefs.Save();
            }
            else
            {
                tutorialManager.enabled = false;
            }

            guns[gunLevel - 1].gameObject.SetActive(true);
            SavingGun.AddListener(SavingGun_Invoke);
            NextLevelGunProgress.ChangeGun.AddListener(ChangeGun_Invoke);
            GunPartHolder gunPartHolder = GunPartHolder.instance;

            for (var i = 0; i < gunPartHolder.changePowers.Length; i++)
            {
                gunPartHolder.changePowers[i].Init(guns[gunLevel - 1]);
            }

            AnalyzeGun(false);
            if (isTutorial)
            {
                for (var i = 0; i < table.inventories.Length; i++)
                {
                    if (i == 0)
                    {
                        return;
                    }

                    table.inventories[i].isLocked = true;
                }
            }

            AdManager.adManager.LoadBanner();
            //FalconMediation.ShowBanner();
            /*FAdLog fAdLog = new FAdLog(AdType.Banner, "On Merge Show Banner", PlayerPrefs.GetInt("Level", 1));
            fAdLog.Send();*/
        }

        public void OpenNewItem(Sprite targetImage, int level, float fireRate, float fireRange, float oldLevel,
            float oldFireRate, float oldFireRange)
        {
            for (var i = 0; i < offPanel.Length; i++)
            {
                offPanel[i].SetActive(false);
            }

            center.transform.localScale = Vector3.zero;
            gun.sprite = targetImage;
            newItemPanel.transform.gameObject.SetActive(true);
            this.fireRate = fireRate;
            this.fireRange = fireRange;
            gunLevelText.text = $"+{oldLevel}";
            fireRateText.text = $"+{oldFireRate}";
            fireRangeText.text = $"+{oldFireRange}";
            nextGunLevelText.text = $"+{level}";
            nextFireRateText.text = $"+{fireRate}";
            nextFireRangeText.text = $"+{fireRange}";
            center.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f).SetEase(Ease.Linear).OnComplete((() =>
            {
                center.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.Linear);
            }));
        }

        private void AnalyzeGun(bool isChangeWeapon)
        {
            /*indexGunParts = 0;
            for (int i = 0; i < gunLevel - 1; i++)
            {
                indexGunParts += gunPartsCountEveryGun[i];
            }

            int gunSlider = 0;
            for (int i = 0; i < gunPartsCountEveryGun[gunLevel - 1]; i++)
            {
                int savedGunPrt = PlayerPrefs.GetInt(SavedGunParts + (i + indexGunParts), 1);
                gunParts[i + indexGunParts].GetComponent<LevelUI>().AddValue(savedGunPrt);
                gunSlider += savedGunPrt;
            }*/
            GunPartHolder gunPartHolder = GunPartHolder.instance;
            if (!isChangeWeapon)
            {
                gunPartHolder.partDatas.Clear();
                for (var i = 0; i < guns.Length; i++)
                {
                    var partData = new GunPartUpgradeData();

                    gunPartHolder.partDatas.Add(partData);
                    int defaultValue = 0;
                    gunPartHolder.partDatas[i].partsUpgrade = new List<int>();

                    for (int j = 0; j < gunPartsCountEveryGun[i]; j++)
                    {
                        gunPartHolder.partDatas[i].partsUpgrade.Add(defaultValue);
                    }
                }
            }


            indexGunParts = 0;
            for (int i = 0; i < gunLevel - 1; i++)
            {
                indexGunParts += gunPartsCountEveryGun[i];
            }

            int gunSlider = 0;
            var data = gunPartHolder.partDatas[gunLevel - 1];
            for (var i1 = 0; i1 < data.partsUpgrade.Count; i1++)
            {
                data.partsUpgrade[i1] = PlayerPrefs.GetInt($"{gunLevel - 1} Weapon part {i1}", 0);
                var gun = guns[gunLevel - 1].gameObject.GetComponent<GunPartData>();
                if (data.partsUpgrade[i1] >= 1)
                {
                    if (!gun.transparentGunParts[i1].originGunPart.activeSelf)
                    {
                        gun.transparentGunParts[i1].originGunPart.gameObject.SetActive(true);
                    }

                    gun.transparentGunParts[i1].originGunPart.gameObject.GetComponent<GunPartLevelVisualizer>()
                        .AddValue(data.partsUpgrade[i1]);
                    gunSlider += data.partsUpgrade[i1];
                }
            }

            /*
            for (var i = 0; i < PlayerRay.instance.changePowers.Length; i++)
            {
                PlayerRay.instance.changePowers[i].ChangeValue(false);
            }*/


            NextLevelGunProgress.AddMaxValue?.Invoke(gunPartsCountEveryGun[gunLevel - 1] *
                                                     (gunPartHolder.maxLevelPart + 1));
            NextLevelGunProgress.AddValue?.Invoke(gunSlider);
        }

        private void ChangeGun_Invoke()
        {
            guns[gunLevel - 1].gameObject.SetActive(false);
            NextLevelGunProgress.nextLevelGunProgress.guns[gunLevel - 1].gameObject.SetActive(false);
            guns[gunLevel].gameObject.SetActive(true);
            gunLevel++;
            NextLevelGunProgress.nextLevelGunProgress.guns[gunLevel - 1].gameObject.SetActive(true);

            PlayerPrefs.SetInt("GunLevel", gunLevel);
            PlayerPrefs.Save();
            AnalyzeGun(true);
        }

        private void SavingGun_Invoke()
        {
            for (int i = 0; i < gunPartsCountEveryGun[gunLevel - 1]; i++)
            {
                int level = gunParts[i + indexGunParts].GetComponent<GunPartLevelVisualizer>().GetLevel();
                PlayerPrefs.SetInt(SavedGunParts + (i + indexGunParts), level);
            }

            PlayerPrefs.Save();
        }
    }
}