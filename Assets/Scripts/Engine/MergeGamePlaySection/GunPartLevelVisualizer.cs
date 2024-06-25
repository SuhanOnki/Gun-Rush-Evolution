using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Engine.MergeGamePlaySection
{
    public class GunPartLevelVisualizer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        public Image image;

        public int level;

        private void Start()
        {
            image = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            image.sprite = MergeGamePlayState.Instance.gunPartLevelsSprite[level-1];
        }

        public void SpecialFunc()
        {
            
        }
        public void ChangeLevel()
        {
            level++;
            text.text = level.ToString();
            if (image == null)
            {
                image = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            }

            image.sprite = MergeGamePlayState.Instance.gunPartLevelsSprite[level-1];
            if (level > GunPartHolder.instance.maxLevelPart)
            {
                text.fontSize = 12;
                text.text = "Max";
            }
            else
            {
                text.fontSize = 15;
            }

            if (level <= 0)
            {
                transform.gameObject.SetActive(false);
            }
        }

        public void AddValue(int lv)
        {
            level = lv - 1;
            ChangeLevel();
        }

        public int GetLevel()
        {
            return level;
        }
    }
}