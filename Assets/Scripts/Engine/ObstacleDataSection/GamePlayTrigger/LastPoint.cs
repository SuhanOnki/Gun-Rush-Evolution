using DG.Tweening;
using Engine.GameSections;
using Engine.PlayerGunSection;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Engine.ObstacleDataSection.GamePlayTrigger
{
    public class LastPoint : MonoBehaviour
    {
        public static UnityEvent PlayerWentFinish = new UnityEvent();

        [SerializeField] private GameObject cam;
        public GameObject stoneCam;
        public GameObject finishZombieCam;
        [SerializeField] private Transform playerMovement;
        public GameObject moneyTarget;

        private void Start()
        {
            transform.GetChild(1).gameObject.SetActive(true);
            DOVirtual.DelayedCall(1, (() =>
            {
                if (GameplayMaestro.Instance.levelType == GameplayMaestro.LevelType.Stone)
                {
                    transform.GetChild(1).gameObject.SetActive(false);
                }
            }));
        }

        public void SpecialFunc()
        {
            
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Gun"))
            {
                transform.GetComponent<Collider>().enabled = false;
                cam.SetActive(false);
                if (GameplayMaestro.Instance.levelType == GameplayMaestro.LevelType.KillZombie)
                {
                    SwipeSensor.swipeSensor.swipeDetectionValue = 0.6f;
                    GunBehavior.gunBehavior.reBoot = true;
                    GunBehavior.gunBehavior.transform.gameObject.GetComponent<BoxCollider>().enabled = true;
                    // GunController.gunController.fogs.SetActive(false);
                    GameplayMaestro.Instance.playerState = GameplayMaestro.State.Finish;
                    /*other.transform.localPosition =
                    new Vector3(other.transform.localPosition.x, 0.1f, other.transform.localPosition.z);*/
                    stoneCam.gameObject.SetActive(false);
                    finishZombieCam.SetActive(true);
                    ;
                    playerMovement.DOMoveX(0, .5f).SetEase(Ease.Linear);
                    float range = Random.Range(0.4f, 0.75f);
                    //playerMovement.DOMoveZ(transform.position.z, .5f).SetEase(Ease.Linear);
                    DOVirtual.DelayedCall(1f, (() => { PlayerWentFinish?.Invoke(); }));
                    transform.GetChild(0).transform.gameObject.SetActive(true);
                }

                if (GameplayMaestro.Instance.levelType == GameplayMaestro.LevelType.Stone)
                {
                    DOVirtual.DelayedCall(2.5f, (() => { PlayerWentFinish?.Invoke(); }));
                }
            }
        }
    }
}