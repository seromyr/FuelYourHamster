using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class Player : Entity, IControlable, IUpgradeable, IDamageble, IFuelConsumable, IMoneyObtainable
{
    public static Player main;

    #region Gameplay Variables Declaration
    private PlayerMechanic      _mechanic;
    private bool                _allowPlayerControl;
    private float               _caffeineLevel;
    private bool                _isConsumingFuel;
    private float               _caffeineMaxLevel;
    private float               _caffeineConsumingSpeed;
    private SpriteRenderer      _spriteRenderer;
    private Color               _collisionColor;

    // Player get access to vault
    private ObjectReserve.Vault _vault01, _vault02, _vault03;
    private string              _currentVaultName;

    private int _moneyTotal;
    private int _moneyCurrent;

    // Money magnet upgrade
    private GameObject          _moneyMagnet;

    // Hamster ball upgrade
    private GameObject          _hamsterBall;
    private HamsterBallMechanic _hamsterBallMechanic;
    private int                 _hamsterBallLevel;
    #endregion

    #region Accessors
    public PlayerMechanic      Mechanic             { get { return _mechanic; } }
    public bool                AllowPlayerControl   { get { return _allowPlayerControl; } }
    public float               CaffeineCurrentLevel { get { return _caffeineLevel; } }
    public bool                IsRunning            { get { return _caffeineLevel > 0 && _isConsumingFuel; } }
    public float               CaffeineMaxLevel     { get { return _caffeineMaxLevel; } }
    public float               FuelConsumptionSpeed { get { return _caffeineConsumingSpeed; } }
    public SpriteRenderer      SpriteRenderer       { get { return _spriteRenderer; } }
    public Color               CollisionColor       { get { return _collisionColor; } }
    public int                 Wallet               { get { return _moneyTotal; } }
    public int                 Income               { get { return _moneyCurrent; } }
    public HamsterBallMechanic HamsterBall          { get { return _hamsterBallMechanic; } }
    public int                 HamsterBallLevel     { get { return _hamsterBallLevel; } }
    #endregion

    public Player()
    {
        CreateBody();
        CreateAvatar();
        CreateMechanic();
        CreateVFX();

        ControllerSetup();

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
        GameObject _vfx = new GameObject("VFX");
        _vfx.transform.parent = _body.transform;
        _vfx.transform.localPosition = Vector3.zero;

        _collisionColor = Color.white;
        _collisionColor.a = 1;
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

        // Allow control
        _allowPlayerControl = false;

        HamsterBallSetup();
    }

    private void HamsterBallSetup()
    {
        _hamsterBall                         = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _hamsterBall.name                    = "HamsterBall";
        _hamsterBall.transform.parent        = _avatar.transform;
        _hamsterBall.transform.localPosition = Vector3.zero;
        _hamsterBallMechanic                 = _hamsterBall.AddComponent<HamsterBallMechanic>();

        _hamsterBallLevel = CONST.DEFAULT_HAMSTERBALL_LEVEL;
    }

    #region Interfaces Implementation
    public void ControllerSetup()
    {
        _body.AddComponent<PlayerController>();
    }

    public void ControlPermission(bool value)
    {
        _allowPlayerControl = value;
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
        _hamsterBallLevel += CONST.HAMSTERBALL_UPGRADE_VALUE;
        _hamsterBallMechanic.ResetSphere();
    }

    public void UpgradeMoneyMagnet()
    {
        _moneyMagnet = new GameObject("MoneyMagnet");
        _moneyMagnet.AddComponent<MoneyMagnet>();
    }

    public void HealthLimiter()
    {
        if (_health > _maxHealth)
        {
            FullLoadHealth();
        }
        else if (_health < 0)
        {
            EmptyHealth();
        }
    }

    public void TakeDamage(int damageTaken)
    {
        _health -= damageTaken;
    }

    public void RestoreHealth(int value)
    {
        _health += value;
    }

    public void FullLoadHealth()
    {
        _health = _maxHealth;
    }

    public void EmptyHealth()
    {
        _health = 0;
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

    public void IsConsumingFuel(bool value)
    {
        _isConsumingFuel = value;
    }

    public void IntakeFuel(float amount)
    {
        _caffeineLevel += amount;
    }

    public void ConsumeFuel(float amount)
    {
        if (_isConsumingFuel)
        {
            _caffeineLevel -= amount;
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

    public void SetFund(int value)
    {
        _moneyTotal = value;
    }

    public void AddFundToWallet(int value)
    {
        _moneyTotal += value;
    }

    public void AddIncome(int value)
    {
        _moneyCurrent += value;
    }

    public void ResetIncome()
    {
        _moneyCurrent = 0;
    }
    #endregion

    public void AssignVault()
    {
        _vault01 = GameObject.Find(PrimeObj.OBJPOOL).GetComponent<ObjectReserve>().Vault01;
        _vault02 = GameObject.Find(PrimeObj.OBJPOOL).GetComponent<ObjectReserve>().Vault02;
        _vault03 = GameObject.Find(PrimeObj.OBJPOOL).GetComponent<ObjectReserve>().Vault03;
    }

    public void IsInLaneNumber(int value)
    {
        switch (value)
        {
            case 1:  _currentVaultName = _vault01.name; break;
            case 2:  _currentVaultName = _vault02.name; break;
            case 3:  _currentVaultName = _vault03.name; break;
        }
    }

    public void CollectiblePooling()
    {
        // Coin pooling on player side
        switch (_currentVaultName)
        {
            case PrimeObj.VAULT01:
                for (int i = 0; i < _vault01.capacity; i++)
                {
                    if (!_vault01.isInVault[i])
                    {
                        _vault01.isInVault[i] = true;
                        break;
                    }
                }
                break;
            case PrimeObj.VAULT02:
                for (int i = 0; i < _vault02.capacity; i++)
                {
                    if (!_vault02.isInVault[i])
                    {
                        _vault02.isInVault[i] = true;
                        break;
                    }
                }
                break;
            case PrimeObj.VAULT03:
                for (int i = 0; i < _vault03.capacity; i++)
                {
                    if (!_vault03.isInVault[i])
                    {
                        _vault03.isInVault[i] = true;
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

    public void ChangeCollisionColor(Color color)
    {
        _collisionColor = color;
    }
}
