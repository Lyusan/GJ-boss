using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour
{
    float staticCreditDisplayTimer;
    static float staticCreditDisplayTime = 4f;
    static float minimalPosY = -3.6f;
    Rigidbody2D rigidBody2D;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        staticCreditDisplayTimer = staticCreditDisplayTime;
    }

    void Update()
    {
        BossController[] bossControllers = GameObject.FindObjectsOfType<BossController>();
        if (bossControllers.Length > 0)
            return;
        if (rigidBody2D.position.y > minimalPosY)
        {
            Vector2 position = rigidBody2D.position;
            position = position + new Vector2(0, -1f) * 1f * Time.deltaTime;
            rigidBody2D.MovePosition(position);
        }
        if (rigidBody2D.position.y <= minimalPosY)
        {
            staticCreditDisplayTimer = staticCreditDisplayTime;
        }
        staticCreditDisplayTimer -= Time.deltaTime;
        if (staticCreditDisplayTimer <= 0)
        {
            SceneManager.LoadScene("StartMenuScene");
        }
    }
}
