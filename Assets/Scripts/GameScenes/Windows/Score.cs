using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public static class Score
{
    private static int lifes;

    public static void InitializeStatic(int lifeAmount)
    {
        lifes = lifeAmount;
    }

    public static int GetLifes()
    {
        return lifes;
    }

    private static void SetLifes(int lifesChange)
    {
        lifes += lifesChange;
        ScoreWindow.UpdateLivesStatic();
    }

    public static void AddLifes()
    {
        SetLifes(1);
    }

    public static void RemoveLifes(int lifesChange, PlayerControl player)
    {
        SetLifes(-lifesChange);
        if (lifes < 0)
        {
            player.PlayerDied();
        }
    }

    public static int GetRestarts()
    {
        int restarts = PlayerPrefs.GetInt("restart");
        return restarts;
    }

    private static void SetRestarts(int restartsChange)
    {
        PlayerPrefs.SetInt("restart", GetRestarts() + restartsChange);
        PlayerPrefs.Save();
    }

    public static void RemoveRestart()
    {
        SetRestarts(-1);
    }

    public static void ResetRestarts()
    {
        PlayerPrefs.SetInt("restart", 10);
        PlayerPrefs.Save();
    }

    private static void SetSavedLevel(int levelNumber, int levelChange)
    {
        PlayerPrefs.SetInt("saveLevel", levelNumber + levelChange);
        PlayerPrefs.Save();
    }

    public static void AddSavedLevel(int levelNumber)
    {
        SetSavedLevel(levelNumber, 1);
    }

    public static void ResetSavedLevel()
    {
        PlayerPrefs.SetInt("saveLevel", 1);
        PlayerPrefs.Save();
    }
}