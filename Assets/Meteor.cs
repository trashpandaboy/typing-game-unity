using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField]
    string _word;

    private void Update()
    {
        transform.position += Vector3.down * Time.deltaTime;
    }

    internal void SetupWord(string word)
    {
        _word = word;
    }
}
