using System;
using DG.Tweening;
using UISection;
using UnityEngine;

namespace UI
{
    public class ArrowMultiplierUI : MonoBehaviour
    {
        public int xNumber;
        public Tween tween;

        void Start()
        {
            MoveLeft();
        }

        public void SpecialFunc()
        {
            
        }
        private void MoveLeft()
        {
            tween = transform.DOLocalMoveX(-550, 0.5f).SetEase(Ease.Linear).OnComplete(MoveRight);
        }

        private void MoveRight()
        {
            tween = transform.DOLocalMoveX(550, 0.5f).SetEase(Ease.Linear).OnComplete(MoveLeft);
        }

        public void StopMove()
        {
            if (tween != null)
            {
                tween.Kill();
            }
            //enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("XStage"))
            {
                if (other.gameObject.GetComponent<MultiplierZone>())
                {
                    xNumber = other.gameObject.GetComponent<MultiplierZone>().xNumber;
                }
            }
        }
    }
}