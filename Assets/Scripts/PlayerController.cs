using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField] private float speed = 5f;
    [SerializeField] private float lookSensitivity = 20f;
    [SerializeField] private float thrusterforce = 1000f;

    [Header("Joint Options: ")] 
    [SerializeField] private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField] private float jointSpring = 20f;
    [SerializeField] private float jointMaxForce = 2f;


    private PlayerMotor motor;
    private ConfigurableJoint joint;

    private void Start() {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(jointSpring);
    }

    private void Update() {
        float _xMove = Input.GetAxisRaw("Horizontal");
        float _zMove = Input.GetAxisRaw("Vertical");     

        Vector3 moveHorizontal = transform.right * _xMove;
        Vector3 moveVertical = transform.forward * _zMove;

        Vector3 _velocity = (moveHorizontal + moveVertical).normalized * speed;

        motor.Move(_velocity);

        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        motor.Rotate(_rotation);
        
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _camerarotationX = _xRot * lookSensitivity;

        motor.RotateCamera(_camerarotationX);

        Vector3 _thrusterForce = Vector3.zero;

        if (Input.GetButton("Jump")) {
            _thrusterForce = Vector3.up * thrusterforce;
            SetJointSettings(0f);
        }
        else {
            SetJointSettings(jointSpring);
        }

        motor.ApplyThruster(_thrusterForce);
    }

    private void SetJointSettings(float _jointSpring) {
        joint.yDrive = new JointDrive {mode = jointMode, positionSpring = jointSpring, maximumForce = jointMaxForce};
    }
}
