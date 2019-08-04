using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : LivingEntity
{
    private const float BlockedTime = 5f;
    public float speed = 3.0f;
    public int maxHealth = 4;
    public GameObject arrowPrefab;
    float arrowCount = 1f;
    public bool auto;
    Vector2 movingDirection;
    Rigidbody2D rigidBody2D;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    float randomDirectionTimer;
    float invincibleTimer;
    float deathTimer;
    bool dead;
    float popTimer;
    static float appearAnimationTime = 1.5f;
    int arrowForce;
    static int arrowBaseForce = 150;
    float startGameTimer;
    float arrowDommage;
    static float arrowBaseDommage = 2f;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        arrowForce = 150;
        popTimer = 1.5f;
        arrowDommage = arrowBaseDommage;
        startGameTimer = BlockedTime;
        dead = false;
        movingDirection = new Vector2(0f, 1f);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 move;
        invincibleTimer -= Time.deltaTime;
        deathTimer -= Time.deltaTime;
        popTimer -= Time.deltaTime;
        startGameTimer -= Time.deltaTime;
        BossController[] boss = GameObject.FindObjectsOfType<BossController>();
        if (popTimer > 0f && auto == true)
            GetComponent<Renderer>().sortingOrder = -10;
        else
            GetComponent<Renderer>().sortingOrder = 1;
        if (startGameTimer > 0f && auto)
            return;
        if (boss.Length > 0 && boss[0].health <= 0)
        {
            animator.SetBool("moving", false);
            return;
        }
        if (!dead)
        {
            if (invincibleTimer > 0)
                animator.SetBool("invicible", true);
            else
                animator.SetBool("invicible", false);
        }
        if (deathTimer <= 0 && dead)
        {
            dead = false;
            auto = true;
            deathTimer = 0;
            Color[] colors = new Color[7] { Color.green, Color.blue, Color.cyan, Color.black, Color.magenta, Color.yellow, Color.white };
            GetComponent<Renderer>().material.color = colors[(int)Random.Range(0, 7)];
            maxHealth += 1;
            currentHealth = maxHealth;
            animator.SetBool("death", false);
            animator.SetBool("invicible", false);
            Vector2 position = rigidBody2D.position;
            Debug.Log(position);
            position.x = 0;
            position.y = 0;
            rigidBody2D.MovePosition(position);
            if (speed < 4f)
                speed += 0.1f;
            arrowCount += 0.6f;
            if (arrowForce < 300)
                arrowForce += 20;
            return;
        }
        if (!dead)
        {
            if (auto)
            {
                if (randomDirectionTimer <= 0)
                {
                    randomDirectionTimer = Random.Range(0.2f, 1f);
                    if (Random.Range(0f, 1f) > 0.8f)
                    {
                        movingDirection = new Vector2(0f, 0f);
                    }
                    else
                    {
                        movingDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    }
                    LivingEntity target = FindTarget();
                    Launch(GetAngleBetweenToPosition(transform.position, target.transform.position));
                }
                else
                {
                    randomDirectionTimer -= Time.deltaTime;
                }
                move = movingDirection;
            }
            else
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                move = new Vector2(horizontal, vertical);
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Launch(GetCurrentPointerAngle());
                }
            }
            SetLookDirection(move);
            Move(move);
        }
    }
    void SetLookDirection(Vector2 move)
    {
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        animator.SetFloat("x", lookDirection.x);
        animator.SetFloat("y", lookDirection.y);
    }

    void Move(Vector2 move)
    {
        move.x *= Helper.PerspectiveXRatio;
        if (move.x != 0 || move.y != 0)
            animator.SetBool("moving", true);
        else
            animator.SetBool("moving", false);
        Vector2 position = rigidBody2D.position;
        position = position + move * speed * Time.deltaTime;
        rigidBody2D.MovePosition(position);
    }

    float GetCurrentPointerAngle()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference = -difference;
        return Mathf.Atan2(-difference.x, difference.y) * Mathf.Rad2Deg;
    }

    float GetAngleBetweenToPosition(Vector3 source, Vector3 target)
    {
        Vector3 difference = target - source;
        difference = -difference;
        return Mathf.Atan2(-difference.x, difference.y) * Mathf.Rad2Deg;
    }

    void Launch(float angle)
    {
        void LaunchOne(float dangle)
        {
            GameObject arrowObject = Instantiate(arrowPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
            ArrowController ac = arrowObject.GetComponent<ArrowController>();
            ac.Launch(dangle, arrowForce, arrowDommage);
        }
        if (arrowCount > 2.9f)
        {
            LaunchOne(angle + 20);
            LaunchOne(angle - 20);
        }
        if (arrowCount > 4.9f)
        {
            LaunchOne(angle + 10);
            LaunchOne(angle - 10);
        }
        if (arrowCount > 5.9f)
        {
            LaunchOne(angle + 5);
            LaunchOne(angle - 5);
        }
        if (arrowCount > 6.9f)
        {
            LaunchOne(angle + 15);
            LaunchOne(angle - 15);
        }
        LaunchOne(angle);

    }

    LivingEntity FindTarget()
    {
        float closestEnemyDistance = Mathf.Infinity;
        LivingEntity target = null;
        FireMonsterController[] allEnemies = GameObject.FindObjectsOfType<FireMonsterController>();
        foreach (FireMonsterController currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - gameObject.transform.position).sqrMagnitude;
            if (currentEnemy.health > 0 && currentEnemy.invocationTimer <= 0)
            {
                if (distanceToEnemy < 5000f && distanceToEnemy < closestEnemyDistance)
                {
                    closestEnemyDistance = distanceToEnemy;
                    target = currentEnemy;
                }
            }
        }

        Vector3 difference;
        if (target == null)
        {
            BossController[] bossControllers = GameObject.FindObjectsOfType<BossController>();
            target = bossControllers[0];
        }
        return target;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (auto)
            randomDirectionTimer = 0f;
        if (col.gameObject.tag == "BadGuy")
            Dommage(1);
    }

    public void Dommage(int value)
    {
        if (invincibleTimer <= 0)
        {
            invincibleTimer = 0.5f;
            if (!auto)
                return;
            currentHealth -= value;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        deathTimer = 2f;
        dead = true;
        animator.SetBool("death", true);
        auto = false;
    }
}
