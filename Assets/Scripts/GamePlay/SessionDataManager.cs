using System;
using com.trashpandaboy.core;
using com.trashpandaboy.core.Utils;
using UnityEngine;
using UnityEngine.Events;
using static Utils;

public class SessionDataManager : Manager<SessionDataManager>
{
    [SerializeField]
    int _points = 0;
    [SerializeField]
    int _wpm = 0;
    [SerializeField]
    int _amountCharacterTyped = 0;

    DateTime? _startingDigitTime = null;


    public int Points { get { return _points; } }

    public int WPM { get { return _wpm; } }

    #region Events and Actions

    UnityAction<DataSet> _onScorePointsAction;
    UnityAction<DataSet> _onKeyPressedAction;
    UnityAction<DataSet> _onWordSpelledCorrectlyAction;

    private void OnScorePointsActionMethod(DataSet data)
    {
        int points = data.GetData<int>("points");
        _points += points;
    }

    private void OnKeyPressedActionMethod(DataSet data)
    {
        EventDispatcher.StopListening(GameEvent.KeyPressed.ToString(), _onKeyPressedAction);
        _startingDigitTime = DateTime.Now;
    }

    private void OnWordSpelledCorrecltyActionMethod(DataSet data)
    {
        int length = data.GetData<int>("length");
        _amountCharacterTyped += length;
    }

    #endregion

    #region Unity

    protected override void Awake()
    {
        base.Awake();
        _onScorePointsAction = new UnityAction<DataSet>(OnScorePointsActionMethod);
        _onKeyPressedAction = new UnityAction<DataSet>(OnKeyPressedActionMethod);
        _onWordSpelledCorrectlyAction = new UnityAction<DataSet>(OnWordSpelledCorrecltyActionMethod);
        EventDispatcher.StartListening(GameEvent.ScorePoints.ToString(), _onScorePointsAction);
        EventDispatcher.StartListening(GameEvent.KeyPressed.ToString(), _onKeyPressedAction);
        EventDispatcher.StartListening(GameEvent.WorldSpelledCorrectly.ToString(), _onWordSpelledCorrectlyAction);
    }

    private void FixedUpdate()
    {
        if (_startingDigitTime.HasValue)
        {
            var result = DateTime.Now - _startingDigitTime.Value;

            _wpm = (int)Math.Floor((_amountCharacterTyped / 5) / result.TotalMinutes);
        }
    }

    #endregion
}
