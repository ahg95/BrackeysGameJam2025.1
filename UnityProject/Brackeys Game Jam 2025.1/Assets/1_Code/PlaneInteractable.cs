using UnityEngine;

namespace _1_Code
{
    /// <summary>
    /// Attach this to a Plane GameObject. 
    /// It implements how the plane should behave when the user clicks it.
    /// </summary>
    [RequireComponent(typeof(Plane))]
    public class PlaneInteractable : MonoBehaviour, IInteractable
    {
        private Plane _plane;

        private void Awake()
        {
            // Get the Plane component on the same GameObject.
            _plane = GetComponent<Plane>();
        }

        public void OnInteract()
        {
            // Selecting this plane in the SelectionManager.
            SelectionManager.Instance.SelectPlane(_plane);
        }
    }
}