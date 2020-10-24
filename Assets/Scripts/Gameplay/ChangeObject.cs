using Constants;
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

    // Pool of Collectibles reference
    private List<Sprite> goodPool, badPool;

    void Start()
    {
        goodPool = GameObject.Find("CollectiblePool").GetComponent<MutationPool>().Beneficials;
        badPool = GameObject.Find("CollectiblePool").GetComponent<MutationPool>().Harmfuls;

        // Construct lanes
        // In each set, a new game object is construct with suitable components and settings to make it become
        // a receptor that change the object sprite each time of passing by 

        //--------------- name -------- size ------------------- position ------------ out parameters
        ConstructLane("OuterLane",  Vector3.one, new Vector3(2.25f, 0, 0),    out laneOuter);
        ConstructLane("MiddleLane", Vector3.one,     Vector3.zero,           out laneMid);
        ConstructLane("InnerLane",  Vector3.one, new Vector3(-2.25f, 0, 0),   out laneInner);
    }

    private void ConstructLane(string name, Vector3 localScale, Vector3 localPosition, out SpawnLane lane)
    {
        lane = new SpawnLane();
        lane.name = name;

        lane.body = new GameObject(lane.name);
        lane.body.transform.SetParent(transform);
        lane.body.tag = TAG.SPAWNCOLLIDER;

        lane.body.transform.localScale = localScale;
        lane.body.transform.localPosition = localPosition;

        lane.triggerZone = lane.body.AddComponent<BoxCollider>();
        lane.triggerZone.isTrigger = true;

        lane.random = lane.body.AddComponent<Randomize>();

        lane.harmfulObjectPool = badPool;
        lane.beneficialObjectPool = goodPool;
        //lane.switchObject = lane.body.AddComponent<CollectableCollisionCheck>();
        lane.nextSprite = lane.random.RandomizeMe(lane.harmfulObjectPool);
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
