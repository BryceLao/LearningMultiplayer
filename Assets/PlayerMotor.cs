using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    [SerializeField] private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 camerarotation = Vector3.zero;

    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 Velocity) {
        velocity = Velocity;
    }

    public void Rotate(Vector3 Rotation) {
        rotation = Rotation;
    }

    public void RotateCamera(Vector3 Camerarotation) {
        camerarotation = Camerarotation;
    }

    void FixedUpdate() {
        PerformMovement();
        PerformRotation();
    }

    void PerformMovement() {
        if (velocity != Vector3.zero) {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    void PerformRotation() {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null) {
            cam.transform.Rotate(-camerarotation);
        }
    }

}
