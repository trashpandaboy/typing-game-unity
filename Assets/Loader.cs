using System;
using System.Collections;
using System.Collections.Generic;
using com.trashpandaboy.core;
using com.trashpandaboy.core.Pooling;
using TMPro;
using UnityEngine;

public class Loader : Manager<Loader>
{
    [Header("In Scene object")]
    public GameObject SpawnArea;
    public GameObject FirstBG;
    public GameObject SecondBG;
    public GameObject UICanvas;

    [Header("Objects to initialize")]
    [SerializeField] GameObject _eventDispatcherPrefab;
    [SerializeField] GameObject _sessionDataPrefab;
    [SerializeField] GameObject _poolManagerPrefab;
    [SerializeField] GameObject _wordManagerPrefab;
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] GameObject _keyHandlerPrefab;
    [SerializeField] GameObject _meteorSpawnerPrefab;
    [SerializeField] GameObject _backgroundScrollerPrefab;
    [SerializeField] GameObject _uiIngamePrefab;
    [Header("UI Elements")]
    [SerializeField]
    [Space] private TextMeshProUGUI _loaderText;
    [SerializeField] RectTransform _fillerProgress;

    int _percentage = 0;


    private GameObject _player;

    public GameObject Player => _player;

    public void SetPlayerObject(GameObject obj)
    {
        _player = obj;
    }

    private void Start()
    {
        StartCoroutine(InitializeAllComponents());
    }

    private IEnumerator InitializeAllComponents()
    {
        Instantiate(_eventDispatcherPrefab);
        yield return new WaitForSeconds(0.2f);
        _percentage = 14;

        Instantiate(_sessionDataPrefab);
        do
        {
            yield return new WaitForSeconds(0.2f);
        } while (!SessionDataManager.Instance.Initialized);
        _percentage = 28;

        Instantiate(_poolManagerPrefab);
        yield return new WaitForSeconds(0.2f);
        _percentage = 42;

        Instantiate(_wordManagerPrefab);
        do
        {
            yield return new WaitForSeconds(0.2f);
        } while (!WordManager.Instance.Initialized);
        _percentage = 56;
        Instantiate(_playerPrefab);
        yield return new WaitForSeconds(0.2f);
        _percentage = 70;

        Instantiate(_keyHandlerPrefab);
        yield return new WaitForSeconds(0.2f);
        _percentage = 84;


        Instantiate(_meteorSpawnerPrefab);
        do
        {
            yield return new WaitForSeconds(0.2f);
        } while (!MeteorSpawner.Instance.Initialized);
        _percentage = 90;


        Instantiate(_backgroundScrollerPrefab);
        yield return new WaitForSeconds(0.2f);
        _percentage = 96;


        Instantiate(_uiIngamePrefab, UICanvas.transform);
        _percentage = 100;

        yield return new WaitForSeconds(1f);


        yield break;
    }

    private void FixedUpdate()
    {
        _loaderText.text = $"{_percentage}%";
        _fillerProgress.offsetMax = new Vector2(-(240 * (_percentage/100)), _fillerProgress.offsetMax.y);
        if (_percentage == 100)
            gameObject.SetActive(false);
    }
}
