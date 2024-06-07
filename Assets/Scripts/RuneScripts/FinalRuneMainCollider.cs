using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FinalRuneMainCollider : MonoBehaviour
{
    [SerializeField] private List<FinalRuneColliders> listOfSubColliders;
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject thorns;
    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightContoller;

    private GameObject crystal;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("WhiteCrystal") &
            !other.gameObject.CompareTag("RedCrystal") &
            !other.gameObject.CompareTag("PurpleCrystal")
            )
        {
            return;
        }
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

        crystal = other.gameObject;
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
        StartCoroutine(CheckSubCollidersStatus());
    }

    private IEnumerator CheckSubCollidersStatus()
    {
        while (true)
        {
            bool allActivated = listOfSubColliders.All(subCollider => subCollider.IsSubRuneActivate);
            if (!allActivated)
            {
                yield return new WaitForSeconds(1);
            }
            else
            {
                break;
            }
        }
        EndOfTheGame();
    }

    private void EndOfTheGame()
    {
        switch (crystal.tag)
        {
            case "WhiteCrystal":
                Debug.Log("Win");
                break;
            case "RedCrystal":
                StartCoroutine(SendOutThorns());
                break;
            case "PurpleCrystal":
                Debug.Log("50/50");
                break;
            default:
                // Код для всех остальных случаев
                break;
        }
    }

    private IEnumerator SendOutThorns()
    {
        leftController.SetActive(false);
        rightContoller.SetActive(false);
        Vector3 originalPosition = thorns.transform.position;
        Vector3 targetPosition = thorns.transform.position + new Vector3(0, 1.5f, 0);
        float duration = 0.5f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            thorns.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
