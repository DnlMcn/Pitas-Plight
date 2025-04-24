using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnTaker : MonoBehaviour
{
    public UnityEvent startTurnResponse;
    public GameEvent endTurn;
    public int initiative;

    public void StartTurn()
    {
        startTurnResponse.Invoke();
    }

    public void EndTurn()
    {
        endTurn.Raise();
    }

    void OnEnable()
    {
        TurnManager.instance.AddToList(this);
    }

    void OnDisable()
    {
        TurnManager.instance.RemoveFromList(this);
    }
}
