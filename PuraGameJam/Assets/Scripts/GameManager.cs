﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGameOver, isGameStarted, levelFinished, birdFinished, fishFinished;
    public int birdCollectedCount = 0, fishCollectedCount = 0;
    public List<GameObject> levels;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        Application.targetFrameRate = 60;
        Instantiate(levels[PlayerPrefs.GetInt("CurrentLevel") - 1]);
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.8f);
        isGameOver = false;
        isGameStarted = true;
    }

    public void PauseGame()
    {
        if (!levelFinished)
        {
            MenuController.Instance.OpenPanel(MenuController.Instance.gamePausedPanel);
            StartCoroutine(DoAfterSeconds(0.4f, () => Time.timeScale = 0f));
        }
    }

    public void ResumeGame()
    {
        if (!levelFinished)
        {
            Time.timeScale = 1f;
            MenuController.Instance.ClosePanel(MenuController.Instance.gamePausedPanel);
        }
    }

    public void FinishLevel()
    {
        if (birdFinished && fishFinished)
        {
            LevelManager.Instance.FinishLevel();
            levelFinished = true;

            foreach (CharacterController character in InputManager.Instance.characterControllers)
            {
                character.canMove = false;
            }

            MenuController.Instance.OpenPanel(MenuController.Instance.victoryPanel); 
        }
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

    public void GoToMenu()
    {
        DOTween.KillAll();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public IEnumerator DoAfterSeconds(float time, Action func)
    {
        yield return new WaitForSeconds(time);
        func();
    }
}