using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_AgentAnimationScript : MonoBehaviour
{
    public float speed = 4;
    public float rotSpeed = 80;
    float rot = 0;
    public float gravity = 8;
    //public Text velocityText;
    //public Text angularVelocityText;

    Vector3 moveDir = Vector3.zero;
    Vector3 lastPosition;
    Vector3 lastRotation;

    CharacterController controller;
    Animator animator;

    float velocity = 0;
    float getVelocity()
    {
        return velocity;
    }
    float angularVelocity = 0;
    float getAngularVelocity()
    {
        return angularVelocity;
    }

    void Start()
    {
        //controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
        lastPosition = transform.position;
        lastRotation = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        ////if(controller.isGrounded)
        ////{
        //if (Input.GetKey(KeyCode.W))
        //{
        //    moveDir = new Vector3(0, 0, 1);
        //    moveDir *= speed;
        //    moveDir = transform.TransformDirection(moveDir);
        //}
        //if (Input.GetKeyUp(KeyCode.W))
        //{
        //    moveDir = new Vector3(0, 0, 0);
        //}
        ////}

        //rot += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        //transform.eulerAngles = new Vector3(0, rot, 0);
        //moveDir.y -= gravity *= Time.deltaTime;
        //controller.Move(moveDir * Time.deltaTime);

        velocity = (transform.position - lastPosition).magnitude / Time.deltaTime;
        animator.SetFloat("velocity", velocity);

        angularVelocity = (transform.rotation.eulerAngles.y - lastRotation.y) / Time.deltaTime;
        float min = -90;
        float max = 90;
        float scaledAngularVelocity = (angularVelocity - min) / (max - min);
        animator.SetFloat("turn", scaledAngularVelocity);

        lastPosition = transform.position;
        lastRotation = transform.rotation.eulerAngles;
    }
}