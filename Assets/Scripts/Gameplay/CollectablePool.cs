using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePool : MonoBehaviour
{
    [SerializeField, Header("Harmful Collectables")]
    private List<Sprite> harmfulSprites;

    [SerializeField, Header("Beneficial Collectables")]
    private List<Sprite> beneficialSprites;

    public List<Sprite> HarmfulSpritePool { get { return harmfulSprites; } }
    public List<Sprite> BeneficialSpritePool { get { return beneficialSprites; } }

    void Awake()
    {
        // Construct Harmful Collectables
        harmfulSprites = new List<Sprite>();
        harmfulSprites.AddRange(Resources.LoadAll<Sprite>("Collectable Sprites/Harmful"));
        // Construct Benefical Collectables
        beneficialSprites = new List<Sprite>();
        beneficialSprites.AddRange(Resources.LoadAll<Sprite>("Collectable Sprites/Beneficial"));
    }
}
