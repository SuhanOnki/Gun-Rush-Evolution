using DG.Tweening;
using Engine.GameSections;
using UnityEngine;

namespace Engine.ObstacleDataSection.FinishObjects
{
    public class StoneMultiplierGround : MonoBehaviour
    {
        public float scaleValue;
        public MeshRenderer meshRenderer;
        public GameObject text;
        private Vector3 originScale;

        public void SpecialFunc()
        {
            
        }
        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            GameplayMaestro.Instance.xGrounds.Add(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                originScale = text.transform.localScale;
                GameplayMaestro.Instance.playerFinishHalfRoad = true;
                text.transform.DOScale(scaleValue, 0.5f).SetEase(Ease.Linear);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                text.transform.DOScale(originScale, 0.5f).SetEase(Ease.Linear);
            }
        }
    }
}