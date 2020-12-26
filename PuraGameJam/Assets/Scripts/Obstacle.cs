using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType { Rotor, Hunter, Piranha, Raven, Rod };
    public ObstacleType obstacleType;
    public CharacterController.CharacterType characterType;

    public float enemySpeed;
    public bool canKill;

    private void Awake()
    {
        canKill = true;

        switch (obstacleType)
        {
            case ObstacleType.Rotor:
                break;
            case ObstacleType.Hunter:
                break;
            case ObstacleType.Piranha:
                //transform.Translate(Vector2.left * enemySpeed * Time.deltaTime);
                break;
            case ObstacleType.Raven:
                break;
            case ObstacleType.Rod:
                break;
            default:
                break;
        }
    }
    
    public void DestroyObstacle()
    {
        canKill = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canKill && collision.gameObject.tag == characterType.ToString())
        {
            GameManager.Instance.GameOver();
        }
    }
}
