using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum ControlKeys { Left, Right, Up, Down, Action, Skill1, Skill2 };
    [SerializeField] List<CharacterController> characterControllers;

    public static InputManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    [System.Serializable]
    public class KeyBinding
    {
        public ControlKeys controlKey;
        public KeyCode keyCode;
    }

    public KeyCode GetKey(int playerID, ControlKeys key)
    {
        return characterControllers[playerID].GetKeyCode(key);
    }
}