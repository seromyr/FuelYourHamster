using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gameplay_Mechanic : MonoBehaviour
{
    public static UI_Gameplay_Mechanic main;

    private Canvas canvas;

    private GameObject preRunNotice, endRunNotice, speedUpNotice;

    public Image[] questImageSlots;
    private Color questColorDefault, questColorNotCollected, questColorCollected;
    private int questImageIndex;

    private void Awake()
    {
        // Make UI_Gameplay_Mechanic become a Singleton
        Singletonizer();

        TryGetComponent(out canvas);

        preRunNotice = transform.Find("CountDown").gameObject;
        endRunNotice = transform.Find("EndRun").gameObject;
        speedUpNotice = transform.Find("SpeedUp").gameObject;

        questColorDefault = Color.black;
        questColorDefault.a = 0;

        ColorUtility.TryParseHtmlString("#696969", out questColorNotCollected);
        questColorNotCollected.a = 105 / 255f;

        questColorCollected = Color.white;
        questColorCollected.a = 1;

        questImageSlots = GameObject.Find("QuestSlots").GetComponentsInChildren<Image>();

        foreach (Image image in questImageSlots)
        {
            image.sprite = null;
            image.color = questColorDefault;
        }
    }

    private void Start()
    {
        preRunNotice.SetActive(false);
        endRunNotice.SetActive(false);
        speedUpNotice.SetActive(false);
    }
    private void Singletonizer()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("Gameplay UI created created");
            main = this;
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetCanvasActive(bool value)
    {
        canvas.enabled = value;
    }

    public void StartCountDown()
    {
        preRunNotice.SetActive(true);
    }

    public void ShowEndRunNotice()
    {
        endRunNotice.SetActive(true);
    }

    public void ShowSpeedUpNotice()
    {
        speedUpNotice.SetActive(true);
    }

    public void DisplayActiveQuest()
    {
        for (int i = 0; i < QuestController.main.CurrentQuestSprite.Count; i++)
        {
            questImageSlots[i].sprite = QuestController.main.CurrentQuestSprite[i];
            questImageSlots[i].color = questColorNotCollected;
        }

        for (int j = QuestController.main.CurrentQuestSprite.Count; j < questImageSlots.Length; j++)
        {
            questImageSlots[j].color = questColorDefault;
        }

        questImageIndex = 0;
    }

    public void HighlightQuestImageOnHUD()
    {
        questImageSlots[questImageIndex].color = questColorCollected;
        questImageIndex++;
        Debug.Log("item: " + gameObject.name);
    }
}
