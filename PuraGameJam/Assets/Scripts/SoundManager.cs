using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioClip buttonClip;
    public AudioClip gameOverClip;
    public AudioClip collectClip;
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;
    public AudioClip victoryClip;

    public bool musicCreated = false;
    public GameObject musicObject;
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(gameObject);

        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetInt("Music", 1);
        if (!PlayerPrefs.HasKey("Sound"))
            PlayerPrefs.SetInt("Sound", 1);

        CreateMusicObject(menuMusic);
        StartCoroutine(PlayMusicWithLoop(1f, menuMusic));
    }

    public void PlaySound(AudioClip audioClip)
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            GameObject soundGameObject = new GameObject(audioClip.name);
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

            audioSource.PlayOneShot(audioClip);
            Destroy(soundGameObject, audioClip.length);
        }
    }

    public System.Collections.IEnumerator PlayMusicWithLoop(float waitTime, AudioClip clip)
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            yield return new WaitForSeconds(waitTime);

            if (musicCreated)
            {
                musicObject.GetComponent<AudioSource>().UnPause();
            }
            else
            {
                CreateMusicObject(clip);
                PlayMusicWithLoop(0f, clip);
            }
        }
    }

    public void CreateMusicObject(AudioClip audioClip)
    {
        if (!musicCreated)
        {
            musicObject = new GameObject(audioClip.name);
            DontDestroyOnLoad(musicObject);
            musicObject.tag = "MenuMusic";
            AudioSource audioSource = musicObject.AddComponent<AudioSource>();

            audioSource.loop = true;
            audioSource.volume = 0.5f;
            audioSource.clip = audioClip;
            musicCreated = true;
            audioSource.Play();
            audioSource.Pause();
        }
    }

    public void ChangeSoundStatus()
    {
        PlayerPrefs.SetInt("Sound", PlayerPrefs.GetInt("Sound") == 0 ? 1 : 0);
    }

    public void ChangeMusicStatus()
    {
        PlayerPrefs.SetInt("Music", PlayerPrefs.GetInt("Music") == 0 ? 1 : 0);

        if (PlayerPrefs.GetInt("Music") == 0)
            musicObject.GetComponent<AudioSource>().Pause();
        else
            musicObject.GetComponent<AudioSource>().UnPause();
    }
}