using UnityEngine;

public class CharcterController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 720f;
    [SerializeField] float groundRadius = 0.5f;
    [SerializeField] Vector3 groundOffset;
    [SerializeField] LayerMask groundMask;

    float h;
    float v;
    bool isGrounded;
    float ySpeed;

    Quaternion targetRotation;
    CameraController cameraController;
    Animator animator;
    CharacterController characterController;

    bool isAttacking = false; // Track if the character is attacking

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        GroundCheck();

        if (!isAttacking)
        {
            HandleMovement();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            Attack();
        }
    }

    void HandleMovement()
    {
        if (isGrounded)
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));
        var moveInput = new Vector3(h, 0, v).normalized;
        var moveDir = cameraController.planerRotation * moveInput;

        var velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundOffset), groundRadius, groundMask);
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
    }

    // This method should be called by an animation event at the end of the attack animation
    public void EndAttack()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundOffset), groundRadius);
    }
}
