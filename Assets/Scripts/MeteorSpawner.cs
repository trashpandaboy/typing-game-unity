using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject _meteorPrefab;
    [SerializeField]
    GameObject _spawnArea;
    BoxCollider2D _spawnAreaBoxCollider;

    DateTime? _lastSpawn;
    [SerializeField]
    float _delay = 2f;

    private void Start()
    {
        _spawnAreaBoxCollider = _spawnArea?.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if(!_lastSpawn.HasValue || _lastSpawn.Value.AddSeconds(_delay) < DateTime.Now)
        {
            _lastSpawn = DateTime.Now;


            Meteor tmpMeteor = Instantiate(_meteorPrefab, RandomPointInBounds(_spawnAreaBoxCollider.bounds), Quaternion.identity, _spawnArea.transform).GetComponent<Meteor>();
            tmpMeteor.SetupWord(WordManager.Instance.GetRandomWord(WordManager.Instance.GetRandomLength()));
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
