using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType { BirdCollectable, FishCollectable };
    public CollectableType collectableType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BirdCharacter" && collectableType == CollectableType.BirdCollectable)
        {
            Destroy(gameObject);
            GameManager.Instance.CollectObject(collectableType);
        }
        else if (collision.gameObject.tag == "FishCharacter" && collectableType == CollectableType.FishCollectable)
        {
            Destroy(gameObject);
            GameManager.Instance.CollectObject(collectableType);
        }
    }
}