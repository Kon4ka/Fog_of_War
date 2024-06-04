using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Butoneer : MonoBehaviour
{

    [SerializeField] private InputActionReference gripAction;

    private float _gripValue;

    private void Awake()
    {
        //m_Text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        gripAction.action.performed += GetGripData;
    }

    private void GetGripData(InputAction.CallbackContext context)
    {
        _gripValue = context.ReadValue<float>();
        //if (_gripValue > 0.5f)
        //    m_Text.text = _gripValue.ToString();
    }


    private void OnDestroy()
    {
        gripAction.action.performed -= GetGripData;
    }
}