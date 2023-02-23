using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float m_movementSpeed = 5f;
    [SerializeField]
    private float m_jumpForce = 5f;
    [SerializeField]
    private LayerMask m_groundLayer;

    [SerializeField] 
    private Transform m_groundCheck;

    [SerializeField] 
    private SpriteRenderer m_sprite;
    
    private bool m_isGrounded = false;
    private Rigidbody m_rigidbody;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
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
        direction = direction.normalized;

        m_rigidbody.velocity = new Vector3(direction.x * m_movementSpeed, m_rigidbody.velocity.y ,direction.y*m_movementSpeed);


        if (!m_sprite.flipX && direction.x < 0)
        {
            m_sprite.flipX = true;
        }
        else if (m_sprite.flipX && direction.x > 0)
        {
            m_sprite.flipX = false;
        }
    }

    private void Jump()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(m_groundCheck.position, Vector3.down, out hit, 3f, m_groundLayer))
        {
            m_isGrounded = true;
        }
        else
        {
            m_isGrounded = false;
        }


        if (Input.GetKeyDown(KeyCode.Space) && m_isGrounded)
        {
            m_rigidbody.velocity += new Vector3(0f, m_jumpForce, 0f);
        }
    }

    private void OnCollisionStay(Collision _col)
    {
        //if (_col.gameObject.layer == 6)
        //{
        //  m_isGrounded = true;
        //  m_isInAir = false;
        //}
    }
}
