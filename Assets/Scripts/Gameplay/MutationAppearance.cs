using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationAppearance : MonoBehaviour
{
    private Sprite skin;

    public Sprite Skin { set { transform.Find("Mesh").GetComponent<SpriteRenderer>().sprite = value; } }
}
