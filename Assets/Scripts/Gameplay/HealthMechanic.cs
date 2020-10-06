using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMechanic : MonoBehaviour
{
    // player reference
    private Player player;
    
    // health UI
    private Image health;
    // all the health state sprites
    [SerializeField]
    private List<Sprite> healthGraphics;

    // Start is called before the first frame update
    void Start()
    {
        healthGraphics = new List<Sprite>();
        healthGraphics.AddRange(Resources.LoadAll<Sprite>("UIComponents/Health"));
        
        player = GameObject.Find("Player").GetComponent<Player>();
        health = GameObject.Find("Health").GetComponent<Image>();
        
        health.sprite = healthGraphics[1];
    }

    // Update is called once per frame
    void Update()
    {
        switch (player.Health)
        {
            case 0: health.sprite = healthGraphics[0]; break;
            case 1: health.sprite = healthGraphics[2]; break;
            case 2: health.sprite = healthGraphics[1]; break;
            default: health.sprite = healthGraphics[1]; break;
        }
    }

}
