using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Events;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public List<TurnTaker> turns;
    private int totalTurns = 0;
    private int currentTurn = 0;

    bool gameOver = false;

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
        EventBus<EndTurn>.OnEvent += EndTurn;
        EventBus<PlayerDied>.OnEvent += GameOver;
    }

    void OnDisable()
    {
        EventBus<EndTurn>.OnEvent -= EndTurn;
        EventBus<PlayerDied>.OnEvent -= GameOver;
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

    void GameOver(PlayerDied playerDeathEvent)
    {
        
    }

    public void EndTurn(EndTurn endTurnEvent)
    {
        if (gameOver) return;

        print("End Turn Event Received!");

        totalTurns++;
        currentTurn++;

        // Wrap current turn back to 0
        currentTurn = currentTurn >= turns.Count ? 0 : currentTurn;

        print("Current turn: " + currentTurn);
        print("Turn takers: " + turns.Count);

        turns[currentTurn].StartTurn();
    }

    // public void AddToList(TurnTaker turnTaker)
    // {
    //     turns.Add(turnTaker);
    // }

    public void RemoveFromList(TurnTaker turnTaker)
    {
        int index = turns.IndexOf(turnTaker);
        if (index == -1) return;

        print("Removing from turns list: " + turnTaker.gameObject.name);
        turns.Remove(turnTaker);

        print("Current turn is " + currentTurn);
        print("Turn taker is #" + index + " in the turns list");

        if (index <= currentTurn)
        {
            print("Adjusting next turn to be #" + (currentTurn - 1));
            currentTurn--;
        }
    }
}
