using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel, gameplayPanel, gameOverPanel, helpPanel, teamPanel, settingsPanel, gamePausedPanel, victoryPanel, levelStarPanel, levelPanel;
    public RectTransform logo, buttonsPanel, levelSlidePanel, helpSlidePanel;
    public Button previousLevelButton, nextLevelButton, previousHelpButton, nextHelpButton, victoryNextLevelButton, startLevelButton, playButton, helpButton;
    public Image soundImage, musicImage, inGameSoundImage, inGameMusicImage, levelImage, helpItemImage;
    public Sprite toggleOn, toggleOff, grayButtonLongImage, greenButtonLongImage, lockIcon;
    public Text levelNoText, levelNameText, helpDescriptionText, birdCollectedCountText, fishCollectedCountText;

    private int currentlyDisplayedLevel = -1, currentlyDisplayedHelpItem = -1;
    public static MenuController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        PlayMenuAnimations();
        StartCoroutine(OpenMenu());

        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() => DisplayLevel(PlayerPrefs.GetInt("CurrentLevel") - 1));

        playButton.onClick.RemoveAllListeners();
        helpButton.onClick.AddListener(() => DisplayHelpItem(0));
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
        startLevelButton.interactable = false;
        startLevelButton.GetComponent<Image>().sprite = grayButtonLongImage;

        levelImage.color = new Color(0.5f, 0.5f, 0.5f);
    }

    public void UnlockLevel(int index)
    {
        levelStarPanel.SetActive(true);

        foreach (Transform starTransform in levelStarPanel.transform)
        {
            Image star = starTransform.GetComponent<Image>();
            if (star.transform.GetSiblingIndex() < PlayerPrefs.GetInt("LevelStar" + (index + 1)))
                star.color = new Color(1f, 0.8f, 0f);
            else
                star.color = Color.white;
        }
        levelImage.sprite = LevelManager.Instance.levels[index].levelImage;
        levelImage.color = Color.white;

        startLevelButton.interactable = true;
        startLevelButton.GetComponent<Image>().sprite = greenButtonLongImage;
        startLevelButton.onClick.RemoveAllListeners();
        startLevelButton.onClick.AddListener(() => LevelManager.Instance.StartLevel(index));
        startLevelButton.onClick.AddListener(() => StartGame());
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

    public void DisplayHelpItem(int index)
    {
        if (index >= 0 && index < HelpManager.Instance.helpItems.Count && currentlyDisplayedHelpItem != index)
        {
            if (currentlyDisplayedHelpItem < index && currentlyDisplayedHelpItem != -1) //goes right
            {
                helpSlidePanel.DOAnchorPosX(helpSlidePanel.anchoredPosition.x - 800f, 0.2f);
                helpSlidePanel.DOAnchorPosX(helpSlidePanel.anchoredPosition.x + 1600f, 0).SetDelay(0.2f);
                helpSlidePanel.DOAnchorPosX(0, 0.2f).SetDelay(0.21f);
            }
            else if (currentlyDisplayedHelpItem > index)
            {
                helpSlidePanel.DOAnchorPosX(helpSlidePanel.anchoredPosition.x + 800f, 0.2f);
                helpSlidePanel.DOAnchorPosX(helpSlidePanel.anchoredPosition.x - 1600f, 0).SetDelay(0.2f);
                helpSlidePanel.DOAnchorPosX(0, 0.2f).SetDelay(0.21f);
            }

            currentlyDisplayedHelpItem = index;
            helpDescriptionText.text = HelpManager.Instance.helpItems[index].helpDescription;
            helpItemImage.sprite = HelpManager.Instance.helpItems[index].helpImage;
            AdjustHelpButtons(index);
        }
    }

    public void AdjustHelpButtons(int index)
    {
        previousHelpButton.onClick.RemoveAllListeners();
        nextHelpButton.onClick.RemoveAllListeners();

        previousHelpButton.onClick.AddListener(() => DisplayHelpItem(index - 1));
        nextHelpButton.onClick.AddListener(() => DisplayHelpItem(index + 1));

        if (index == 0)
        {
            previousHelpButton.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            previousHelpButton.interactable = false;
        }
        else
        {
            previousHelpButton.GetComponent<Image>().color = new Color(0.2666667f, 0.7490196f, 0.9764706f);
            previousHelpButton.interactable = true;
        }

        if (index == HelpManager.Instance.helpItems.Count - 1)
        {
            nextHelpButton.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            nextHelpButton.interactable = false;
        }
        else
        {
            nextHelpButton.GetComponent<Image>().color = new Color(0.2666667f, 0.7490196f, 0.9764706f);
            nextHelpButton.interactable = true;
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
        if (Time.timeScale > 0)
            button.transform.DOScale(1f, 0.1f);
        else
            button.transform.localScale = Vector3.one;
    }

    public void buttonDown(Button button)
    {
        if (button.interactable)
        {
            if (Time.timeScale > 0)
                button.transform.DOScale(0.9f, 0.1f);
            else
                button.transform.localScale = Vector3.one * 0.9f;
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
        {
            soundImage.sprite = toggleOn;
            inGameSoundImage.sprite = toggleOn;
        }
        else
        {
            soundImage.sprite = toggleOff;
            inGameSoundImage.sprite = toggleOff;
        }

        if (PlayerPrefs.GetInt("Music") == 1)
        {
            musicImage.sprite = toggleOn;
            inGameMusicImage.sprite = toggleOn;
        }
        else
        {
            musicImage.sprite = toggleOff;
            inGameMusicImage.sprite = toggleOff;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}