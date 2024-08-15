using UnityEngine;
using UnityEngine.UI;

public class LipSync : MonoBehaviour
{
    public bool useSprites; // Use sprite swapping for lip sync
    public bool altSprites; // Toggle for alternative sprites
    public float volumeThreshold = 0.1f; // Adjust based on the audio's loudness
    public Sprite[] syncedSprites; // Array of sprites for the Image component
    public Sprite[] syncedAltSprites; // Alternative sprites for different lip sync styles
    public AudioSource audioDevice; // The audio source that drives the lip sync
    private Image image; // Reference to the Image component

    private void Start()
    {
        if (useSprites)
        {
            image = GetComponent<Image>(); // Get the Image component
        }
    }

    private void Update()
    {
        if (audioDevice.isPlaying && useSprites)
        {
            float volumeLevel = GetVolumeLevel();
            int currentFrame = Mathf.FloorToInt(volumeLevel * (syncedSprites.Length - 1));

            // Clamp currentFrame to ensure it's within bounds
            currentFrame = Mathf.Clamp(currentFrame, 0, syncedSprites.Length - 1);

            if (volumeLevel < volumeThreshold)
            {
                currentFrame = 0; // Set to the first frame when volume is below the threshold
            }

            if (image != null)
            {
                // Ensure we do not access out-of-bounds elements in syncedAltSprites
                Sprite[] currentSprites = altSprites ? syncedAltSprites : syncedSprites;
                
                if (currentSprites.Length > 0)
                {
                    image.sprite = currentSprites[currentFrame];
                }
            }
        }
    }

    private float GetVolumeLevel()
    {
        float[] samples = new float[256];
        audioDevice.GetOutputData(samples, 0);
        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }
        float average = sum / samples.Length;
        float volumeLevel = Mathf.Clamp01(average / volumeThreshold);
        return volumeLevel;
    }
}
