using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFallController : MonoBehaviour
{
    private float handTime;
    void Start () {

    }
    void Awake () {
        handTime = 1.8f;
    }

    // Update is called once per frame
    void Update () {
        handTime -= Time.deltaTime;
        if (handTime < 0) {
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
            if (handTime < 1.1f && handTime > 0.8f) {
                e.Dommage(3);
            }
        }
    }
}