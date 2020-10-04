using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObject : MonoBehaviour
{
    // Construct a custom random class with no duplicate result
    private Randomize random;

    private List<Sprite> spritePool;

    [SerializeField]
    private Sprite nextSprite;

    void Start()
    {
        spritePool = GameObject.Find("CollectablePool").GetComponent<CollectablePool>().SpritePool;
        random = GetComponent<Randomize>();

        //Initialize the 1st next sprite
        nextSprite = random.RandomizeMe(spritePool);
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            other.transform.GetComponentInChildren<SpriteRenderer>().sprite = nextSprite;

            //Re - randomize next sprite;
            nextSprite = random.RandomizeMe(spritePool);
        }

    }
}
