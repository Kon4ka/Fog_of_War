using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SecondDoorOpenScript : MonoBehaviour
{
    [SerializeField] private GameObject rune;
    [SerializeField] private ColliderScript previousRune;
    [SerializeField] private Material activateMaterial;
    [SerializeField] private Material unActivateMaterial;
    [SerializeField] private TextMeshPro supportText;
    [SerializeField] private GameObject door;

    private Quaternion originalDoorRotation;
    private bool isDoorOpen = false;
    private bool isRuneActivate = false;

    private Coroutine rotateDoorCoroutine;

    private void Awake()
    {
        originalDoorRotation = door.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRuneActivate || !other.gameObject.CompareTag("Crystal"))
        {
            return;
        }
        Debug.Log("Вошло");
        Debug.Log(isRuneActivate.ToString());
        isRuneActivate = !isRuneActivate;
        other.isTrigger = true;
        if (other.gameObject.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
        }
        if (other.gameObject.TryGetComponent<XRGrabInteractable>(out var grabInteractable))
        {
            grabInteractable.enabled = false;
        }
        StartCoroutine(MoveCrystal(other.gameObject));
    }

    private void OnTriggerStay(Collider other)
    {
        if (previousRune.IsRuneActivate)
        {
            if (supportText.enabled)
            {
                supportText.enabled = false;
            }
            if (!isDoorOpen)
            {
                isDoorOpen = true;
                ToggleDoor();
            }

        }
        else
        {
            if (!supportText.enabled)
            {
                supportText.enabled = true;
            }
            if (isDoorOpen)
            {
                isDoorOpen = false;
                ToggleDoor();
            }
            supportText.text = "Первая руна должна быть активирована!";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Вышло");

        if (!isRuneActivate || !other.gameObject.CompareTag("Crystal"))
        {
            return;
        }
        supportText.enabled = false;
        other.isTrigger = false;
        isRuneActivate = !isRuneActivate;
        ChangeRuneMaterial();
        if (isDoorOpen)
        {
            isDoorOpen = false;
            ToggleDoor();
        }
    }

    private void ChangeRuneMaterial()
    {
        if (rune.TryGetComponent<Renderer>(out var runeRenderer))
        {
            runeRenderer.material = isRuneActivate ? activateMaterial : unActivateMaterial;
        }
    }

    private void ToggleDoor()
    {
        if (rotateDoorCoroutine != null)
        {
            StopCoroutine(rotateDoorCoroutine);
        }
        rotateDoorCoroutine = StartCoroutine(RotateDoor());
    }

    private IEnumerator MoveCrystal(GameObject crystal)
    {

        Vector3 originalPosition = crystal.transform.position;
        Vector3 targetPosition = rune.transform.position;
        float duration = 1.5f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            crystal.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (crystal.TryGetComponent<XRGrabInteractable>(out var grabInteractable))
        {
            grabInteractable.enabled = true;
        }
        Debug.Log("Корутина завершена");
        ChangeRuneMaterial();
    }

    private IEnumerator RotateDoor()
    {
        Quaternion targetRotation = isDoorOpen ? originalDoorRotation * Quaternion.Euler(0, -90, 0) : originalDoorRotation;
        float duration = 3f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            door.transform.rotation = Quaternion.Lerp(door.transform.rotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
