using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public enum InteractionType { LaunchObstacle, DestroyObstacle, LeverSwitch, Boat }
    public InteractionType interactionType;
    public Obstacle connectedObstacle;

    public bool obstacleLaunched = false, obstacleDestroyed = false;
    public Sprite leverOff;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (interactionType)
        {
            case InteractionType.LaunchObstacle:
                if (!obstacleLaunched && (collision.gameObject.tag == "BirdCharacter" && connectedObstacle.characterType == CharacterController.CharacterType.BirdCharacter ||
                    collision.gameObject.tag == "FishCharacter" && connectedObstacle.characterType == CharacterController.CharacterType.FishCharacter))
                {
                    connectedObstacle.MoveObstacle();
                    obstacleLaunched = true;
                }
                break;
            case InteractionType.DestroyObstacle:

                if (!obstacleDestroyed && (collision.gameObject.tag == "BirdCharacter" && connectedObstacle.characterType == CharacterController.CharacterType.FishCharacter))
                {
                    connectedObstacle.DestroyObstacle();
                    obstacleDestroyed = true;
                }
                break;
            case InteractionType.LeverSwitch:

                if (!obstacleDestroyed && (collision.gameObject.tag == "FishCharacter" && connectedObstacle.characterType == CharacterController.CharacterType.BirdCharacter))
                {
                    Destroy(connectedObstacle.transform.GetChild(0).gameObject);
                    Destroy(connectedObstacle.transform.GetComponent<Animator>());
                    Destroy(connectedObstacle.transform.GetComponent<BoxCollider2D>());
                    obstacleDestroyed = true;
                    GetComponent<SpriteRenderer>().sprite = leverOff;
                }
                break;
            case InteractionType.Boat:
                if (!obstacleDestroyed && (collision.gameObject.tag == "FishCharacter" && connectedObstacle.characterType == CharacterController.CharacterType.BirdCharacter))
                {
                    Destroy(connectedObstacle.transform.gameObject);
                    obstacleDestroyed = true;
                }
                break;
            default:
                break;
        }

    }
}
