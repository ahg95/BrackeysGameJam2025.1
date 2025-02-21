// Parameterized GameEvent

using UnityEngine;

namespace _Scripts.Events_system.Event_types
{
    [CreateAssetMenu(menuName = "Events/Int Game Event")]
    public sealed class IntGameEvent : BaseGameEvent<int>
    {
    }
}