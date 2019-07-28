using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    void Start () {

    }
    void Awake () {
        rigidbody2d = GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    void Update () {
        if (transform.position.magnitude > 1000.0f) {
            Destroy(gameObject);
        }
    }

    public void Launch (float angle, float force) {
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.down;
        rigidbody2d.AddForce(dir * force);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    void OnCollisionEnter2D (Collision2D other) {
        FireMonsterController e = other.collider.GetComponent<FireMonsterController> ();
        if (e != null) {
            e.Dommage(2);
        }
        BossController b = other.collider.GetComponent<BossController> ();
        if (b != null) {
            b.Dommage(2);
        }
        //we also add a debug log to know what the projectile touch
        // Debug.Log ("Projectile Collision with " + other.gameObject);
        Destroy (gameObject);
    }
      
}
