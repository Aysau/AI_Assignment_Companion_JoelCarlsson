using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _groundDrag = 5f;
    [SerializeField] private float _airDrag = 2f;
    [SerializeField] private float _groundDist = 0.4f;

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;

    private Rigidbody _rb;
    private bool _isGrounded;
    private Vector2 _moveInput;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() //Physics update isnt every frame, only the frames where physics calculations are done
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDist, _groundLayer);

        Vector3 moveDirection = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        Vector3 targetHorizontalVelocity = moveDirection * _moveSpeed;


        _rb.linearVelocity = new Vector3(targetHorizontalVelocity.x, _rb.linearVelocity.y, targetHorizontalVelocity.z);
        _rb.linearDamping = _isGrounded ? _groundDrag : _airDrag;
    }

    public void OnMove(CallbackContext context) //Forward is the blue arrow and green arrow is up, y is vertical axis, z is forward axis
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(CallbackContext context)
    {
        if (context.performed && _isGrounded)
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

    }
}
