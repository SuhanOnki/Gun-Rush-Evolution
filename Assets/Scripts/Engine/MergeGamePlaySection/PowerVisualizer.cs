using Engine.PlayerGunSection;
using UnityEngine;
using UnityEngine.UI;

namespace Engine.MergeGamePlaySection
{
    public class PowerVisualizer : MonoBehaviour
    {
        [SerializeField] private Image image;
        public string namePower;
        public float[] upgradeValue;
        public float value;
        public bool isDamage;

        private void Start()
        {
            value = float.Parse(PlayerPrefs.GetString(namePower, "1"));
            image.fillAmount = value * 0.1f;
        }

        public void SpecialFunc()
        {
            
        }
        public void Init(GunPartData data)
        {
            value = float.Parse(PlayerPrefs.GetString(namePower, "1"));
            image.fillAmount = value * 0.1f;
            if (isDamage)
            {
                upgradeValue = data.upgradeDamage;

                return;
            }


            upgradeValue = data.upgradeData;
        }

        public void ChangeValueToZero(bool isSave)
        {
            if (!isSave)
            {
                value = 1;
                image.fillAmount = value * 0.1f;
            }

            PlayerPrefs.SetString(namePower, value.ToString());
            PlayerPrefs.Save();
        }

        public float GetValue()
        {
            return value;
        }

        public void ChangeValue(bool isSave)
        {
            value += upgradeValue[MergeGamePlayState.Instance.gunLevel - 1];
            image.fillAmount = value * 0.1f;
            ChangeValueToZero(isSave);
        }

        public void DecrementValue()
        {
            value -= 10;
            image.fillAmount = value * 0.01f;
            PlayerPrefs.SetString(namePower, value.ToString());
            PlayerPrefs.Save();
        }
    }
}