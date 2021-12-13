using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject healthBar;
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;
    public Text playerScore;
    public GameObject startBackground;
    public GameObject instructionBackground;
    
    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void UpdateHealth(float currentHealth)
    {
        switch (currentHealth)
        {
            case 3:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(true);
                healthBar.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 2:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(true);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 1:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(false);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 0:
                healthBar.transform.GetChild(0).gameObject.SetActive(false);
                healthBar.transform.GetChild(1).gameObject.SetActive(false);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
        }
    }

    public void UpdateScore()
    {
        playerScore.text = GameManager.instance.playerScore.ToString();
    }

    public void GameOverUI(bool playerDead)
    {
        gameOverPanel.SetActive(playerDead);
    }

    public void ShowInstruction()
    {
        startBackground.SetActive(false);
        instructionBackground.SetActive(true);
    }

    public void ShowMenu()
    {
        startBackground.SetActive(true);
        instructionBackground.SetActive(false);
    }

    public void GoToGameScene()
    {
        SceneManager.LoadScene("game", LoadSceneMode.Single);
    }
}
