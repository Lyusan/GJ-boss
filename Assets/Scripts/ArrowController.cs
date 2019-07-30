using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public static float MaxAcceptableMagnitude = 1000f;
    private float dommage;


    void Awake () {
        rigidbody2d = GetComponent<Rigidbody2D> ();
    }

    void Update () {
        if (transform.position.magnitude > MaxAcceptableMagnitude) {
            Destroy(gameObject);
        }
    }

    public void Launch (float angle, float force, float dommage) {
        this.dommage = dommage;
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.down;
        rigidbody2d.AddForce(dir * force);
        transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
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
        Destroy (gameObject);
    }
      
}
