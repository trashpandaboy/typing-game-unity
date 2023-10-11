using System;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public float infiniteShapeSpeed = 3;
    public float flipInvokeDelay = 10;

    [SerializeField]
    float _rotationPercentage = 0f;

    DateTime? _lastRotationCompletedTime;
    float _rotationDelayInSeconds = 10f;

    void Update()
    {
        if (!SessionDataManager.Instance.GameOver)
        {
            EvaluateInfiniteShape();
            EvaluateFlip();
        }
    }

    private void EvaluateInfiniteShape()
    {
        if(MeteorSpawner.Instance != null)
        {
            var currMeteorPosition = MeteorSpawner.Instance.CurrentTargetTransform?.position ?? Vector3.zero;

            var oldX = transform.position.x;
            var newX = 1f * Mathf.Sin(infiniteShapeSpeed * Time.time) + currMeteorPosition.x;
            var y = 0.25f * Mathf.Sin(2 * infiniteShapeSpeed * Time.time);
            transform.position = new Vector3(Mathf.Lerp(oldX, newX, Time.deltaTime * 10), -3 + y, transform.position.z);

            var size = 1 + 0.05f * Mathf.Sin(3.14f * 0.5f + 2 * infiniteShapeSpeed * Time.time);
            transform.localScale = new Vector3(size, size, size);
        }
    }

    private void EvaluateFlip()
    {
        if (!_lastRotationCompletedTime.HasValue
            || _lastRotationCompletedTime.Value.AddSeconds(_rotationDelayInSeconds) < DateTime.Now)
        {
            _rotationPercentage += Time.deltaTime;
            _rotationPercentage = Mathf.Clamp01(_rotationPercentage);

            transform.rotation = new Quaternion(transform.rotation.x, _rotationPercentage, transform.rotation.z, transform.rotation.w);

            if (_rotationPercentage == 1)
            {
                _lastRotationCompletedTime = DateTime.Now;
                _rotationPercentage = 0f;
            }
        }
    }
}
