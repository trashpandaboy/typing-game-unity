using System;
using System.Collections;
using System.Collections.Generic;
using com.trashpandaboy.core;
using com.trashpandaboy.core.Pooling;
using com.trashpandaboy.core.Utils;
using UnityEngine;
using static Utils;

public class Meteor : MonoBehaviour
{
    /// <summary>
    /// Word who is related to the meteor
    /// </summary>
    [SerializeField]
    string _word;
    /// <summary>
    /// Sprite outline component who will add a red outline to the sprite
    /// </summary>
    [SerializeField]
    SpriteOutline _spriteOutline;

    [SerializeField]
    Transform _meteorSprite;

    /// <summary>
    /// Transform who will contains Letters
    /// </summary>
    [SerializeField]
    GameObject _wordContainer;
    /// <summary>
    /// LetterComponent prefab
    /// </summary>
    [SerializeField]
    GameObject _letterPrefab;

    /// <summary>
    /// List of all LetterComponent gameobjects
    /// </summary>
    [SerializeField]
    List<LetterComponent> _letters;
    /// <summary>
    /// Current letter's index to type
    /// </summary>
    int _currentLetterIndex = 0;
    /// <summary>
    /// Span to space letters
    /// </summary>
    [SerializeField]
    float _letterSpan = 0.3f;
    /// <summary>
    /// X coord where LetterComponet spawns
    /// </summary>
    float _xStart = 0f;
    /// <summary>
    /// Amount of errors while typing the word
    /// </summary>
    private int _errors = 0;

    /// <summary>
    /// LetterComponent pool
    /// </summary>
    ObjectPool _lettersPool;

    int _hitReceived = 0;

    Vector3 _direction;

    #region Unity

    private void Awake()
    {
        _letters = new List<LetterComponent>();
        _lettersPool = PoolsManager.Instance.GetObjectPool(_letterPrefab);
        _lettersPool.name = "LetterObjectPool";
    }

    private void Update()
    {
        if (!SessionDataManager.Instance.GameOver)
        {
            var speed = Time.deltaTime;
            var pointsMult = 1 + SessionDataManager.Instance.Points * 0.01f;
            speed *= pointsMult;
            transform.position += _direction * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _hitReceived++;
        if (_hitReceived == _word.Length)
        {
            StartCoroutine(RemoveMeteor());
        }
    }

    #endregion

    #region Setup

    internal void Setup(string word, Vector3 direction)
    {
        Reset();

        _word = word;
        _direction = direction.normalized;
        SetupSize();
        SetupLetterComponents();
    }

    public void Reset()
    {
        _direction = Vector3.zero;
        _hitReceived = 0;
        _currentLetterIndex = 0;

        if (_letters.Count > 0)
        {
            foreach (var letterComp in _letters)
            {
                if (letterComp != null)
                    _lettersPool.ReleaseGameobject(letterComp.gameObject);
            }
            _letters.Clear();
        }
        _spriteOutline.enabled = false;
    }

    private void SetupSize()
    {
        var size = 1 + _word.Length / 5f;
        _meteorSprite.localScale = Vector3.one * size;
    }

    #endregion

    #region Letters

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
            SpawnLetter(letter, localPos);
            count++;
        }
    }

    private void SpawnLetter(char letter, Vector3 localPos)
    {
        LetterComponent tempLetter = _lettersPool.ProvideGameobject().GetComponent<LetterComponent>();
        tempLetter.gameObject.transform.parent = _wordContainer.transform;
        tempLetter.gameObject.transform.localPosition = localPos;
        tempLetter.Setup(letter);
        _letters.Add(tempLetter);
    }

    #endregion

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
        if(_currentLetterIndex < _letters.Count)
        {
            _letters[_currentLetterIndex].SetLetterAsStroked();
            _currentLetterIndex++;
        }
    }

    internal void WrongLetter()
    {
        _errors++;
    }

    internal void DestroyMeteor()
    {
        DataSet eventData = new DataSet();
        eventData.AddData("length", _word.Length);
        EventDispatcher.TriggerEvent(GameEvent.WorldSpelledCorrectly.ToString(), eventData);
    }

    IEnumerator RemoveMeteor()
    {
        yield return new WaitForSeconds(0.01f);
        DataSet data = new DataSet();
        int points = 2 * _word.Length;
        int pointsWithMalus = Math.Clamp(points - _errors, 0, points);
        data.AddData("points", pointsWithMalus);
        EventDispatcher.TriggerEvent(GameEvent.ScorePoints.ToString(),data);
        ObjectPool meteorPool = PoolsManager.Instance.GetObjectPoolOfType(gameObject.GetType());
        Reset();
        if(meteorPool != null)
            meteorPool.ReleaseGameobject(gameObject);
        yield break;
    }

}
