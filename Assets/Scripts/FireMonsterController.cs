using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class FireMonsterController : MonoBehaviour
{

    public AIPath aIPath;
    public float speed = 4.0f;
    public int maxHealth = 4;
    public int health { get { return currentHealth; } }
    public float timeInvicible = 0.5f;
    // public GameObject projectilePrefab;
    int currentHealth;
    float invincibleTimer;
    float timeDead;

    Rigidbody2D rigidBody2D;
    Animator animator;
    Vector2 lookDirection = new Vector2 (1, 0);

    void Start () {
        rigidBody2D = GetComponent<Rigidbody2D> ();
        aIPath = GetComponent<AIPath> ();
        currentHealth = maxHealth;
        invincibleTimer = 0f;
        timeDead = 2f;
        animator = GetComponent<Animator> ();
        aIPath.target = HeroController.instance.transform;
    }

    void Awake() {
        // aIPath.destination = HeroController.instance.transform.position;
    }

    void Update () {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");
        Vector2 move = new Vector2 (horizontal, vertical);

        // aIPath.destination = HeroController.instance.transform.position;
        if (!Mathf.Approximately (move.x, 0.0f) || !Mathf.Approximately (move.y, 0.0f)) {
            lookDirection.Set (move.x, move.y);
            lookDirection.Normalize ();
        }
        Debug.Log(currentHealth);

        animator.SetFloat ("x", lookDirection.x);
        animator.SetFloat ("y", lookDirection.y);
        if (currentHealth <= 0) {
            animator.SetBool ("dead", true);
            timeDead -= Time.deltaTime;
            if (timeDead <= 0)
                Destroy (gameObject);
        }
        if (invincibleTimer > 0f)
            animator.SetBool("invicible", true);
        // Vector2 position = rigidBody2D.position;

        // position = position + move * speed * Time.deltaTime;

        // rigidBody2D.MovePosition (position);
        if (invincibleTimer > 0f) {
            float newInvincibleTimer = invincibleTimer - Time.deltaTime;
            invincibleTimer = newInvincibleTimer >= 0f ? newInvincibleTimer : 0f;
            if (invincibleTimer == 0f) {
                animator.SetBool("invicible", false);
            }
        }
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
    //     GameObject arrowObject = Instantiate (arrowPrefab, GetComponent<Rigidbody2D>().position + Vector2.up * 0.5f, Quaternion.identity);

    //     Projectile arrow = arrowObject.GetComponent<Projectile> ();
    //     arrow.Launch (lookDirection, 300);

        //animator.SetTrigger ("Launch");
    // }

    public void Dommage(int value) {
        if (invincibleTimer == 0f) {
            currentHealth -= value;
            Debug.Log(currentHealth);
            if (currentHealth <= 0) {
                aIPath.isStopped = true;
            }
            invincibleTimer = timeInvicible;
        }
    }
}
