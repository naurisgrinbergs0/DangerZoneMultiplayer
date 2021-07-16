using System;
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
    private float _pushPower = 1;//2;
    private float _gravity = 20;
    
    private float _initialScale;
    private float _xRotation = 0f;
    private Vector3 _moveDirection = Vector3.zero;
    public bool ignoreInputs = false;

    private bool _isCrouching = false;
    private bool _isJumping = false;

    public Joystick joystickMove;
    public Joystick joystickRotate;
    public GameObject playerBody;
    public Transform cameraObj;
    public GameObject canvas;

    private float velocityZ = 0;
    private float velocityX = 0;
    private float _acceleration = 2;
    private float _deceleration = 2;
    private float _maxRunVel = 1;
    private float _maxWalkVel = .5f;

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
        //vSideways = joystickMove.Horizontal * _speedMove;
        //vForward = joystickMove.Vertical * _speedMove;

#if UNITY_EDITOR
        CalculateVelocity();
#endif

        // with joystick player could walk & run
        playerBody.GetComponent<Animator>().SetFloat(Animator.StringToHash("vForward"), velocityZ);
        playerBody.GetComponent<Animator>().SetFloat(Animator.StringToHash("vSideways"), velocityX);

        // process jump and gravity
        if (GetComponent<CharacterController>().isGrounded)
        {
            _moveDirection = new Vector3(velocityX, 0, velocityZ);
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

    private void CalculateVelocity()
    {
        bool forwardPress = Input.GetKey("w");
        bool backwardPress = Input.GetKey("s");
        bool leftPress = Input.GetKey("a");
        bool rightPress = Input.GetKey("d");
        bool shiftPress = Input.GetKey("left shift");

        float maxVel = shiftPress ? _maxRunVel : _maxWalkVel;

        // accelerate
        if (forwardPress && velocityZ < maxVel)
            velocityZ += Time.deltaTime * _acceleration;
        if (backwardPress && velocityZ > -maxVel)
            velocityZ -= Time.deltaTime * _acceleration;
        if (leftPress && velocityX > -maxVel)
            velocityX -= Time.deltaTime * _acceleration;
        if (rightPress && velocityX < maxVel)
            velocityX += Time.deltaTime * _acceleration;
        
        // decelerate walk
        if(!forwardPress && velocityZ > 0)
            velocityZ = Mathf.Max(velocityZ - Time.deltaTime * _deceleration, 0);
        if (!backwardPress && velocityZ < 0)
            velocityZ = Mathf.Min(velocityZ + Time.deltaTime * _deceleration, 0);
        if (!rightPress && velocityX > 0)
            velocityX = Mathf.Max(velocityX - Time.deltaTime * _deceleration, 0);
        if (!leftPress && velocityX < 0)
            velocityX = Mathf.Min(velocityX + Time.deltaTime * _deceleration, 0);

        // decelerate run
        if (forwardPress && !shiftPress && velocityZ > _maxWalkVel)
            velocityZ = Mathf.Max(velocityZ - Time.deltaTime * _deceleration, _maxWalkVel);
        if (backwardPress && !shiftPress && velocityZ < -_maxWalkVel)
            velocityZ = Mathf.Min(velocityZ + Time.deltaTime * _deceleration, -_maxWalkVel);
        if (rightPress && !shiftPress && velocityX > _maxWalkVel)
            velocityX = Mathf.Max(velocityX - Time.deltaTime * _deceleration, _maxWalkVel);
        if (leftPress && !shiftPress && velocityX < -_maxWalkVel)
            velocityX = Mathf.Min(velocityX + Time.deltaTime * _deceleration, -_maxWalkVel);
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

        // Apply the push
        //body.velocity = pushDir * (body.mass * .1f);
        body.AddForce(pushDir * (body.mass * .1f), ForceMode.Impulse);
    }
}
