using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PortalColliderLogic : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject winnerRoom;
    private CharacterController characterController;
    private void Awake()
    {
        characterController = player.GetComponent<CharacterController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }
        Debug.Log("Было");
        // Перемещение игрока в позицию комнаты-победителя
        characterController.enabled = false; // Отключаем CharacterController перед перемещением
        player.transform.position = winnerRoom.transform.position;
        characterController.enabled = true; // Включаем CharacterController обратно
    }
}
