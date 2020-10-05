using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    private int currentScene;

    private Canvas mainMenu, upgradeMenu;

    private void Awake()
    {
        if (gameManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }

        currentScene = SceneManager.GetActiveScene().buildIndex;

        mainMenu = GameObject.Find("Main Menu").GetComponent<Canvas>();
        upgradeMenu = GameObject.Find("Upgrade Menu").GetComponent<Canvas>();
    }

    void Start()
    {
        if (currentScene == 0)
        {
            SceneManager.LoadScene(1);
            mainMenu.enabled = true;
            upgradeMenu.enabled = false;
        }
        
    }
    private void OnLevelWasLoaded(int level)
    {
        switch (level)
        {
            case 0:
                // This will never happen
                break;
            case 1:
                Debug.Log("Main Menu loaded");

                break;
            case 2:
                Debug.Log("Gameplay loaded");
                mainMenu.enabled = false;
                upgradeMenu.enabled = true;
                break;
        }
    }
}

[Serializable]
class PlayerData
{
    // Placeholder
}
