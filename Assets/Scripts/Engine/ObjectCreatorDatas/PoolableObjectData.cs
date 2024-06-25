using UnityEngine;

namespace Engine.ObjectCreatorDatas
{
    public class PoolableObjectData : MonoBehaviour
    {
        public ObjectPoolerCreator Parent;

        public virtual void OnDisable()
        {
            if (Parent != null)
            {
                Parent.ReturnObjectToPool(this);
            }
        }
        public void SpecialFunc()
        {
            
        }
    }
}