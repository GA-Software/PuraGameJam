using UnityEngine;

[System.Serializable]
public class Level : MonoBehaviour
{
    public string levelName;
    [TextArea(3,10)]
    public string levelJournal;
    public Sprite levelImage;
    public int birdCollectableCount;
    public int fishCollectableCount;
    public Transform spawnPoint;
}