using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationPool : MonoBehaviour
{
    [SerializeField, Header("Harmful Mutation")]
    private List<Sprite> harmfulSprites;

    [SerializeField, Header("Beneficial Mutation")]
    private List<Sprite> beneficialSprites;

    [SerializeField, Header("Extreme Mutation")]
    private List<Sprite> extremeSprites;

    public List<Sprite> Harmfuls { get { return harmfulSprites; } }
    public List<Sprite> Beneficials { get { return beneficialSprites; } }
    public List<Sprite> Extremes { get { return extremeSprites; } }

    void Awake()
    {
        // Construct Harmful Collectibles
        harmfulSprites = new List<Sprite>();
        harmfulSprites.AddRange(Resources.LoadAll<Sprite>("Collectable Sprites/Harmful"));
        // Construct Benefical Collectibles
        beneficialSprites = new List<Sprite>();
        beneficialSprites.AddRange(Resources.LoadAll<Sprite>("Collectable Sprites/Beneficial"));
        // Construct Extreme Collectibles
        extremeSprites = new List<Sprite>();
        extremeSprites.AddRange(Resources.LoadAll<Sprite>("Collectable Sprites/Extreme"));
    }
}
