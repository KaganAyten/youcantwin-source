using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class MovementController : MonoBehaviour
{

    private int maxSpeed = 10;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask hookLayer;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteCounter;
    [SerializeField] private float dashCooldown = 1;

    [SerializeField] private float hookRadius = 2;
    [SerializeField] private float fallMultiplier = 1.3f;

    [SerializeField] private SpriteRenderer dashSprite;
    


    private Rigidbody2D rb;
    private CircleCollider2D coll;
    private DistanceJoint2D djoint;
    private LineRenderer lr;

    private bool isSwinging = false; 
    private bool canDash = true; 
    private bool canDoubleJump = false;


    private float moveHorizontal;
    private bool jumpInput;
    private bool dashInput;
    private bool clickInput;

    private float jumpCooldown = 0.3f;
    private bool canClickJump=true;

    private WaitForSeconds dashWait;
    private WaitForSeconds jumpWait;

    private Collider2D closestHook = null;
      

    public bool IsMobile()
    {
        if (Application.isMobilePlatform)
        {
            return true;
        }
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        djoint = GetComponent<DistanceJoint2D>();
        lr = GetComponent<LineRenderer>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hookRadius);
    }
    private void Update()
    {
        CoyoteTime();
        GetInput();
        RotatebyInput();
        BetterFall();
       
        if (jumpInput)
        {
            if (coyoteCounter > 0 && canClickJump)
            {
                StartCoroutine(JumpCooldown());
                Jump();
            }
            else if(canDoubleJump)
            {
                DoubleJump();
            }
        }
        if (dashInput && canDash)
        {
            Dash();
        }
        if (clickInput)
        {
            DoHook();
        }
        else
        {
            if (isSwinging)
            {
                OnHookEnd();
            }
        }
        if (isSwinging)
        {
            DrawHookLine();
        } 
    }
    private void DrawHookLine()
    { 
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, closestHook.transform.position);
    }
    private void BetterFall()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
    }
    private void RotatebyInput()
    {
        if (moveHorizontal > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveHorizontal < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
    private void DoHook()
    {
        if (isSwinging) return; 
        closestHook = null;
        Collider2D[] hooks = Physics2D.OverlapCircleAll(transform.position, hookRadius,hookLayer);
        float dist = 10000;
        foreach(Collider2D hook in hooks)
        {
            float distance = Vector2.Distance(transform.position, hook.transform.position);
            if (distance <= dist)
            {
                dist = distance;
                closestHook = hook;
            }
        }
        if (closestHook != null)
        {
            isSwinging = true;
            djoint.enabled = true;
            djoint.connectedAnchor = (Vector2)closestHook.transform.position;
        } 
    }
    private void OnHookEnd()
    { 
        isSwinging = false;
        djoint.enabled = false;
        djoint.connectedAnchor = Vector2.zero;
        lr.positionCount = 0;
    }
    private void CoyoteTime()
    {
        if (IsGrounded())
        {
            coyoteCounter = coyoteTime; 
        }
        else
        { 
            coyoteCounter -= Time.deltaTime;
        }
    }
    private void GetInput()
    {
        if (IsMobile()) return;
        moveHorizontal = Input.GetAxis("Horizontal");
        jumpInput = Input.GetKeyDown(KeyCode.Space);
        dashInput = Input.GetKeyDown(KeyCode.LeftShift);
        clickInput = Input.GetMouseButton(0);
    }

    private void FixedUpdate()
    {
        if (isSwinging) return;
        Vector2 movement = new Vector2(moveHorizontal * (speed / maxSpeed), rb.velocity.y);
        rb.velocity = movement;
    }
    private void Dash()
    {
        canDash = false;
        DoDashFX();
        Vector3 pos = transform.position + (transform.right * 0.3f);
        rb.DOMoveX(pos.x, 0.2f);
        dashSprite.DOFade(0, 0.5f);
        StartCoroutine(OnDashEnd());
    }
    private void DoDashFX()
    {
        dashSprite.transform.position = transform.position;
        dashSprite.transform.rotation = transform.rotation;
        dashSprite.DOFade(1, 0);
    }
    private void Jump()
    {
        canDoubleJump = true;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        coyoteCounter = 0;
    }
    private void DoubleJump()
    {
        canDoubleJump = false;
        Vector3 rot = transform.rotation.eulerAngles + new Vector3(0, 0, 360);
        transform.DORotate(rot, 0.2f, RotateMode.FastBeyond360);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(coll.bounds.center, Vector2.down, coll.bounds.extents.y, groundLayer);
        if (hit.collider)
        {
            return true;
        }
        return false;
    } 
     
    private IEnumerator OnDashEnd()
    {
        if (dashWait == null)
        {
            dashWait = new WaitForSeconds(dashCooldown);
        }
        yield return dashWait;
        canDash = true;
    }
    private IEnumerator JumpCooldown()
    {
        canClickJump = false;
        if (jumpWait == null)
        {
            jumpWait = new WaitForSeconds(jumpCooldown);
        }
        yield return jumpWait;
        canClickJump = true;
    }
    public void SetHorizontal(float val)
    {
        moveHorizontal = val;
    }
    public void SetJump(bool status)
    {
        jumpInput = status;
    }
    public void SetDash(bool status)
    {
        dashInput = status;
    }
    public void SetClick(bool status)
    {
        clickInput = status;
    }
}