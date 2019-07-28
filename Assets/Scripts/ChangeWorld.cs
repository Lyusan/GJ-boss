using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeWorld : MonoBehaviour
{
    // Start is called before the first frame update
    public string level;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(level);
         
        }
    }
}