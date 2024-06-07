using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FinalRuneColliders : MonoBehaviour
{

    private bool isSubRuneActivate = false;
    public bool IsSubRuneActivate
    {
        get { return isSubRuneActivate; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Crystal"))
        {
            return;
        }
        Debug.Log("Вошло");
        isSubRuneActivate = true;
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

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Вышло");

        if (!other.gameObject.CompareTag("Crystal"))
        {
            return;
        }
        isSubRuneActivate = false;
    }


    private IEnumerator MoveCrystal(GameObject crystal)
    {

        Vector3 originalPosition = crystal.transform.position;
        Vector3 targetPosition = transform.position;
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
    }
}
