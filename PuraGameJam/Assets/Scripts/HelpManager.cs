using System.Collections.Generic;
using UnityEngine;

public class HelpManager : MonoBehaviour
{
    [System.Serializable]
    public class HelpItem
    {
        [TextArea(3, 10)]
        public string helpDescription;
        public Sprite helpImage;
    }

    public List<HelpItem> helpItems = new List<HelpItem>();

    public static HelpManager Instance {get; private set;}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
    }
}
