using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnTaker : MonoBehaviour
{
    public UnityEvent startTurnResponse;
    public int initiative;

    public void StartTurn()
    {
        if (this.CompareTag("Player"))
        {
            print("Updating active player");
            EventBus<UpdateActivePlayer>.Raise(new UpdateActivePlayer { gameObject = this.gameObject });
        }

        print("Starting turn response for " + this.gameObject.name);
        startTurnResponse.Invoke();
    }

    public void EndTurn()
    {
        print("Ending turn for " + this.gameObject.name);
        EventBus<EndTurn>.Raise(new());
    }

    // void OnEnable()
    // {
    //     TurnManager.Instance.AddToList(this);
    // }

    void OnDisable()
    {
        TurnManager.Instance.RemoveFromList(this);
    }
}
