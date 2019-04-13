using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController charController;
    private PlayerState playerState;

    [SerializeField] private string horizontalInputName, verticalInputName;
    private float horizInput, vertInput;
    private Vector3 forwardMovement, rightMovement;

    [SerializeField] private float walkSpeed, runSpeed;
    [SerializeField] private float runBuildUpSpeed;
    [SerializeField] private float crouchBrake, backBrake;
    [SerializeField] private KeyCode runKey;

    private float movementSpeed, actualMovementSpeed;

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;
    
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;
    
    private bool isJumping;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        GetMoveInput();

        if (horizInput != 0 || vertInput != 0)
        {
            SetMovementSpeed();
        }
        PlayerMovement();
        JumpInput();
    }

    private void GetMoveInput()
    {
        horizInput = Input.GetAxis(horizontalInputName);
        vertInput = Input.GetAxis(verticalInputName);

        forwardMovement = transform.forward * vertInput;
        rightMovement = transform.right * Input.GetAxis(horizontalInputName);
    }

    private void PlayerMovement()
    {
        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * actualMovementSpeed);

        if ((vertInput != 0 || horizInput != 0) && OnSlope())
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);



    }

    private void SetMovementSpeed()
    {
        if (Input.GetKey(runKey))
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
        else
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);

        actualMovementSpeed = movementSpeed;
        if (playerState.isCrouching)
        {
            actualMovementSpeed *= crouchBrake;
        }
        if (vertInput<0)
        {
            actualMovementSpeed *= backBrake;
        }
    }


    private bool OnSlope()
    {
        if (isJumping)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
            {
                return true;
            }

        return false;
    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }


    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;
        float forceMultiplier = 1;
        //bukkend springen is half zo sterk.
        if (playerState.isCrouching)
        {
             forceMultiplier= .5f;
        }
        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * forceMultiplier * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        isJumping = false;
        }

}