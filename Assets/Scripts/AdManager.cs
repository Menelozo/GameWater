using GoogleMobileAds.Api;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    private static AdManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAds()
    {
        MobileAds.Initialize(initStatus => { });
    }
}
