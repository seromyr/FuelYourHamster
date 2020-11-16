using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class Coin : Entity, ICollectable
{
    public Coin()
    {
        _isSingleton = false;
        CreateBody();
        CreateAvatar();
        CreateMechanic();
    }

    protected override void CreateBody()
    {
        _name = "Coin";
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
        _avatar = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        _avatar.name = "Avatar";
        _avatar.transform.parent = _body.transform;
        _avatar.transform.localPosition = Vector3.zero;
        _avatar.transform.localRotation = Quaternion.Euler(0, -45, 90);
        _avatar.transform.localScale = new Vector3(1, 0.05f, 1);
        _avatar.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Colors/Color_00 1");
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
        UnityEngine.Object.Destroy(_avatar.GetComponent<CapsuleCollider>());
        _body.AddComponent<CoinMechanic>();
    }
    #endregion
}
