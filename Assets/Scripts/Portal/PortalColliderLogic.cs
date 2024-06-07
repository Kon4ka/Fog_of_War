using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PortalColliderLogic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }
        Debug.Log("Было");
        //other.isTrigger = true;
        //if (other.gameObject.TryGetComponent<Rigidbody>(out var rb))
        //{
        //    rb.isKinematic = true;
        //}
        //if (other.gameObject.TryGetComponent<XRGrabInteractable>(out var grabInteractable))
        //{
        //    grabInteractable.enabled = false;
        //}
    }
}
