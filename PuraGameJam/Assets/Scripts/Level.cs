using UnityEngine;

[System.Serializable]
public class Level : MonoBehaviour
{
    public string levelName;
    public Sprite levelImage;
    public int birdCollectableCount;
    public int fishCollectableCount;
    public Transform spawnPoint;
}