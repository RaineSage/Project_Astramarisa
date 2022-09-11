using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float m_movementSpeed = 5f;
    [SerializeField]
    private float m_jumpForce = 10f;

    private bool m_isGrounded = false;
    private bool m_isInAir = true;

    private Rigidbody2D m_rigidbody;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        direction = direction.normalized * m_movementSpeed;

        direction.y = m_rigidbody.velocity.y;
        m_rigidbody.velocity = direction;

        direction.y = 0;
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && m_isGrounded && !m_isInAir)
        {
            m_rigidbody.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);

            m_isGrounded = false;
            m_isInAir = true;
        }
    }

    private void OnCollisionStay2D(Collision2D _col)
    {
        if (_col.gameObject.layer == 6)
        {
          m_isGrounded = true;
          m_isInAir = false;
        }
    }
}
