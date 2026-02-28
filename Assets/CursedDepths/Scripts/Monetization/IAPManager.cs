using UnityEngine;

namespace CursedDepths.Monetization
{
    public class IAPManager : MonoBehaviour
    {
        [SerializeField] private string[] productIds;

        public void Initialize()
        {
            Debug.Log("IAP initialized");
        }

        public void Purchase(string productId)
        {
            Debug.Log($"Purchase requested: {productId}");
        }
    }
}
