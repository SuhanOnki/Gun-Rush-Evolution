using System;
using System.Collections.Generic;
using DG.Tweening;
using Engine.GameSections;
using Engine.PlayerGunSection;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Engine.ObstacleDataSection.FRPGates
{
    public class GateParent : MonoBehaviour
    {
        [SerializeField] protected int power;
        [SerializeField] protected int increaseCount;
        [SerializeField] protected TextMeshProUGUI increaseNumber;
        [SerializeField] protected TextMeshProUGUI powerText;
        [SerializeField] protected GameObject particle;
        public GateType gateType;
        public List<WallGate> gateObstacle;
        [SerializeField] protected FireGateParent.VariantGate variantGate;
        [SerializeField] protected GameObject[] particleStm;
        public MeshRenderer meshRenderer;
        public bool setManually;

        public int defenderState;
        public int defenderStateMax;
        protected bool oneTrigger;
        protected AudioEngine audioEngine;
        protected Vector3 originalScale;

        public void SpecialFunc()
        {
            
        }
        void Start()
        {
            audioEngine = AudioEngine.audioEngine;
            particle.transform.SetParent(transform.parent.parent);
            if (!setManually)
            {
                int isMinus = Random.Range(1, 100);
                if (isMinus > 0 && isMinus < 50)
                {
                    power = Random.Range(-15, 3);
                }
                else
                {
                    power = Random.Range(10, 3);
                }

                increaseCount = Random.Range(1, 3);
                if (increaseCount == 0)
                {
                    increaseCount = Random.Range(1, 5);
                }

                if (power < -5)
                {
                    increaseCount = Random.Range(2, 5);
                }
            }

            powerText.text = power > 0 ? "+" + power : power.ToString();
            increaseNumber.text = increaseCount > 0 ? "+" + increaseCount : increaseCount.ToString();
            defenderStateMax = gateObstacle.Count;
            if (defenderStateMax <1)
            {
                oneTrigger = true;
            }
            meshRenderer = GetComponent<MeshRenderer>();
            originalScale = transform.localScale;
        }

        void Update()
        {
            if (power > 0 && gateType != GateType.Plus)
            {
                gateType = GateType.Plus;
                for (var i = 0; i < meshRenderer.materials.Length; i++)
                {
                    meshRenderer.materials[i].DOColor(GameplayMaestro.Instance.plus[i].color, 0.5f).SetEase(Ease.Linear);
                }
            }
            else if (power <= 0 && gateType != GateType.Minus)
            {
                gateType = GateType.Minus;
                for (var i = 0; i < meshRenderer.materials.Length; i++)
                {
                    meshRenderer.materials[i].DOColor(GameplayMaestro.Instance.minus[i].color, 0.5f).SetEase(Ease.Linear);
                }
            }
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Arrow"))
            {
                GameplayMaestro.Instance.PlaySound(audioEngine.bulletHitAudioClip);
                transform.DOShakeScale(.15f, .75f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    transform.localScale = originalScale;
                });
                if (defenderState <= defenderStateMax) defenderState++;
                if (defenderState <= defenderStateMax) gateObstacle[defenderState - 1].gameObject.SetActive(false);
                if (defenderState <= defenderStateMax)
                {
                    // Instantiate(particle, transform.position, Quaternion.identity, transform);
                    particle.GetComponent<ParticleSystem>().Play();
                }

                if (defenderState == defenderStateMax + 1)
                {
                    if (!oneTrigger)
                    {
                        oneTrigger = true;
                    }

                    if (power < 0)
                    {
                        power += Math.Abs(increaseCount);
                    }
                    else
                    {
                        power += increaseCount;
                    }

                    powerText.text = power > 0 ? "+" + power : power.ToString();
                }

                other.gameObject.SetActive(false);
                GunBehavior.gunBehavior.arrowPooler.ReturnObjectToPool(other.gameObject.GetComponent<Bullet.Bullet>());
            }
        }

        public enum VariantGate
        {
            Beton,
            Tagta,
            Bosh
        }

        public enum GateType
        {
            None,
            Minus,
            Plus
        }
    }
}