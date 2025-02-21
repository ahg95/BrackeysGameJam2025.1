using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager Instance;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        // If no instance exists, assign this one and persist across scenes.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Try to get an existing AudioSource, or add one if it doesn't exist.
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            // Destroy duplicate instances.
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Static method to play an AudioClip at the specified volume.
    /// </summary>
    /// <param name="clip">The AudioClip to play.</param>
    /// <param name="volume">The volume at which to play the clip (default is 1.0f).</param>
    public static void PlayClip(AudioClip clip, float volume = 1.0f)
    {
        if (!Instance)
        {
            Debug.LogError("AudioManager instance is null. Make sure an AudioManager object is in the scene.");
            return;
        }
        
        Instance.Play(clip, volume);
    }

    /// <summary>
    /// Instance method that actually plays the AudioClip.
    /// </summary>
    /// <param name="clip">The AudioClip to play.</param>
    /// <param name="volume">The volume at which to play the clip.</param>
    private void Play(AudioClip clip, float volume)
    {
        if (!clip)
        {
            Debug.LogWarning("AudioManager: Tried to play a null AudioClip.");
            return;
        }
        
        audioSource.PlayOneShot(clip, volume);
    }
}