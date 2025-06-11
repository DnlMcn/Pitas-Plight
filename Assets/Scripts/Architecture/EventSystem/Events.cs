using UnityEngine;

public static class Events
{
    public struct EndTurn : IEvent { }

    public struct UpdateActivePlayer : IEvent
    {
        public GameObject gameObject;
    }

    public struct PlayerDied : IEvent { }
}