using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class UI_UnPauseGame : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ContinueGame);
    }
    public void ContinueGame()
    {
        GameManager.main.UnPauseGame();
    }
}
