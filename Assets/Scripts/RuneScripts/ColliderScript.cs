using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class ColliderScript : MonoBehaviour
{
    [SerializeField] private GameObject rune;
    
    [SerializeField] private Material activateMaterial;
    [SerializeField] private Material unActivateMaterial;
    [SerializeField] private TextMeshPro timerText;
    [SerializeField] private float timerDuration;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject secondCrystal;

    private Quaternion originalDoorRotation;

    private bool isDoorOpen = false;
    private bool isRuneActivate = false;

    public bool IsRuneActivate
    {
        get { return isRuneActivate; }
    }

    private Coroutine crystalMoving;
    private Coroutine rotateDoorCoroutine;
    private Coroutine timerCoroutine;

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
        crystalMoving = StartCoroutine(MoveCrystal(other.gameObject));


    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Вышло" + isRuneActivate.ToString());

        if (!isRuneActivate || !other.gameObject.CompareTag("Crystal"))
        {
            return;
        }
        other.isTrigger = false;
        isRuneActivate = !isRuneActivate;
        ChangeRuneMaterial();
        StopCoroutine(crystalMoving);
        ToggleDoor();
        StopTimer();
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
        isDoorOpen = !isDoorOpen;
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
        ChangeRuneMaterial();
        ToggleDoor();
        timerCoroutine = StartCoroutine(StartTimer());
        
    }

    private void StopTimer()
    {
        timerText.enabled = false;
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private IEnumerator StartTimer()
    {
        float timeLeft = timerDuration;
        timerText.enabled = true;
        Camera main_camera = Camera.main;
        bool isEndMessageShowed = false;
        while (timeLeft > 0)
        {
            Debug.Log(main_camera.transform.position.ToString() + secondCrystal.transform.position.ToString() + door.transform.position.ToString());
            if (main_camera.transform.position.x < door.transform.position.x &&
                main_camera.transform.position.z < door.transform.position.z &&
                secondCrystal.transform.position.x < door.transform.position.x &&
                secondCrystal.transform.position.z < door.transform.position.z &&
                !isEndMessageShowed)
            {
                isEndMessageShowed = !isEndMessageShowed;
                timerText.text = "Вы успели!";
                ToggleDoor();
                yield return new WaitForSeconds(3);
            }
            else if (isEndMessageShowed)
            {
                timerText.enabled = false;
                yield break;
            }
            timeLeft -= Time.deltaTime;
            string seconds = timeLeft.ToString("00");
            timerText.text = seconds;
            yield return null;
        }
        timerText.text = "Вы не успели";
        ToggleDoor();
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
