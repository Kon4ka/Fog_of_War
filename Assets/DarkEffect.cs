using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SonarEffect : MonoBehaviour
{
    public KeyCode activationKey = KeyCode.Space;
    public PostProcessVolume sonarVolume;
    private bool isSonarActive = false;

    void Start()
    {
        if (sonarVolume != null)
        {
            sonarVolume.enabled = false;
        }
        else
        {
            Debug.LogError("Sonar Volume is not assigned.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            isSonarActive = !isSonarActive;
            sonarVolume.enabled = isSonarActive;
        }
    }
}