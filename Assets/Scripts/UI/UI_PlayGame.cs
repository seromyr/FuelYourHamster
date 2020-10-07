﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class UI_PlayGame : MonoBehaviour
{
    private GameManager gameManager;
    private Button button;

    private void Start()
    {
        gameManager = GameObject.Find(PrimeObj.GAMEMANAGER).GetComponent<GameManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(NewGame);
    }
    public void NewGame()
    {
        gameManager.NewGame();
    }
}
