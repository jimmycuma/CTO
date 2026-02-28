using System.Collections.Generic;
using UnityEngine;

namespace CursedDepths.Core
{
    public class PoolManager : MonoBehaviour
    {
        [System.Serializable]
        public class PoolDefinition
        {
            public string key;
            public Poolable prefab;
            public int size = 8;
        }

        [SerializeField] private List<PoolDefinition> pools = new List<PoolDefinition>();

        private readonly Dictionary<string, ObjectPool> poolLookup = new Dictionary<string, ObjectPool>();

        private void Awake()
        {
            foreach (var pool in pools)
            {
                if (pool.prefab == null || string.IsNullOrEmpty(pool.key))
                {
                    continue;
                }

                var objectPool = new ObjectPool(pool.prefab, pool.size, transform);
                poolLookup[pool.key] = objectPool;
            }
        }

        public T Spawn<T>(string key, Vector3 position, Quaternion rotation) where T : Poolable
        {
            if (!poolLookup.TryGetValue(key, out var pool))
            {
                return null;
            }

            var obj = pool.Get();
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.OnSpawned();
            return obj as T;
        }

        public void Despawn(Poolable poolable)
        {
            if (poolable == null)
            {
                return;
            }

            if (poolLookup.TryGetValue(poolable.PoolKey, out var pool))
            {
                pool.ReturnToPool(poolable);
            }
            else
            {
                Destroy(poolable.gameObject);
            }
        }
    }
}
