using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObject : MonoBehaviour
{
    // Lane data
    private struct SpawnLane
    {
        public string name;
        public GameObject body;
        public BoxCollider triggerZone;
        public Randomize random;
        public List<Sprite> harmfulObjectPool;
        public List<Sprite> beneficialObjectPool;
        public CollectableCollisionCheck switchObject;
        public Sprite nextSprite;
    }

    // Actual lane in gameplay
    private SpawnLane laneInner, laneMid, laneOuter;

    void Start()
    {
        // Construct lanes
        // In each set, a new game object is construct with suitable components and settings to make it become
        // a receptor that change the object sprite each time of passing by 
        laneOuter = new SpawnLane();
        laneOuter.name = "OuterLane";
        laneOuter.body = new GameObject(laneOuter.name);
        laneOuter.body.transform.SetParent(transform);
        laneOuter.body.transform.localScale = new Vector3(0.5f, 1, 1);
        laneOuter.body.transform.localPosition = new Vector3(0.9f, 0, 0);
        laneOuter.triggerZone = laneOuter.body.AddComponent<BoxCollider>();
        laneOuter.triggerZone.isTrigger = true;
        laneOuter.random = laneOuter.body.AddComponent<Randomize>();
        laneOuter.harmfulObjectPool = GameObject.Find("CollectablePool").GetComponent<CollectablePool>().HarmfulSpritePool;
        laneOuter.beneficialObjectPool = GameObject.Find("CollectablePool").GetComponent<CollectablePool>().BeneficialSpritePool;
        laneOuter.switchObject = laneOuter.body.AddComponent<CollectableCollisionCheck>();
        laneOuter.nextSprite = laneOuter.random.RandomizeMe(laneOuter.harmfulObjectPool);
        // ---
        laneMid = new SpawnLane();
        laneMid.name = "MiddleLane";
        laneMid.body = new GameObject(laneMid.name);
        laneMid.body.transform.SetParent(transform);
        laneMid.body.transform.localScale = new Vector3(0.5f, 1, 1);
        laneMid.body.transform.localPosition = Vector3.zero;
        laneMid.triggerZone = laneMid.body.AddComponent<BoxCollider>();
        laneMid.triggerZone.isTrigger = true;
        laneMid.random = laneMid.body.AddComponent<Randomize>();
        laneMid.harmfulObjectPool = GameObject.Find("CollectablePool").GetComponent<CollectablePool>().HarmfulSpritePool;
        laneMid.beneficialObjectPool = GameObject.Find("CollectablePool").GetComponent<CollectablePool>().BeneficialSpritePool;
        laneMid.switchObject = laneMid.body.AddComponent<CollectableCollisionCheck>();
        laneMid.nextSprite = laneMid.random.RandomizeMe(laneOuter.harmfulObjectPool);
        // ---
        laneInner = new SpawnLane();
        laneInner.name = "InnerLane";
        laneInner.body = new GameObject(laneInner.name);
        laneInner.body.transform.SetParent(transform);
        laneInner.body.transform.localScale = new Vector3(0.5f, 1, 1);
        laneInner.body.transform.localPosition = new Vector3(-0.8f, 0, 0);
        laneInner.triggerZone = laneInner.body.AddComponent<BoxCollider>();
        laneInner.triggerZone.isTrigger = true;
        laneInner.random = laneInner.body.AddComponent<Randomize>();
        laneInner.harmfulObjectPool = GameObject.Find("CollectablePool").GetComponent<CollectablePool>().HarmfulSpritePool;
        laneInner.beneficialObjectPool = GameObject.Find("CollectablePool").GetComponent<CollectablePool>().BeneficialSpritePool;
        laneInner.switchObject = laneInner.body.AddComponent<CollectableCollisionCheck>();
        laneInner.nextSprite = laneInner.random.RandomizeMe(laneOuter.harmfulObjectPool);
    }

    void Update()
    {
        //Monitoring trigger zone in lanes
        if (laneMid.switchObject.Ready)
        {
            laneMid.switchObject.Target.transform.GetComponentInChildren<SpriteRenderer>().sprite = laneMid.nextSprite;

            //Re - randomize next sprite;
            laneMid.nextSprite = laneMid.random.RandomizeMe(laneMid.harmfulObjectPool);
        }

        if (laneOuter.switchObject.Ready)
        {
            laneOuter.switchObject.Target.transform.GetComponentInChildren<SpriteRenderer>().sprite = laneOuter.nextSprite;

            //Re - randomize next sprite;
            laneOuter.nextSprite = laneOuter.random.RandomizeMe(laneOuter.harmfulObjectPool);
        }

        if (laneInner.switchObject.Ready)
        {
            laneInner.switchObject.Target.transform.GetComponentInChildren<SpriteRenderer>().sprite = laneInner.nextSprite;

            //Re - randomize next sprite;
            laneInner.nextSprite = laneInner.random.RandomizeMe(laneInner.beneficialObjectPool);
        }
    }
}
