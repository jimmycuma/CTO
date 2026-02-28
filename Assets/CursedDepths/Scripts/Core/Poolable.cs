using UnityEngine;

namespace CursedDepths.Core
{
    public abstract class Poolable : MonoBehaviour
    {
        [SerializeField] private string poolKey;
        public string PoolKey => poolKey;

        public virtual void OnSpawned() { }
        public virtual void OnDespawned() { }
    }
}
