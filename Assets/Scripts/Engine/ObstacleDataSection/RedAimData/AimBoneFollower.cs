using DG.Tweening;
using UnityEngine;

namespace Engine.ObstacleDataSection.RedAimData
{
    public class AimBoneFollower : MonoBehaviour
    {
        [SerializeField] private GameObject[] redCircleFollowers;
        [SerializeField] private GameObject zalyuz;
        [SerializeField] private GameObject[] yakmalyGate;
        [SerializeField] private GameObject[] ochurmeliGate;
        public bool isTutorial;

        private int indexFollower;
        private float zalyuzLength = 0.11f;

        public void SpecialFunc()
        {
            
        }
        private void Start()
        {
            if (isTutorial)
            {
                zalyuzLength = 0.34f;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("RedCircle"))
            {
                Destroy(other.gameObject);
                if (redCircleFollowers.Length > indexFollower)
                {
                    redCircleFollowers[indexFollower].SetActive(true);
                    indexFollower++;
                    if (indexFollower == 3)
                    {
                        yakmalyGate[0].SetActive(true);
                        ochurmeliGate[0].SetActive(false);
                    }

                    if (indexFollower == 6)
                    {
                        yakmalyGate[1].SetActive(true);
                        ochurmeliGate[1].SetActive(false);
                    }

                    if (indexFollower == 9)
                    {
                        yakmalyGate[2].SetActive(true);
                        ochurmeliGate[2].SetActive(false);
                    }

                    zalyuz.transform.DOScaleZ(zalyuz.transform.localScale.z - zalyuzLength, 0.2f).SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            if (zalyuz.transform.localScale.z < 0)
                            {
                                zalyuz.SetActive(false);
                            }
                        });
                }
            }
        }

        public void OffAllObjects()
        {
            for (int i = 0; i < 3; i++)
            {
                if (yakmalyGate.Length > i)
                {
                    yakmalyGate[i].SetActive(false);
                }

                ochurmeliGate[i].SetActive(false);
            }

            zalyuz.SetActive(false);

            transform.parent.DOMoveY(-1.5f, 1f).SetEase(Ease.Linear);
        }
    }
}