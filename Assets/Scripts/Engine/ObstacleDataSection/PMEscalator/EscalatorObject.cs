using System;
using Engine.PlayerGunSection;
using UnityEngine;

namespace Escalator
{
    public class EscalatorObject : MonoBehaviour
    {
        public EscalatorType escalatorType;

        public void SpecialFunc()
        {
            
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.gameObject.GetComponent<GunMovement>();
                if (escalatorType == EscalatorType.Minus)
                {
                    player.gameplayMaestro.speedPlayer /= 2f;
                }

                if (escalatorType == EscalatorType.Plus)
                {
                    player.gameplayMaestro.speedPlayer *= 2f;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.gameObject.GetComponent<GunMovement>();
                if (escalatorType == EscalatorType.Minus)
                {
                    player.gameplayMaestro.speedPlayer = player.gameplayMaestro.defaultSpeed;
                }

                if (escalatorType == EscalatorType.Plus)
                {
                    player.gameplayMaestro.speedPlayer = player.gameplayMaestro.defaultSpeed;
                }
            }
        }
    }
}

public enum EscalatorType
{
    None,
    Minus,
    Plus
}