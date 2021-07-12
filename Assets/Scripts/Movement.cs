using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Movement : MonoBehaviour
{
    private float _speedJump = 6;
    private float _speedMove = 7;
    private float _speedRotateJoystickX = 100;
    private float _speedRotateJoystickY = 100;
    private float _speedRotateMouseX = 200;
    private float _speedRotateMouseY = 5;
    private float _pushPower = 2;
    private float _gravity = 20;
    
    private float _initialScale;
    private float _xRotation = 0f;
    private Vector3 _moveDirection = Vector3.zero;
    public bool ignoreInputs = false;

    private bool _isCrouching = false;
    private bool _isJumping = false;
    private bool _isWalking = false;
    private bool _isRunning = false;
    private bool _isSneaking = false;

    public Joystick joystickMove;
    public Joystick joystickRotate;
    public GameObject playerBody;
    public Transform cameraObj;
    public GameObject canvas;

    void Start()
    {
        _initialScale = gameObject.transform.localScale.y;
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        canvas.SetActive(false);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (!ignoreInputs)
        {
            Move();
            Rotate();
        }
    }

    private void Move()
    {
        // receive inputs
        float hInput = joystickMove.Horizontal * _speedMove;
        float vInput = joystickMove.Vertical * _speedMove;

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.A))
            hInput = -.5f;
        if (Input.GetKey(KeyCode.D))
            hInput = .5f;
        if (Input.GetKey(KeyCode.W))
            vInput = .5f;
        if (Input.GetKey(KeyCode.S))
            vInput = -.5f;
        if (Input.GetKey("left shift"))
        {
            vInput *= 2f;
            hInput *= 2f;
        }
#endif

        // with joystick player could walk & run
        playerBody.GetComponent<Animator>().SetFloat(Animator.StringToHash("Velocity"), vInput);

        // process jump and gravity
        if (GetComponent<CharacterController>().isGrounded)
        {
            _moveDirection = new Vector3(hInput, 0, vInput);
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= _speedMove;

            if (_isJumping)
            {
                _isJumping = false;
                playerBody.GetComponent<Animator>().SetBool("isJumping", false);
            }
            if (Input.GetButtonDown("Jump"))
            {
                _moveDirection.y = _speedJump;
                _isJumping = true;
                playerBody.GetComponent<Animator>().SetBool("isJumping", true);
            }
        }
        _moveDirection.y -= _gravity * Time.deltaTime;
        GetComponent<CharacterController>().Move(_moveDirection * Time.deltaTime);

        // process crouch
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_isCrouching)
            {
                // uncrouch only if there is nothing above or it is far
                Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit);
                if(hit.collider == null || hit.distance > 2)
                {
                    _isCrouching = false;
                    playerBody.GetComponent<Animator>().Play("crouch", -1, 1);
                }
            }
            else
            {
                _isCrouching = true;
                //playerBody.GetComponent<Animator>().SetBool("isCrouching", true);
            }
        }
        if (_isCrouching)
        {
            GetComponent<CharacterController>().height = 0.7f;
            //gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
            //transform.position = new Vector3(transform.position.x, transform.position.y - 0.01f, transform.position.z);
        }
        else
        {
            GetComponent<CharacterController>().height = 1.8f;
            //gameObject.transform.localScale = new Vector3(1, 1f, 1);
            //transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        }
    }

    private void Rotate()
    {
        float hInput = joystickRotate.Horizontal * _speedRotateJoystickX;
        float vInput = joystickRotate.Vertical * _speedRotateJoystickY;

#if UNITY_EDITOR
        hInput = Input.GetAxis("Mouse X") * _speedRotateMouseX;
        vInput = Input.GetAxis("Mouse Y") * _speedRotateMouseY;
#endif

        _xRotation -= vInput;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        cameraObj.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        gameObject.transform.Rotate(Vector3.up * hInput * Time.deltaTime);
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * _pushPower;
    }
}
