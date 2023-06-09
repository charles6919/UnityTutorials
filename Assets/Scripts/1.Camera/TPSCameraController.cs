using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCameraController : MonoBehaviour
{
    public GameObject Capsule;
    public GameObject Target;
    public GameObject CameraArm;
    public Camera TPSCamera;

    public float ArmLength = 35f;
    public float SpeedRot = 360f;
    public float SpeedMove = 100f;
    public float SpeedZoom = 50f;

    public float checkX;
    
    private float _xRot;
    private float _yRot;
    private float _armLengthMin = 10f;
    private float _armLengthMax = 100f;
    private bool _isBoost = false;

    private Vector3 _originPos;
    private Quaternion _originRot;
    private float _originArmLength;
    
    

    private void Init()
    {
        CameraArm.transform.localPosition = _originPos;
        CameraArm.transform.localRotation = _originRot;
        ArmLength = _originArmLength;
        _xRot = 0f;
        _yRot = 0f;
    }
    
    private void CameraUpdate()
    {
        Ray ray = new Ray(CameraArm.transform.position, -CameraArm.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, ArmLength))
        {
            TPSCamera.transform.position = hit.point;
        }
        else
        {
            TPSCamera.transform.position = CameraArm.transform.position + (-CameraArm.transform.forward * ArmLength);
        }
    }

    private void Rotate()
    {
        if (!Input.GetMouseButton(1)) return;

        var xMove = Input.GetAxis("Mouse X");
        var yMove = -Input.GetAxis("Mouse Y");

        _xRot += yMove * SpeedRot * Time.deltaTime;
        _yRot += xMove * SpeedRot * Time.deltaTime;

        _xRot = Mathf.Clamp(_xRot, -85f, 85f);

        CameraArm.transform.localRotation = Quaternion.Euler(_xRot, _yRot, 0f);
    }

    private void Move()
    {
        //이동
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isBoost = true;
        }
        else
        {
            _isBoost = false;
        }
        
        var speed = _isBoost ? SpeedMove * 2f : SpeedMove;

        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("UpDown");
        var moveZ = Input.GetAxis("Vertical");

        checkX = moveX;

        if (Target)
        {
            Target.transform.position += (CameraArm.transform.right * moveX + CameraArm.transform.up * moveY + CameraArm.transform.forward * moveZ).normalized * (speed * Time.deltaTime);
        }
        else 
        {
            CameraArm.transform.position += (CameraArm.transform.right * moveX + CameraArm.transform.up * moveY + CameraArm.transform.forward * moveZ).normalized * (speed * Time.deltaTime);
        }
        
    }

    private void Zoom()
    {
        var zoom = Input.GetAxis("Mouse ScrollWheel") * SpeedZoom;

        ArmLength += zoom;
        ArmLength = Mathf.Clamp(ArmLength, _armLengthMin, _armLengthMax);
    }

    public void SetTarget(GameObject target)
    {
        Target = target;
        if (target != null)
        {
            CameraArm.transform.SetParent(Target.transform);
            CameraArm.transform.localPosition = Vector3.zero;
            CameraArm.transform.localScale = Vector3.one;
            CameraArm.transform.localRotation = Quaternion.identity;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _originPos = CameraArm.transform.localPosition;
        _originRot = CameraArm.transform.localRotation;
        _originArmLength = ArmLength;

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Move();
        Zoom();
        CameraUpdate();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Target)
            {
                SetTarget(null);
            }
            else
            {
                SetTarget(Capsule);
            }
        }
    }
}
