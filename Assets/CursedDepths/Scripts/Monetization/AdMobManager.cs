using UnityEngine;

namespace CursedDepths.Monetization
{
    public class AdMobManager : MonoBehaviour
    {
        [SerializeField] private string appId = "ADMOB_APP_ID";
        [SerializeField] private string bannerId = "ADMOB_BANNER_ID";
        [SerializeField] private string interstitialId = "ADMOB_INTERSTITIAL_ID";

        public void Initialize()
        {
            Debug.Log($"AdMob initialized with {appId}");
        }

        public void ShowBanner()
        {
            Debug.Log($"Show banner {bannerId}");
        }

        public void ShowInterstitial()
        {
            Debug.Log($"Show interstitial {interstitialId}");
        }
    }
}
