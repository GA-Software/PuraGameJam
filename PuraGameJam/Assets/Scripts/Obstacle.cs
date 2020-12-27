using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType { Rotor, Hunter, Piranha, Raven, Rod };
    public ObstacleType obstacleType;
    public CharacterController.CharacterType characterType;

    public bool canKill;

    private void Awake()
    {
        canKill = false;
    }

    public void MoveObstacle()
    {
        canKill = true;
        Debug.Log("started moving obstacle");

        switch (obstacleType)
        {
            case ObstacleType.Rotor:
                canKill = false;
                break;
            case ObstacleType.Hunter:
                transform.GetChild(0).DOMoveX(transform.position.x - 50, 1f).SetEase(Ease.Linear).SetLoops(-1);
                HunterShoot();
                break;
            case ObstacleType.Piranha:
                transform.DOMoveX(transform.position.x - 50, 6f).SetEase(Ease.Linear);
                GameManager.Instance.DoAfterSeconds(4f, () => DestroyObstacle());
                break;
            case ObstacleType.Raven:
                transform.DOMoveX(transform.position.x - 50, 6f).SetEase(Ease.Linear);
                GameManager.Instance.DoAfterSeconds(4f, () => DestroyObstacle());
                break;
            case ObstacleType.Rod:
                canKill = true;
                break;
            default:
                break;
        }
    }

    public void DestroyObstacle()
    {
        Destroy(transform.parent.gameObject);
        Debug.Log("obstacle destroyed");
        canKill = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canKill && collision.gameObject.tag == characterType.ToString())
        {
            GameManager.Instance.GameOver();
        }
    }

    IEnumerator HunterShoot()
    {
        if (canKill)
        {
            for (int i = 0; i < 50; i++)
            {
                yield return new WaitForSeconds(1.0f);
                GameManager.Instance.DoAfterSeconds(1f, () => transform.GetChild(0).position = new Vector3(1.5f, 0.65f, 0f));
            }
        }
    }
}
