using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class CoinMechanic : MonoBehaviour
{
    private void Start()
    {
        Player.main.Mechanic.OnCollisionWithCollectible += OnCollected;
    }

    public void OnCollected(object sender, CollectibleType e)
    {
        if (e.hashCode == gameObject.name)
        {
            Destroy(gameObject);
            Player.main.AddFundToWallet(1);
            Player.main.AddIncome(1);
            //Debug.Log("1 coin collected");
            Player.main.ChangeCollisionColor(Color.white);
        }
    }

    private void OnDestroy()
    {
        //Debug.Log("Coin: " + transform.name + " destroyed");
        Player.main.Mechanic.OnCollisionWithCollectible -= OnCollected;
    }
}
