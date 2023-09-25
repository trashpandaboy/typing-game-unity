using TMPro;
using UnityEngine;

public class LetterComponent : MonoBehaviour
{
    [SerializeField]
    TextMeshPro _letter;

    public void Setup(char letter)
    {
        Reset();
        _letter.text = letter.ToString();
    }

    private void Reset()
    {
        _letter.color = Color.white;
    }

    public void SetLetterAsStroked()
    {
        _letter.color = Color.red;
    }
}
