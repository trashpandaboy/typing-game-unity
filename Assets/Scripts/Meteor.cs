using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField]
    string _word;
    SpriteOutline _spriteOutline;

    [SerializeField]
    GameObject _wordContainer;
    [SerializeField]
    GameObject _letterPrefab;

    List<LetterComponent> _letters;
    int _currentLetterIndex = 0;
    [SerializeField]
    float _letterSpan = 0.3f;
    [SerializeField]
    float _xStart = 0f;

    public string Word { get { return _word; } }

    private void Awake()
    {
        _letters = new List<LetterComponent>();
        _spriteOutline = GetComponent<SpriteOutline>();
    }

    private void Update()
    {
        transform.position += Vector3.down * Time.deltaTime;
    }

    internal void SetupWord(string word)
    {
        _word = word;
        SetupLetterComponents();
    }

    private void SetupLetterComponents()
    {
        float xOffset = 0;
        if (_word.Length % 2 != 0)
        {
            xOffset = _letterSpan * -1;
        }
        else
        {
            xOffset = (_letterSpan / 2) * -1;
        }

        _xStart = (_word.Length / 2) * -1;
        _xStart *= _letterSpan;
        _xStart += xOffset;

        int count = 1;
        foreach (char letter in _word)
        {
            Vector3 localPos = new Vector3(_xStart + (count * _letterSpan), 0, 0);
            Debug.Log(localPos);
            LetterComponent tempLetter = Instantiate(_letterPrefab, _wordContainer.transform).GetComponent<LetterComponent>();
            tempLetter.gameObject.transform.localPosition = localPos;
            tempLetter.Setup(letter);
            _letters.Add(tempLetter);
            count++;
        }
    }

    public void SelectMeteor()
    {
        _spriteOutline.enabled = true;
    }

    internal bool IsCurrentLetterEqualsTo(char letter)
    {
        return _word[_currentLetterIndex] == letter;
    }

    internal void StrokeLetter()
    {
        _letters[_currentLetterIndex].SetLetterAsStroked();
        _currentLetterIndex++;
    }

    internal bool IsWordCompleted()
    {
        return _currentLetterIndex == _word.Length;
    }
}
