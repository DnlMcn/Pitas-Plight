using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public List<TurnTaker> turns;
    private int currentTurn = 0;

    EventBinding<EndTurn> endTurnEventBinding;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void OnEnable()
    {
        endTurnEventBinding = new EventBinding<EndTurn>(EndTurn);
        EventBus<EndTurn>.Register(endTurnEventBinding);
    }

    void OnDisable()
    {
        EventBus<EndTurn>.Deregister(endTurnEventBinding);
    }

    void Start()
    {
        if (turns.Count == 0)
        {
            print("There are no turns set up!");
        }
        else
        {
            // turns.Sort((a, b) => b.initiative.CompareTo(a.initiative));
            turns[0].StartTurn();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventBus<EndTurn>.Raise(new());
        }
    }

    public void EndTurn(EndTurn endTurnEvent)
    {
        print("End Turn Event Received!");

        currentTurn += 1;
        int turnsIndex = currentTurn % turns.Count;

        print("Current turn: " + currentTurn);
        print("Turn index: " + turnsIndex);

        turns[turnsIndex].StartTurn();
    }

    // public void AddToList(TurnTaker turnTaker)
    // {
    //     turns.Add(turnTaker);
    // }

    public void RemoveFromList(TurnTaker turnTaker)
    {
        int index = turns.IndexOf(turnTaker);
        if (index == -1) return;

        if (index <= currentTurn % turns.Count)
        {
            currentTurn--;
        }

        turns.Remove(turnTaker);
    }
}
