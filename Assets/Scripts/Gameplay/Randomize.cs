using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class is for randomization
public class Randomize : MonoBehaviour
{
    private List<int> list;
    [SerializeField]
    private int randomCount;
    public int RandomCount { set { randomCount = value; } }

    // Start is called before the first frame update
    void Awake()
    {
        list = new List<int>();
    }

    // Using Game Object Array
    public int RandomizeMe(GameObject[] prefabPool)
    {
        // Get access to pool array
        // Create a list
        // Pick an object in the pool, use its index to add a new element within the list, check if it's already inside
        // Count each time the result is final, after the n-th time, reset the list

        // Random an index from the pool
        int result = Random.Range(0, prefabPool.Length);

        // Re-random if the result existed inside the list
        while (list.Contains(result))
        {
            result = Random.Range(0, prefabPool.Length);
        }

        // Pick the result and add it to the list
        if (!list.Contains(result))
        {
            list.Add(result);
            // if the list length is >= randomCount, remove the 1st element
            if (list.Count >= randomCount)
            {
                list.RemoveAt(0);
            }
        }

        return result;
    }

    // Same as above but using Game Object List
    public int RandomizeMe(List<GameObject> prefabPool)
    {
        // Random an index from the pool
        int result = Random.Range(0, prefabPool.Count);

        // Re-random if the result existed inside the list
        while (list.Contains(result))
        {
            result = Random.Range(0, prefabPool.Count);
        }

        // Pick the result and add it to the list
        if (!list.Contains(result))
        {
            list.Add(result);
            // if the list length is >= randomCount, remove the 1st element
            if (list.Count >= randomCount)
            {
                list.RemoveAt(0);
            }
        }

        return result;
    }

    // Same as above but using Sprite List
    public Sprite RandomizeMe(List<Sprite> spritePool)
    {
        // Random an index from the pool
        int result = Random.Range(0, spritePool.Count);

        // Re-random if the result existed inside the list
        while (list.Contains(result))
        {
            result = Random.Range(0, spritePool.Count);
        }

        // Pick the result and add it to the list
        if (!list.Contains(result))
        {
            list.Add(result);
            // if the list length is >= randomCount, remove the 1st element
            if (list.Count >= randomCount)
            {
                list.RemoveAt(0);
            }
        }

        return spritePool[result];
    }
}
