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

    public Vector3 CurrentTargetPosition
    {
        get { return _meteorSelected?.transform.position ?? Vector3.zero; }
    }

    public Transform CurrentTargetTransform
    {
        get { return _meteorSelected?.transform ?? null; }
    }

    DateTime? _lastSpawn;
    [SerializeField]
    float _delay = 0.1f;

    UnityAction<DataSet> _onKeyPressed;

    private void Start()
    {
        _meteorPool = PoolsManager.Instance.GetObjectPool(_meteorPrefab);
        _meteorPool.name = "MeteorObjectPool";
        _meteorsInField = new List<GameObject>();
        _spawnAreaBoxCollider = _spawnArea?.GetComponent<BoxCollider2D>();
        _onKeyPressed = new UnityAction<DataSet>(OnKeyPressed);

        EventDispatcher.StartListening(GameEvent.KeyPressed.ToString(), _onKeyPressed);
    }

    private void OnKeyPressed(DataSet value)
    {
        char letter = value.GetData<char>("key");

        if(_meteorSelected == null)
        {
            foreach(var meteor in _meteorsInField)
            {
                if(meteor != null)
                {
                    if(meteor.GetComponent<Meteor>().IsCurrentLetterEqualsTo(letter))
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
            if(_meteorSelected.IsCurrentLetterEqualsTo(letter))
            {
                _meteorSelected.StrokeLetter();
                EventDispatcher.TriggerEvent(GameEvent.PlayerShot.ToString());
            }
            else
            {
                _meteorSelected.WrongLetter();
            }
        }

        if(_meteorSelected != null && _meteorSelected.gameObject != null && _meteorSelected.IsWordCompleted())
        {
            if(_meteorsInField.Contains(_meteorSelected.gameObject))
                _meteorsInField.Remove(_meteorSelected.gameObject);

            _meteorSelected.DestroyMeteor();
            _meteorSelected = null;
        }
    }

    private void FixedUpdate()
    {
        if(!_lastSpawn.HasValue || _lastSpawn.Value.AddSeconds(_delay) < DateTime.Now)
        {
            _lastSpawn = DateTime.Now;
            SpawnMeteor();
        }
    }

    private void SpawnMeteor()
    {
        Meteor tmpMeteor = _meteorPool.ProvideGameobject().GetComponent<Meteor>();
        var position = RandomPointInBounds(_spawnAreaBoxCollider.bounds);
        tmpMeteor.gameObject.transform.parent = _spawnArea.transform;
        tmpMeteor.gameObject.transform.position = position;
        tmpMeteor.Setup(WordManager.Instance.GetRandomWord());
        tmpMeteor.gameObject.SetActive(true);
        _meteorsInField.Add(tmpMeteor.gameObject);
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
            0
        );
    }
}
