using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public enum CharacterType { Bird, Fish };

    public CharacterType characterType;
    public int characterID;
    [SerializeField] private float movementSpeed;
    [SerializeField] private List<InputManager.KeyBinding> keyBindings;

    private void Update()
    {
        if (!GameManager.Instance.isGameStarted && !GameManager.Instance.isGameOver)
        {
            if (Input.GetKey(InputManager.Instance.GetKey(characterID, InputManager.ControlKeys.Left)))
                transform.Translate(Vector2.left * movementSpeed * Time.deltaTime);

            if (Input.GetKey(InputManager.Instance.GetKey(characterID, InputManager.ControlKeys.Right)))
                transform.Translate(Vector2.right * movementSpeed * Time.deltaTime);

            if (Input.GetKey(InputManager.Instance.GetKey(characterID, InputManager.ControlKeys.Up)))
                transform.Translate(Vector2.up * movementSpeed * Time.deltaTime);

            if (Input.GetKey(InputManager.Instance.GetKey(characterID, InputManager.ControlKeys.Down)))
                transform.Translate(Vector2.down * movementSpeed * Time.deltaTime);
        }
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