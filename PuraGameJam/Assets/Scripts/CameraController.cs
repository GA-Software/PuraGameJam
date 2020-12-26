using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private List<Transform> characters = new List<Transform>();
    [SerializeField]private Vector3 targetPos;

    private void Start()
    {
        for (int i = 0; i < InputManager.Instance.characterControllers.Count; i++)
        {
            characters.Add(InputManager.Instance.characterControllers[i].transform);
        }

        FollowCharacters();
    }

    private void Update()
    {
        if (GameManager.Instance.isGameStarted && !GameManager.Instance.isGameOver)
        {
            FollowCharacters();
        }
    }

    public void FollowCharacters()
    {
        Vector3 medianPoint = Vector3.zero;

        foreach (Transform character in characters)
        {
            medianPoint += character.position;
        }
        medianPoint /= characters.Count;

        targetPos = new Vector3(medianPoint.x, 0, -10);
        transform.position = targetPos;
    }
}
