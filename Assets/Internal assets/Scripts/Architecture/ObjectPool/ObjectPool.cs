using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Internal_assets.Scripts.Architecture.ObjectPool
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private T Prefab { get; }
        private bool AutoExpand { get; }
        private Transform Container {get; }
    
        private List<T> _pool;
    
        public ObjectPool(T prefab, int poolSize, bool autoExpand, Transform container = null)
        {
            this.Prefab = prefab;
            AutoExpand = autoExpand;
            this.Container = container;
        
            this.CreatePool(poolSize);
        }

        private void CreatePool(int poolSize)
        {
            this._pool = new List<T>();
            for (var i = 0; i < poolSize; ++i)
            {
                this.CreateObject();
            }
        }

        private T CreateObject(bool isActiveByDefault = false)
        {
            var createdObject = Object.Instantiate(this.Prefab, this.Container);
            createdObject.gameObject.SetActive(isActiveByDefault);
            this._pool.Add(createdObject);
            return createdObject;
        }

        private bool HasFreeElement(out T element)
        {
            foreach (var obj in _pool.Where(obj => !obj.gameObject.activeInHierarchy))
            {
                obj.gameObject.SetActive(true);
                element = obj;
                return true;
            }

            element = null;
            return false;
        }

        public T GetFreeElement()
        {
            if (this.HasFreeElement(out var element))
                return element;
            if (this.AutoExpand)
                return this.CreateObject(true);
        
            throw new Exception($"There are no such {typeof(T)} objects in pool");
        }
    
    }
}
