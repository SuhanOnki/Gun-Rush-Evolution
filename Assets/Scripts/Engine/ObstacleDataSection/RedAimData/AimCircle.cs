using DG.Tweening;
using Engine.GameSections;
using UnityEngine;

namespace Engine.ObstacleDataSection.RedAimData
{
    public class AimCircle : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float riderTextureXPos;
        public Quaternion starterRotation;
        private GameplayMaestro gameplayMaestro;

        private bool onRiderTexture;

        public void SpecialFunc()
        {
            
        }
        private void Start()
        {
            gameplayMaestro = GameplayMaestro.Instance;
            starterRotation = transform.localRotation;
            transform.DOMoveY(transform.position.y + .7f, 0.15f).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOMoveY(0.15f, 0.15f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    transform.DOMoveX(riderTextureXPos, 0.05f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        transform.DOLocalRotate(new Vector3(90, 180, 45), 0.15f).SetEase(Ease.InElastic);

                        onRiderTexture = true;
                    });
                });
            });

            transform.DORotate(new Vector3(90, 180, 45), 0.15f).SetEase(Ease.InElastic);
        }

        private void Update()
        {
            if (onRiderTexture && !gameplayMaestro.playerFinishHalfRoad)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, starterRotation, Time.deltaTime * 5);
                transform.position += Vector3.forward * speed * 1.5f * Time.deltaTime;
            }
        }
    }
}