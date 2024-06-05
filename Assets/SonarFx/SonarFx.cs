using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class SonarFx : MonoBehaviour
{
    // Sonar mode (directional or spherical)
    public enum SonarMode { Directional, Spherical }
    [SerializeField] SonarMode _mode = SonarMode.Spherical; // Default to Spherical
    public SonarMode mode { get { return _mode; } set { _mode = value; } }


    // Wave direction (used only in the directional mode)
    [SerializeField] Vector3 _direction = Vector3.forward;
    public Vector3 direction { get { return _direction; } set { _direction = value; } }

    // Base color (albedo)
    [SerializeField] Color _baseColor = new Color(0.2f, 0.2f, 0.2f, 0);
    public Color baseColor { get { return _baseColor; } set { _baseColor = value; } }

    // Wave color
    [SerializeField] Color _waveColor = new Color(1.0f, 0.2f, 0.2f, 0);
    public Color waveColor { get { return _waveColor; } set { _waveColor = value; } }

    // Wave color amplitude
    [SerializeField] float _waveAmplitude = 2.0f;
    public float waveAmplitude { get { return _waveAmplitude; } set { _waveAmplitude = value; } }

    // Exponent for wave color
    [SerializeField] float _waveExponent = 22.0f;
    public float waveExponent { get { return _waveExponent; } set { _waveExponent = value; } }

    // Interval between waves
    [SerializeField] float _waveInterval = 20.0f;
    public float waveInterval { get { return _waveInterval; } set { _waveInterval = value; } }

    // Wave speed
    [SerializeField] float _waveSpeed = 10.0f;
    public float waveSpeed { get { return _waveSpeed; } set { _waveSpeed = value; } }

    // Additional color (emission)
    [SerializeField] Color _addColor = Color.black;
    public Color addColor { get { return _addColor; } set { _addColor = value; } }

    // Reference to the shader.
    [SerializeField] Shader shader;

    // Private shader variables
    int baseColorID;
    int waveColorID;
    int waveParamsID;
    int waveVectorID;
    int addColorID;

    private bool sonarActive = false;
    private float sonarStartTime;

    void Awake()
    {
        baseColorID = Shader.PropertyToID("_SonarBaseColor");
        waveColorID = Shader.PropertyToID("_SonarWaveColor");
        waveParamsID = Shader.PropertyToID("_SonarWaveParams");
        waveVectorID = Shader.PropertyToID("_SonarWaveVector");
        addColorID = Shader.PropertyToID("_SonarAddColor");
    }


    [SerializeField] private InputActionReference gripAction;
 

    private float _gripValue;


    private void Start()
    {
        gripAction.action.performed += GetGripData;
    }

    private void GetGripData(InputAction.CallbackContext context)
    {
        Debug.Log("Было в нажатии");
        sonarActive = !sonarActive;
        if (sonarActive)
        {
            GetComponent<Camera>().SetReplacementShader(shader, null);
        }
        else
        {
            GetComponent<Camera>().ResetReplacementShader();
        }
    }



    void Update()
    {
        if (sonarActive)
        {
            Debug.Log("Было в апдейте");
            Vector3 playerPosition = transform.position;
            Shader.SetGlobalVector(waveVectorID, playerPosition);
            Shader.SetGlobalColor(baseColorID, _baseColor);
            Shader.SetGlobalColor(waveColorID, _waveColor);
            Shader.SetGlobalColor(addColorID, _addColor);

            var param = new Vector4(_waveAmplitude, _waveExponent, _waveInterval, _waveSpeed);
            Shader.SetGlobalVector(waveParamsID, param);
            Shader.EnableKeyword("SONAR_SPHERICAL");
        }
        else
        {
            Shader.SetGlobalColor(baseColorID, Color.black);
            Shader.SetGlobalColor(waveColorID, Color.black);
            Shader.SetGlobalColor(addColorID, Color.black);
            Shader.DisableKeyword("SONAR_SPHERICAL");
        }
    }
    private void OnDestroy()
    {
        gripAction.action.performed -= GetGripData;
    }
}

