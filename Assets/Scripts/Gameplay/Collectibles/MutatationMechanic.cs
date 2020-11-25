using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class MutatationMechanic : MonoBehaviour, IAudible
{
    [SerializeField]
    private Constants.CollectibleType _mutationType;

    private Color interactColor;

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
            switch (_mutationType)
            {
                case Constants.CollectibleType.Good:
                    //Debug.LogWarning("Healed 1 HP");
                    if (Player.main.Health < Player.main.MaxHealth) Player.main.RestoreHealth(1);
                    ColorUtility.TryParseHtmlString(CONST.PLAYER_COLLECT_COLOR, out interactColor);
                    PlaySound(SoundController.main.SoundLibrary[6]);
                    break;

                case Constants.CollectibleType.Bad:
                    //Debug.LogError("Take 1 damage");
                    if (Player.main.HamsterBall != null && Player.main.HamsterBall.IsActive)
                    {
                        Player.main.HamsterBall.DegradeSphere();
                        interactColor = Color.white;
                    }
                    else
                    {
                        if (Player.main.Health > 0) Player.main.TakeDamage(1);
                        ColorUtility.TryParseHtmlString(CONST.PLAYER_HIT_COLOR, out interactColor);
                    }
                    PlaySound(SoundController.main.SoundLibrary[5]);
                    break;

                case Constants.CollectibleType.VeryBad:
                    //Debug.Log("Very bad object");
                    Player.main.TakeDamage(1);
                    ColorUtility.TryParseHtmlString(CONST.PLAYER_HIT_COLOR, out interactColor);
                    PlaySound(SoundController.main.SoundLibrary[5]);
                    break;

                case Constants.CollectibleType.Bean:
                    //Debug.Log("Bean");
                    Player.main.IntakeFuel(10);
                    ColorUtility.TryParseHtmlString(CONST.PLAYER_COLLECT_COLOR, out interactColor);
                    PlaySound(SoundController.main.SoundLibrary[4]);
                    break;
            }
            Player.main.ChangeCollisionColor(interactColor);
        }
    }

    private void OnDestroy()
    {
        //Debug.Log("Mutation " + transform.name + " destroyed");
        Player.main.Mechanic.OnCollisionWithCollectible -= OnCollected;
    }

    public void ChangeMutationType(Constants.CollectibleType type)
    {
        _mutationType = type;
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
