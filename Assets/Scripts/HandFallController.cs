using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFallController : MonoBehaviour
{
    static float handFallTimer = 1.8f;
    static float handFallDommageMaxTime = 1.1f;
    static float handFallDommageMinTime = 0.8f;
    static float dommage = 3;
    void Update () {
        handFallTimer -= Time.deltaTime;
        if (handFallTimer < 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        onTrigger(other);
    }
    private void OnTriggerStay2D(Collider2D other) {
        onTrigger(other);
    }

    private void onTrigger(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            HeroController e = other.GetComponent<HeroController> ();
            if (handFallTimer < handFallDommageMaxTime && handFallTimer > handFallDommageMinTime) {
                e.Dommage(dommage);
            }
        }
    }
}