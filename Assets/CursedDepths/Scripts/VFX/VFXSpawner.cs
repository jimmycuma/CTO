using UnityEngine;
using CursedDepths.Core;

namespace CursedDepths.VFX
{
    public class VFXSpawner : MonoBehaviour
    {
        [SerializeField] private string poolKey = "ImpactVFX";

        public void Spawn(Vector3 position)
        {
            var vfx = GameSession.Instance.PoolManager.Spawn<Poolable>(poolKey, position, Quaternion.identity);
            if (vfx != null)
            {
                GameSession.Instance.PoolManager.Despawn(vfx);
            }
        }
    }
}
