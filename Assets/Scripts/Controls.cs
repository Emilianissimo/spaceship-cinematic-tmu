using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 0f;  
    [SerializeField] private float sprintSpeed = 30f;    

    [SerializeField] private float rotationSpeed = 50f;  

    [SerializeField] private float acceleration = 3f;     
    [SerializeField] private float deceleration = 4f;  

    private PlayerInputActions inputActions;
    private Vector2 inputVector = Vector2.zero;

    private float currentForwardSpeed = 0f;
    private Vector3 currentRotationVelocity = Vector3.zero;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.controls.Enable();
    }

    private void OnDisable()
    {
        inputActions.controls.Disable();
    }

    private void Update()
    {
        inputVector = inputActions.controls.move.ReadValue<Vector2>();

        bool isSprinting = inputActions.controls.sprint.ReadValue<float>() > 0.5f;
        float targetMaxSpeed = isSprinting ? sprintSpeed : walkSpeed;

        currentForwardSpeed = Mathf.Lerp(currentForwardSpeed, targetMaxSpeed, acceleration * Time.deltaTime);

        Vector3 targetRotation = new Vector3(-inputVector.y, inputVector.x, 0f) * rotationSpeed;

        float currentRate = (targetRotation.sqrMagnitude > 0f) ? acceleration : deceleration;
        
        currentRotationVelocity = Vector3.Lerp(currentRotationVelocity, targetRotation, currentRate * Time.deltaTime);

        
        transform.Rotate(currentRotationVelocity * Time.deltaTime, Space.Self);

        transform.Translate(Vector3.forward * currentForwardSpeed * Time.deltaTime, Space.Self);
    }
}