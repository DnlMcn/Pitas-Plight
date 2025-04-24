using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance { get; private set; }

    public List<TurnTaker> turns;
    private int currentTurn = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        if (turns.Count == 0)
        {
            print("There are no turns set up!");
        }

        turns.Sort((a, b) => b.initiative.CompareTo(a.initiative));
        turns[0].StartTurn();
    }

    public void NextTurn()
    {
        currentTurn += 1;

        print("Current turn: " + currentTurn);

        if (currentTurn > turns.Count)
        {
            currentTurn = 0;
        }

        turns[currentTurn].StartTurn();
    }

    public void AddToList(TurnTaker turnTaker)
    {
        turns.Add(turnTaker);
    }

    public void RemoveFromList(TurnTaker turnTaker)
    {
        turns.Remove(turnTaker);
    }
}
