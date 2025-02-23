using UnityEngine;

public class DeleteLater : MonoBehaviour
{
    [SerializeField] private float groundCheckDistance = 0.6f; // Distance for ground check
    [SerializeField] private LayerMask groundLayer; // Layers considered as ground

    private bool IsGrounded()
    {
        bool isHit = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        // Debug visualization
        Debug.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance,
            isHit ? Color.green : Color.red);
        return isHit;
    }

    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            Debug.Log("Grounded.");
        }
        else
        {
            Debug.Log("Not Grounded.");
        }
    }
}