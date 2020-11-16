using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController main;

    public GameObject _objectContainer;
    private SpriteRenderer _questMaster;
    public string QuestMasterName { get { return _questMaster.sprite.name; } }

    private float _beginQuestCheck, _durationQuestCheck;

    public List<Sprite> questCharacters, quest1, quest2, quest3, quest4, quest5;

    private int currentQuestID, currentQuestProgress;
    public int CurrentActiveQuestID { get { return currentQuestID; } }

    public Queue<Sprite> currentQuest;

    private bool isChecking;

    private string characterCollected;
    public string CharacterCollected { get { return characterCollected; } }

    private void Awake()
    {
        Singletonize();

        questCharacters = new List<Sprite>();
        questCharacters.AddRange(Resources.LoadAll<Sprite>("Collectable Sprites/Quest"));

        currentQuestID = 1;
        currentQuest = new Queue<Sprite>();
        isChecking = false;
    }

    private void Start()
    {
        QuestSetup("FYH"               , out quest1);
        QuestSetup("Buu"               , out quest2);
        QuestSetup("Josh"              , out quest3);
        QuestSetup("Nibbles"           , out quest4);
        QuestSetup("Fuel Your Hamster" , out quest5);
    }

    private void Singletonize()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("Quest Controller created");
            main = this;
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        QuestCheck();
    }

    public void AssignCollectibleContainer()
    {
        _objectContainer = GameObject.Find("ObjectContainer");
        _questMaster = GameObject.Find("Quest Item").transform.GetComponentInChildren<SpriteRenderer>();
        Debug.Log("Acquired Collectible container: " + _objectContainer.name);
        Debug.Log("Acquired Quest Master Object: " + _questMaster.name);
        _durationQuestCheck = 10f;
        ResetQuestCheck();
    }

    private void ResetQuestCheck()
    {
        _beginQuestCheck = Time.time;
    }

    private void QuestCheck()
    {
        if (    isChecking
             && Time.time                             >= _beginQuestCheck + _durationQuestCheck
             && _objectContainer                      != null
             && _objectContainer.transform.childCount >  0                                      )
        {
            ResetQuestCheck();
            GameObject toReplaceCollectible = _objectContainer.transform.GetChild(_objectContainer.transform.childCount - 1).gameObject;
            Instantiate(_questMaster.transform.parent.gameObject, toReplaceCollectible.transform.position, Quaternion.identity, _objectContainer.transform);
            Destroy(toReplaceCollectible);
        }
    }

    private void QuestSetup(string input, out List<Sprite> quest)
    {
        quest = new List<Sprite>();
        foreach (char _character in input)
        {
            foreach (Sprite _sprite in questCharacters)
            {
                // Ignore the _ in sprite name
                if (_sprite.name[0] == _character)
                {
                    quest.Add(_sprite);
                }
            }
        }
    }

    public void ActivateQuest(int questID)
    {
        currentQuest.Clear();

        switch (questID)
        {
            case 1:
                for (int i = 0; i < quest1.Count; i++)
                {
                    currentQuest.Enqueue(quest1[i]);
                }
                break;

            case 2:
                for (int i = 0; i < quest2.Count; i++)
                {
                    currentQuest.Enqueue(quest2[i]);
                }
                break;

            case 3:
                for (int i = 0; i < quest3.Count; i++)
                {
                    currentQuest.Enqueue(quest3[i]);
                }
                break;

            case 4:
                for (int i = 0; i < quest4.Count; i++)
                {
                    currentQuest.Enqueue(quest4[i]);
                }
                break;

            case 5:
                for (int i = 0; i < quest5.Count; i++)
                {
                    currentQuest.Enqueue(quest5[i]);
                }
                break;
        }

        currentQuestProgress = currentQuest.Count;
        Debug.Log("Loaded Quest " + questID + ". Number of steps: " + currentQuestProgress);
    }

    public void TrackingQuest(bool value)
    {
        isChecking = value;
    }

    public void LoadNextQuestCollectible()
    {
        if (currentQuest.Count != 0)
        {
            _questMaster.sprite = currentQuest.Dequeue();
        }
    }

    public void AddCollectedCharacter(string character)
    {
        characterCollected += character;
    }

    public void ClearCollectedCharacters()
    {
        characterCollected = null;
    }

    public bool IsCurrentQuestFinished()
    {
        if (currentQuestProgress == 0)
        {
            TrackingQuest(false);
            Debug.Log("No more quest collectible to spawn");
        }

        return currentQuestProgress == 0;
    }

    public void ProgressQuest()
    {
        if (currentQuestProgress > 0)
        {
            currentQuestProgress--;
        }
    }

    public void AdvanceToNextQuest()
    {
        if (currentQuestID < 5)
        {
            currentQuestID++;
        }
    }
}
