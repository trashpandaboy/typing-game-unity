using System;
using System.Collections;
using System.Collections.Generic;
using com.trashpandaboy.core;
using com.trashpandaboy.core.Utils;
using UnityEngine;
using static Utils;

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
    private int _errors = 0;

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
        if (_currentLetterIndex == _word.Length)
            return false;
        return _word[_currentLetterIndex] == letter;
    }

    internal bool IsWordCompleted()
    {
        return _currentLetterIndex == _word.Length;
    }

    internal void StrokeLetter()
    {
        _letters[_currentLetterIndex].SetLetterAsStroked();
        _currentLetterIndex++;
    }

    internal void WrongLetter()
    {
        _errors++;
    }


    internal void DestroyMeteor()
    {
        StartCoroutine(RemoveMeteor());
    }

    IEnumerator RemoveMeteor()
    {
        yield return new WaitForSeconds(0.2f);
        DataSet data = new DataSet();
        int points = 2 * _word.Length;
        int pointsWithMalus = Math.Clamp(points - _errors, 0, points);
        Debug.Log($"{points} - {pointsWithMalus} - {_errors}");
        data.AddData("points", pointsWithMalus);
        EventDispatcher.TriggerEvent(GameEvent.ScorePoints.ToString(),data);
        Destroy(gameObject);
    }
}
