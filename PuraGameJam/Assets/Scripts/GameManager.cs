using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGameOver, isGameStarted, levelFinished, birdFinished, fishFinished;
    public int birdCollectedCount = 0, fishCollectedCount = 0;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        Application.targetFrameRate = 60;
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

            int starCount = 0;

            int birdRequired = LevelManager.Instance.levels[PlayerPrefs.GetInt("CurrentLevel")].birdCollectableCount;
            int fishRequired = LevelManager.Instance.levels[PlayerPrefs.GetInt("CurrentLevel")].fishCollectableCount;

            float percent = (float)(birdCollectedCount + fishCollectedCount) / (float)(birdRequired + fishRequired);

            if (percent < 0.6f)
                starCount = 1;
            else if (percent < 0.85f)
                starCount = 2;
            else
                starCount = 3;

            foreach (CharacterController character in InputManager.Instance.characterControllers)
            {
                character.canMove = false;
            }
            PlayerPrefs.SetInt("LevelStar" + PlayerPrefs.GetInt("CurrentLevel"), starCount);
            MenuController.Instance.OpenPanel(MenuController.Instance.victoryPanel); 
        }
    }

    public void GameOver()
    {
        if (!isGameOver && isGameStarted)
        {
            isGameOver = true;

            
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

    public void CollectObject(Collectable.CollectableType collectableType)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.collectClip);

        switch (collectableType)
        {
            case Collectable.CollectableType.BirdCollectable:
                birdCollectedCount++;
                MenuController.Instance.birdCollectedCountText.text = birdCollectedCount.ToString();
                break;
            case Collectable.CollectableType.FishCollectable:
                fishCollectedCount++;
                MenuController.Instance.fishCollectedCountText.text = fishCollectedCount.ToString();
                break;
            default: Debug.LogError("Collectable type isn't defined.");
                break;
        }
    }

    public IEnumerator DoAfterSeconds(float time, Action func)
    {
        yield return new WaitForSeconds(time);
        func();
    }
}