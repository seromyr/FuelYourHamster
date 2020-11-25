using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using Cinemachine;

public class PlayerMechanic : MonoBehaviour
{
    public event EventHandler<CollectibleType> OnCollisionWithCollectible;
    public bool RequestCoffee { get { return AskForCoffee(); } }

    private float coffeePerServing;

    // Camera option
    private CinemachineVirtualCamera vCamera;

    private void Start()
    {
        coffeePerServing = 20; // For cheating, remember to remove before gold

        // Player start with some caffeine in its blood
        Player.main.FullLoadFuel();

        // Initialize color effects
        Player.main.SpriteRenderer.color = Color.white;

    }

    private void Update()
    {
        // Player caffeine level is continously decreasing
        Player.main.ConsumeFuel(Time.deltaTime * Player.main.FuelConsumptionSpeed);

        // For cheat/debugging purpose, remember to remove before gold
        if (VomitCoffee()) Player.main.ConsumeFuel(coffeePerServing);
        else if (AskForCoffee()) { Player.main.ConsumeFuel(-coffeePerServing); Player.main.SetFund(99999); Player.main.FullLoadHealth(); }

        // Prevent caffeine level going outside permited range
        Player.main.FuelLimiter();
        // Prevent health level going outside permited range
        Player.main.HealthLimiter();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAG.COLLECTIBLE))
        {
            // Grant a unique ID to know which object is in contact
            string _temporaryID = DateTime.Now.GetHashCode().ToString();

            // Try to check if the object in contact is a quest item
            if (other.transform.childCount > 0 && other.transform.GetChild(0).TryGetComponent(out SpriteRenderer spriteRenderer) && QuestController.main.QuestMasterName == spriteRenderer.sprite.name)
            {
                // Override _temporaryID
                _temporaryID = spriteRenderer.sprite.name;
            }

            other.transform.name = _temporaryID;

            OnCollisionWithCollectible?.Invoke(this, new CollectibleType { hashCode = _temporaryID });
            Player.main.CollectiblePooling();
            StartCoroutine(ColorSwitch(Color.white, Player.main.CollisionColor));
        }
    }

    private bool AskForCoffee()
    {
        return Input.GetKeyDown(KeyCode.Return);
    }

    private bool VomitCoffee()
    {
        return Input.GetKeyDown(KeyCode.Backspace);
    }

    private IEnumerator ColorSwitch(Color colorBase, Color colorCollision)
    {
        if (colorCollision != Color.white)
        {
            for (int i = 0; i < 5; i++)
            {
                Player.main.SpriteRenderer.color = colorBase;
                yield return new WaitForSeconds(0.1f);
                Player.main.SpriteRenderer.color = colorCollision;
                yield return new WaitForSeconds(0.1f);
            }
        }
        Player.main.SpriteRenderer.color = colorBase;
    }

    private void OnEnable()
    {
        AssignVCamera();
        ClearEVent();
    }

    private void AssignVCamera()
    {
        GameObject.Find("Virtual Camera").TryGetComponent(out vCamera);
        vCamera.Follow = Player.main.Avatar.transform;
        vCamera.LookAt = Player.main.Avatar.transform;
    }

    private void ClearEVent()
    {
        OnCollisionWithCollectible = null;
    }
}

public class CollectibleType : EventArgs
{
    public string hashCode;
}
