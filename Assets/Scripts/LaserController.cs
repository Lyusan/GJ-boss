using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    static float laserTime = 1f;
    static float activeLaserTime = .2f;
    float laserTimer;
    static int dommage = 2;
    void Start () {

    }
    void Awake () {
        laserTimer = laserTime;
    }

    void Update () {
        laserTimer -= Time.deltaTime;
        if (laserTimer < 0) {
            Destroy(gameObject);
        }
    }

    public void Launch (float angle) {
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
            if (laserTimer < activeLaserTime) {
                e.Dommage(dommage);
            }
        }
    }
}