using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isGameOver, isGameStarted;
    public int birdCollectedCount = 0, fishCollectedCount = 0;
    public GameObject levelPrefab;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        Application.targetFrameRate = 60;
    }

    public void StartGame()
    {
        isGameOver = false;
        isGameStarted = true;
        Instantiate(levelPrefab);
    }

    public void GameOver()
    {
        if (!isGameOver && isGameStarted)
        {
            isGameOver = true;

            int birdRequired = LevelManager.Instance.levels[PlayerPrefs.GetInt("CurrentLevel")].birdCollectableCount;
            int fishRequired = LevelManager.Instance.levels[PlayerPrefs.GetInt("CurrentLevel")].fishCollectableCount;

            if (birdCollectedCount ==  birdRequired && fishCollectedCount == fishRequired)
            {
                //To be implemented star system
            }

            MenuController.Instance.gameplayPanel.SetActive(false);
            MenuController.Instance.OpenPanel(MenuController.Instance.gameOverPanel);
            SoundManager.Instance.PlaySound(SoundManager.Instance.gameOverClip);
        }
    }
}