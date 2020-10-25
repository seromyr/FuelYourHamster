using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMechanic : MonoBehaviour
{
    private Player player;
    private GameManager gameManager;

    [SerializeField]
    private List<Image> healthGraphics;

    
    // health UI
    private Sprite heart_OK, heart_notOK;

    void Start()
    {
        healthGraphics.AddRange( GetComponentsInChildren<Image>());

        gameManager = GameObject.Find(PrimeObj.GAMEMANAGER).GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<Player>();

        for (int i = player.MaxHealth; i < healthGraphics.Count; i++)
        {
            healthGraphics[i].enabled = false;
        }

        heart_OK = Resources.Load<Sprite>("UIComponents/Health/Heart");
        heart_notOK = Resources.Load<Sprite>("UIComponents/Health/Heart_Black");
    }

    private void Update()
    {
        HealthUpdate();
    }

    private void HealthUpdate()
    {
        // Update max health
        for (int i = 0; i < player.MaxHealth; i++)
        {
            healthGraphics[i].enabled = true;
        }

        // Update current health
        for (int i = 0; i < player.Health; i++)
        {
            healthGraphics[i].sprite = heart_OK;
        }

        for (int i = player.Health; i < player.MaxHealth; i++)
        {
            healthGraphics[i].sprite = heart_notOK;
        }
    }
}
