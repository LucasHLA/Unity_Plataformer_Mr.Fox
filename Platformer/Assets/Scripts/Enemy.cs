using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected private Animator anim;
    protected private Rigidbody2D rb;
    protected private AudioSource death;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponent<Animator>(); 
        rb = GetComponent<Rigidbody2D>();
        death = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void JumpedOn()
    {
        anim.SetTrigger("Death");
        death.Play();
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;
    }
    private void Death()
    {
        Destroy(this.gameObject);
    }
}
