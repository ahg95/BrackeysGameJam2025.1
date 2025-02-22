using Fusion;

public struct BallInputData : INetworkInput
{
    public float Horizontal;
    public float Vertical;
    // Track whether the jump key is currently held down
    public bool JumpHeld;
    // Track whether the jump key was released in this frame
    public bool JumpReleased;
}