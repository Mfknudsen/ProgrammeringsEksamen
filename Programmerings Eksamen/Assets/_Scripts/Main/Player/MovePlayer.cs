#region Systems
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class MovePlayer : MonoBehaviour
{
    #region Public Data
    [Header("Public:")]
    public float moveSpeed = 1.0f;
    public float gravity = 9.81f;
    public float jumpForce = 1.0f;
    #endregion

    #region Private Data
    [Header("Private:")]
    Rigidbody2D RB;
    float HorizontalDir = 0;
    Vector2 MoveDir = Vector2.zero;
    Vector2 JumpDir = Vector2.zero;
    bool IsGrounded = true;
    #endregion

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();

        if (RB == null)
        {
            Debug.Log("The player didn´t have a Rigidbody2D and so one has been added to it!");
            RB = gameObject.AddComponent<Rigidbody2D>();
        }
        RB.gravityScale = 0;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        HorizontalDir = Input.GetAxis("Horizontal");

        MoveDir = RB.transform.right * HorizontalDir * moveSpeed * Time.deltaTime;

        MoveDir = new Vector2(RB.transform.position.x + MoveDir.x, RB.transform.position.y + MoveDir.y);

        RB.transform.position = Vector2.Lerp(RB.transform.position, MoveDir, 0.9f);
    }

    private void Jump()
    {
        if (IsGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                JumpDir = RB.transform.up * jumpForce;

                RB.AddRelativeForce(JumpDir);
                RB.gravityScale = 1;
            }
        }

        while (RB.velocity.y > 0)
        {
            RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y - (gravity * Time.deltaTime));
        }

        if (RB.velocity.y < 0)
        {
            RB.gravityScale = 0;
            RB.velocity = new Vector2(RB.velocity.x, 0);
        }
    }
}
