using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class ObjectSpawner : MonoBehaviour
{
    private Transform coinContainer;

    private ObjectReserve.Vault vault;

    [SerializeField]
    private Vault currentVault; // Require manually assignment

    private int nextID;

    [SerializeField]
    private float spawnDelay;
    private float time;

    void Start()
    {
        switch (currentVault)
        {
            case Vault.Alpha:
                vault = GameObject.Find(PrimeObj.OBJPOOL).GetComponent<ObjectReserve>().Vault01;
                break;
            case Vault.Beta:
                vault = GameObject.Find(PrimeObj.OBJPOOL).GetComponent<ObjectReserve>().Vault02;
                break;
            case Vault.Gamma:
                vault = GameObject.Find(PrimeObj.OBJPOOL).GetComponent<ObjectReserve>().Vault03;
                break;
        }

        coinContainer = GameObject.Find(PrimeObj.OBJCONTAINER).transform;

        nextID = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAG.SPAWNCOLLIDER))
        {
            time = Time.time;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(TAG.SPAWNCOLLIDER))
        {
            if (Time.time >= time + spawnDelay)
            {
                time = Time.time;

                nextID = (nextID + 1) % vault.capacity;

                if (vault.isInVault[nextID])
                {
                    SpawnCoinInPool(nextID, other.transform.position);
                    vault.isInVault[nextID] = false;
                }
            }
        }
    }

    private void SpawnCoinInPool(int id, Vector3 position)
    {
        Instantiate(vault.objPool[id], position + Vector3.down, Quaternion.identity, coinContainer);
    }
}
