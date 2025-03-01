using UnityEngine;

namespace _1_Code.Interaction
{
    /// <summary>
    /// Keeps track of the currently selected plane, if any.
    /// </summary>
    public class SelectionManager : MonoBehaviour
    {
        public static SelectionManager Instance { get; private set; }

        /// <summary>
        /// The currently selected plane (or null if none).
        /// </summary>
        public Plane SelectedPlane { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        
        public void DeselectPlane()
        {
            ClearSelection();
            Debug.Log("Plane deselection: selection cleared.");
        }

        /// <summary>
        /// Selects a plane and deselects any previously selected one.
        /// </summary>
        /// <param name="plane">The plane to select.</param>
        public void SelectPlane(Plane plane)
        {
            SelectedPlane = plane;
            Debug.Log("Plane selected: " + plane.gameObject.name);
        }

        /// <summary>
        /// Clears selection (no plane selected).
        /// </summary>
        private void ClearSelection()
        {
            SelectedPlane = null;
        }
    }
}