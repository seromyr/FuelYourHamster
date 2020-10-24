using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationPool : MonoBehaviour
{
    [SerializeField, Header("Harmful Mutation")]
    private List<Sprite> harmfulSprites;

    [SerializeField, Header("Beneficial Mutation")]
    private List<Sprite> beneficialSprites;

    public List<Sprite> Harmfuls { get { return harmfulSprites; } }
    public List<Sprite> Beneficials { get { return beneficialSprites; } }

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
