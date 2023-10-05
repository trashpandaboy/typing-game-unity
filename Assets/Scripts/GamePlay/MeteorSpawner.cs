using System;
using System.Collections.Generic;
using com.trashpandaboy.core;
using com.trashpandaboy.core.Pooling;
using com.trashpandaboy.core.Utils;
using UnityEngine;
using UnityEngine.Events;
using static Utils;

public class MeteorSpawner : Manager<MeteorSpawner>
{
    [SerializeField]
    GameObject _meteorPrefab;
    [SerializeField]
    GameObject _spawnArea;

    BoxCollider2D _spawnAreaBoxCollider;

    ObjectPool _meteorPool;

    List<GameObject> _meteorsInField;

    Meteor _meteorSelected = null;

    [SerializeField]
    float _spawnDelay = 0.1f;
    DateTime? _lastSpawn;


    public Vector3 CurrentTargetPosition
    {
        get { return _meteorSelected?.transform.position ?? Vector3.zero; }
    }

    public Transform CurrentTargetTransform
    {
        get { return _meteorSelected?.transform ?? null; }
    }

    public Transform Player;

    #region Events and Action

    UnityAction<DataSet> _onKeyPressed;
    UnityAction<DataSet> _onNewGame;

    private void OnKeyPressed(DataSet value)
    {
        char letter = value.GetData<char>("key");

        if (_meteorSelected == null)
        {
            foreach (var meteor in _meteorsInField)
            {
                if (meteor != null)
                {
                    if (meteor.GetComponent<Meteor>().IsCurrentLetterEqualsTo(letter))
                    {
                        _meteorSelected = meteor.gameObject.GetComponent<Meteor>();
                        _meteorSelected.SelectMeteor();
                        _meteorSelected.StrokeLetter();
                        EventDispatcher.TriggerEvent(GameEvent.PlayerShot.ToString());
                        break;
                    }
                }
            }
        }
        else
        {
            if (_meteorSelected.IsCurrentLetterEqualsTo(letter))
            {
                _meteorSelected.StrokeLetter();
                EventDispatcher.TriggerEvent(GameEvent.PlayerShot.ToString());
            }
            else
            {
                _meteorSelected.WrongLetter();
            }
        }

        if (_meteorSelected != null && _meteorSelected.gameObject != null && _meteorSelected.IsWordCompleted())
        {
            if (_meteorsInField.Contains(_meteorSelected.gameObject))
                _meteorsInField.Remove(_meteorSelected.gameObject);

            _meteorSelected.DestroyMeteor();
            _meteorSelected = null;
        }
    }

    private void OnNewGame(DataSet arg0)
    {
        _meteorSelected = null;
        ReleaseAllMeteors();
    }

    #endregion

    #region Unity

    private void Start()
    {
        _meteorPool = PoolsManager.Instance.GetObjectPool(_meteorPrefab);
        _meteorPool.name = "MeteorObjectPool";
        _meteorsInField = new List<GameObject>();
        _spawnAreaBoxCollider = _spawnArea?.GetComponent<BoxCollider2D>();
        _onKeyPressed = new UnityAction<DataSet>(OnKeyPressed);
        _onNewGame = new UnityAction<DataSet>(OnNewGame);

        EventDispatcher.StartListening(GameEvent.KeyPressed.ToString(), _onKeyPressed);
        EventDispatcher.StartListening(GameEvent.NewGame.ToString(), _onNewGame);
    }


    private void FixedUpdate()
    {
        if(!SessionDataManager.Instance.GameOver)
        {
            if (!_lastSpawn.HasValue || _lastSpawn.Value.AddSeconds(_spawnDelay) < DateTime.Now)
            {
                _lastSpawn = DateTime.Now;
                SpawnMeteor();
            }
        }
    }

    #endregion

    private void SpawnMeteor()
    {
        Meteor tmpMeteor = _meteorPool.ProvideGameobject().GetComponent<Meteor>();
        var position = RandomPointInBounds(_spawnAreaBoxCollider.bounds);
        tmpMeteor.gameObject.transform.parent = _spawnArea.transform;
        tmpMeteor.gameObject.transform.position = position;
        tmpMeteor.Setup(WordManager.Instance.GetRandomWord(), Player.transform.position - position);
        tmpMeteor.gameObject.SetActive(true);
        _meteorsInField.Add(tmpMeteor.gameObject);
    }


    private void ReleaseAllMeteors()
    {
        foreach(var meteor in _meteorsInField)
        {
            meteor.GetComponent<Meteor>().Reset();
            _meteorPool.ReleaseGameobject(meteor);
        }
    }


    private static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
            0
        );
    }
}
