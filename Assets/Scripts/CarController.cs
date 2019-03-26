using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float brake = 1f;
    [SerializeField] private float control = 5f;
    [SerializeField] private TimerController timer;

    [SerializeField] private Button left;
    [SerializeField] private Button right;

    private bool leftPovorot = false;
    private bool rightPovorot = false;

    private bool braking = false;
    private float currentSpeed = 0;
    private float currentControl;
    
    void Update()
    {
        if (currentSpeed <= maxSpeed && !braking)
        {
            currentSpeed += Time.deltaTime*acceleration;
        }
        if (currentSpeed>maxSpeed)
        {
            currentSpeed = maxSpeed;
        }
        transform.position += transform.right * currentSpeed;

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(transform.up, control);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(transform.up, -control);
        }
        if (Input.GetKey(KeyCode.S)&&currentSpeed>maxSpeed*0.4f)
        {
            currentSpeed -= Time.deltaTime * brake;
            braking = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            braking = false;
        }

        if (leftPovorot ) transform.Rotate(transform.up, -control);
        if (rightPovorot) transform.Rotate(transform.up, control);
        if (braking && currentSpeed > maxSpeed * 0.4f) currentSpeed -= Time.deltaTime * brake;
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
