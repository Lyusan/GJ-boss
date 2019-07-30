using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{

    public static HeroController instance { get; private set; }
    public float speed = 3.0f;
    public int maxHealth = 4;
    public int health { get { return currentHealth; } }
    // public float timeInvicible = 2.0f;
    public GameObject arrowPrefab;
    int currentHealth;
    float arrowCount = 1f;
    public bool auto;
    float randomDirectionTimer;
    // float invincibleTimer;
    Vector2 movingDirection;
    Rigidbody2D rigidBody2D;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    float invincibleTimer;
    public float deathTimer;
    public bool dead;
    float popTimer;
    int arrowForce;
    float startGameTimer;
    float arrowDommage;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        invincibleTimer = 0;
        arrowForce = 150;
        deathTimer = 0;
        popTimer = 1.5f;
        arrowDommage = 2;
        startGameTimer = 5f;
        dead = false;
        randomDirectionTimer = 0f;
        movingDirection = new Vector2(0f, 0f);
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
        if (popTimer > 0f && auto == true) {
            GetComponent<Renderer>().sortingOrder = -10;
        } else {
            GetComponent<Renderer>().sortingOrder = 1;
        }
        if (startGameTimer > 0f && auto) {
            return;
        }
        if (boss.Length > 0 && boss[0].health <= 0) {
            animator.SetBool("moving", false);
            return;
        }
        if (deathTimer > 0)
        {
            dead = true;
            animator.SetBool("death", true);
            auto = false;
        } else {
            if (invincibleTimer > 0)
                animator.SetBool("invicible", true);
            else
                animator.SetBool("invicible", false);
        }
        if (deathTimer <= 0 && dead) {
            //Destroy(gameObject);
            dead = false;
            auto = true;
            deathTimer = 0;
            Color[] colors = new Color[7] {Color.green, Color.blue, Color.cyan, Color.black, Color.magenta, Color.yellow, Color.white};
            GetComponent<Renderer>().material.color = colors[(int)Random.Range(0, 7)];
            maxHealth += 1;
            currentHealth = maxHealth;
            animator.SetBool("death", false);
            animator.SetBool("invicible", false);
            if (speed < 4f)
                speed += 0.1f;
            arrowCount += 0.6f;
            if (arrowForce < 300)
                arrowForce += 20;
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
                    ShootOnClosestEnemy();
                }
                else
                {
                    randomDirectionTimer -= Time.deltaTime;
                }

                move = movingDirection;

                // if (invincibleTimer > 0f) {
                //     float newInvincibleTimer = invincibleTimer - Time.deltaTime;
                //     invincibleTimer = newInvincibleTimer >= 0f ? newInvincibleTimer : 0f;
                // }

            }
            else
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                move = new Vector2(horizontal, vertical);
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Launch();
                }
            }
            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }
            // Debug.Log(lookDirection);

            animator.SetFloat("x", lookDirection.x);
            animator.SetFloat("y", lookDirection.y);
            if (move.x != 0 || move.y != 0)
                animator.SetBool("moving", true);
            else
                animator.SetBool("moving", false);

            Vector2 position = rigidBody2D.position;

            position = position + move * speed * Time.deltaTime;

            rigidBody2D.MovePosition(position);
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
        GameObject arrowObject = Instantiate(arrowPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
        ArrowController ac = arrowObject.GetComponent<ArrowController>();
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference = -difference;
        float angle = Mathf.Atan2(-difference.x, difference.y) * Mathf.Rad2Deg;
        //ac.Launch(angle + 100, arrowForce);
        // ac.Launch(angle - 100, arrowForce);
        ac.Launch(angle, arrowForce, arrowDommage);
        // Debug.Log("Arrow angle: " + angle);

        // animator.SetTrigger ("Launch");
    }



    void ShootOnClosestEnemy()
    {
        float closestEnemyDistance = Mathf.Infinity;
        FireMonsterController closestEnemy = null;
        FireMonsterController[] allEnemies = GameObject.FindObjectsOfType<FireMonsterController>();
        Debug.Log(allEnemies);
        foreach (FireMonsterController currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - gameObject.transform.position).sqrMagnitude;
            // Debug.Log("Ennemy: " + currentEnemy);
            // Debug.Log("Distance: " + distanceToEnemy);
            // Debug.Log("Old: " + closestEnemyDistance);
            if (currentEnemy.health > 0 && currentEnemy.invocationTimer <= 0)
            {
                if (distanceToEnemy < 5000f && distanceToEnemy < closestEnemyDistance)
                {
                    closestEnemyDistance = distanceToEnemy;
                    closestEnemy = currentEnemy;
                }
            }
            // Debug.Log("New: " + closestEnemyDistance);
            // Debug.Log("New enemy: " + closestEnemy);
        }
        Vector3 difference;
        if (closestEnemy == null)
        {
            BossController[] bossControllers = GameObject.FindObjectsOfType<BossController>();
            BossController bossController = bossControllers[0];
            difference = bossController.transform.position - transform.position;
        }
        else
        {
            difference = closestEnemy.transform.position - transform.position;
        }
        difference = -difference;
        float angle = Mathf.Atan2(-difference.x, difference.y) * Mathf.Rad2Deg;
        if (arrowCount > 2.9f) {
            GameObject arrowObject1 = Instantiate(arrowPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
            GameObject arrowObject2 = Instantiate(arrowPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
            ArrowController ac1 = arrowObject1.GetComponent<ArrowController>();
            ArrowController ac2 = arrowObject2.GetComponent<ArrowController>();
            ac1.Launch(angle + 20, arrowForce, arrowDommage);
            ac2.Launch(angle - 20, arrowForce, arrowDommage);
        }
        if (arrowCount > 4.9f) {
            GameObject arrowObject1 = Instantiate(arrowPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
            GameObject arrowObject2 = Instantiate(arrowPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
            ArrowController ac1 = arrowObject1.GetComponent<ArrowController>();
            ArrowController ac2 = arrowObject2.GetComponent<ArrowController>();
            ac1.Launch(angle + 10, arrowForce, arrowDommage);
            ac2.Launch(angle - 10, arrowForce, arrowDommage);
        }
        if (arrowCount > 5.9f) {
            GameObject arrowObject1 = Instantiate(arrowPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
            GameObject arrowObject2 = Instantiate(arrowPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
            ArrowController ac1 = arrowObject1.GetComponent<ArrowController>();
            ArrowController ac2 = arrowObject2.GetComponent<ArrowController>();
            ac1.Launch(angle + 5, arrowForce, arrowDommage);
            ac2.Launch(angle - 5, arrowForce, arrowDommage);
        }
        if (arrowCount > 6.9f) {
            GameObject arrowObject1 = Instantiate(arrowPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
            GameObject arrowObject2 = Instantiate(arrowPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
            ArrowController ac1 = arrowObject1.GetComponent<ArrowController>();
            ArrowController ac2 = arrowObject2.GetComponent<ArrowController>();
            ac1.Launch(angle + 15, arrowForce, arrowDommage);
            ac2.Launch(angle - 15, arrowForce, arrowDommage);
        }
        GameObject arrowObject = Instantiate(arrowPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
        ArrowController ac = arrowObject.GetComponent<ArrowController>();
        ac.Launch(angle, arrowForce, arrowDommage);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (auto)
        {
            movingDirection = -movingDirection;
            randomDirectionTimer = 1f;
            ShootOnClosestEnemy();
        }
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
            if (currentHealth <= 0) {
                deathTimer = 2f;
                animator.SetBool("death", true);
            }
        }
    }
}
