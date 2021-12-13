using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private PlayerController player;
    private Door doorExit;

    public int playerScore;
    public bool gameOver;
    
    public List<Enemy> enemies = new List<Enemy>();
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        player = FindObjectOfType<PlayerController>();
        doorExit = FindObjectOfType<Door>();
    }

    private void Update()
    {
        gameOver = player.isDead;
        UIManager.instance.GameOverUI(gameOver);
    }

    public void IsEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    //set portal open when all the enemies are dead
    public void EnemyDead(Enemy enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            doorExit.OpenDoor();
        }
    }

    public void AddScore()
    {
        playerScore += 100;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("start");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
