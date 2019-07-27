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

    public void Launch (Vector2 direction, float force) {
        rigidbody2d.AddForce(direction * force);

        float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
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
