using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class CoinMechanic : MonoBehaviour, IAudible
{
    private void Start()
    {
        Player.main.Mechanic.OnCollisionWithCollectible += OnCollected;
        SoundSetup();
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
            PlaySound(SoundController.main.SoundLibrary[1]);
        }
    }

    private void OnDestroy()
    {
        //Debug.Log("Coin: " + transform.name + " destroyed");
        Player.main.Mechanic.OnCollisionWithCollectible -= OnCollected;
    }

    #region Interfaces Implementation
    private AudioSource soundPlayer;
    public void SoundSetup()
    {
        soundPlayer = SoundController.main.CreateASoundPlayer(transform);
    }

    public void PlaySound(AudioClip sound)
    {
        SoundController.main.PlaySound(soundPlayer, sound);
    }
    #endregion
}
