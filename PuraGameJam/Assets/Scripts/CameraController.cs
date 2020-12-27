using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private List<Transform> characters = new List<Transform>();
    public Transform midPointTransform;

    private void Start()
    {
        for (int i = 0; i < InputManager.Instance.characterControllers.Count; i++)
        {
            characters.Add(InputManager.Instance.characterControllers[i].transform.GetChild(0));
        }

        CalculateMidpoint();
    }

    private void Update()
    {
        if (GameManager.Instance.isGameStarted && !GameManager.Instance.isGameOver)
        {
            CalculateMidpoint();
        }
    }

    public void CalculateMidpoint()
    {
        Vector3 medianPoint = Vector3.zero;

        foreach (Transform character in characters)
        {
            medianPoint += character.position;
        }
        medianPoint /= characters.Count;

        midPointTransform.position = new Vector3(medianPoint.x, 0, -10);
    }
}
