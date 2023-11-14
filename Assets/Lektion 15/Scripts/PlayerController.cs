using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float jumpSpeed;

    private CharacterController cc;
    private Animator anim;
    private XRIDefaultInputActions customActions;

    private float ySpeed;
    private Vector3 lastDirection;
    private Vector3 mDirection;
    private float speedMagnitude;
    private bool isJump;

    private void Awake()
    {
        customActions = new XRIDefaultInputActions();
        customActions.Enable();
    }

    private void OnEnable()
    {
        customActions.Gameplay.Move.performed += inputValue => MoveInput(inputValue.ReadValue<Vector2>());
        customActions.Gameplay.Move.canceled += inputValue => MoveInputUp(inputValue.ReadValue<Vector2>());
        customActions.Gameplay.Move.started += inputValue => MoveInputDown(inputValue.ReadValue<Vector2>());
        //customActions.Gameplay.Jump.started += inputValue => JumpInput(inputValue.action.IsPressed());
        customActions.Gameplay.Interact.performed += inputValue => InteractInput(inputValue.action.IsPressed());
    }

    private void OnDisable()
    {
        customActions.Gameplay.Move.performed -= inputValue => MoveInput(inputValue.ReadValue<Vector2>());
        customActions.Gameplay.Move.canceled -= inputValue => MoveInputUp(inputValue.ReadValue<Vector2>());
        customActions.Gameplay.Move.started -= inputValue => MoveInputDown(inputValue.ReadValue<Vector2>());
        customActions.Gameplay.Jump.started -= inputValue => JumpInput(inputValue.action.IsPressed());
        customActions.Gameplay.Interact.performed -= inputValue => InteractInput(inputValue.action.IsPressed());
    }

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }


    private void MoveInput(Vector2 moveDirection)
    {
        // kod f�r att f�rflytta character

        mDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
    }

    private void MoveInputUp(Vector2 moveDirection)
    {
        mDirection = new Vector3(moveDirection.x, 0, moveDirection.y);

    }

    private void MoveInputDown(Vector2 moveDirection)
    {
        mDirection = new Vector3(moveDirection.x, 0, moveDirection.y);

    }

    private void JumpInput(bool isPressed)
    {
        // Kod f�r att hoppa
        isJump = isPressed;

    }

    private void InteractInput(bool isPressed)
    {
        // kod f�r att interagera ....
        Debug.Log("Interacting....");
    }

    private void Update()
    {
        speedMagnitude = Mathf.Clamp01(mDirection.magnitude) * moveSpeed;

        mDirection.Normalize();

        Vector3 velocity = mDirection * speedMagnitude;

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (cc.isGrounded && isJump)
        {
            ySpeed = jumpSpeed;
            isJump = false;
        }

        velocity.y = ySpeed;

        anim.SetFloat("Motion", speedMagnitude);

        cc.Move(velocity * Time.deltaTime);

        // Update last direction if a key is pressed
        if (mDirection != Vector3.zero)
        {
            lastDirection = mDirection;
        }

        Quaternion toRotation = Quaternion.LookRotation(lastDirection, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }
}
