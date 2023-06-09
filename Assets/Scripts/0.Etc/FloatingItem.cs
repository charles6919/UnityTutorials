using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    public GameObject Object;
    public float RotSpeed = 100f;
    public float _floatSize = 5f;
    
    private float _timer = 0f;



    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Floating()
    {
        var height = Mathf.Sin(_timer) * _floatSize;
        Object.transform.localPosition =
            new Vector3(Object.transform.localPosition.x, height, Object.transform.localPosition.z);
    }

    private void Rotating()
    {
        Object.transform.localEulerAngles += new Vector3(0f, RotSpeed * Time.deltaTime, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= Mathf.PI * 2f)
        {
            _timer -= Mathf.PI * 2f;
        }
        
        Floating();
        Rotating();
    }
}
