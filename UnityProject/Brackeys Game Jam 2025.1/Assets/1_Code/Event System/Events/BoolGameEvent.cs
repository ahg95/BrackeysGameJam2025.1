// Parameterless GameEvent

using UnityEngine;

namespace _Scripts.Events_system.Event_types
{
    [CreateAssetMenu(menuName = "Events/Bool Game Event")]
    public sealed class BoolGameEvent : BaseGameEvent<bool>
    {
    }
}