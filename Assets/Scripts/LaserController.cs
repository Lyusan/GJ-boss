using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float laserTime;
    void Start () {

    }
    void Awake () {
        laserTime = 0.8f;
    }

    // Update is called once per frame
    void Update () {
        laserTime -= Time.deltaTime;
        if (laserTime < 0) {
            Destroy(gameObject);
        }
    }

    public void Launch (Vector2 direction) {
        float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        Debug.Log(angle);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnCollisionEnter2D (Collision2D other) {
        FireMonsterController e = other.collider.GetComponent<FireMonsterController> ();
        if (e != null) {
            e.Dommage(2);
        }
        //we also add a debug log to know what the projectile touch
        Debug.Log ("Projectile Collision with " + other.gameObject);
        Destroy (gameObject);
    }
      
}
