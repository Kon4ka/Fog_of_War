
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ForceKinematic : MonoBehaviour
{
    private void Awake()
    {
        if (TryGetComponent(out XRGrabInteractable grabInteractable))
        {
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }


    private void OnRelease(SelectExitEventArgs arg1)
    {
        if (arg1.interactableObject.transform.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
        }
    }
}
