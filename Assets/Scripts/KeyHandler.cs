using System.Collections.Generic;
using com.trashpandaboy.core;
using com.trashpandaboy.core.Utils;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    string _desiredKeys = "abcdefghijklmnopqrstuvwxyz";
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
            string input = Input.inputString.ToLower();
            if(_desiredKeysList.Contains(input[0]))
            {
                DataSet eventData = new DataSet();
                eventData.AddData("key", input[0]);

                EventDispatcher.TriggerEvent(Utils.GameEvent.KeyPressed.ToString(), eventData);
            }
        }
    }
}
