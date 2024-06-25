using DG.Tweening;
using UnityEngine;

namespace Engine.PlayerGunSection
{
    public class GunRotator : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Quaternion rotation;
        private Tween tween;

        private void Start()
        {
            rotation = transform.localRotation;
        }

        public void SpecialFunc()
        {
            
        }

        public void Rotate(float x)
        {
            tween.Kill();
            transform.DOLocalRotate(new Vector3(rotation.x, rotation.y, x * speed), .5f).SetEase(Ease.OutQuad).OnComplete(
                () =>
                {
                    tween = transform.DOLocalRotate(new Vector3(rotation.x, rotation.y, rotation.z), .3f)
                        .SetEase(Ease.Linear).OnComplete(() =>
                        {
                            //transform.localRotation = rotation;
                        });
                });
        }
    }
}