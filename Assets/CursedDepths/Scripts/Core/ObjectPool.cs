using System.Collections.Generic;
using UnityEngine;

namespace CursedDepths.Core
{
    public class ObjectPool
    {
        private readonly Poolable prefab;
        private readonly Queue<Poolable> pool = new Queue<Poolable>();
        private readonly Transform parent;

        public ObjectPool(Poolable prefab, int size, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
            for (int i = 0; i < size; i++)
            {
                CreateInstance();
            }
        }

        public Poolable Get()
        {
            if (pool.Count == 0)
            {
                CreateInstance();
            }

            var instance = pool.Dequeue();
            instance.gameObject.SetActive(true);
            return instance;
        }

        public void ReturnToPool(Poolable poolable)
        {
            poolable.gameObject.SetActive(false);
            poolable.transform.SetParent(parent, false);
            pool.Enqueue(poolable);
        }

        private void CreateInstance()
        {
            var instance = Object.Instantiate(prefab, parent);
            instance.gameObject.SetActive(false);
            pool.Enqueue(instance);
        }
    }
}
