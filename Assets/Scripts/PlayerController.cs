using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Player")]
    Rigidbody2D rb2d;
    BoxCollider2D boxCollider2D;
    SpringJoint2D joint2D;
    LineRenderer lineRenderer;

    [Header("Grappling Hook")]
    [SerializeField] float lineWidth = 0.1f;
    [SerializeField] float grapplingDistance = 0f;
    [SerializeField] Transform grappleGunStartPoint;
    [SerializeField] LayerMask isGrappable;
    [SerializeField] float distanceBetweenAtachedBodies;
    [SerializeField] float distanceAllowGrapple = 5f;
    Vector3 clickedWorldPoint;
    Vector2 grapplePoint;
    bool isGrapling = false;
    
    [Header("Movement")]
    [SerializeField] LayerMask isGround;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpForce;
    float moveInput;
    bool canMove = true;
    bool canJump = false;
    bool isRunning = false;
    bool isJumping = false;

    [Header("Dash")]
    [SerializeField] float dashDistance = 15f;
    [SerializeField] float dashCoolDown = 0.5f;
    [SerializeField] ParticleSystem dashEffect;
    bool isDashing = false;
    bool canDash = false;

    [Header("Wall Jump")]
    [SerializeField] LayerMask isWallJumpable;
    [SerializeField] float wallSlideSpeed = 15f;
    [SerializeField] float wallJumpForce = 15f;
    [SerializeField] float stopMovementWallJump = 0.3f;
    bool isWallSliding = false;

    [Header("Ghost Stalk")]
    [SerializeField] float registerPositionDelay = 0.01f;
    [SerializeField] float coolDownCollision = 1f;
    List<Vector3> playerPositions = new List<Vector3>();
    List<Vector3> playerScale = new List<Vector3>();
    List<bool> playerRunning = new List<bool>();
    List<bool> playerDashing = new List<bool>();
    List<bool> playerGrappling = new List<bool>();
    List<bool> playerJumping = new List<bool>();
    List<bool> playerSliding = new List<bool>();
    bool canCollide = true;

    CameraFade cameraFade;
    AudioPlayer audioPlayer;
    LevelManager levelManager;
    Animator animator;
    [SerializeField] float fadeDelayDeath = 1f;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        joint2D = GetComponent<SpringJoint2D>();
        animator = GetComponent<Animator>();

        cameraFade = FindObjectOfType<CameraFade>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    void Start()
    {
        joint2D.enabled = false;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        StartCoroutine(RegisterPlayerPosition());

        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }

        CheckIfIsWallSliding();
        CheckIfDashIsOver();
        CheckIfIsJumping();

        animator.SetBool("isDashing", isDashing);
        animator.SetBool("isGrapling", isGrapling);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            canJump = true;
        }

        if (Input.GetMouseButtonDown(1))
        {
            canDash = true;
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Run();
            FlipSprite();

            if (canJump)
            {
                Jump();
                canJump = false;
            }

            if (canDash && !isDashing)
            {
                StartCoroutine(Dash());
                audioPlayer.PlayDashClip();
                canDash = false;
            }
        }
    }

    void LateUpdate()
    {
        if (isGrapling)
        {
            DrawRope();
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x), 1f);
        }

    }

    IEnumerator RegisterPlayerPosition()
    {
        yield return new WaitForSeconds(registerPositionDelay);

        playerPositions.Add(transform.position);
        playerScale.Add(transform.localScale);
        playerRunning.Add(isRunning);
        playerDashing.Add(isDashing);
        playerGrappling.Add(isGrapling);
        playerJumping.Add(isJumping);
        playerSliding.Add(isWallSliding);
    }

    public List<Vector3> GetPlayerPositions()
    {
        return playerPositions;
    }

    public List<Vector3> GetPlayerScale()
    {
        return playerScale;
    }

    public List<bool> GetPlayerRunning()
    {
        return playerRunning;
    }

    public List<bool> GetPlayerDashing()
    {
        return playerDashing;
    }

    public List<bool> GetPlayerGrappling()
    {
        return playerGrappling;
    }

    public List<bool> GetPlayerJumping()
    {
        return playerJumping;
    }

    public List<bool> GetPlayerSliding()
    {
        return playerSliding;
    }

    void Run()
    {
        float xMovement = moveInput * runSpeed;
        bool isPlayerNotMoving = Mathf.Abs(xMovement) < Mathf.Epsilon;
        isRunning = !isPlayerNotMoving;

        animator.SetBool("isRunning", isRunning);

        Vector2 playerVelocity = new Vector2(xMovement, rb2d.velocity.y);

        if (isPlayerNotMoving && isGrapling)
        {
            playerVelocity.x = rb2d.velocity.x;
        }
        else if (isWallSliding)
        {
            playerVelocity.y = -wallSlideSpeed;
        }

        rb2d.velocity = playerVelocity;
    }

    void Jump()
    {

        bool isTouchingGround = boxCollider2D.IsTouchingLayers(isGround);

        if (!isDashing && (isTouchingGround || isGrapling))
        {
            rb2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            isGrapling = false;
            joint2D.enabled = false;
            lineRenderer.enabled = false;
            isJumping = true;
        }
        else if (isWallSliding)
        {
            float movementX = wallJumpForce * -moveInput;
            rb2d.AddForce(new Vector2(movementX, wallJumpForce), ForceMode2D.Impulse);
            StartCoroutine(StopMovementPlayer(stopMovementWallJump));
            isJumping = true;
        }

        audioPlayer.PlayJumpClip();

    }

    void CheckIfIsWallSliding()
    {
        bool isTouchingWall = boxCollider2D.IsTouchingLayers(isWallJumpable);
        bool isTouchingGround = boxCollider2D.IsTouchingLayers(isGround);
        bool hasHorizontalSpeed = Mathf.Abs(moveInput) > Mathf.Epsilon;

        if (isTouchingWall && !isTouchingGround && hasHorizontalSpeed)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

        animator.SetBool("isWallSliding", isWallSliding);
    }

    void CheckIfIsJumping()
    {
        bool isTouchingGround = boxCollider2D.IsTouchingLayers(isGround);
        bool isTouchingWall = boxCollider2D.IsTouchingLayers(isWallJumpable);

        if ((!isTouchingGround && !isTouchingWall) || isGrapling)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        animator.SetBool("isJumping", isJumping);
    }

    void CheckIfDashIsOver()
    {
        bool isTouchingGround = boxCollider2D.IsTouchingLayers(isGround);
        bool isTouchingWall = boxCollider2D.IsTouchingLayers(isWallJumpable);

        if (isTouchingGround || isGrapling || isTouchingWall)
        {
            isDashing = false;
        }
    }


    IEnumerator StopMovementPlayer(float stopTime)
    {
        canMove = false;
        yield return new WaitForSeconds(stopTime);
        canMove = true;
    }

    IEnumerator Dash()
    {
        isDashing = true;
        bool isPlayerNotMoving = Mathf.Abs(moveInput) < Mathf.Epsilon;

        rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);

        Vector2 dashForce;

        if (isPlayerNotMoving)
        {
            dashForce = new Vector2(dashDistance * 4f * Mathf.Sign(transform.localScale.x), 0f);
        }
        else
        {
            dashForce = new Vector2(dashDistance * moveInput, 0f);
        }

        rb2d.AddForce(dashForce, ForceMode2D.Impulse);

        PlayDashParticleEffect();

        float gravity = rb2d.gravityScale;
        rb2d.gravityScale = 0;
        canMove = false;

        yield return new WaitForSeconds(dashCoolDown);

        canMove = true;
        rb2d.gravityScale = gravity;
        //isDashing = false;
    }

    void PlayDashParticleEffect()
    {
        float direction = Mathf.Sign(transform.localScale.x);
        ParticleSystem instance = Instantiate(dashEffect, transform.position, Quaternion.Euler(0f, 0f, transform.rotation.z * direction), transform);
        Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
    }

    void StartGrapple()
    {
        clickedWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 difference = transform.position - clickedWorldPoint;
        difference.Normalize();

        RaycastHit2D hit = Physics2D.Raycast(clickedWorldPoint, difference, -grapplingDistance, isGrappable);

        if (hit.collider != null && CheckGrappleDistance(clickedWorldPoint))
        {
            isGrapling = true;
            grapplePoint = hit.point;
            ConfigureSpringJoint();
            joint2D.enabled = true;
            audioPlayer.PlayGrapplingClip();
        }
    }

    bool CheckGrappleDistance(Vector2 point)
    {
        float distance = Vector2.Distance(transform.position, point);

        if (distance > distanceAllowGrapple)
        {
            return false;
        }

        return true;
    }

    void ConfigureSpringJoint()
    {
        joint2D.autoConfigureConnectedAnchor = false;
        joint2D.autoConfigureDistance = false;
        joint2D.connectedAnchor = grapplePoint;
        joint2D.dampingRatio = 1f;
        joint2D.enableCollision = true;
        joint2D.breakForce = 5000f;

        float distanceFromPoint = Vector2.Distance(grappleGunStartPoint.position, grapplePoint);
        joint2D.distance = distanceFromPoint * distanceBetweenAtachedBodies;
    }

    private void DrawRope()
    {
        Vector2 currentGrapplePosition = Vector2.Lerp(grapplePoint, grappleGunStartPoint.position, Time.deltaTime * 8f);

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, grappleGunStartPoint.position);
        lineRenderer.SetPosition(1, currentGrapplePosition);

        lineRenderer.enabled = true;
    }

    void AddNewSpringJoint()
    {
        joint2D = gameObject.AddComponent(joint2D.GetType()) as SpringJoint2D;
        joint2D.enabled = false;
        joint2D.enableCollision = true;
        joint2D.breakForce = 5000f;
    }

    public void Die()
    {
        StartCoroutine(StopCollisionGhost());
        StartCoroutine(CameraFadeDeath());
        StartCoroutine(StopMovementPlayer(fadeDelayDeath));

        levelManager.AddToDeathCount();
    }

    void ResetPosition()
    {
        transform.position = playerPositions[0];
        playerPositions = new List<Vector3>();
        joint2D.enabled = false;
        lineRenderer.enabled = false;
        isGrapling = false;
        isDashing = false;
        rb2d.velocity = Vector2.zero;
    }

    public bool CanPlayerCollide()
    {
        return canCollide;
    }

    IEnumerator CameraFadeDeath()
    {
        cameraFade.FadeIn();

        yield return new WaitForSeconds(fadeDelayDeath);

        ResetPosition();
        
        cameraFade.FadeOut();
    }

    IEnumerator StopCollisionGhost()
    {
        canCollide = false;

        yield return new WaitForSeconds(coolDownCollision);

        canCollide = true;

    }

    void OnJointBreak2D(Joint2D brokenJoint)
    {
        lineRenderer.enabled = false;
        isGrapling = false;

        AddNewSpringJoint();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ghost" && canCollide)
        {
            Die();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Ghost" && canCollide)
        {
            Die();
        }
    }

}