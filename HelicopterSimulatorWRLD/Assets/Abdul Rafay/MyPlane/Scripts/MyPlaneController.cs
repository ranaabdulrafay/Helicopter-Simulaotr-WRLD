using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wrld;
using Wrld.Common.Maths;
using Wrld.Space;
using Wrld.Space.Positioners;

public class MyPlaneController : MonoBehaviour
{
    public float ForwardInput,speed = 10;
    public float SprintMultiplier = 2;

    public float CamFollowSpeed = 5, CamRotateSpeed = 5;
    public float RollSpeed = 2, m_Roll, RollInput, RollRecoverSpeed = 2;
    public float PitchSpeed = 2, m_Pitch, PitchRecoverSpeed = 2;
    public float AltitudeInput, AltitudeSpeed = 2;
    //public float /*ForwarSpeed = 2, m_Forward,*/ ForwardInput/*, ForwardRecoverSpeed = 2*/;
    public float YawSpeed = 2, m_Yaw, YawInput, YawRecoverSpeed = 2;

    public float OverAllRotationSpeed = 2;
    public Rigidbody m_Rigidbody;

    public Transform CamTarget;
    public Camera Cam;
    public float Up, Back;

    [Header("Rotation Limits")]
    public float RollAngle = 80;
    public float PitchAngle = 90;

    [Header("Body")]
    public Transform Body;

    public AiSpawnerOnBuilding spawner;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        Cam = Camera.main;
    }

    // Update is called once per frame
    //void Update()
    //{
    void FixedUpdate()
    {
        Inputs();
        m_Roll = RollInput * RollSpeed;
        m_Pitch = ForwardInput * PitchSpeed;
        m_Yaw = YawInput * YawSpeed;
        //m_Forward = ForwardInput * YawSpeed;

        m_Rigidbody.velocity = transform.forward * Time.fixedDeltaTime * ((speed * ForwardInput) + (((SprintMultiplier - 1) * speed) * (Input.GetButton("Jump") ? 1f : 0f)));
        m_Rigidbody.position = transform.position+(Vector3.up * (AltitudeInput* AltitudeSpeed * Time.fixedDeltaTime));
        
        Vector3 PitchRot = -Vector3.right * Time.fixedDeltaTime * m_Pitch;
        //Vector3 ForwardRot = -Vector3.right * Time.fixedDeltaTime * m_Forward;
        Vector3 RollRot = Vector3.forward * Time.fixedDeltaTime * m_Roll;
        Vector3 YawRot = Vector3.up * Time.fixedDeltaTime * m_Yaw;

        Quaternion FinalRot = Quaternion.Euler(m_Rigidbody.rotation.eulerAngles + YawRot);
        Quaternion BodyRot = Quaternion.Euler(Body.rotation.eulerAngles + RollRot - PitchRot);

        Body.rotation = BodyRot;
        m_Rigidbody.rotation = FinalRot;

        // body z  Roll recovery
        if (Mathf.Approximately(RollInput, 0))
            Body.rotation = Quaternion.Euler(
                Body.rotation.eulerAngles.x,
                Body.rotation.eulerAngles.y,
                Mathf.MoveTowards(Body.rotation.eulerAngles.z, Body.rotation.eulerAngles.z > 180 ? 360 : 0, Time.fixedDeltaTime * RollRecoverSpeed));
        // body x forward recovery
        if (Mathf.Approximately(ForwardInput, 0))
            Body.rotation = Quaternion.Euler(
                Mathf.MoveTowards(Body.rotation.eulerAngles.x, Body.rotation.eulerAngles.x > 180 ? 360 : 0, Time.fixedDeltaTime * PitchRecoverSpeed),
                Body.rotation.eulerAngles.y,
                Body.rotation.eulerAngles.z
                );

        Body.rotation = Quaternion.Euler(
            ClampAngle(Body.rotation.eulerAngles.x, -PitchAngle, PitchAngle),
            Body.rotation.eulerAngles.y,
            ClampAngle(Body.rotation.eulerAngles.z, -RollAngle, RollAngle));

        /*
        m_Rigidbody.rotation = Quaternion.Euler(
            //ClampAngle(m_Rigidbody.rotation.eulerAngles.x, -PitchAngle, PitchAngle),
             m_Rigidbody.rotation.eulerAngles.x,
            m_Rigidbody.rotation.eulerAngles.y,
            m_Rigidbody.rotation.eulerAngles.z
            //ClampAngle(m_Rigidbody.rotation.eulerAngles.z, -RollAngle, RollAngle)
            );
        */

        //}
        //void FixedUpdate()
        //{
        Cam.transform.rotation = Quaternion.Slerp(Cam.transform.rotation, Quaternion.LookRotation(transform.forward), Time.fixedDeltaTime * CamRotateSpeed);
        Cam.transform.position = Vector3.Lerp(Cam.transform.position, CamTarget.position, Time.fixedDeltaTime * CamFollowSpeed);


        spawner.SetPosition(m_Rigidbody.position);

    }
    void Inputs()
    {
        YawInput = (Input.GetAxis("Horizontal"));
        RollInput = (-Input.GetAxis("Horizontal"));

        ForwardInput = (Input.GetAxis("Vertical"));
        AltitudeInput = (Input.GetAxis("Altitude"));

        //YawInput = (CnInputManager.GetAxis("Horizontal"));
        //RollInput = (-CnInputManager.GetAxis("Horizontal"));
        //PitchInput = (CnInputManager.GetAxis("Vertical"));
    }

    float ClampAngle(float angle, float from, float to)
    {
        // accepts e.g. -80, 80
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }

}
