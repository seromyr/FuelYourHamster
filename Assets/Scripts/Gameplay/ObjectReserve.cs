using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReserve : MonoBehaviour
{
    private Empty empty;
    private Coin coin;
    private CoffeeBean coffeeBean;
    private MutatedCollectible collectible;
    private ExtremeMutatedCollectible collectibleEx;
    private Quest questCollectible;

    [Serializable]
    public struct Vault
    {
        public int capacity;
        public GameObject[] objPool;
        public bool[] isInVault;
        public string name;
    }

    private MutationPool mutationPool;

    [SerializeField]
    private Vault vault01, vault02, vault03;

    public Vault Vault01 { get { return vault01; } }
    public Vault Vault02 { get { return vault02; } }
    public Vault Vault03 { get { return vault03; } }

    private Randomize randomize;

    private void Awake()
    {
        // Create collectible blueprints
        empty            = new Empty();
        coin             = new Coin();
        coffeeBean       = new CoffeeBean();
        collectible      = new MutatedCollectible();
        collectibleEx    = new ExtremeMutatedCollectible();
        questCollectible = new Quest();

        mutationPool     = GameObject.Find(PrimeObj.MUTATIONPOOL).GetComponent<MutationPool>();
                         
        randomize        = GetComponent<Randomize>();

        VaultSetup(PrimeObj.VAULT01, CONST.VAULT_01_CAPACITY, out vault01);
        VaultSetup(PrimeObj.VAULT02, CONST.VAULT_02_CAPACITY, out vault02);
        VaultSetup(PrimeObj.VAULT03, CONST.VAULT_03_CAPACITY, out vault03);
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
            //vault.objPool[i] = coin.Form;
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
        // Occurance range example:
        // 0---------------------------------------------------1000
        // 0----100                                                  : CoffeeBean 
        //       101----------------800                              : Coin
        //                           801-------950                   : Bad Mutation
        //                                      951---975            : Good Mutation
        //                                              976-----1000 : Extremely Bad Mutation

        for (int i = 0; i < theVault.isInVault.Length; i++)
        {
            int spawnChance = RollADice(DiffilcultyController.main.emptyScaling.startRange, DiffilcultyController.main.questScaling.endRange);

            // Empty Object Spawning
            if      (IsBetweenTwoNumber(spawnChance, DiffilcultyController.main.emptyScaling.startRange, DiffilcultyController.main.emptyScaling.endRange))
            {
                theVault.objPool[i] = empty.Form;
            }

            // Coin Spawning
            else if (IsBetweenTwoNumber(spawnChance, DiffilcultyController.main.coinScaling.startRange, DiffilcultyController.main.coinScaling.endRange))
            {
                theVault.objPool[i] = coin.Form;
            }

            // Coffee Bean Spawning
            else if (IsBetweenTwoNumber(spawnChance, DiffilcultyController.main.beanScaling.startRange, DiffilcultyController.main.beanScaling.endRange))
            {
                theVault.objPool[i] = coffeeBean.Form;
                theVault.objPool[i].GetComponent<MutatationMechanic>().ChangeMutationType(Constants.CollectibleType.Bean);
            }

            // Good Object Spawning
            else if (IsBetweenTwoNumber(spawnChance, DiffilcultyController.main.goodMutationScaling.startRange, DiffilcultyController.main.goodMutationScaling.endRange))
            {
                theVault.objPool[i] = collectible.Form;
                theVault.objPool[i].TryGetComponent<MutationAppearance>(out var skin);
                if (skin != null)
                {
                    skin.Skin = randomize.RandomizeMe(mutationPool.Beneficials);
                    theVault.objPool[i].GetComponent<MutatationMechanic>().ChangeMutationType(Constants.CollectibleType.Good);
                }
            }

            // Bad Object Spawning
            else if (IsBetweenTwoNumber(spawnChance, DiffilcultyController.main.badMutationScaling.startRange, DiffilcultyController.main.badMutationScaling.endRange))
            {
                theVault.objPool[i] = collectible.Form;
                theVault.objPool[i].TryGetComponent<MutationAppearance>(out var skin);
                if (skin != null)
                {
                    skin.Skin = randomize.RandomizeMe(mutationPool.Harmfuls);
                    theVault.objPool[i].GetComponent<MutatationMechanic>().ChangeMutationType(Constants.CollectibleType.Bad);
                }
            }

            // Extremely Bad Spawning
            else if (IsBetweenTwoNumber(spawnChance, DiffilcultyController.main.exBadMutationScaling.startRange, DiffilcultyController.main.exBadMutationScaling.endRange))
            {
                theVault.objPool[i] = collectibleEx.Form;
                theVault.objPool[i].TryGetComponent<MutationAppearance>(out var skin);
                if (skin != null)
                {
                    skin.Skin = randomize.RandomizeMe(mutationPool.Extremes);
                    theVault.objPool[i].GetComponent<MutatationMechanic>().ChangeMutationType(Constants.CollectibleType.VeryBad);
                }
            }

            // Quest Object Spawning
            else if (IsBetweenTwoNumber(spawnChance, DiffilcultyController.main.questScaling.startRange, DiffilcultyController.main.questScaling.endRange))
            {
                //  nothing for now
            }
        }
    }

    public int RollADice(int fromNumber, int toNumber)
    {
        return UnityEngine.Random.Range(fromNumber, toNumber + 1);
    }

    public bool IsBetweenTwoNumber(int evaluatingNumber, int min, int max)
    {
        return evaluatingNumber >= min && evaluatingNumber <= max;
    }
}