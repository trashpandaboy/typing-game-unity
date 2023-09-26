using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    BoxCollider2D _boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();

        var aspect = (float)Screen.width / Screen.height;
        var orthoSize = Camera.main.orthographicSize;

        var width = 2.0f * orthoSize * aspect;
        var height = 2.0f * Camera.main.orthographicSize;

        width *= 0.7f;

        _boxCollider.size = new Vector2(width, _boxCollider.size.y);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
