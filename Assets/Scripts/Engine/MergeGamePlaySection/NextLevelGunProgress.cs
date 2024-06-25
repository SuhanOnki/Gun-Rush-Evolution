using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Engine.MergeGamePlaySection
{
    public class NextLevelGunProgress : MonoBehaviour
    {
        public static UnityEvent ChangeGun = new UnityEvent();
        public static UnityEvent<int> AddValue = new UnityEvent<int>();
        public static NextLevelGunProgress nextLevelGunProgress;
        public static UnityEvent<int> AddMaxValue = new UnityEvent<int>();

        [SerializeField] private GameObject getPanel;
        [SerializeField] private Slider slider;

        [SerializeField] private Image[] images;
        [SerializeField] private Vector3[] positions;
        [SerializeField] private Vector3[] scales;
        [SerializeField] private Sprite[] imageGuns;
        public Image[] guns;
        private int gunLevel;
        private MergeGamePlayState mergeGamePlayState;

        private void Awake()
        {
            nextLevelGunProgress = this;
        }

        public void SpecialFunc()
        {
            
        }
        private void Start()
        {
            mergeGamePlayState = MergeGamePlayState.Instance;
            gunLevel = PlayerPrefs.GetInt("GunLevel", 1);
            images[0].sprite = imageGuns[gunLevel - 1];
            images[1].sprite = imageGuns[gunLevel];
            Init();

            DOVirtual.DelayedCall(0.1f, (() => { guns[mergeGamePlayState.gunLevel - 1].gameObject.SetActive(true); }));
            //images[2].sprite = imageGuns[gunLevel];
            slider.value = 0;
            AddValue.AddListener(AddValue_Instance);
            AddMaxValue.AddListener(AddMaxValue_Instance);
        }

        private void Init()
        {
            if (positions[gunLevel - 1] != Vector3.zero)
            {
                images[0].transform.localPosition = positions[gunLevel - 1];
            }

            if (positions[gunLevel] != Vector3.zero)
            {
                images[1].transform.localPosition = positions[gunLevel];
            }

            if (scales[gunLevel - 1] != Vector3.zero)
            {
                images[0].transform.localScale = scales[gunLevel - 1];
            }

            if (scales[gunLevel] != Vector3.zero)
            {
                images[1].transform.localScale = scales[gunLevel];
            }
        }

        public void SliderMax()
        {
            if (slider.value == slider.maxValue && gunLevel < 5)
            {
                getPanel.SetActive(true);
                slider.value = 0;
                foreach (var powerUI in GunPartHolder.instance.changePowers)
                {
                    powerUI.value = 0;
                }

                ChangeGun?.Invoke();
                MergeGamePlayState.Instance.preventShowNewItem = true;
                gunLevel++;
                images[0].sprite = imageGuns[gunLevel - 1];
                images[1].sprite = imageGuns[gunLevel];
                Init();
            }
        }

        public void AddValue_Instance(int value)
        {
            slider.value = value;
        }

        public void AddMaxValue_Instance(int value)
        {
            slider.maxValue = value;
        }
    }
}