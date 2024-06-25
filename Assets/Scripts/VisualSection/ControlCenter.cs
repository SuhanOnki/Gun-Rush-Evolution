using DG.Tweening;
using Engine.GameSections;
using UnityEngine;
using UnityEngine.UI;

namespace UISection
{
    public class ControlCenter : MonoBehaviour
    {
        [SerializeField] private GameObject[] backgrounds;
        [SerializeField] private GameObject[] foregrounds;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private float fillAmountChangeSpeed;

        private bool toggle1;
        private bool toggle2;
        private bool toggle3;
        private Image image;
        public int music;
        public int haptic;
        private AudioEngine audioEngine;
        private bool fillAmountChange;

        public void SpecialFunc()
        {
        }

        private void Start()
        {
            audioEngine = AudioEngine.audioEngine;
            image = backgrounds[1].GetComponent<Image>();
            music = PlayerPrefs.GetInt($"Music", 1);
            if (music == 1)
            {
                audioEngine.isMusicPlay = true;
                toggle2 = false;
            }
            else
            {
                audioEngine.isMusicPlay = false;
                toggle2 = true;
            }

            haptic = PlayerPrefs.GetInt($"Haptic", 1);

            if (haptic == 1)
            {
                toggle3 = false;
                audioEngine.isHaptic = true;
            }
            else
            {
                toggle3 = true;
                audioEngine.isHaptic = false;
            }

            MusicToggle(false);
            VibrationToggle(false);
        }

        private void Update()
        {
            if (fillAmountChange && image.fillAmount != 1)
            {
                image.fillAmount += fillAmountChangeSpeed * Time.deltaTime;
                if (image.fillAmount >= 0.35f)
                {
                    backgrounds[0].SetActive(false);
                }
            }
            else if (!fillAmountChange && image.fillAmount != 0)
            {
                image.fillAmount -= fillAmountChangeSpeed * Time.deltaTime;
                if (image.fillAmount <= 0.35f)
                {
                    backgrounds[0].SetActive(true);
                    backgrounds[1].SetActive(false);
                }
            }
        }

        public void ToggleButton()
        {
            toggle1 = !toggle1;
            if (toggle1)
            {
                foregrounds[0].transform.DORotateQuaternion(Quaternion.Euler(0, 0, -90), .5f).SetEase(Ease.OutQuint);
                foregrounds[1].SetActive(true);
                foregrounds[2].SetActive(true);
                foregrounds[1].transform.DOLocalMoveY(-130, .2f).SetEase(Ease.Linear);
                foregrounds[2].transform.DOLocalMoveY(-260, .2f).SetEase(Ease.Linear);
                image.fillAmount = 0;
                backgrounds[1].SetActive(true);
                fillAmountChange = true;
            }
            else
            {
                foregrounds[0].transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), .5f).SetEase(Ease.OutQuint);
                foregrounds[1].transform.DOLocalMoveY(0, .2f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    foregrounds[1].SetActive(false);
                    foregrounds[2].SetActive(false);
                });
                fillAmountChange = false;
                foregrounds[2].transform.DOLocalMoveY(0, .2f).SetEase(Ease.Linear);
            }
        }

        public void MusicToggle(bool canChange)
        {
            if (canChange)
            {
                toggle2 = !toggle2;
            }

            if (toggle2)
            {
                foregrounds[1].GetComponent<Button>().image.sprite = sprites[0];
                audioEngine.isMusicPlay = false;
                music = 2;
                PlayerPrefs.SetInt($"Music", music);
            }
            else
            {
                foregrounds[1].GetComponent<Button>().image.sprite = sprites[1];
                audioEngine.isMusicPlay = true;
                music = 1;

                PlayerPrefs.SetInt($"Music", music);
            }

            PlayerPrefs.Save();
        }

        public void VibrationToggle(bool canChange)
        {
            if (canChange)
            {
                toggle3 = !toggle3;
            }

            if (toggle3)
            {
                foregrounds[2].GetComponent<Button>().image.sprite = sprites[2];
                audioEngine.isHaptic = false;
                haptic = 2;
                PlayerPrefs.SetInt($"Haptic", haptic);
            }
            else
            {
                haptic = 1;

                foregrounds[2].GetComponent<Button>().image.sprite = sprites[3];
                audioEngine.isHaptic = true;
                PlayerPrefs.SetInt($"Haptic", haptic);
            }


            PlayerPrefs.Save();
        }
    }
}