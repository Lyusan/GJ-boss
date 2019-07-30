using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationText : MonoBehaviour
{
    float timer = 5f;
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            Destroy(gameObject);
        }
    }
}
