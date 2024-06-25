using DG.Tweening;
using UnityEngine;

namespace Engine.ObstacleDataSection.RedAimData
{
    public class AimAnimatedItem : MonoBehaviour
    {
        [SerializeField] private Vector3 animVector;
        [SerializeField] private GameObject redCircle;
        [SerializeField] private GameObject redCirclePrefab;

        private float duration = .5f;

        public void SpecialFunc()
        {
            
        }
        public void Animate()
        {
            GetComponent<BoxCollider>().enabled = false;
            redCircle.SetActive(false);
            Instantiate(redCirclePrefab, redCircle.transform.position, Quaternion.identity);
            transform.DOLocalRotate(animVector, duration).SetEase(Ease.OutQuint);
        }
    }
}
