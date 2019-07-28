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

    public bool auto;
    float randomDirectionTimer;
    // float invincibleTimer;
    Vector2 movingDirection;
    Rigidbody2D rigidBody2D;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        //     invincibleTimer = 0;
        randomDirectionTimer = 0f;
        movingDirection = new Vector2(0f, 0f);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 move;
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
        //ac.Launch(angle + 100, 300);
        // ac.Launch(angle - 100, 300);
        ac.Launch(angle, 300);
        // Debug.Log("Arrow angle: " + angle);

        // animator.SetTrigger ("Launch");
    }

    public void Dommage(int value)
    {
        currentHealth -= value;
        if (currentHealth <= 0)
            Destroy(gameObject);
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
        GameObject arrowObject = Instantiate(arrowPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
        GameObject arrowObject2 = Instantiate(arrowPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
        GameObject arrowObject3 = Instantiate(arrowPrefab, rigidBody2D.position + new Vector2(0.2f, 0.1f), Quaternion.identity);
        ArrowController ac = arrowObject.GetComponent<ArrowController>();
        ArrowController ac2 = arrowObject2.GetComponent<ArrowController>();
        ArrowController ac3 = arrowObject3.GetComponent<ArrowController>();
        difference = -difference;
        float angle = Mathf.Atan2(-difference.x, difference.y) * Mathf.Rad2Deg;
        Debug.Log("Arrow angle: " + closestEnemy);
        ac.Launch(angle, 300);
        ac2.Launch(angle + 20, 300);
        ac3.Launch(angle - 20, 300);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (auto) {
            movingDirection = -movingDirection;
            randomDirectionTimer = 1f;
            ShootOnClosestEnemy();
        }
    }
}