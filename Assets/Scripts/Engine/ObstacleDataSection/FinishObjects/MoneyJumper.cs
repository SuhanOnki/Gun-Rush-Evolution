using DG.Tweening;
using Engine.GameSections;
using UnityEngine;

namespace Engine.ObstacleDataSection.FinishObjects
{
    public class MoneyJumper : MonoBehaviour
    {
        public bool canMove;

        public void SpecialFunc()
        {
            
        }
        void Start()
        {
            canMove = false;
            DOVirtual.DelayedCall(1, (() =>
            {
                transform.DOMove(
                    GameplayMaestro.Instance.moneyTarget.transform.position, 0.35f).SetEase(Ease.InFlash).OnComplete((() =>
                {
                    var i = Random.Range(5, 15);
                    GameplayMaestro.Instance.winMoney += i;
                    PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + i);
                    PlayerPrefs.Save();
                    gameObject.SetActive(false);
                }));
            }));
        }
    }
}