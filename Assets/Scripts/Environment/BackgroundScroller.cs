using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public GameObject bg1;
    public GameObject bg2;

    public float speed;

    float size = 6f*5;

    void Start()
    {
        bg1.transform.position = Vector3.zero;
        bg2.transform.position = bg1.transform.position + new Vector3(0,size,0);
    }

    void Update()
    {
        if (!SessionDataManager.Instance.GameOver)
        {
            var pointsMult = 1 + SessionDataManager.Instance.Points * 0.01f;
            bg1.transform.Translate(Vector3.down * speed * pointsMult * Time.deltaTime);
            bg2.transform.Translate(Vector3.down * speed * pointsMult * Time.deltaTime);
            if (bg1.transform.position.y < -size)
                bg1.transform.Translate(2f * Vector3.up * size);
            if (bg2.transform.position.y < -size)
                bg2.transform.Translate(2f * Vector3.up * size);
        }
    }
}
