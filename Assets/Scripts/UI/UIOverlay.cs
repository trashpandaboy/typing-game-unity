using System;
using com.trashpandaboy.core;
using com.trashpandaboy.core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static Utils;

public class UIOverlay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _points;
    [SerializeField]
    TextMeshProUGUI _wpm;
    [SerializeField]
    TextMeshProUGUI _gameOver;

    UnityAction<DataSet> _onGameOverAction;
    UnityAction<DataSet> _onNewGameAction;

    private void Awake()
    {
        OnGameOverMethod(null);
        _onGameOverAction = new UnityAction<DataSet>(OnGameOverMethod);
        _onNewGameAction = new UnityAction<DataSet>(OnNewGameMethod);
        EventDispatcher.StartListening(GameEvent.GameOver.ToString(), _onGameOverAction);
        EventDispatcher.StartListening(GameEvent.NewGame.ToString(), _onNewGameAction);
    }

    private void OnNewGameMethod(DataSet arg0)
    {
        _gameOver.gameObject.SetActive(false);
    }

    private void OnGameOverMethod(DataSet arg0)
    {
        _gameOver.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        _points.text = $"{SessionDataManager.Instance.Points}".PadLeft(6, '0');
        _wpm.text = $"WPM: {SessionDataManager.Instance.WPM}";
    }
}
