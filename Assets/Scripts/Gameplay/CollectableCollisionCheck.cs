﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class CollectableCollisionCheck : MonoBehaviour
{
    private bool _switch;
    public bool Ready { get {return _switch; } }
    private Collider _target;
    public Collider Target { get { return _target; } }

    private SphereCollider sc;
    private int chance;
    private string origincalTag;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAG.COIN) || other.CompareTag(TAG.OBJGOOD))
        {
            sc = other.transform.GetComponent<SphereCollider>();
            origincalTag = other.tag;

            _switch = true;
            _target = other;
        }
        else if (other.CompareTag(TAG.OBJBAD))
        {
            other.tag = origincalTag;
            sc.isTrigger = false;

            _switch = true;
            _target = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TAG.COIN) || other.CompareTag(TAG.OBJGOOD))
        {
            _switch = false;
            _target = other;

            // Create a chance that this object is non-interactable
            chance = Random.Range(0, 11);
            if (chance >= 6)
            {
                other.transform.GetComponentInChildren<SpriteRenderer>().sprite = null;
                other.tag = TAG.OBJBAD;
                sc.isTrigger = true;
                chance = 0;
            }
            
        }
    }
}
