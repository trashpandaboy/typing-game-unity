using TMPro;
using UnityEngine;

public class UIOverlay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _points;
    [SerializeField]
    TextMeshProUGUI _wpm;

    private void FixedUpdate()
    {
        _points.text = $"{SessionDataManager.Instance.Points}".PadLeft(6, '0');
        _wpm.text = $"WPM: {SessionDataManager.Instance.WPM}";
    }
}
