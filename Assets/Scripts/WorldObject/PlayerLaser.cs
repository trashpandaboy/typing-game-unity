using System;
using com.trashpandaboy.core;
using com.trashpandaboy.core.Pooling;
using com.trashpandaboy.core.Utils;
using UnityEngine;
using static Utils;

public class PlayerLaser : MonoBehaviour
{
    [SerializeField]
    float _baseSpeed = 25f;

    SpriteRenderer _spriteRenderer;
    Vector3 _direction;
    bool _canMove = false;
    bool _triggered = false;

    ObjectPool _laserPool;

    DateTime _spawnTime;
    private bool _initialized = false;

    Transform _target;


    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _laserPool = PoolsManager.Instance.GetObjectPoolOfType(typeof(PlayerLaser));
        Reset();
    }

    private void OnEnable()
    {
        Reset();
    }

    private void Reset()
    {
        _triggered = false;
        _spriteRenderer.enabled = false;
        _direction = Vector3.zero;
        _canMove = false;
        _initialized = false;
    }

    internal void Initialize(Transform target)
    {
        _target = target;
        _spawnTime = DateTime.Now;
        Reset();

        _direction = transform.position - _target.position;
        _canMove = true;
        _spriteRenderer.enabled = true;
        _initialized = true;

        DataSet sound = new DataSet();
        sound.AddData("sound", SfxSound.Shoot.ToString());
        EventDispatcher.TriggerEvent(GameEvent.PlaySound.ToString(), sound);
    }

    private void Update()
    {
        if(_initialized)
        {
            if (_spawnTime.AddSeconds(5) < DateTime.Now)
                DestroyLaser();

            if (_triggered)
                DestroyLaser();
            else
                if(_canMove)
                {
                    _direction = _target.position - transform.position;
                    transform.right = _direction;
                    var speed = Time.deltaTime * _baseSpeed;
                    var pointsMult = 1 + SessionDataManager.Instance.Points * 0.001f;
                    speed *= pointsMult;
                    transform.position += _direction.normalized * speed;
                }
        }
    }

    private void DestroyLaser()
    {
        _initialized = false;
        if (_laserPool == null)
            _laserPool = PoolsManager.Instance.GetObjectPoolOfType(gameObject.GetType());
        _laserPool.ReleaseGameobject(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _triggered = true;
    }
}
