using DG.Tweening;
using Engine.MergeGamePlaySection;
using UnityEngine;

namespace UISection
{
    public class GunPreview : MonoBehaviour
    {
        [SerializeField] private Transform sun;
        [SerializeField] private Transform gun;

        private void Start()
        {
            gun.DORotate(new Vector3(0f, 0f, 30f), .5f).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        }

        public void SpecialFunc()
        {
            
        }
        private void Update()
        {
            sun.Rotate(0, 0, -10 * Time.deltaTime);
        }

        public void Cancel()
        {
            if (MergeGamePlayState.Instance.preventShowNewItem)
            {
                MergeGamePlayState.Instance.preventShowNewItem = false;
            }

            GunPartHolder gunPartHolder = GunPartHolder.instance;
            for (var i = 0; i < gunPartHolder.changePowers.Length; i++)
            {
                gunPartHolder.changePowers[i].Init(MergeGamePlayState.Instance.guns[MergeGamePlayState.Instance.gunLevel - 1]);
            }

            this.gameObject.SetActive(false);
        }
    }
}