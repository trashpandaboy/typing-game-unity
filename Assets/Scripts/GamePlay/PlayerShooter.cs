using com.trashpandaboy.core;
using com.trashpandaboy.core.Pooling;
using com.trashpandaboy.core.Utils;
using UnityEngine;
using UnityEngine.Events;
using static Utils;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField]
    GameObject _laserPrefab;
    ObjectPool _laserPool;

    #region Events and Actions

    UnityAction<DataSet> _onFireEventTriggered;

    private void Shoot(DataSet data)
    {
        var laserObj = _laserPool.ProvideGameobject();
        PlayerLaser laser = laserObj.GetComponent<PlayerLaser>();
        laser.transform.position = transform.position;
        laser.Initialize(GetMeteorTransform());
    }

    #endregion

    #region Unity

    private void Awake()
    {
        _laserPool = PoolsManager.Instance.GetObjectPool(_laserPrefab);
        _laserPool.name = "PlayerLaserPool";
        _onFireEventTriggered = new UnityAction<DataSet>(Shoot);
        EventDispatcher.StartListening(GameEvent.PlayerShot.ToString(), _onFireEventTriggered);
    }

    private void OnDestroy()
    {
        EventDispatcher.StopListening(GameEvent.PlayerShot.ToString(), _onFireEventTriggered);
    }

    #endregion

    private Transform GetMeteorTransform()
    {
        return MeteorSpawner.Instance.CurrentTargetTransform;
    }
}
