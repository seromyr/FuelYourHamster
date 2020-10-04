using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePool : MonoBehaviour
{
    [SerializeField, Header("Collectable List")]
    private List<Sprite> sprites;

    public List<Sprite> SpritePool { get { return sprites; } }

    // Start is called before the first frame update
    void Awake()
    {
        sprites = new List<Sprite>();
        sprites.AddRange(Resources.LoadAll<Sprite>("Collectable Sprites"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
