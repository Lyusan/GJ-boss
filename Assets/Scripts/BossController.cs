using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float speed = 5.0f;
    public int maxHealth = 10;
    public int health { get { return currentHealth; } }
    // public float timeInvicible = 2.0f;
    // public GameObject projectilePrefab;
    int currentHealth;
    // float invincibleTimer;

    Rigidbody2D rigidBody2D;
    Animator animator;
    Vector2 lookDirection = new Vector2 (1, 0);

    void Start () {
        rigidBody2D = GetComponent<Rigidbody2D> ();
    //     currentHealth = maxHealth;
    //     invincibleTimer = 0;
        animator = GetComponent<Animator> ();
    }

    void Update () {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");

        Vector2 move = new Vector2 (horizontal, vertical);

        if (!Mathf.Approximately (move.x, 0.0f) || !Mathf.Approximately (move.y, 0.0f)) {
            lookDirection.Set (move.x, move.y);
            lookDirection.Normalize ();
        }

        animator.SetFloat ("x", lookDirection.x);
        animator.SetFloat ("y", lookDirection.y);

        Vector2 position = rigidBody2D.position;

        position = position + move * speed * Time.deltaTime;

        rigidBody2D.MovePosition (position);
        // if (invincibleTimer > 0f) {
        //     float newInvincibleTimer = invincibleTimer - Time.deltaTime;
        //     invincibleTimer = newInvincibleTimer >= 0f ? newInvincibleTimer : 0f;
        // }
        // if (Input.GetKeyDown (KeyCode.C)) {
        //     Launch ();
        // }
    }

    // public void ChangeHealth (int amount) {
    //     if (amount < 0) {
    //         if (invincibleTimer != 0f)
    //             return;
    //         else
    //             invincibleTimer = timeInvicible;
    //     }
    //     currentHealth = Mathf.Clamp (currentHealth + amount, 0, maxHealth);
    //     Debug.Log (currentHealth + "/" + maxHealth);
    //     UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    // }

    // void Launch () {
    //     GameObject projectileObject = Instantiate (projectilePrefab, GetComponent<Rigidbody2D>().position + Vector2.up * 0.5f, Quaternion.identity);

    //     Projectile projectile = projectileObject.GetComponent<Projectile> ();
    //     projectile.Launch (lookDirection, 300);

    //     animator.SetTrigger ("Launch");
    // }

}
