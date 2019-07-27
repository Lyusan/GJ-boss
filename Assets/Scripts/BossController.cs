using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float speed = 5.0f;
    public int maxHealth = 30;
    public int health { get { return currentHealth; } }
    public float laserBlockTime = 0.8f;
    private float blockTimer = 0.8f;
    public GameObject laserPrefab;
    public GameObject monsterPrefab;
    int currentHealth;
    // float invincibleTimer;

    Rigidbody2D rigidBody2D;
    Animator animator;
    Vector2 lookDirection = new Vector2 (1, 0);

    void Start () {
        rigidBody2D = GetComponent<Rigidbody2D> ();
        currentHealth = maxHealth;
        blockTimer = 0;
        // animator = GetComponent<Animator> ();
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

        // animator.SetFloat ("x", lookDirection.x);
        // animator.SetFloat ("y", lookDirection.y);
        if (blockTimer == 0f) {
            Vector2 position = rigidBody2D.position;
            position = position + move * speed * Time.deltaTime;
            rigidBody2D.MovePosition (position);
        }

        if (blockTimer > 0f) {
            blockTimer -= Time.deltaTime;
            blockTimer = blockTimer >= 0f ? blockTimer : 0f;
        }
        if (Input.GetKeyDown (KeyCode.Mouse0)) {
            Launch();
        }
        if (Input.GetKeyDown (KeyCode.Mouse1)) {
            Invoc();
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
        GameObject laserObject = Instantiate (laserPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
        LaserController lc = laserObject.GetComponent<LaserController> ();
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        lc.Launch(-difference);
        blockTimer = laserBlockTime;

        // animator.SetTrigger ("Launch");
    }

    void Invoc() {
        GameObject invoc = Instantiate (monsterPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
    }

}