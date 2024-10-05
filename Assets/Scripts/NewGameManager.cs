using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameManager : MonoBehaviour
{
    private bool _isGameOver = false;
    public bool isCoOpMode = false;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
            isCoOpMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            if (isCoOpMode)
                SceneManager.LoadScene(2);
            else
                SceneManager.LoadScene(1);
        }
    }

    public void SetGameOver()
    {
        _isGameOver = true;
    }
}
