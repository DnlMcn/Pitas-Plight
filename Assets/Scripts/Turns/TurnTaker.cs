using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnTaker : MonoBehaviour
{
    public UnityEvent startTurnResponse;
    // public GameEvent endTurn;
    public int initiative;

    public void StartTurn()
    {
        if (this.CompareTag("Player"))
        {
            EventBus<UpdateActivePlayer>.Raise(new UpdateActivePlayer { gameObject = this.gameObject });
        }

        startTurnResponse.Invoke();
    }

    public void EndTurn()
    {
        EventBus<EndTurn>.Raise(new());
    }

    // void OnEnable()
    // {
    //     TurnManager.Instance.AddToList(this);
    // }

    // void OnDisable()
    // {
    //     TurnManager.Instance.RemoveFromList(this);
    // }
}
