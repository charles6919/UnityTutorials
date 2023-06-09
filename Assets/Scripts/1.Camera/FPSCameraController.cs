using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCameraController : MonoBehaviour
{
    public float SpeedMove;     //카메라 이동 속도
    public float SpeedRotate;   //카메라 회전 속도
    public float SpeedZoom;     //카메라 확대/축소 속도

    public float Min_Fov;   //카메라 Field Of View 최소값
    public float Max_Fov;   //카메라 Field Of View 최대값
    
    private Camera _camera;
    private float _rotX = 0f; //카메라 x축 회전값
    private float _rotY = 0f; //카메라 y축 회전값
    private bool _isBoost = false;
    
    //초기화용 값
    private Vector3 _originPos;
    private Quaternion _originRot;
    private float _originFOV;


    private void Init()
    {
        //초기화
        transform.position = _originPos;
        transform.localRotation = _originRot;
        if (_camera) _camera.fieldOfView = _originFOV;

        _rotX = transform.localRotation.eulerAngles.x;
        _rotY = transform.localRotation.eulerAngles.y;
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
        
        transform.position += (transform.right * moveX + transform.up * moveY + transform.forward * moveZ).normalized * (speed * Time.deltaTime);
    }

    private void Rotate()
    {
        //회전
        if (!Input.GetMouseButton(1)) return;
        
        var moveX = Input.GetAxis("Mouse X") * Time.deltaTime;
        var moveY = -Input.GetAxis("Mouse Y") * Time.deltaTime;

        _rotX = Mathf.Clamp(_rotX + moveY * SpeedRotate, -85f, 85f);
        _rotY = _rotY + moveX * SpeedRotate;

        transform.localRotation = Quaternion.Euler(_rotX, _rotY, 0f);
    }

    private void Zoom()
    {
        //확대 및 축소
        if (_camera == null) return;
        
        var zoom = Input.GetAxis("Mouse ScrollWheel");

        var resultFOV = _camera.fieldOfView + zoom * SpeedZoom;

        if (resultFOV > Max_Fov)
        {
            _camera.fieldOfView = Max_Fov;
        }
        else if (resultFOV < Min_Fov)
        {
            _camera.fieldOfView = Min_Fov;
        }
        else
        {
            _camera.fieldOfView = resultFOV;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();

        _originPos = transform.position;
        _originRot = transform.localRotation;
        if (_camera) _originFOV = _camera.fieldOfView;
        
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
        Zoom();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Init();
        }
    }
}
