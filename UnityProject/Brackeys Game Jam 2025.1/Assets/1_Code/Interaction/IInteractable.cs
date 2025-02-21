namespace _1_Code.Interaction
{
    /// <summary>
    /// Any object that can be clicked or interacted with should implement this interface.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Called by a central input handler when the object is clicked.
        /// </summary>
        void OnInteract();
    }
}