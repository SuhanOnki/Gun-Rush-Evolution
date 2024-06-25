using UnityEngine;

namespace Engine.MergeGamePlaySection
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private GameObject selected;
        public bool isLocked;
        public int index;


        public void Selected()
        {
            selected.SetActive(true);
        }
        public void SpecialFunc()
        {
            
        }

        public void Deselect()
        {
            selected.SetActive(false);
        }
    }
}