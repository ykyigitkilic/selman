using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ZeroGMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float verticalSpeed = 8f;
    [SerializeField] private float rotationSpeed = 10f; // How fast the player turns

    private CharacterController _controller;
    private Animator _anim;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>(); 
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        Vector3 inputDir = Vector3.zero;

        // Collect Input
        if (keyboard.wKey.isPressed) inputDir += transform.forward;
        if (keyboard.sKey.isPressed) inputDir -= transform.forward;
        if (keyboard.aKey.isPressed) inputDir -= transform.right;
        if (keyboard.dKey.isPressed) inputDir += transform.right;

        // Calculate Vertical
        float vMove = 0;
        if (keyboard.spaceKey.isPressed) vMove = verticalSpeed;
        if (keyboard.leftShiftKey.isPressed) vMove = -verticalSpeed;

        // 1. HANDLE MOVEMENT
        Vector3 moveVector = (inputDir.normalized * moveSpeed);
        Vector3 finalVelocity = moveVector + (transform.up * vMove);
        _controller.Move(finalVelocity * Time.deltaTime);

        // 2. HANDLE ROTATION (The "Turning Around" Fix)
        // We only rotate if we are actually moving horizontally
        if (inputDir.sqrMagnitude > 0.01f)
        {
            // Calculate which way we should be looking
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            
            // Smoothly rotate toward that direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 3. UPDATE ANIMATOR
        if (_anim != null)
        {
            // Use the magnitude of movement to trigger your Run animation
            _anim.SetFloat("Speed", inputDir.magnitude * moveSpeed);
        }
    }
}