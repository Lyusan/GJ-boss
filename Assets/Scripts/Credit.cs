using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour
{
    float timer = 0f;
    Rigidbody2D rigidBody2D;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        BossController[] bossControllers = GameObject.FindObjectsOfType<BossController>();
        if (bossControllers.Length == 0 && rigidBody2D.position.y > -3.6f) {
            Vector2 position = rigidBody2D.position;
            position = position + new Vector2(0, -1f) * 1f * Time.deltaTime;
            rigidBody2D.MovePosition(position);
            return;
        }
        if (rigidBody2D.position.y <= -3.6f && timer <= 0f) {
            timer = 4f;
        }
        if (timer <= 1 && timer >= 0) {
            SceneManager.LoadScene("Start");
        }
    }
}
