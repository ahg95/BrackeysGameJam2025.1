using Fusion;
using UnityEngine;

public struct BallInputData : INetworkInput
{
    public Vector2 Movement;
    // Track whether the jump key is currently held down
    public bool JumpHeld;
    // Track whether the jump key was released in this frame
    public bool JumpReleased;
}