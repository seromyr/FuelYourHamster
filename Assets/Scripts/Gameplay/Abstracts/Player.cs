using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class Player : Entity, IControlable, IUpgradeable, IDamageble, IFuelConsumable
{
    public static Player main;

    #region Gameplay Variables Declaration
    private PlayerMechanic _mechanic;
    public PlayerMechanic Mechanic { get { return _mechanic; } }

    private float _caffeineLevel;
    public float CaffeineCurrentLevel { get { return _caffeineLevel; } }

    private bool _isConsumingFuel;
    public bool IsConsumingFuel { set { _isConsumingFuel = value; } }
    public bool IsRunning { get { return _caffeineLevel > 0 && _isConsumingFuel; } }

    private float _caffeineMaxLevel;
    public float CaffeineMaxLevel { get { return _caffeineMaxLevel; } }

    private float _caffeineConsumingSpeed;
    public float FuelConsumptionSpeed { get { return _caffeineConsumingSpeed; } }

    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } }

    // Player get access to vault
    private ObjectReserve.Vault vault01, vault02, vault03;
    private string currentVaultName;

    // Money magnet upgrade
    private GameObject moneyMagnet;

    // Hamster ball upgrade
    private GameObject _hamsterBall;
    private HamsterBallMechanic _hamsterBallMechanic;
    public HamsterBallMechanic HamsterBall { get { return _hamsterBallMechanic; } }
    private int hamsterBallLevel;
    public int HamsterBallLevel { get { return hamsterBallLevel; } }
    #endregion
     
    public Player()
    {
        CreateBody();
        CreateAvatar();
        CreateMechanic();
        CreateVFX();

        Control();

        _isSingleton = true;
        Singletonize("Player created as a singleton");

        _isAlive = true;

        GameplaySetup();
    }

    protected override void Singletonize(string debugMessage = null)
    {
        if (_isSingleton)
        {
            if (main == null)
            {
                UnityEngine.Object.DontDestroyOnLoad(_body);
                main = this;
            }
            else if (main != null)
            {
                UnityEngine.Object.Destroy(_body);
            }
            Debug.Log(debugMessage);
        }
    }

    protected override void CreateBody()
    {
        _name                            = PrimeObj.PLAYER;
        _body                            = new GameObject(_name);
        _body.transform.position         = CONST.PLAYER_DEFAULT_POSITION;
        _body.tag                        = _name;
        _collider                        = _body.AddComponent<CapsuleCollider>();
        _collider.center                 = new Vector3(0, 0.5f, 0);
        _collider.radius                 = 1;
        _rigidbody                       = _body.AddComponent<Rigidbody>();
        _rigidbody.constraints           = (RigidbodyConstraints)(RigidbodyConstraints.FreezeAll - RigidbodyConstraints.FreezePositionY);
        _rigidbody.isKinematic           = true;
    }

    protected override void CreateAvatar()
    {
        _avatar                          = new GameObject("Avatar");
        _avatar.transform.parent         = _body.transform;
        _avatar.transform.localPosition  = CONST.PLAYER_DEFAULT_AVATAR_POSITION;
        _avatar.transform.localRotation  = CONST.PLAYER_DEFAULT_AVATAR_ROTATION;
        _avatar.transform.localScale     = CONST.PLAYER_DEFAULT_AVATAR_SIZE;
        _avatar.AddComponent<SpriteShadowEnabler>();

        _spriteRenderer                  = _avatar.AddComponent<SpriteRenderer>();
        _spriteRenderer.sprite           = Resources.Load<Sprite>("Player/Hamster_rear");
        _spriteRenderer.material         = Resources.Load<Material>("Materials/SpriteShadow");
    }

    protected override void CreateMechanic()
    {
        _mechanic = _body.AddComponent<PlayerMechanic>();
    }

    protected override void CreateVFX()
    {

    }

    protected override void GameplaySetup()
    {
        // Health info
        _maxHealth                       = CONST.DEFAULT_MAX_HEALTH;
        _health                          = _maxHealth;

        _caffeineMaxLevel                = CONST.DEFAULT_MAX_CAFFEINE_LEVEL;

        // Player start with some caffeine in its blood
        _caffeineLevel                   = _caffeineMaxLevel;

        _caffeineConsumingSpeed          = CONST.DEFAULT_CAFFEINE_COSUMING_SPEED;
        _isConsumingFuel                 = false;

        // Initialize color effects
        _spriteRenderer.color            = Color.white;

        HamsterBallSetup();
    }

    private void HamsterBallSetup()
    {
        _hamsterBall                         = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _hamsterBall.name                    = "HamsterBall";
        _hamsterBall.transform.parent        = _avatar.transform;
        _hamsterBall.transform.localPosition = Vector3.zero;
        _hamsterBallMechanic                 = _hamsterBall.AddComponent<HamsterBallMechanic>();

        hamsterBallLevel = CONST.DEFAULT_HAMSTERBALL_LEVEL;
    }

    #region Interfaces Implementation
    public void Control()
    {
        _body.AddComponent<PlayerController>();
    }

    public void UpgradeFuelEfficiency()
    {
        _caffeineConsumingSpeed -= CONST.CAFFEINE_COSUMING_SPEED_UPGRADE_VALUE;
    }

    public void UpgradeMaxHealth()
    {
        _maxHealth += CONST.MAX_HEALTH_UPGRADE_VALUE;
    }

    public void UpgradeMaxFuel()
    {
        _caffeineMaxLevel += CONST.CAFFEINE_UPGRADE_VALUE;
    }

    public void UpgradeHamsterBall()
    {
        hamsterBallLevel += CONST.HAMSTERBALL_UPGRADE_VALUE;
        _hamsterBallMechanic.ResetSphere();
    }

    public void UpgradeMoneyMagnet()
    {
        moneyMagnet = new GameObject("MoneyMagnet");
        moneyMagnet.AddComponent<MoneyMagnet>();
    }

    public void TakeDamage(int damageTaken)
    {
        _health -= damageTaken;
    }

    public void RestoreHealth(int value)
    {
        _health += value;
    }

    public void ResetHealth()
    {
        _health = _maxHealth;
    }

    public void IntakeFuel(float amount)
    {

    }

    public void ConsumeFuel(float amount)
    {
        if (_isConsumingFuel)
        {
            _caffeineLevel += amount;
        }
    }

    public void FuelLimiter()
    {
        if (_caffeineLevel > _caffeineMaxLevel)
        {
            FullLoadFuel();
        }
        else if (_caffeineLevel < 0)
        {
            EmptyFuel();
        }
    }

    public void FullLoadFuel()
    {
        _caffeineLevel = _caffeineMaxLevel;
    }

    public void EmptyFuel()
    {
        _caffeineLevel = 0;
    }
    #endregion

    public void AssignVault()
    {
        vault01 = GameObject.Find(PrimeObj.OBJPOOL).GetComponent<ObjectReserve>().Vault01;
        vault02 = GameObject.Find(PrimeObj.OBJPOOL).GetComponent<ObjectReserve>().Vault02;
        vault03 = GameObject.Find(PrimeObj.OBJPOOL).GetComponent<ObjectReserve>().Vault03;
    }

    public void IsInLaneNumber(int value)
    {
        switch (value)
        {
            case 1: currentVaultName = vault01.name; break;
            case 2: currentVaultName = vault02.name; break;
            case 3: currentVaultName = vault03.name; break;
            default: break;
        }
    }

    public void CollectiblePooling()
    {
        // Coin pooling on player side
        switch (currentVaultName)
        {
            case PrimeObj.VAULT01:
                for (int i = 0; i < vault01.capacity; i++)
                {
                    if (!vault01.isInVault[i])
                    {
                        vault01.isInVault[i] = true;
                        break;
                    }
                }
                break;
            case PrimeObj.VAULT02:
                for (int i = 0; i < vault02.capacity; i++)
                {
                    if (!vault02.isInVault[i])
                    {
                        vault02.isInVault[i] = true;
                        break;
                    }
                }
                break;
            case PrimeObj.VAULT03:
                for (int i = 0; i < vault03.capacity; i++)
                {
                    if (!vault03.isInVault[i])
                    {
                        vault03.isInVault[i] = true;
                        break;
                    }
                }
                break;
        }
    }

    public void IsKinematic(bool value)
    {
        _rigidbody.isKinematic = value;
    }

    public void SetActive(bool value)
    {
        _body.SetActive(value);
    }
}
