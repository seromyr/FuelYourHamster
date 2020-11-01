using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMechanic : MonoBehaviour
{
    [SerializeField]
    private List<Image> healthGraphics;
    
    // health UI
    private Sprite heart_OK, heart_notOK;

    void Start()
    {
        healthGraphics.AddRange( GetComponentsInChildren<Image>());

        for (int i = Player.main.MaxHealth; i < healthGraphics.Count; i++)
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
        for (int i = 0; i < Player.main.MaxHealth; i++)
        {
            healthGraphics[i].enabled = true;
        }

        // Update current health
        for (int i = 0; i < Player.main.CurrentHealth; i++)
        {
            healthGraphics[i].sprite = heart_OK;
        }

        for (int i = Player.main.CurrentHealth; i < Player.main.MaxHealth; i++)
        {
            healthGraphics[i].sprite = heart_notOK;
        }
    }
}
