using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [SerializeField] private float maxSpeed_km_per_h = 1f;
    [SerializeField] private float acceleration_km_per_h2 = 1f;
    [SerializeField] private float brake_km_per_h2 = 1f;
    [SerializeField] private float control_degree = 5f;
    [SerializeField] private TimerController timer;

    [SerializeField] private Button left;
    [SerializeField] private Button right;

    private bool leftPovorot = false;
    private bool rightPovorot = false;

    private bool braking = false;
    private float currentSpeed = 0;
    private float currentControl;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (currentSpeed <= maxSpeed_km_per_h && !braking)
        {
            currentSpeed += Time.deltaTime* acceleration_km_per_h2;
        }
        if (currentSpeed> maxSpeed_km_per_h)
        {
            currentSpeed = maxSpeed_km_per_h;
        }

        rb.MovePosition(transform.position + transform.right * currentSpeed/100); 

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(transform.up, control_degree);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(transform.up, -control_degree);
        }
        if (Input.GetKey(KeyCode.S)&&currentSpeed> maxSpeed_km_per_h * 0.4f)
        {
            currentSpeed -= Time.deltaTime * brake_km_per_h2;
            braking = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            braking = false;
        }

        if (leftPovorot ) transform.Rotate(transform.up, -control_degree);
        if (rightPovorot) transform.Rotate(transform.up, control_degree);
        if (braking && currentSpeed > maxSpeed_km_per_h * 0.4f) currentSpeed -= Time.deltaTime * brake_km_per_h2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Start"))
        {
            timer.StartLap();
        }

        if (other.tag.Equals("Finish"))
        {
            timer.EndLap();
        }
    }

    
    public void LeftStart()
    {
        leftPovorot = true;
    }

    public void LeftEnd()
    {
        leftPovorot = false;
    }

    public void RightStart()
    {
        rightPovorot = true;
    }
    public void RightEnd()
    {
        rightPovorot = false;
    }

    public void BrakingStart()
    {
        braking = true;
    }
    public void brakingEnd()
    {
        braking = false;
    }


}
