using GoogleMobileAds.Api;
using UnityEngine;

public class BannerAd : MonoBehaviour
{
    private BannerView bannerView;

    void Start()
    {
        RequestBanner();
    }

    private void RequestBanner()
    {
        string adUnitId = "ca-app-pub-8292387101575753/7202370690";
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest();
        bannerView.LoadAd(request);
    }

    public void DestroyBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
}
