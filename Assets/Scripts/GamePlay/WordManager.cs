using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using com.trashpandaboy.core;
using UnityEngine;

public class WordManager : Manager<WordManager>
{
    [SerializeField]
    Dictionary<int, List<string>> _commonWordsByLength;

    [SerializeField]
    List<string> _commonWordsList;

    [SerializeField]
    int _generatedWords;

    bool _initialized = false;

    public bool Initialized => _initialized;

    #region Unity

    private void Start()
    {
        _commonWordsByLength = new Dictionary<int, List<string>>();
        _commonWordsList = new List<string>();

        string listOfWord = Resources.Load("common").ToString();

        using (StringReader sr = new StringReader(listOfWord))
        {
            string wordLine;

            while ((wordLine = sr.ReadLine()) != null)
            {
                if (wordLine.Length > 1 && Regex.IsMatch(wordLine, @"[A-Za-z]"))
                {
                    wordLine = wordLine.ToLower();
                    _commonWordsList.Add(wordLine);
                }
            }
        }

        PopulateDictionaryCommondWordByLength();

        _initialized = true;
    }

    #endregion

    private int[] GetAvailablesLength()
    {
        return _commonWordsByLength.Keys.ToArray();
    }


    public void PopulateDictionaryCommondWordByLength()
    {
        for(int i=0;i < _commonWordsList.Count; i++)
        {
            if (!_commonWordsByLength.ContainsKey(_commonWordsList[i].Length))
                _commonWordsByLength[_commonWordsList[i].Length] = new List<string>();

            _commonWordsByLength[_commonWordsList[i].Length].Add(_commonWordsList[i]);
        }
    }

    public string GetRandomWord()
    {
        string word = "null";
        int index = 0;
        if(_commonWordsByLength.Keys.Count < 1)
        {
            PopulateDictionaryCommondWordByLength();   
        }

        int length = GetRandomLength();
        index = UnityEngine.Random.Range(0, _commonWordsByLength[length].Count);

        word = _commonWordsByLength[length][index];

        _commonWordsByLength[length].RemoveAt(index);

        if (_commonWordsByLength[length].Count < 1)
        {
            _commonWordsByLength.Remove(length);
        }

        return word;
    }

    public int GetRandomLength()
    {
        int[] availablesLength = GetAvailablesLength();
        int index = UnityEngine.Random.Range(0, availablesLength.Length);

        return availablesLength[index];
    }
}
