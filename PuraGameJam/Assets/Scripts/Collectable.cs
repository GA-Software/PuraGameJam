using UnityEngine;

public class Collectable : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //To be implemented
            Destroy(gameObject);

            SoundManager.Instance.PlaySound(SoundManager.Instance.collectClip);
        }
    }
}