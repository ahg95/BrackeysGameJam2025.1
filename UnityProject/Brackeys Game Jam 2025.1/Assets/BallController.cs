using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class BallController : NetworkBehaviour
{
    [SerializeField] private NetworkRigidbody3D rb;

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

    }

}
