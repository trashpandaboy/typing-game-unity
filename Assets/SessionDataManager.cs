using System;
using System.Collections;
using System.Collections.Generic;
using com.trashpandaboy.core;
using com.trashpandaboy.core.Utils;
using UnityEngine;
using UnityEngine.Events;
using static Utils;

public class SessionDataManager : Manager<SessionDataManager>
{
    [SerializeField]
    int _points = 0;

    public int Points { get { return _points; } }

    UnityAction<DataSet> _onScorePointsAction;

    protected override void Awake()
    {
        base.Awake();
        _onScorePointsAction = new UnityAction<DataSet>(OnScorePointsActionMethod);
        EventDispatcher.StartListening(GameEvent.ScorePoints.ToString(), _onScorePointsAction);
    }

    private void OnScorePointsActionMethod(DataSet data)
    {
        int points = data.GetData<int>("points");
        _points += points;
    }
}
