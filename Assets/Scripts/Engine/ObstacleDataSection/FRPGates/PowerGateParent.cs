using DG.Tweening;
using Engine.GameSections;
using Engine.PlayerGunSection;
using UnityEngine;
using UnityEngine.Events;

namespace Engine.ObstacleDataSection.FRPGates
{
    public class PowerGateParent : GateParent
    {
        public static UnityEvent<int> OnPowerGateTrigger = new UnityEvent<int>();


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
                        if (gateType == GateParent.GateType.Plus)
                        {
                            GameplayMaestro.Instance.StartEvent($"+{power} Power", Color.green);
                            GunBehavior.gunBehavior.Rotate();
                            GameplayMaestro.Instance.PlaySound(audioEngine.upgradeWeaponGate);
                        }
                        else
                        {
                            GameplayMaestro.Instance.StartEvent($"{power} Power", Color.red);
                            Instantiate(particleStm[0], GunBehavior.gunBehavior.transform.position, Quaternion.identity,
                                other.transform);
                        }
                    }
                    else
                    {
                        GameplayMaestro.Instance.StartEvent($"+{power} Power", Color.green);
                        GunBehavior.gunBehavior.Rotate();
                        GunBehavior.gunBehavior.upgrade.Play();
                        GameplayMaestro.Instance.PlaySound(audioEngine.upgradeWeaponGate);
                        /*Instantiate(particleStm[1], GunController.gunController.transform.position, Quaternion.identity,
                        other.transform);*/
                    }

                    OnPowerGateTrigger?.Invoke(power);
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