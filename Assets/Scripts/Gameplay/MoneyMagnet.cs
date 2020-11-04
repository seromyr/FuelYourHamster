using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyMagnet : MonoBehaviour
{
    public static MoneyMagnet main;

    private GameObject coinContainer, coinCatcher;

    private BoxCollider bc;
    private Rigidbody rb;
    private Vector3 pos;
    private float attractionSpeed;

    private Vector3 posOffset;

    private void Awake()
    {
        Singletonizer();

        transform.position = Player.main.transform.position;
        transform.SetParent(GameManager.main.transform);

        coinContainer = new GameObject("TemporaryCoinContainer");
        coinContainer.transform.SetParent(transform);

        coinCatcher = new GameObject("CoinCatcher");
        coinCatcher.transform.SetParent(transform);

        bc = coinCatcher.AddComponent<BoxCollider>();
        bc.size = new Vector3(12, 10, 1);
        bc.center = Vector3.zero;
        bc.isTrigger = true;

        pos = new Vector3();
        posOffset = new Vector3(0, 0, -7);

        rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        attractionSpeed = 100;
    }
    private void Start()
    {
        pos = transform.position;
    }

    private void Singletonizer()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("Money Magnet created");
            main = this;
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        FollowPlayer();
        MoveCoinTowardPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAG.COIN))
        {
            other.transform.SetParent(coinContainer.transform);
            Debug.Log(other.name);
        }
    }

    private void FollowPlayer()
    {
        pos.x = Player.main.transform.position.x;
        coinCatcher.transform.position = pos + posOffset;
    }

   private void MoveCoinTowardPlayer()
    {
        if (coinContainer.transform.childCount > 0)
        {
            for (int i = 0; i < coinContainer.transform.childCount; i++)
            {
                coinContainer.transform.GetChild(i).transform.position = Vector3.MoveTowards(coinContainer.transform.GetChild(i).transform.position, Player.main.transform.position, Time.deltaTime * attractionSpeed);
            }
        }
    }
}
