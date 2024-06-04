using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScript : MonoBehaviour
{
    [SerializeField] private GameObject rune;
    [SerializeField] private Material activateMaterial;
    [SerializeField] private Material unActivateMaterial;
    private Coroutine crystalMoving;

    private bool isActivate = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Объект с тегом в триггере: " + other.gameObject.tag);
        if (other.gameObject.CompareTag("Crystal"))
        {
            isActivate = !isActivate;
            other.isTrigger = true;
            if (other.gameObject.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.isKinematic = true;
            }
            if (rune.gameObject.TryGetComponent<Renderer>(out var runeRenderer))
            {
                runeRenderer.material = isActivate ? activateMaterial : unActivateMaterial;
            }
            crystalMoving = StartCoroutine(MoveCrystal(other.gameObject));
        }
    }

    //private void OnTriggerExit(Collider other)
    //{   
    //    if (other.gameObject.CompareTag("Crystal"))
    //    {
    //        StopCoroutine(crystalMoving);
    //        isActivate = !isActivate;
    //        Debug.Log("Шарик вне зоны руны");
    //        if (other.gameObject.TryGetComponent<Rigidbody>(out var rb))
    //        {
    //            rb.isKinematic = false;
    //        }
            
    //        if (rune.gameObject.TryGetComponent<Renderer>(out var runeRenderer))
    //        {
    //            runeRenderer.material = isActivate ? activateMaterial : unActivateMaterial;
    //        }
    //    }
        
    //}

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
    }


}
