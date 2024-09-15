using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public BannerAd bannerAd;

    public void PlayGame()
    {
        Score.ResetRestarts();
        Score.ResetSavedLevel();
        bannerAd.DestroyBanner();
        SceneManager.LoadSceneAsync(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        if (Score.GetRestarts() > 0)
        {
            Score.RemoveRestart();
            int savedLevel = PlayerPrefs.GetInt("saveLevel");
            SceneManager.LoadSceneAsync(savedLevel + 1);
        }
    }
}
