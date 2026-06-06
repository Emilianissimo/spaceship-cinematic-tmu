using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CinemachineCameraToggler : MonoBehaviour
{
    [SerializeField] private InputActionReference toggleAction;

    [SerializeField] private CinemachineCamera[] cameras;

    private int currentCameraIndex = 0;

    private void OnEnable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.Enable();
            toggleAction.action.performed += OnToggleInput;
        }
    }

    private void OnDisable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.performed -= OnToggleInput;
        }
    }

    private void Start()
    {
        ResetCameraPriorities();
    }

    private void OnToggleInput(InputAction.CallbackContext context)
    {
        ToggleCamera();
    }

    public void ToggleCamera()
    {
        if (cameras == null || cameras.Length == 0) return;

        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

        ResetCameraPriorities();
    }

    private void ResetCameraPriorities()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] != null)
            {
                cameras[i].Priority.Enabled = true;
                cameras[i].Priority.Value = (i == currentCameraIndex) ? 20 : 10;
            }
        }
    }
}