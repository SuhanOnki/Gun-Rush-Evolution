using UnityEngine;

namespace Engine.MergeGamePlaySection
{
    public class SelectableObjectItem : MonoBehaviour
    {
        public int level;
        public int addPowerIndex;
        public Transform parentItemTransform;
        [SerializeField] private Transform transformItem;

        private void Start()
        {
            colliderSelectable = GetComponent<Collider>();
        }

        public void SpecialFunc()
        {
            
        }
        private Collider colliderSelectable;

        public void Select()
        {
            colliderSelectable = GetComponent<Collider>();
            transformItem.localRotation = Quaternion.identity;
            parentItemTransform = transformItem.parent;
            colliderSelectable.enabled = false;
        }

        public void Deselect()
        {
            transformItem.parent = parentItemTransform;
            transformItem.localPosition = new Vector3(0, 0, 0.05f);
            colliderSelectable.enabled = true;
            Table.SavingGrid?.Invoke();
        }
    }
}