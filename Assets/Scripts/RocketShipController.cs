using UnityEngine;

internal enum RocketRotationState
{
    Left,
    Right,
    Still
}

internal enum RocketMovementState
{
    Up,
    Still
}

public class RocketShipController : MonoBehaviour
{
    public float torque = 2f;
    public float thrust = 2f;
    public float speedThreshold = 0.25f;
    public float angleRange = 10f;
    public SpriteRenderer fireRenderer;

    private Rigidbody2D rb;
    private SpriteRenderer rocketRenderer;
    private RocketMovementState rocketMovementState = RocketMovementState.Still;
    private RocketRotationState rocketRotationState = RocketRotationState.Still;
    private bool isCollisionGood = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rocketRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Rotate();
        Accelerate();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rocketMovementState = RocketMovementState.Up;
        }
        else
        {
            rocketMovementState = RocketMovementState.Still;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rocketRotationState = RocketRotationState.Left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rocketRotationState = RocketRotationState.Right;
        }
        else
        {
            rocketRotationState = RocketRotationState.Still;
        }
    }

    private void Rotate()
    {
        switch (rocketRotationState)
        {
            case RocketRotationState.Left:
                rb.AddTorque(torque);
                break;

            case RocketRotationState.Right:
                rb.AddTorque(-torque);
                break;

            default:
                break;
        }
    }

    private void Accelerate()
    {
        switch (rocketMovementState)
        {
            case RocketMovementState.Up:
                rb.AddForce(transform.up * thrust, ForceMode2D.Force);
                fireRenderer.enabled = true;
                break;

            default:
                fireRenderer.enabled = false;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCollisionGood = collision.gameObject.layer == LayerMask.NameToLayer("LandingPad");
    }

    private void OnTriggerExit(Collider other)
    {
        isCollisionGood = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float speed = collision.relativeVelocity.sqrMagnitude;
        float angle = transform.localEulerAngles.z;
        Debug.Log("speed: " + speed + " z: " + angle);

        bool isLandingPadCollision = collision.gameObject.layer == LayerMask.NameToLayer("LandingPad");
        bool isSpeedInRange = speed <= speedThreshold;
        bool isAngleInRange = angle > 360 - angleRange || angle < angleRange;

        if (isCollisionGood && isLandingPadCollision && isSpeedInRange && isAngleInRange)
        {
            Debug.Log("Landed");
        }
        else
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        //rocketRenderer.enabled = false;
    }
}