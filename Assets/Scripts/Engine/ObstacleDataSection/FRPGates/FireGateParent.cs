using DG.Tweening;
using Engine.GameSections;
using Engine.PlayerGunSection;
using UnityEngine;
using UnityEngine.Events;

namespace Engine.ObstacleDataSection.FRPGates
{
    public class FireGateParent : global::Engine.ObstacleDataSection.FRPGates.GateParent
    {
        public static UnityEvent<float> OnFireRateGateTrigger = new UnityEvent<float>();

        public void SpecialFunc()
        {
            
        }

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.CompareTag("Player"))
            {
                GameplayMaestro.Instance.getGateTime = Time.time;

                if (defenderState == defenderStateMax + 1)
                {
                    if (power < 0)
                    {
                        if (gateType == GateType.Plus)
                        {
                            GameplayMaestro.Instance.PlaySound(audioEngine.upgradeWeaponGate);
                            GameplayMaestro.Instance.StartEvent($"+{power} Fire Rate", Color.green);
                            GunBehavior.gunBehavior.Rotate();
                        }
                        else
                        {
                            GameplayMaestro.Instance.StartEvent($"{power} Fire Rate", Color.red);
                            Instantiate(particleStm[0], transform.position, Quaternion.identity, other.transform);
                        }
                    }
                    else
                    {
                        GameplayMaestro.Instance.PlaySound(audioEngine.upgradeWeaponGate);
                        GunBehavior.gunBehavior.Rotate();
                        GunBehavior.gunBehavior.upgrade.Play();
                        GameplayMaestro.Instance.StartEvent($"+{power} Fire Rate", Color.green);
                        Instantiate(particleStm[1], transform.position, Quaternion.identity, other.transform);
                    }

                    OnFireRateGateTrigger?.Invoke(power);
                    transform.DOMoveY(-1, 1f).SetEase(Ease.Linear);
                }
                else if (!oneTrigger)
                {
                    oneTrigger = true;
                    other.transform.DOMoveZ(transform.position.z - 1.5f, 0.3f).SetEase(Ease.Linear);
                }
            }
        }
    }
}