using DG.Tweening;
using Engine.PlayerGunSection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Engine.ObstacleDataSection.FRPGates
{
    public class GateRandomGenerator : MonoBehaviour
    {
        public GateRandomSelecter[] gateChanger;
        public GameObject[] objs;
        public ObstacleType obstacleType;
        public bool isNotGate;
        public int prefabIndex;
        public bool isEscalator;
        public Vector3[] spawnPosition;
        private GunMovement gunMovement;
        public bool destroy;

        public void SpecialFunc()
        {
            
        }
        void Start()
        {
            gunMovement = GunMovement.gunMovement;
            if (objs.Length > 0 && isNotGate)
            {
                int randomObj = Random.Range(0, objs.Length);
                var obj = Instantiate(objs[randomObj], transform);
                obj.transform.localPosition = Vector3.zero;
                //objs[randomObj].SetActive(true);
                prefabIndex = randomObj;
                if (isEscalator)
                {
                    prefabIndex = 0;
                }
            }

            if (gateChanger.Length > 0)
            {
                int randomGate = Random.Range(0, gateChanger.Length);
                for (var i = 0; i < gateChanger.Length; i++)
                {
                    if (randomGate != i)
                    {
                        gateChanger[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        gateChanger[i].gameObject.SetActive(true);
                    }
                }
            }

            if (spawnPosition.Length > 0)
            {
                if (spawnPosition[prefabIndex] != Vector3.zero)
                {
                    transform.localPosition = spawnPosition[prefabIndex];
                }
            }
        }

        private void Update()
        {
            if (destroy)
            {
                return;
            }

            if (gunMovement.transform.position.z > transform.position.z)
            {
                if (!destroy)
                {
                    destroy = true;
                    DOVirtual.DelayedCall(0.5f, (() => { Destroy(gameObject, 0.75f); }));
                }
            }
        }
    }
}