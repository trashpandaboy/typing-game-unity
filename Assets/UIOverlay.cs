using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIOverlay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _points;

    private void FixedUpdate()
    {
        _points.text = $"{SessionDataManager.Instance.Points}".PadLeft(6, '0');
    }
}
