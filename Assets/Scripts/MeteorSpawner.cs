using System;
using System.Collections;
using System.Collections.Generic;
using com.trashpandaboy.core;
using com.trashpandaboy.core.Utils;
using UnityEngine;
using UnityEngine.Events;
using static Utils;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject _meteorPrefab;
    [SerializeField]
    GameObject _spawnArea;
    BoxCollider2D _spawnAreaBoxCollider;


    List<GameObject> _meteorsInField;
    Meteor _meteorSelected = null;

    DateTime? _lastSpawn;
    [SerializeField]
    float _delay = 2f;

    UnityAction<DataSet> _onKeyPressed;

    private void Start()
    {
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
                        Debug.Log($"Meteor selected: {_meteorSelected.Word}");
                        _meteorSelected.StrokeLetter();
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
            }
        }

        if(_meteorSelected != null && _meteorSelected.gameObject != null && _meteorSelected.IsWordCompleted())
        {
            if(_meteorsInField.Contains(_meteorSelected.gameObject))
                _meteorsInField.Remove(_meteorSelected.gameObject);
            Destroy(_meteorSelected.gameObject);
        }
    }

    private void Update()
    {
        if(!_lastSpawn.HasValue || _lastSpawn.Value.AddSeconds(_delay) < DateTime.Now)
        {
            _lastSpawn = DateTime.Now;


            Meteor tmpMeteor = Instantiate(_meteorPrefab, RandomPointInBounds(_spawnAreaBoxCollider.bounds), Quaternion.identity, _spawnArea.transform).GetComponent<Meteor>();
            tmpMeteor.SetupWord(WordManager.Instance.GetRandomWord(WordManager.Instance.GetRandomLength()));
            _meteorsInField.Add(tmpMeteor.gameObject);
        }
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
