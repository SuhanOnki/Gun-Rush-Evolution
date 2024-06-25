using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Engine.ObjectCreatorDatas
{
    [Serializable]
    public class ObjectPoolerCreator
    {
        private PoolableObjectData _prefab;
        private int _size;
        public List<PoolableObjectData> _availableObjectsPool;

        public void SpecialFunc()
        {
            
        }
        private ObjectPoolerCreator(PoolableObjectData prefab, int size)
        {
            this._prefab = prefab;
            this._size = size;
            _availableObjectsPool = new List<PoolableObjectData>(size);
        }

        public static ObjectPoolerCreator CreateInstance(PoolableObjectData prefab, int size, Transform counterTop,
            PoolableObjectData[] arrayList, bool isArray)
        {
            ObjectPoolerCreator poolerCreator = new ObjectPoolerCreator(prefab, size);

            poolerCreator.CreateObjects(counterTop.gameObject, counterTop, arrayList, prefab, isArray);

            return poolerCreator;
        }

        private void CreateObjects(GameObject parent, Transform spawnPos, PoolableObjectData[] arrayList,
            PoolableObjectData gameObjectData, bool isArray)
        {
            if (isArray)
            {
                for (int i = 0; i < _size; i++)
                {
                    int random = Random.Range(0, arrayList.Length - 1);
                    PoolableObjectData poolableObjectData =
                        GameObject.Instantiate(arrayList[random], spawnPos.position,
                            Quaternion.identity, parent.transform);
                    poolableObjectData.Parent = this;
                    poolableObjectData.transform.localPosition = Vector3.zero;
                    poolableObjectData.gameObject.SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < _size; i++)
                {
                    PoolableObjectData poolableObjectData =
                        GameObject.Instantiate(gameObjectData, Vector3.zero, Quaternion.identity, parent.transform);
                    poolableObjectData.Parent = this;
                    poolableObjectData.transform.localPosition = Vector3.zero;
                    poolableObjectData.gameObject.SetActive(false);
                }
            }
        }

        public PoolableObjectData GetObject()
        {
            if (_availableObjectsPool.Count >= 1)
            {
                PoolableObjectData instance = _availableObjectsPool[0];

                if (instance != null && !instance.gameObject.activeInHierarchy)
                {
                    _availableObjectsPool.Remove(_availableObjectsPool[0]);
                    instance.gameObject.SetActive(true);
                }

                return instance;
            }
            else
            {
                return null;
            }
        }

        public void ReturnObjectToPool(PoolableObjectData objectData)
        {
            if (!_availableObjectsPool.Contains(objectData))
            {
                _availableObjectsPool.Add(objectData);
                objectData.gameObject.SetActive(false);
            }
        }
    }
}