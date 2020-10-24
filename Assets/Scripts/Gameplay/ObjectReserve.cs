using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReserve : MonoBehaviour
{
    [Serializable]
    public struct Vault
    {
        public int capacity;
        public GameObject[] objPool;
        public bool[] isInVault;
        public string name;
    }


    private MutationPool mutationPool;
    private GameObject obj_Mutated, obj_coin;

    [SerializeField]
    private Vault vault01, vault02, vault03;

    public Vault Vault01 { get { return vault01; } }
    public Vault Vault02 { get { return vault02; } }
    public Vault Vault03 { get { return vault03; } }

    private Randomize randomize;

    private void Awake()
    {
        obj_coin = Resources.Load<GameObject>(Prefab.COIN);
        obj_Mutated = Resources.Load<GameObject>(Prefab.MUTATED);
        mutationPool = GameObject.Find(PrimeObj.MUTATIONPOOL).GetComponent<MutationPool>();

        VaultSetup(PrimeObj.VAULT01, CONST.VAULT_01_CAPACITY, out vault01);
        VaultSetup(PrimeObj.VAULT02, CONST.VAULT_02_CAPACITY, out vault02);
        VaultSetup(PrimeObj.VAULT03, CONST.VAULT_03_CAPACITY, out vault03);

        randomize = GetComponent<Randomize>();
    }

    private void VaultSetup(string name, int capacity, out Vault vault)
    {
        vault = new Vault()
        {
            name = name,
            capacity = capacity,
        };
        vault.objPool = new GameObject[vault.capacity];
        vault.isInVault = new bool[vault.capacity];

        for (int i = 0; i < vault.capacity; i++)
        {
            vault.objPool[i] = obj_coin;
            vault.isInVault[i] = true;
        }
    }

    private void Update()
    {
        MutationOccurence(vault01);
        MutationOccurence(vault02);
        MutationOccurence(vault03);
    }

    private void MutationOccurence(Vault theVault)
    {
        // Mutation only happens when a coin is inside pool 
        for (int i = 0; i < theVault.isInVault.Length; i++)
        {
            if (theVault.isInVault[i] && theVault.objPool[i] == obj_coin)
            {
                var mutationChance = UnityEngine.Random.Range(0, 1001);

                // Each mutation has % of occurence
                if (mutationChance >= 950)
                {
                    theVault.objPool[i] = obj_Mutated;
                    theVault.objPool[i].TryGetComponent<MutationAppearance>(out var skin);
                    if (skin != null)
                    {
                        var mutationType = UnityEngine.Random.Range(0, 3);
                        if (mutationType < 2)
                        {
                            skin.Skin = randomize.RandomizeMe(mutationPool.Beneficials);
                            theVault.objPool[i].tag = TAG.OBJGOOD;
                        }
                        else
                        {
                            skin.Skin = randomize.RandomizeMe(mutationPool.Harmfuls);
                            theVault.objPool[i].tag = TAG.OBJBAD;
                        }
                    }
                }
            }
            else if (theVault.isInVault[i] && theVault.objPool[i] != obj_coin)
            {
                var deMutationChance = UnityEngine.Random.Range(0, 1001);

                // Each de-mutation has % of occurence
                if (deMutationChance >= 400)
                {
                    theVault.objPool[i] = obj_coin;
                }
            }
        }
    }
}
