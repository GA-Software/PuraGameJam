using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinisher : MonoBehaviour
{
    public enum FinisherType { BirdFinisher, FishFinisher};
    public FinisherType finisherType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BirdCharacter" && finisherType == FinisherType.BirdFinisher)
        {
            GameManager.Instance.birdFinished = true;
            GameManager.Instance.FinishLevel();
        }
        else if (collision.gameObject.tag == "FishCharacter" && finisherType == FinisherType.FishFinisher)
        {
            GameManager.Instance.fishFinished = true;
            GameManager.Instance.FinishLevel();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BirdCharacter" && finisherType == FinisherType.BirdFinisher)
        {
            GameManager.Instance.birdFinished = false;
        }
        else if (collision.gameObject.tag == "FishCharacter" && finisherType == FinisherType.FishFinisher)
        {
            GameManager.Instance.fishFinished = false;
        }
    }

}
