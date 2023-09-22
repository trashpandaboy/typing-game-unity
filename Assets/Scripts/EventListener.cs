using System;
using com.trashpandaboy.core;
using com.trashpandaboy.core.Utils;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    UnityAction<DataSet> _OnKeyPressed;

    private void Start()
    {
        _OnKeyPressed = new UnityAction<DataSet>(OnKeyPressedMethod);
        EventDispatcher.StartListening(Utils.GameEvent.KeyPressed.ToString(),_OnKeyPressed);
    }

    private void OnDestroy()
    {
        EventDispatcher.StopListening(Utils.GameEvent.KeyPressed.ToString(), _OnKeyPressed);
    }

    private void OnKeyPressedMethod(DataSet obj)
    {
        char key = obj.GetData<char>("key");
        Debug.Log("Received event KeyPressed - Char: " + key);
    }
}
