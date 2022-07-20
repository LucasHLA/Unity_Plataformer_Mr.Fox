using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy 
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float jumpLength;
    [SerializeField] private float jumpHeight;
    
    
    private Collider2D col;
    [SerializeField] private LayerMask ground;
    private bool facingLeft = true;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("jumping"))
        {
            if(rb.velocity.y < .1f)
            {
                anim.SetBool("falling", true);
                anim.SetBool("jumping", false);
            }
        }
        if(col.IsTouchingLayers(ground) && anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
        }
    }

    void Move()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1,transform.position.z);
                }

                if (col.IsTouchingLayers(ground))
                {
                    //jump
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("jumping", true);
                }
                
            }
            else
            {
                facingLeft = false;
                
            }
        }

        else
        {
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1,1,transform.position.z);
                }

                if (col.IsTouchingLayers(ground))
                {
                    //jump
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("jumping", true);
                }
                
            }
            else
            {
                facingLeft = true;
            }
        }
    }
    
}

