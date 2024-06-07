using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FirstDoorOpenScript : MonoBehaviour
{
    [SerializeField] private GameObject door;
    private Coroutine rotateDoorCoroutine;
    private Quaternion originalDoorRotation;
    private bool isDoorOpen = false;

    private void Awake()
    {
        if (TryGetComponent(out XRGrabInteractable grabInteractable))
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
        originalDoorRotation = door.transform.rotation;
    }

    private void OnGrab(SelectEnterEventArgs arg0)
    {
        ToggleDoor();
    }

    private void OnRelease(SelectExitEventArgs arg1)
    {
        MakeKinematic(arg1.interactableObject);
        ToggleDoor();
    }

    private void ToggleDoor()
    {
        isDoorOpen = !isDoorOpen;
        if (rotateDoorCoroutine != null)
        {
            StopCoroutine(rotateDoorCoroutine);
        }
        rotateDoorCoroutine = StartCoroutine(RotateDoor());
    }

    private IEnumerator RotateDoor()
    {
        Quaternion targetRotation = isDoorOpen ? originalDoorRotation * Quaternion.Euler(0, 90, 0) : originalDoorRotation;
        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            door.transform.rotation = Quaternion.Lerp(door.transform.rotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        door.transform.rotation = targetRotation;
    }

    private void MakeKinematic(IXRSelectInteractable interactableObject)
    {
        if (interactableObject.transform.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
        }
    }
}
