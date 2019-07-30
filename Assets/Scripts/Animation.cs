using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    float timer = 1.5f;
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            Destroy(gameObject);
        }
    }
}
