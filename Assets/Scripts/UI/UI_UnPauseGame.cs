﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class UI_UnPauseGame : MonoBehaviour
{
    private GameManager gameManager;
    private Button button;

    private void Start()
    {
        gameManager = GameObject.Find(PrimeObj.GAMEMANAGER).GetComponent<GameManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(ContinueGame);
    }
    public void ContinueGame()
    {
        gameManager.UnPauseGame();
    }
}
