using UnityEngine;
using Wrld;
using Wrld.Space;

public class MyPlaneController : MonoBehaviour
{
    public float ForwardInput, speed = 10;
    public float SprintMultiplier = 2;

    public float CamFollowSpeed = 5, CamRotateSpeed = 5;
    public float RollSpeed = 2, m_Roll, RollInput, RollRecoverSpeed = 2;
    public float PitchSpeed = 2, m_Pitch, PitchRecoverSpeed = 2;
    public float AltitudeInput, AltitudeSpeed = 2;
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
    Vector3 targetpositionVctr;
    LatLongAltitude targetposition;

    Vector3 BodyRotVctr;
    Vector3 YawRot;
    Quaternion FinalRot;
    Quaternion BodyRot;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        Cam = Camera.main;
        WrldMap wrld = GameObject.FindObjectOfType<WrldMap>();
        Api.Instance.CameraApi.SetCustomRenderCamera(Cam);
        Api.Instance.CameraApi.WorldToGeographicPoint(transform.position);
        transform.position = targetpositionVctr = Api.Instance.CameraApi.GeographicToWorldPoint(new LatLongAltitude((float)wrld.LatitudeDegrees,(float)wrld.LongitudeDegrees
            , Api.Instance.CameraApi.WorldToGeographicPoint(transform.position).GetAltitude()));
    }
    void FixedUpdate()
    {
        Inputs();

        m_Yaw = YawInput * YawSpeed;
        
        m_Rigidbody.position = transform.position + (Vector3.up * (AltitudeInput * AltitudeSpeed * Time.fixedDeltaTime));

        targetpositionVctr = transform.forward * Time.fixedDeltaTime * ((speed * ForwardInput) + (((SprintMultiplier - 1) * speed) * (Input.GetButton("Jump") ? 1f : 0f)));
        targetposition = Api.Instance.CameraApi.WorldToGeographicPoint(targetpositionVctr);
        Api.Instance.SetOriginPoint(targetposition);
        
        YawRot = Vector3.up * Time.fixedDeltaTime * m_Yaw;

        BodyRotVctr = Vector3.zero;
        BodyRotVctr.Set ((ForwardInput * PitchAngle) , BodyRotVctr.y, -(YawInput * RollAngle));

        FinalRot = Quaternion.Euler(m_Rigidbody.rotation.eulerAngles + YawRot);
        BodyRot = Quaternion.Euler(BodyRotVctr);

        Body.localRotation = Quaternion.Slerp(Body.rotation,BodyRot,Time.fixedDeltaTime * RollSpeed);
        m_Rigidbody.rotation = FinalRot;

        Cam.transform.rotation = Quaternion.Slerp(Cam.transform.rotation, Quaternion.LookRotation(transform.forward), Time.fixedDeltaTime * CamRotateSpeed);
        Cam.transform.position = Vector3.Lerp(Cam.transform.position, CamTarget.position, Time.fixedDeltaTime * CamFollowSpeed);
        
    }
    void Inputs()
    {
        YawInput = (Input.GetAxis("Horizontal"));
        ForwardInput = (Input.GetAxis("Vertical"));
        AltitudeInput = (Input.GetAxis("Altitude"));
    }
}
