using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterComponent : MonoBehaviour
{
    [SerializeField]
    TextMeshPro _letter;

    public void Setup(char letter)
    {
        _letter.text = letter.ToString();
    }

    public void SetLetterAsStroked()
    {
        _letter.color = Color.red;
    }
}
