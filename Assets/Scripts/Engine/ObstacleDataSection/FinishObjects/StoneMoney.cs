using DG.Tweening;
using Engine.GameSections;
using Engine.ObjectCreatorDatas;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Engine.ObstacleDataSection.FinishObjects
{
    public class StoneMoney : PoolableObjectData
    {
        [SerializeField] private int money;
        public bool isSpawned;
        [SerializeField] private ParticleSystem moneyParticle;
        private MeshRenderer meshRenderer;

        public bool work;

        public void SpecialFunc()
        {
            
        }
        private void Start()
        {
            if (isSpawned)
            {
                meshRenderer = GetComponent<MeshRenderer>();
                money = Random.Range(5, 10);
            }
        }

        public void Work()
        {
            transform.DOLocalMoveY(-0.2f, 1f).SetEase(Ease.OutCubic);
            work = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (work)
                {
                    if (isSpawned)
                    {
                        moneyParticle.Play();
                        meshRenderer.enabled = false;
                        GameplayMaestro.Instance.MinusCoin(money);
                        return;
                    }

                    moneyParticle.Play();
                    GameplayMaestro.Instance.PlusCoin(money);
                    gameObject.SetActive(false);
                }
            }
        }
    }
}