using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float speed = 5.0f;
    public int maxHealth = 25;
    public int health { get { return currentHealth; } }
    public float laserBlockTime = 1f;
    private float blockTimer = 0.8f;
    public GameObject laserPrefab;
    public GameObject monsterPrefab;
    public GameObject handFallPrefab;
    float invincibleTimer;
    float deathTimer;
    private float laserTimer;
    private float monsterTimer;
    private float handFallTimer;
    int currentHealth;
    // float invincibleTimer;
    Animator animator;

    bool dead;
    Rigidbody2D rigidBody2D;
    Vector2 lookDirection = new Vector2(1, 0);

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        blockTimer = 0;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        invincibleTimer -= Time.deltaTime;
        deathTimer -= Time.deltaTime;
        if (deathTimer > 0)
        {
            dead = true;
            animator.SetBool("death", true);
        }
        else
        {
            if (invincibleTimer > 0)
                animator.SetBool("invicible", true);
            else
                animator.SetBool("invicible", false);
        }
        if (deathTimer <= 0 && dead) {
            HeroController[] heros = GameObject.FindObjectsOfType<HeroController>();
            if (heros.Length > 0) {
                Destroy (heros[0].gameObject);
            }
            Destroy(gameObject);
        }
        if (!dead)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector2 move = new Vector2(horizontal, vertical);

            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }
            // Debug.Log(lookDirection);

            // animator.SetFloat ("x", lookDirection.x);
            // animator.SetFloat ("y", lookDirection.y);
            if (blockTimer == 0f)
            {
                Vector2 position = rigidBody2D.position;
                position = position + move * speed * Time.deltaTime;
                rigidBody2D.MovePosition(position);
            }

            if (blockTimer > 0f)
            {
                blockTimer -= Time.deltaTime;
                blockTimer = blockTimer >= 0f ? blockTimer : 0f;
            }
            if (laserTimer <= 0f && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Launch();
                laserTimer = 1f;
            }
            if (monsterTimer <= 0f && Input.GetKeyDown(KeyCode.Mouse1))
            {
                Invoc();
                monsterTimer = 2f;
            }
            if (handFallTimer <= 0f && vertical < 0)
            {
                HandFall();
                handFallTimer = 1f;
            }
            laserTimer -= Time.deltaTime;
            monsterTimer -= Time.deltaTime;
            handFallTimer -= Time.deltaTime;
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

    void Launch()
    {
        GameObject laserObject = Instantiate(laserPrefab, rigidBody2D.position + new Vector2(0.08f, 0.1f), Quaternion.identity);
        LaserController lc = laserObject.GetComponent<LaserController>();
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference = -difference;
        float angle = Mathf.Atan2(-difference.x, difference.y) * Mathf.Rad2Deg;
        lc.Launch(angle);
        // lc.Launch(angle + 100);
        // lc.Launch(angle - 100);
        blockTimer = laserBlockTime;

        // animator.SetTrigger ("Launch");
    }

    void Invoc()
    {
        GameObject invoc = Instantiate(monsterPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
    }

    void HandFall()
    {
        GameObject hand = Instantiate(handFallPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
    }

    public void Dommage(int value)
    {
        if (invincibleTimer <= 0)
        {
            currentHealth -= value;
            invincibleTimer = 0.5f;
            if (currentHealth <= 0)
            {
                deathTimer = 4f;
                animator.SetBool("dead", true);
            }
        }
    }

}