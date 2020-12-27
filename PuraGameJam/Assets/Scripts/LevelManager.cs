using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int levelCount = 3;
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

        for (int i = 1; i <= levelCount; i++)
        {
            if (!PlayerPrefs.HasKey("LevelStar" + i))
                PlayerPrefs.SetInt("LevelStar" + i, 0);
        }

        LevelControl();
    }

    public void LevelControl()
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

    public void StartLevel(int index)
    {
        LevelControl();
        DG.Tweening.DOTween.KillAll();

        if (index >= 0 && index <= PlayerPrefs.GetInt("CurrentMaxLevel"))
        {
            Instantiate(levels[index]);
            PlayerPrefs.SetInt("CurrentLevel", index + 1);
        }
        else if (index == PlayerPrefs.GetInt("CurrentMaxLevel"))
        {
            //To be implemented
            Debug.Log("You Completed the game.");
            SceneManager.LoadScene(0);
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        LevelControl();
        DG.Tweening.DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void FinishLevel()
    {
        if (PlayerPrefs.GetInt("CurrentLevel") == PlayerPrefs.GetInt("CurrentMaxLevel"))
            PlayerPrefs.SetInt("CurrentMaxLevel", PlayerPrefs.GetInt("CurrentMaxLevel") + 1);

        if (PlayerPrefs.GetInt("CurrentLevel") < PlayerPrefs.GetInt("CurrentMaxLevel"))
        {
            PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
        }
    }
}