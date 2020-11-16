using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class MutatedCollectible : Entity, ICollectable
{
    public MutatedCollectible()
    {
        _isSingleton = false;
        CreateBody();
        CreateAvatar();
        CreateMechanic();
    }

    protected override void CreateBody()
    {
        _name = "MutatedCollectible";
        _body = new GameObject(_name);
        _body.transform.position = Vector3.zero;
        _body.tag = TAG.COLLECTIBLE;
        _body.AddComponent<MutationAppearance>();
        _collider = _body.AddComponent<CapsuleCollider>();
        _collider.center = Vector3.zero;
        _collider.radius = 0.5f;
        _rigidbody = null; // No rigidbody
    }
    protected override void CreateAvatar()
    {
        _avatar = new GameObject("Avatar");
        _avatar.transform.parent = _body.transform;
        _avatar.transform.localPosition = Vector3.zero;
        _avatar.AddComponent<SpriteRenderer>();
        _avatar.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/SpriteShadow");
        _avatar.AddComponent<SpriteShadowEnabler>();
        _avatar.AddComponent<AlwaysLookAtCamera>();
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
        _body.AddComponent<MutatationMechanic>();
    }
    #endregion
}
