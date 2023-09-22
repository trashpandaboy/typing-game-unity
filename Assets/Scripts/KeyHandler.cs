using System.Collections.Generic;
using com.trashpandaboy.core;
using com.trashpandaboy.core.Utils;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    string _desiredKeys = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    [SerializeField]
    List<char> _desiredKeysList;

    private void Awake()
    {
        _desiredKeysList = new List<char>(_desiredKeys.ToCharArray());
    }

    private void Update()
    {
        if(Input.anyKeyDown && Input.inputString.Length == 1)
        {
            if(_desiredKeysList.Contains(Input.inputString[0]))
            {
                DataSet eventData = new DataSet();
                eventData.AddData("key", Input.inputString[0]);

                EventDispatcher.TriggerEvent(Utils.GameEvent.KeyPressed.ToString(), eventData);
            }
        }
    }
}
