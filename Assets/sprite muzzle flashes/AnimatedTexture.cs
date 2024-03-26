using UnityEngine;

public class AnimatedTexture : MonoBehaviour
{
    public float fps = 30.0f;
    public Texture2D[] frames;

    private int frameIndex;
    private MeshRenderer rendererMy;
    private bool isPlaying;

    void Start()
    {
        rendererMy = GetComponent<MeshRenderer>();
        // We're not starting the animation immediately
        // isPlaying = true;
        // InvokeRepeating("NextFrame", 1 / fps, 1 / fps);
    }

    // Method to start the animation
    public void Play()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            frameIndex = 0; // Start from the first frame
            InvokeRepeating("NextFrame", 0, 1 / fps); // Start from the beginning immediately
        }
    }

    // Method to stop the animation
    public void Stop()
    {
        isPlaying = false;
        CancelInvoke("NextFrame");
    }

    // Method to reset the animation to the first frame
    public void ResetAnimation()
    {
        frameIndex = 0;
        NextFrame();
    }

    void NextFrame()
    {
        if (frameIndex < frames.Length)
        {
            rendererMy.sharedMaterial.SetTexture("_MainTex", frames[frameIndex]);
            frameIndex++;
        }
        else
        {
            Stop(); // Stop the animation when it reaches the last frame
        }
    }
}
