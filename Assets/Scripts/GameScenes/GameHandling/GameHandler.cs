using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;
    private LevelGrid levelGrid;
    [SerializeField] private PlayerControl player;
    [SerializeField] private TilemapManager tilemapManager;
    [SerializeField] private EnemyManager enemyManager; 

    [SerializeField] LevelConfiguration levelConfiguration;
    internal static List<Vector3Int> TileWorldPositions;
    internal static Vector2Int TileWorldSize;
    internal static int LevelNumber;
    internal static bool IsMaxLevel;

    private static State state;
    internal enum State
    {
        Paused,
        Active,
        Dead,
        Win,
    }

    private void Awake()
    {
        instance = this;
        state = State.Active;
        TileWorldSize = levelConfiguration.TileWorldSize;
        TileWorldPositions = levelConfiguration.TileWorldPositions;
        LevelNumber = levelConfiguration.LevelNumber;
        IsMaxLevel = levelConfiguration.MaxLevel;
        Score.InitializeStatic(levelConfiguration.LifeAmount);
        Progress.InitializeStatic(tilemapManager.TilemapBackground, levelConfiguration.WinExpectation, levelConfiguration.FoodStep);
    }

    private void Start()
    {
        levelGrid = new LevelGrid(tilemapManager);
        player.Setup(levelGrid, tilemapManager);
        tilemapManager.Setup(levelGrid, player, enemyManager);
    }

    private void Update()
    {
        if (state == State.Paused || state==State.Active)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (state == State.Paused)
                {
                    GameHandler.ResumeGame(player);
                }
                else
                {
                    GameHandler.PauseGame(player);
                }
            }
        }
        else if (state == State.Win)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                NextLevel();
            }
        }
        else if (state == State.Dead)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ReloadScene();
            }
        }
    }

    public static void NextLevel()
    {
        if (!IsMaxLevel)
        {
            SoundManager.PlaySound(SoundManager.Sound.Click);
            Loader.LoadScene("Level" + (LevelNumber + 1).ToString());
        }
        else
        {
            FinishGame();       
        }
    }

    public static void FinishGame()
    {
        Loader.LoadScene("Ending");
    }

    public static void ReloadScene()
    {
        if (Score.GetRestarts()>0)
        {
            SoundManager.PlaySound(SoundManager.Sound.Click);
            Score.RemoveRestart();
            Loader.LoadScene("Level" + LevelNumber.ToString());
        }
        else
        {
            SoundManager.PlaySound(SoundManager.Sound.BadClick);
        }
    }

    public static void GoToMainMenu()
    {
        Loader.LoadScene("MainMenu");
        SoundManager.PlaySound(SoundManager.Sound.Click);
    }

    public static void PauseGame(PlayerControl player)
    {
        StartGameWindow.HideStatic();
        PauseGameWindow.ShowStatic();
        state = State.Paused;
        player.PlayerPause();
    }

    public static void ResumeGame(PlayerControl player)
    {
        PauseGameWindow.HideStatic();
        state = State.Active;
        player.PlayerResume();
    }

    public void PlayerNewLife()
    {
        if (Score.GetLifes()>0)
        {
            Score.RemoveLifes(1, null);
            StartCoroutine(DelayedNewLife());
        }
        else
        {
            SoundManager.PlaySound(SoundManager.Sound.BadClick);
            state = State.Dead;
            GameOverWindow.ShowStatic();
        }
    }

    IEnumerator DelayedNewLife()
    {
        yield return new WaitForSeconds(3f);
        player.NewLife();
        state = State.Active;
        GameOverWindow.HideStatic();
        SoundManager.PlaySound(SoundManager.Sound.NewLife);
    }

    public static void PlayerDied(TilemapManager tilemapManager)
    {
        SoundManager.PlaySound(SoundManager.Sound.Die);
        tilemapManager.DestroyGhostTiles();
        instance.PlayerNewLife();
    }

    public static void PlayerWin()
    {
        SoundManager.PlaySound(SoundManager.Sound.Win); 
        if (IsMaxLevel)
        {
            Score.ResetRestarts();     
        }
        if (LevelNumber<16) 
        {
            Score.AddSavedLevel(LevelNumber);
        }
        state = State.Win;
        WinGameWindow.ShowStatic();
    }
}
