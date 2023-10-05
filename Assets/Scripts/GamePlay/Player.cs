using System.Collections;
using System.Collections.Generic;
using com.trashpandaboy.core;
using UnityEngine;
using static Utils;

public class Player : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!SessionDataManager.Instance.GameOver)
            EventDispatcher.TriggerEvent(GameEvent.GameOver.ToString());
    }
}
