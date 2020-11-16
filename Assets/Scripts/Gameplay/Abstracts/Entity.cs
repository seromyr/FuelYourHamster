using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity
{
    // High Level Variables
    protected string            _name;
    protected GameObject        _body;
    protected GameObject        _avatar;
    protected GameObject        _skin;
    protected Rigidbody         _rigidbody;
    protected CapsuleCollider   _collider;
    protected bool              _isAlive;
    protected bool              _isSingleton;

    // Low Level Variables
    protected int               _health;
    protected int               _maxHealth;

    // Accessors
    public int                  Health      { get { return _health; } }
    public int                  MaxHealth   { get { return _maxHealth; } }

    public string               Name        { get { return _name; } }
    public GameObject           Form        { get { return _body; } }
    public GameObject           Avatar      { get { return _avatar; } }
    public Rigidbody            RigidBody   { get { return _rigidbody; } }

    // Abstract Methods
    protected abstract void CreateBody();
    protected abstract void CreateAvatar();
    protected abstract void CreateMechanic();
    protected abstract void CreateVFX();
    protected abstract void Singletonize(string debugMessage = null);
    protected abstract void GameplaySetup();
}
