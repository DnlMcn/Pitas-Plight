using UnityEngine;

public interface IEvent { }

public struct EndTurn : IEvent { }

public struct UpdateActivePlayer : IEvent {
    public GameObject gameObject;
}

// public struct PlayerEvent : IEvent {
//     public int health;
//     public int mana;
// }