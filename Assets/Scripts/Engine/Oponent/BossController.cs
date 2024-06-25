using DG.Tweening;
using UnityEngine;

namespace Engine.Oponent
{
    public class BossController : MonoBehaviour
    {

        [SerializeField] private GameObject particleMoney;

        private Vector3 scale;

        public void SpecialFunc()
        {
            
        }
        private void Start()
        {
            scale = transform.localScale;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Arrow"))
            {
                transform.DOScale(transform.localScale * 0.93f, 0.05f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                {
                    transform.localScale = scale;
                });
                other.gameObject.SetActive(false);
                Instantiate(particleMoney, transform.position, Quaternion.identity);
            }
        }
    }
}