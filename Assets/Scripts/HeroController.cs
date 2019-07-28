using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{

    public static HeroController instance { get; private set; }
    public float speed = 5.0f;
    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    // public float timeInvicible = 2.0f;
    public GameObject arrowPrefab;
    int currentHealth;
    // float invincibleTimer;

    Rigidbody2D rigidBody2D;
    Animator animator;
    Vector2 lookDirection = new Vector2 (1, 0);

    void Start () {
        rigidBody2D = GetComponent<Rigidbody2D> ();
        currentHealth = maxHealth;
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
        Debug.Log(lookDirection);

        animator.SetFloat ("x", lookDirection.x);
        animator.SetFloat ("y", lookDirection.y);
        if (move.x != 0 || move.y != 0)
            animator.SetBool ("moving", true);
        else
            animator.SetBool ("moving", false);

        Vector2 position = rigidBody2D.position;

        position = position + move * speed * Time.deltaTime;

        rigidBody2D.MovePosition (position);


        // if (invincibleTimer > 0f) {
        //     float newInvincibleTimer = invincibleTimer - Time.deltaTime;
        //     invincibleTimer = newInvincibleTimer >= 0f ? newInvincibleTimer : 0f;
        // }
        if (Input.GetKeyDown (KeyCode.Mouse0)) {
            Launch();
        }
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

    void Launch () {
        GameObject arrowObject = Instantiate (arrowPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
        ArrowController ac = arrowObject.GetComponent<ArrowController> ();
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        ac.Launch (difference, 300);

        // animator.SetTrigger ("Launch");
    }

}
