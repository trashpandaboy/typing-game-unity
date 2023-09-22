using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using com.trashpandaboy.core;
using UnityEngine;

public class WordManager : Manager<WordManager>
{
    [SerializeField]
    Dictionary<int, List<string>> commonWordsByLenght;

    [SerializeField]
    List<string> commonWordsList;

    private void Start()
    {
        commonWordsByLenght = new Dictionary<int, List<string>>();
        commonWordsList = new List<string>();
         
        string listOfWord = Resources.Load("common").ToString();

        using (StringReader sr = new StringReader(listOfWord))
        {
            string wordLine;

            while ((wordLine = sr.ReadLine()) != null)
            {
                if(wordLine.Length > 0)
                {
                    commonWordsList.Add(wordLine);

                    if (!commonWordsByLenght.ContainsKey(wordLine.Length))
                        commonWordsByLenght[wordLine.Length] = new List<string>();

                    commonWordsByLenght[wordLine.Length].Add(wordLine);
                }
            }
        }

    }


    public string GetRandomWord(int length)
    {
        string word = "null";
        if(commonWordsByLenght.ContainsKey(length))
        {
            int index = UnityEngine.Random.Range(0,commonWordsByLenght[length].Count);

            word = commonWordsByLenght[length][index];
        }

        return word;
    }

    private int[] GetAvailablesLength()
    {
        return commonWordsByLenght.Keys.ToArray();
    }

    public int GetRandomLength()
    {
        int[] availablesLength = GetAvailablesLength();
        int index = UnityEngine.Random.Range(0, availablesLength.Length);

        return availablesLength[index];
    }

}
