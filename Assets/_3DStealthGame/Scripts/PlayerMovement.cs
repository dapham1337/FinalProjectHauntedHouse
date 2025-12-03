using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Animator m_Animator;
    public GameObject flashlight;

    public InputAction MoveAction;

    public float walkSpeed = 1.0f;
    public float sprintSpeed = 3.0f;    // ADDED
    public float turnSpeed = 20f;
    public float sprintMax = 3f;
    float currSprint = 3f;
    bool canSprint = true;

    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        MoveAction.Enable();
        m_Animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        var pos = MoveAction.ReadValue<Vector2>();

        float horizontal = pos.x;
        float vertical = pos.y;

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        m_Rigidbody.MoveRotation(m_Rotation);

        // speed go brrr MINOR CHANGE ps this is where Dinh work next!!
        float currentSpeed = walkSpeed;
        bool isSprinting = Keyboard.current.leftShiftKey.isPressed;
        
        // Turns flashlight on.
        flashlight.SetActive(Keyboard.current.spaceKey.isPressed);

        if(canSprint && isSprinting && currSprint > 0){
            currentSpeed = sprintSpeed;
            currSprint -= Time.deltaTime;
            currSprint = Mathf.Clamp(currSprint, 0, 3f);
            Debug.Log(currSprint);
        }else{
            canSprint = currSprint < 0.5f ? false : true;
            currSprint += Time.deltaTime / 2;
        }
        

        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * currentSpeed * Time.deltaTime);
    }
}
