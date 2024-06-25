//using Falcon.FalconMediation.Core;

using UnityEngine;

namespace Engine.MergeGamePlaySection
{
    public class CellObject : MonoBehaviour
    {
        public ParticleSystem shineEffect;
        public GameObject adButton;
        public bool isRewarded;
        public BoxCollider selectable;
        public bool isWATCHED;

        private void Start()
        {
            if (isRewarded)
            {
                adButton.SetActive(true);
                selectable.enabled = false;
                shineEffect.gameObject.SetActive(true);
                shineEffect.Play();
            }
        }

        public void SpecialFunc()
        {
        }

        public void GetFree()
        {
            AdManager.adManager.ShowRewarded(Table.Instance.OpenAdError, (() =>
            {
                isWATCHED = true;
                /*FAdLog fAdLog = new FAdLog(AdType.Reward, "On Merge Get Free Spare Part",
                    MergeGamePlayState.Instance.lvl);
                fAdLog.Send();*/
                adButton.SetActive(false);
                Table.SavingGrid?.Invoke();
                selectable.enabled = true;
                shineEffect.gameObject.SetActive(false);
            }));
            /*
            FalconMediation.ShowRewardedVideo((() =>
            {*/
            /*
        }), (() => { Table.Instance.OpenAdError(); }));*/
        }
    }
}