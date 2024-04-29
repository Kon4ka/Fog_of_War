using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    private TMP_Text text;

    [SerializeField] private InputActionReference gripAction;
    [SerializeField] private InputActionReference triggetAction;

    private float _grip;
    private float _trigger;


    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        gripAction.action.performed += GetGrip;
    }

    private void GetGrip(InputAction.CallbackContext ctx)
    {
        _grip = ctx.ReadValue<float>();
        if (_grip >= 0.5)
        {
            text.text = _grip.ToString();
        }
    }

    private void OnDestroy()
    {
        gripAction.action.performed -= GetGrip;
    }

}
