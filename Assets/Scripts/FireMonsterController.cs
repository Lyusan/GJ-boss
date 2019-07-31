using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FireMonsterController : MonoBehaviour
{

    public int health { get { return currentHealth; } }
    public float invocationTimer;
    static float invicibleTime = 0.5f;
    float timeDead = 2f;
    float speed = 4.0f;
    float invincibleTimer;
    int currentHealth;
    int maxHealth = 4;

    Rigidbody2D rigidBody2D;
    Animator animator;
    Vector2 lookDirection;

    void Start () {
        currentHealth = maxHealth;
        animator = GetComponent<Animator> ();
        rigidBody2D = GetComponent<Rigidbody2D> ();
        lookDirection = new Vector2 (0, -1);
        rigidBody2D.simulated = false;
        invocationTimer = 2f;
    }

    void Update () {
        invincibleTimer -= Time.deltaTime;
        animator.SetFloat ("x", lookDirection.x);
        animator.SetFloat ("y", lookDirection.y);
        if (invocationTimer > 0) {
            invocationTimer -= Time.deltaTime;
            animator.SetBool("invocation", true);
        } else {
            animator.SetBool("invocation", false);
            rigidBody2D.simulated = true;
        }
        if (currentHealth <= 0) {
            animator.SetBool ("dead", true);
            timeDead -= Time.deltaTime;
            rigidBody2D.simulated = false;
            if (timeDead <= 0)
                Destroy (gameObject);
        }
        if (invincibleTimer > 0f)
            animator.SetBool("invicible", true);
        else 
            animator.SetBool("invicible", false);
    }

    public void Dommage(int value) {
        if (invincibleTimer <= 0f) {
            currentHealth -= value;
            if (currentHealth > 0)
                invincibleTimer = invicibleTime;
        }
    }
}