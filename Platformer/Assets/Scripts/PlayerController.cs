using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    
    [SerializeField]private int speed;
    [SerializeField]private  float jumpForce;
    [SerializeField]private int cherries = 0; 
    [SerializeField] private float hurtForce;
    [SerializeField] private int health;
    private Rigidbody2D rb;
    private Transform playerTR;
    private  Animator anim;
    private Collider2D col;
    [SerializeField] private LayerMask ground;
    [SerializeField] private TextMeshProUGUI CollectableNumber;
    [SerializeField] private TextMeshProUGUI HealthNumber;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource audio;

    //Ladder

    [HideInInspector]public bool canClimb = false;
    [HideInInspector] public bool bottomLadder = false;
    [HideInInspector] public bool topLadder = false;
    [HideInInspector] public Ladder ladder;
    private float naturalGravity;
    [SerializeField] float climbSpeed;
    private enum State { idle, running,jumping, falling, hurt, climb};
    private State state = State.idle;   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTR = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        audio = GetComponent<AudioSource>();
        HealthNumber.text = health.ToString();
        naturalGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {

        HealthNumber.text = health.ToString();
        if(state == State.climb)
        {
            Climb();
        }
        else if (state != State.hurt)
        {
            Movement();
   
        }
        AnimationState();
        anim.SetInteger("state", (int)state);
        Death();
        
    }

    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if(canClimb && Mathf.Abs(Input.GetAxis("Vertical")) > .1f)
        {
            state = State.climb;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            transform.position = new Vector3(ladder.transform.position.x,rb.position.y);
            rb.gravityScale = 0f;
        }
        if (horizontal < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            playerTR.localScale = new Vector2(-1, 1);

        }
        else if (horizontal > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            playerTR.localScale = new Vector2(1, 1);

        }

        if (Input.GetButtonDown("Jump") && col.IsTouchingLayers(ground))
        {
            Jump();

        }
    }
    private void AnimationState()
    {
        if(state == State.climb)
        {

        }
        else if (state == State.jumping)
        {
            if(rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if(state == State.falling)
        {
            if (col.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if(Mathf.Abs(rb.velocity.x) > 2f) 
        {
            //moving 
            state = State.running;
        }
        else
        {
            state=State.idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectable")
        {
            cherry.Play();
            Destroy(collision.gameObject);
            cherries += 1;
            CollectableNumber.text = cherries.ToString();
        }

        if(collision.tag == "Power")
        {
            cherry.Play();
            Destroy(collision.gameObject);
            speed = speed * 2;
            GetComponent<SpriteRenderer>().color = Color.magenta;
            StartCoroutine(ResetPower());
            
        }
        if(collision.tag == "Heart")
        {
            cherry.Play();
            Destroy(collision.gameObject);
            health = health + 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                if (collision.gameObject.tag == "Enemy")
                {
                    state = State.hurt;
                    if (collision.gameObject.transform.position.x > transform.position.x)
                    {
                        rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                        health = health - 1;
                    }
                    else
                    {
                        rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                        health = health - 1;
                    }
                }

            }
        }
        
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void Footstep()
    {
        audio.Play();
    }
    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(10);
        speed = speed / 2;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void Death()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Climb()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            canClimb = false;
            rb.gravityScale = naturalGravity;
            anim.speed = 1f;
            Jump();
            return;
        }
        float vDirection = Input.GetAxis("Vertical");
        if(vDirection > .1f && !topLadder)
        {
            //climbing up
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            anim.speed = 1f;
        }
        else if(vDirection < .1f && !bottomLadder)
        {
            //climbing down 
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            anim.speed = 1f;
        }
        else
        {
            //sitting still
            anim.speed = 0f;
            rb.velocity = Vector2.zero;
        }
    }
}
