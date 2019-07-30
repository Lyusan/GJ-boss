using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    public GameObject laserPrefab;
    public GameObject monsterPrefab;
    public GameObject handFallPrefab;
    public int health { get { return currentHealth; } }

    Animator animator;
    Rigidbody2D rigidBody2D;
    float startGameTimer;
    float speed = 5.0f;
    float laserBlockTime = 1f;
    float blockTimer = 0.8f;
    float invincibleTimer;
    float deathTimer;
    float laserTimer;
    float monsterTimer;
    float handFallTimer;
    int currentHealth;
    int maxHealth = 100;
    bool dead;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        blockTimer = 0;
        startGameTimer = 5f;
        animator = GetComponent<Animator>();
    }

    void Move(Vector2 mvect)
    {
        if (blockTimer < 0f)
        {
            Vector2 position = rigidBody2D.position;
            position = position + mvect * speed * Time.deltaTime;
            rigidBody2D.MovePosition(position);
        }
    }

    void Update()
    {
        invincibleTimer -= Time.deltaTime;
        deathTimer -= Time.deltaTime;
        startGameTimer -= Time.deltaTime;
        laserTimer -= Time.deltaTime;
        monsterTimer -= Time.deltaTime;
        handFallTimer -= Time.deltaTime;
        blockTimer -= Time.deltaTime;
        if (dead)
            UpdateDead();
        else
            UpdateAlive();
    }

    private void UpdateDead()
    {
        if (deathTimer <= 0)
        {
            HeroController[] heros = GameObject.FindObjectsOfType<HeroController>();
            if (heros.Length > 0)
                Destroy(heros[0].gameObject);
            Destroy(gameObject); ;
        }
    }

    private void UpdateAlive()
    {
        if (startGameTimer > 0f)
            return;
        if (invincibleTimer > 0)
            animator.SetBool("invicible", true);
        else
            animator.SetBool("invicible", false);
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 moveVect = new Vector2(horizontal, vertical);
        Move(moveVect);
        if (laserTimer <= 0f && Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootLaser();
            laserTimer = 1f;
        }
        if (monsterTimer <= 0f && vertical < 0)
        {
            Invoc();
            monsterTimer = 5f;
        }
        if (handFallTimer <= 0f && Input.GetKeyDown(KeyCode.Mouse1))
        {
            HandFall();
            handFallTimer = 1f;
        }
    }

    void ShootLaser()
    {
        GameObject laserObject = Instantiate(laserPrefab, rigidBody2D.position + new Vector2(0.08f, 0.1f), Quaternion.identity);
        LaserController lc = laserObject.GetComponent<LaserController>();
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference = -difference;
        float angle = Mathf.Atan2(-difference.x, difference.y) * Mathf.Rad2Deg;
        lc.Launch(angle);
        blockTimer = laserBlockTime;
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
            invincibleTimer = 0.2f;
            if (currentHealth <= 0)
            {
                deathTimer = 4f;
                dead = true;
                animator.SetBool("dead", true);
            }
        }
    }

}