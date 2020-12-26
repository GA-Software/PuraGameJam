using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel, gameplayPanel, gameOverPanel, helpPanel, teamPanel, settingsPanel, gamePausedPanel, victoryPanel, levelStarPanel, levelPanel;
    public RectTransform logo, buttonsPanel, levelSlidePanel;
    public Button previousLevelButton, nextLevelButton, victoryNextLevelButton, playButton;
    public Image soundImage, musicImage, levelImage;
    public Sprite toggleOn, toggleOff, grayButtonLongImage, greenButtonLongImage, lockIcon;
    public Text levelNoText, levelNameText;

    private int currentlyDisplayedLevel = -1;
    public static MenuController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        PlayMenuAnimations();
        StartCoroutine(OpenMenu());

        playButton.onClick.AddListener(() => DisplayLevel(PlayerPrefs.GetInt("CurrentLevel") - 1));
    }

    private void Start()
    {
        updateSoundImages();
    }

    public void StartGame()
    {
        gameplayPanel.SetActive(true);
        menuPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.8f).SetEase(Ease.Linear);
        StartCoroutine(GameManager.Instance.StartGame());
        StartCoroutine(GameManager.Instance.DoAfterSeconds(0.8f, () => menuPanel.SetActive(false)));
    }

    public void DisplayLevel(int index)
    {
        if (index >= 0 && index < LevelManager.Instance.levelCount && currentlyDisplayedLevel != index)
        {
            float waitTime = 0.2f;
            if (currentlyDisplayedLevel < index && currentlyDisplayedLevel != -1) //goes right
            {
                levelSlidePanel.DOAnchorPosX(levelSlidePanel.anchoredPosition.x - 800f, 0.2f);
                levelSlidePanel.DOAnchorPosX(levelSlidePanel.anchoredPosition.x + 1600f, 0).SetDelay(0.2f);
                levelSlidePanel.DOAnchorPosX(0, 0.2f).SetDelay(0.21f);
            }
            else if (currentlyDisplayedLevel > index)
            {
                levelSlidePanel.DOAnchorPosX(levelSlidePanel.anchoredPosition.x + 800f, 0.2f);
                levelSlidePanel.DOAnchorPosX(levelSlidePanel.anchoredPosition.x - 1600f, 0).SetDelay(0.2f);
                levelSlidePanel.DOAnchorPosX(0, 0.2f).SetDelay(0.21f);
            }
            else
                waitTime = 0;

            currentlyDisplayedLevel = index;
            levelNameText.text = LevelManager.Instance.levels[index].levelName;
            levelNoText.text = "BÖLÜM " + (index + 1);
            AdjustLevelButtons(index);
            StartCoroutine(SetLevelLocks(index, waitTime));
        }
    }
    IEnumerator SetLevelLocks(int index, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (index > PlayerPrefs.GetInt("CurrentMaxLevel") - 1)
            LockLevel(index);
        else
            UnlockLevel(index);
    }

    public void LockLevel(int index)
    {
        levelStarPanel.SetActive(false);
        levelImage.sprite = lockIcon;
        playButton.interactable = false;
        playButton.GetComponent<Image>().sprite = grayButtonLongImage;

        levelImage.color = new Color(0.5f, 0.5f, 0.5f);
    }

    public void UnlockLevel(int index)
    {
        levelStarPanel.SetActive(true);

        foreach (Transform starTransform in levelStarPanel.transform)
        {
            Image star = starTransform.GetComponent<Image>();

            if (star.transform.GetSiblingIndex() < PlayerPrefs.GetInt("LevelStar" + index + 1))
                star.color = new Color(1f, 0.8f, 0f);
            else
                star.color = Color.white;
        }
        levelImage.sprite = LevelManager.Instance.levels[index].levelImage;
        levelImage.color = Color.white;

        playButton.interactable = true;
        playButton.GetComponent<Image>().sprite = greenButtonLongImage;
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() => LevelManager.Instance.StartLevel(index + 1));
    }

    public void AdjustLevelButtons(int index)
    {
        previousLevelButton.onClick.RemoveAllListeners();
        nextLevelButton.onClick.RemoveAllListeners();

        previousLevelButton.onClick.AddListener(() => DisplayLevel(index - 1));
        nextLevelButton.onClick.AddListener(() => DisplayLevel(index + 1));

        if (index == 0)
        {
            previousLevelButton.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            previousLevelButton.interactable = false;
        }
        else
        {
            previousLevelButton.GetComponent<Image>().color = new Color(0.2666667f, 0.7490196f, 0.9764706f);
            previousLevelButton.interactable = true;
        }

        if (index == LevelManager.Instance.levelCount - 1)
        {
            nextLevelButton.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            nextLevelButton.interactable = false;
        }
        else
        {
            nextLevelButton.GetComponent<Image>().color = new Color(0.2666667f, 0.7490196f, 0.9764706f);
            nextLevelButton.interactable = true;
        }
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.GetChild(0).DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
    }

    public void ClosePanel(GameObject panel) { StartCoroutine(ClosePanelCoroutine(panel)); }

    IEnumerator ClosePanelCoroutine(GameObject panel)
    {
        panel.transform.GetChild(0).GetComponent<RectTransform>().DOScale(0f, 0.4f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(0.4f);
        panel.SetActive(false);
    }
    IEnumerator OpenMenu()
    {
        gameplayPanel.SetActive(false);
        menuPanel.SetActive(true);

        gameOverPanel.SetActive(false);
        gameOverPanel.transform.GetChild(0).localScale = Vector3.zero;

        gamePausedPanel.SetActive(false);
        gamePausedPanel.transform.GetChild(0).localScale = Vector3.zero;

        victoryPanel.SetActive(false);
        victoryPanel.transform.GetChild(0).localScale = Vector3.zero;

        levelPanel.SetActive(false);
        levelPanel.transform.GetChild(0).localScale = Vector3.zero;

        helpPanel.SetActive(false);
        helpPanel.transform.GetChild(0).localScale = Vector3.zero;

        settingsPanel.SetActive(false);
        settingsPanel.transform.GetChild(0).localScale = Vector3.zero;

        teamPanel.SetActive(false);
        teamPanel.transform.GetChild(0).localScale = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
    }

    private void PlayMenuAnimations()
    {
        logo.DOPunchScale(new Vector3(0.04f, 0.04f, 0.04f), 1f, 1, 1f).SetEase(Ease.Linear).SetLoops(-1);
        logo.DOAnchorPosX(0f, 0.4f).SetEase(Ease.OutBack);
        buttonsPanel.DOAnchorPosY(160f, 0.4f).SetEase(Ease.OutBack);
    }

    public void buttonUp(Button button)
    {
        button.transform.DOScale(1f, 0.1f);
    }

    public void buttonDown(Button button)
    {
        if (button.interactable)
        {
            button.transform.DOScale(0.9f, 0.1f);
            SoundManager.Instance.PlaySound(SoundManager.Instance.buttonClip);
        }
    }

    public void ChangeSoundStatus()
    {
        SoundManager.Instance.ChangeSoundStatus();
        updateSoundImages();
    }

    public void ChangeMusicStatus()
    {
        SoundManager.Instance.ChangeMusicStatus();
        updateSoundImages();
    }

    public void PlaySound(AudioClip clip)
    {
        SoundManager.Instance.PlaySound(clip);
    }

    public void updateSoundImages()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
            soundImage.sprite = toggleOn;
        else
            soundImage.sprite = toggleOff;

        if (PlayerPrefs.GetInt("Music") == 1)
            musicImage.sprite = toggleOn;
        else
            musicImage.sprite = toggleOff;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}