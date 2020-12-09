using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class UI_UpgradeLevelMonitoring : MonoBehaviour
{
    private Image image, frameImage;
    private int statID;
    private Color enableColor, disableColor;

    void Start()
    {
        TryGetComponent(out image);
        GetUpgradeStatID();

        transform.parent.TryGetComponent(out frameImage);

        ColorUtility.TryParseHtmlString("#926143FF", out enableColor);
        ColorUtility.TryParseHtmlString("#9AB9D1FF", out disableColor);
    }

    void Update()
    {
        image.fillAmount = (float)UpgradeData.main.Stats[statID].level / UpgradeData.main.Stats[statID].maxLevel;

        if (UpgradeData.main.CheckUpgradeIsAlreadyMax(statID))
        {
            //image.color = disableColor;
            frameImage.color = disableColor;
        }
        else if (!GameManager.main.AllowUpgradePurchasing(statID))
        {
            //image.color = disableColor;
            frameImage.color = disableColor;
        }
        else
        {
            image.color = enableColor;
            frameImage.color = Color.white;
        }
    }

    private void GetUpgradeStatID()
    {
        switch (transform.parent.name)
        {
            case StatID._01:
                statID = 0;
                break;
            case StatID._02:
                statID = 1;
                break;
            case StatID._03:
                statID = 2;
                break;
            case StatID._04:
                statID = 3;
                break;
            case StatID._05:
                statID = 4;
                break;
        } 
    }
}
