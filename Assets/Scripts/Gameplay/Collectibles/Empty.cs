using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class Empty : Entity, ICollectable
{

    public Empty()
    {
        _isSingleton = false;
        CreateBody();
        CreateAvatar();
        CreateMechanic();
    }

    protected override void CreateBody()
    {
        _name = "Empty";
        _body = new GameObject(_name);
        _body.transform.position = Vector3.zero;
        _body.tag = TAG.COLLECTIBLE;
        _collider = _body.AddComponent<CapsuleCollider>();
        _collider.center = Vector3.zero;
        _collider.radius = 0.5f;
        _rigidbody = null; // No rigidbody
    }
    protected override void CreateAvatar()
    {
        // No avatar
    }
    protected override void CreateMechanic()
    {
        CollectibleSetup();
    }

    protected override void CreateVFX()
    {

    }

    protected override void Singletonize(string debugMessage = null)
    {
        // This object will not be singletonized
    }
    protected override void GameplaySetup()
    {

    }

    #region Interfaces Implementation
    public void CollectibleSetup()
    {
        _collider.isTrigger = true;
        _body.AddComponent<EmptyMechanic>();
    }
    #endregion
}
