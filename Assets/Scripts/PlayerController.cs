
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    // variables
    [SerializeField] private float moveSpeed = 5f;



    [SerializeField] private float LookSensitivity = 0.2f;
    [SerializeField] private float LookAngleLimit = 90f;

    private Camera mainCamera;
    private CharacterController characterController;

    private InputAction moveInput;
    private InputAction interactInput;

    private Vector3 moveDirection = Vector3.zero;
    private float lookAngle = 0f;

    private void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveInput = InputSystem.actions.FindAction("Move");
        interactInput = InputSystem.actions.FindAction("Interact");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveVector = moveInput.ReadValue<Vector2>();
        Vector2 mouseDelta = new Vector2(Mouse.current.delta.x.ReadValue(), Mouse.current.delta.y.ReadValue());

        Movement(moveVector);
        Looking(mouseDelta);

        if (interactInput.WasPressedThisFrame())
        {
            Interact();
        }
    }

    private void Movement(Vector2 moveVector)
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float oldY = moveDirection.y;

        Vector2 newSpeed = new Vector2(moveVector.y * moveSpeed, moveVector.x * moveSpeed);

        moveDirection = (forward * newSpeed.x) + (right * newSpeed.y);
        moveDirection.y = oldY;

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void Looking(Vector2 mouseDelta)
    {
        lookAngle += -mouseDelta.y * LookSensitivity;
        lookAngle = Mathf.Clamp(lookAngle, -LookAngleLimit, LookAngleLimit);

        mainCamera.transform.localRotation = Quaternion.Euler(lookAngle, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseDelta.x * LookSensitivity, 0);
    }

    private void Interact()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider.TryGetComponent<IInteractable> (out var interactable))
            {
                Debug.Log(interactable.ReturnName() + " is interactable.");
            }
        }
        else
        {
            Debug.Log("Hit nothing");
        }
    }
}
