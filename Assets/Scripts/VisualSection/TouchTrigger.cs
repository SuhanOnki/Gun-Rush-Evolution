using DG.Tweening;
using UnityEngine;

namespace UISection
{
    public class TouchTrigger : MonoBehaviour
    {
        public bool isWin;

        private void Start()
        {
            if (!isWin)
            {
                transform.DOScale(transform.localScale * 1.3f, .6f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);
                return;
            }

            transform.DOScale(transform.localScale * 1.3f, .6f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);
            DOVirtual.DelayedCall(1, (() => { gameObject.SetActive(false); }));
        }

        public void SpecialFunc()
        {
        }
    }
}