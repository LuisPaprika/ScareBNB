using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -24f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchSpeed = 2.5f;

    [Header("Look")]
    [SerializeField] private float lookSensitivity = 0.1f;
    [SerializeField] private float maxLookAngle = 90f;
    [SerializeField] private bool lockCursor = true;

    [Header("Head Bobbing")]
    [SerializeField] private float bobbingSpeed = 14f;
    [SerializeField] private float bobbingAmount = 0.05f;
    [SerializeField] private float bobbingSpeedMultiplier = 2f; // For sprinting

    [Header("Audio")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private float stepDistance = 2f;

    private CharacterController controller;
    [field: SerializeField] public static InputSystem_Actions inputActions {get; private set;}
    private Vector3 velocity;
    private Vector2 moveInput;
    private float accumulatedDistance;
    private Vector3 lastPosition;
    private float cameraPitch;
    private bool isSprinting;
    private bool isCrouched;
    private bool allowAccumulateDistance = true;
    private float standingHeight;
    [field: SerializeField] public static FirstPersonController Instance {get; private set;}

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controller = GetComponent<CharacterController>();
        standingHeight = controller.height;

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main != null ? Camera.main.transform : null;
        }

        inputActions = new InputSystem_Actions();
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Crouch.performed += OnCrouch;
        inputActions.Player.Sprint.performed += OnSprintStarted;
        inputActions.Player.Sprint.canceled += OnSprintCanceled;

        ConversationController.OnConversationStart += () => SwitchActionMap(inputActions.UI, inputActions.Player);
        ConversationController.OnConversationEnd += () => SwitchActionMap(inputActions.Player, inputActions.UI);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnDestroy()
    {
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Crouch.performed -= OnCrouch;
        inputActions.Player.Sprint.performed -= OnSprintStarted;
        inputActions.Player.Sprint.canceled -= OnSprintCanceled;
        inputActions.Dispose();
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
        AccumulateDistance();
        HandleHeadBobbing();
    }

    private void HandleLook()
    {
        if (cameraTransform == null)
            return;

        Vector2 lookInput = inputActions.Player.Look.ReadValue<Vector2>();
        cameraPitch -= lookInput.y * lookSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);

        cameraTransform.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);
    }

    private void HandleMovement()
    {
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        float targetSpeed = isCrouched ? crouchSpeed : (isSprinting ? sprintSpeed : walkSpeed);

        Vector3 moveDir = transform.forward * moveInput.y + transform.right * moveInput.x;
        float dot = Vector3.Dot(cameraTransform.forward, moveDir);
        if (dot < 0.5f)
        {
            targetSpeed *= 0.7f;
        }

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * targetSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!controller.isGrounded)
            return;

        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
        isCrouched = !isCrouched;
        controller.height = isCrouched ? crouchHeight : standingHeight;
    }

    private void OnSprintStarted(InputAction.CallbackContext context)
    {
        isSprinting = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        isSprinting = false;
    }

    private void HandleHeadBobbing()
    {
        if (cameraTransform == null || !controller.isGrounded)
            return;

        bool isMoving = moveInput.magnitude > 0.1f;

        Vector3 targetPosition = new Vector3(0f, controller.height / 2f, 0f);

        if (isMoving)
        {
            float speedMultiplier = isSprinting ? bobbingSpeedMultiplier : 1f;
            float amountMultiplier = isCrouched ? 0.5f : 1f;

            float bobbingOffset = Mathf.Sin(Time.time * bobbingSpeed * speedMultiplier) * bobbingAmount * amountMultiplier;
            targetPosition.y += bobbingOffset;
        }

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, targetPosition, Time.deltaTime * 10f);
    }

    private void PlayFootstepSound()
    {
        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    private void AccumulateDistance()
    {
        if(!allowAccumulateDistance || !controller.isGrounded || moveInput.magnitude < 0.1f)
        {
            return;
        }

        Vector3 currentHorizontalPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 lastHorizontalPosition = new Vector3(lastPosition.x, 0f, lastPosition.z);
        float distanceThisFrame = Vector3.Distance(currentHorizontalPosition, lastHorizontalPosition);

        accumulatedDistance += distanceThisFrame;

        if (accumulatedDistance >= stepDistance)
        {
            PlayFootstepSound();
            accumulatedDistance = 0f;
        }

        lastPosition = transform.position;
    }

    private void SwitchActionMap(InputActionMap mapToEnable, InputActionMap mapToDisable)
    {
        mapToDisable.Disable();
        mapToEnable.Enable();
    }

    public void SetAllowAccumulateDistance(bool allow)
    {
        allowAccumulateDistance = allow;
    }
}