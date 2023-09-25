using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using com.trashpandaboy.core;
using UnityEngine;

public class WordManager : Manager<WordManager>
{
    [SerializeField]
    Dictionary<int, List<string>> _commonWordsByLenght;

    [SerializeField]
    List<string> _commonWordsList;


    List<string> _lastTwentyWordsGenerated;

    private void Start()
    {
        _lastTwentyWordsGenerated = new List<string>();
        _commonWordsByLenght = new Dictionary<int, List<string>>();
        _commonWordsList = new List<string>();
         
        string listOfWord = Resources.Load("common").ToString();

        using (StringReader sr = new StringReader(listOfWord))
        {
            string wordLine;

            while ((wordLine = sr.ReadLine()) != null)
            {
                if(wordLine.Length > 1 && Regex.IsMatch(wordLine, @"[A-Za-z]"))
                {
                    wordLine = wordLine.ToLower();
                    _commonWordsList.Add(wordLine);

                    if (!_commonWordsByLenght.ContainsKey(wordLine.Length))
                        _commonWordsByLenght[wordLine.Length] = new List<string>();

                    _commonWordsByLenght[wordLine.Length].Add(wordLine);
                }
            }
        }
    }

    public string GetRandomWord(int length)
    {
        string word = "null";
        if(_commonWordsByLenght.ContainsKey(length))
        {
            int tryCounter = 0;
            int index = 0;
            do
            {
                if (tryCounter == _commonWordsByLenght[length].Count)
                    length = GetRandomLength();

                tryCounter++;
                index = UnityEngine.Random.Range(0, _commonWordsByLenght[length].Count);

            }
            while (_lastTwentyWordsGenerated.Contains(_commonWordsByLenght[length][index]));

            word = _commonWordsByLenght[length][index];
            _lastTwentyWordsGenerated.Add(word);

            if (_lastTwentyWordsGenerated.Count > 100)
                _lastTwentyWordsGenerated.RemoveAt(0);

        }

        return word;
    }

    private int[] GetAvailablesLength()
    {
        return _commonWordsByLenght.Keys.ToArray();
    }

    public int GetRandomLength()
    {
        int[] availablesLength = GetAvailablesLength();
        int index = UnityEngine.Random.Range(0, availablesLength.Length);

        return availablesLength[index];
    }
}
