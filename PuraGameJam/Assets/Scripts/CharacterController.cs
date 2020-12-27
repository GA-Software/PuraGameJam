using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public enum CharacterType { BirdCharacter, FishCharacter };

    public CharacterType characterType;
    public int characterID;
    [SerializeField] private float movementSpeed;
    public bool canMove;
    [SerializeField] private int facingDirection = 1;
    [SerializeField] private List<InputManager.KeyBinding> keyBindings;

    private void Awake()
    {
        canMove = true;
    }
    private void Update()
    {
        if (GameManager.Instance.isGameStarted && !GameManager.Instance.isGameOver && canMove)
        {
            if (Input.GetKey(InputManager.Instance.GetKey(characterID, InputManager.ControlKeys.Left)))
            {
                transform.Translate(Vector2.left * movementSpeed * facingDirection *  Time.deltaTime);
                if (facingDirection == 1)
                    Flip();
            }

            if (Input.GetKey(InputManager.Instance.GetKey(characterID, InputManager.ControlKeys.Right)))
            {
                transform.Translate(Vector2.right * movementSpeed * facingDirection * Time.deltaTime);
                if (facingDirection == -1)
                    Flip();
            }

            if (Input.GetKey(InputManager.Instance.GetKey(characterID, InputManager.ControlKeys.Up)))
                transform.Translate(Vector2.up * movementSpeed * Time.deltaTime);

            if (Input.GetKey(InputManager.Instance.GetKey(characterID, InputManager.ControlKeys.Down)))
                transform.Translate(Vector2.down * movementSpeed * Time.deltaTime);
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public KeyCode GetKeyCode(InputManager.ControlKeys controlKeys)
    {
        foreach (InputManager.KeyBinding keyBinding in keyBindings)
        {
            if (keyBinding.controlKey == controlKeys)
            {
                return keyBinding.keyCode;
            }
        }
        return KeyCode.None;
    }
}