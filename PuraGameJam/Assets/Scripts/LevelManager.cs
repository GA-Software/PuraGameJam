using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int levelCount = 10;
    public Level[] levels;

    public static LevelManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(gameObject);

        Application.targetFrameRate = 30;

        for (int i = 1; i <= levelCount; i++)
        {
            if (!PlayerPrefs.HasKey("LevelStar" + i))
                PlayerPrefs.SetInt("LevelStar" + i, 0);
        }

        levelControl();
    }

    public void levelControl()
    {
        if (!PlayerPrefs.HasKey("CurrentLevel"))
            PlayerPrefs.SetInt("CurrentLevel", 1);
        if (!PlayerPrefs.HasKey("CurrentMaxLevel"))
            PlayerPrefs.SetInt("CurrentMaxLevel", 1);

        if (PlayerPrefs.GetInt("CurrentLevel") > PlayerPrefs.GetInt("CurrentMaxLevel"))
            PlayerPrefs.SetInt("CurrentMaxLevel", PlayerPrefs.GetInt("CurrentLevel"));
        if (PlayerPrefs.GetInt("CurrentMaxLevel") >= levelCount)
            PlayerPrefs.SetInt("CurrentMaxLevel", levelCount);
    }

    public void startLevel(int index)
    {
        levelControl();
        DG.Tweening.DOTween.KillAll();

        if (index >= 0 && index <= PlayerPrefs.GetInt("CurrentMaxLevel"))
        {
            Debug.Log(index);
            PlayerPrefs.SetInt("CurrentLevel", index);
        }
        else if (index == PlayerPrefs.GetInt("CurrentMaxLevel"))
        {
            //To be implemented
            SceneManager.LoadScene(0);
        }
    }

    public void resetProgress()
    {
        PlayerPrefs.DeleteAll();
        levelControl();
        DG.Tweening.DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void setMaxLevel(int index)
    {
        DG.Tweening.DOTween.KillAll();
        PlayerPrefs.SetInt("CurrentMaxLevel", index);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void nextLevel()
    {
        DG.Tweening.DOTween.KillAll();
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
        startLevel(PlayerPrefs.GetInt("CurrentLevel"));
    }

    public void finishLevel()
    {
        if (PlayerPrefs.GetInt("CurrentLevel") == PlayerPrefs.GetInt("CurrentMaxLevel"))
            PlayerPrefs.SetInt("CurrentMaxLevel", PlayerPrefs.GetInt("CurrentMaxLevel") + 1);
    }
}