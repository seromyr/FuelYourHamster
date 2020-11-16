using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class ObjectRecycler : MonoBehaviour
{
    private ObjectReserve.Vault vault;

    [SerializeField]
    private Vault currentVault;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAG.COLLECTIBLE) || other.CompareTag(TAG.OBJGOOD) || other.CompareTag(TAG.OBJBAD))
        {
            Destroy(other.gameObject);
            // Coin pooling on factory side
            for (int i = 0; i < vault.capacity; i++)
            {
                if (!vault.isInVault[i])
                {
                    vault.isInVault[i] = true;
                    break;
                }
            }
        }
    }
}
