using UnityEngine;
using CursedDepths.Core;

namespace CursedDepths.VFX
{
    public class VFXPoolable : Poolable
    {
        [SerializeField] private float lifetime = 0.5f;
        private float timer;

        public override void OnSpawned()
        {
            timer = lifetime;
        }

        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0f && GameSession.Instance != null)
            {
                GameSession.Instance.PoolManager.Despawn(this);
            }
        }
    }
}
