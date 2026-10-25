
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
    
    //Lerping
    
    [SerializeField] private float duration = 1f;
    private float elapsedTime;
    private bool isParkouring;

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

        if(!isParkouring)
        {
            Movement(moveVector);
        }
        
        Looking(mouseDelta);

        if (!isParkouring && interactInput.WasPressedThisFrame())
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
            // See if the collider hit has iinteractable interface in it (is the object interactable)
            if (hitInfo.collider.TryGetComponent<IInteractable> (out var interactable))
            {
                // log statement to check if the raycast works
                Debug.Log(interactable.ReturnName() + " is interactable.");
                // Creating a variable parkourObject to store and use methods from interactable object
                IInteractable parkourObject = hitInfo.collider.GetComponentInParent<IInteractable>();
                // Creating a vector3 variable to hold the top of the objects values
                Vector3 parkourDestination = parkourObject.ObjectTop();
                // log statement to check if the vector3
                Debug.Log("Object top: " + parkourDestination);

                // teleporting the player on top of the object, offsetting due to the teleport being in the center of the player
                float playerOffset = transform.position.y - characterController.bounds.min.y;
                // transform.position = parkourDestination + Vector3.up * playerOffset;

                parkourDestination = parkourDestination + Vector3.up * playerOffset;
                // Get player current position
                Vector3 currentPosition = transform.position;
                StartCoroutine(MoveParkour(parkourDestination, currentPosition));

                
            }
        }
        else
        {
            Debug.Log("Hit nothing");
        }
    }

    private IEnumerator MoveParkour(Vector3 finalDestination, Vector3 currentPosition)
    {
        // make it so player cannot do other actions
        isParkouring = true;

        //resets elapsed time
        elapsedTime = 0f;

        float percentageComplete = 0f;
        // loop to lerp
        while (percentageComplete < 1)
        {
            //update percent complete frame rate independent
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / duration;

            // do lerp
            transform.position = Vector3.Lerp(currentPosition, finalDestination, percentageComplete);
            yield return null;
        }
        // turn off parkour
        isParkouring = false ;

    }
}
