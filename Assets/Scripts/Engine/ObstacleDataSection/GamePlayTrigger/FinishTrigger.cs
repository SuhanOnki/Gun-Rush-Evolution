using UnityEngine;

namespace Engine.ObstacleDataSection.GamePlayTrigger
{
    public class FinishTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("RedAim"))
            {
                other.gameObject.SetActive(false);
            }
        }
        public void SpecialFunc()
        {
            
        }
    }
}