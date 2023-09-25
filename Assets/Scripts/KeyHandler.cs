using System.Text.RegularExpressions;
using com.trashpandaboy.core;
using com.trashpandaboy.core.Utils;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    private void Update()
    {
        string input = Input.inputString.ToLower();
        if (Input.anyKeyDown && input.Length == 1)
        {
            if(Regex.IsMatch(input, @"[A-Za-z]"))
            {
                DataSet eventData = new DataSet();
                eventData.AddData("key", input[0]);

                EventDispatcher.TriggerEvent(Utils.GameEvent.KeyPressed.ToString(), eventData);
            }
        }
    }
}
