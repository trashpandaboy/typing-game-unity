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

    [SerializeField]
    private bool _gameOver = true;
    private bool _initialied = false;

    public bool GameOver { get { return _gameOver; } }

    public bool Initialized => _initialied;

    #region Events and Actions

    UnityAction<DataSet> _onScorePointsAction;
    UnityAction<DataSet> _onKeyPressedAction;
    UnityAction<DataSet> _onWordSpelledCorrectlyAction;
    UnityAction<DataSet> _onGameOverAction;
    UnityAction<DataSet> _onNewGameAction;

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

    private void OnGameOverMethod(DataSet arg0)
    {
        _gameOver = true;
    }

    private void OnNewGameMethod(DataSet arg0)
    {
        _wpm = 0;
        _points = 0;
        _gameOver = false;
    }

    #endregion

    #region Unity

    protected override void Awake()
    {
        base.Awake();
        _onScorePointsAction = new UnityAction<DataSet>(OnScorePointsActionMethod);
        _onKeyPressedAction = new UnityAction<DataSet>(OnKeyPressedActionMethod);
        _onWordSpelledCorrectlyAction = new UnityAction<DataSet>(OnWordSpelledCorrecltyActionMethod);
        _onGameOverAction = new UnityAction<DataSet>(OnGameOverMethod);
        _onNewGameAction = new UnityAction<DataSet>(OnNewGameMethod);
        EventDispatcher.StartListening(GameEvent.ScorePoints.ToString(), _onScorePointsAction);
        EventDispatcher.StartListening(GameEvent.KeyPressed.ToString(), _onKeyPressedAction);
        EventDispatcher.StartListening(GameEvent.WorldSpelledCorrectly.ToString(), _onWordSpelledCorrectlyAction);
        EventDispatcher.StartListening(GameEvent.GameOver.ToString(), _onGameOverAction);
        EventDispatcher.StartListening(GameEvent.NewGame.ToString(), _onNewGameAction);
        _initialied = true;
        _gameOver = true;
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
