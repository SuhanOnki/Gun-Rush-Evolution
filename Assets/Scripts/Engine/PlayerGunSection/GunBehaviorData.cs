using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Engine.PlayerGunSection
{
    public class GunBehaviorData : MonoBehaviour
    {
        public Transform defaultShootPosition;
        public ParticleSystem particleStm;
        public ParticleSystem[] muzzels;
        [FormerlySerializedAs("gunController")] public GunBehavior gunBehavior;
        public Transform[] offsets;
        public int index;
        public bool setDefaultOffset;
        public Vector3 center;
        public Vector3 size;
        public Vector3 centerWithoutMuffer;
        public Vector3 sizeWithoutMuffer;
        public GameObject[] parts1;
        public GameObject[] parts2;
        public GameObject[] parts3;

        private void OnEnable()
        {
            particleStm = muzzels[index];
        }

        public void SpecialFunc()
        {
            
        }
        public void InitGunData(int particleIndex)
        {
        
            if (offsets.Length > particleIndex)
            {
                particleStm.transform.position = offsets[particleIndex].position;
            }
            else
            {
                if (offsets.Length >= 1)
                {
                    particleStm.transform.position = offsets[^1].position;
                }
            }
        }

        public void Shoot()
        {
            gunBehavior.ShootBullet();
        }

        public void AddPartGun(int index)
        {
            if (index == 1)
            {
                for (int i = 0; i < parts1.Length; i++)
                {
                    AnimationPart(parts1[i]);
                }

                if (offsets.Length > 0)
                {
                    particleStm.transform.position = offsets[index].position;
                }
            }
            else if (index == 2)
            {
                for (int i = 0; i < parts2.Length; i++)
                {
                    AnimationPart(parts2[i]);
                }

                if (offsets.Length > 0)
                {
                    particleStm.transform.position = offsets[index].position;
                }
            }
            else
            {
                for (int i = 0; i < parts3.Length; i++)
                {
                    AnimationPart(parts3[i]);
                }

                if (offsets.Length > 0)
                {
                    particleStm.transform.position = offsets[2].position;
                }
            }
        }

        private void AnimationPart(GameObject gm)
        {
            Vector3 vector = gm.transform.localScale;
            gm.transform.localScale = Vector3.zero;
            gm.SetActive(true);
            /*gm.transform.DOShakeScale(1f,.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            gm.transform.localScale = vector;
        });*/
            gm.transform.DOScale(vector, 1f).SetEase(Ease.Linear).OnComplete(() => { gm.transform.localScale = vector; });
        }
    }
}