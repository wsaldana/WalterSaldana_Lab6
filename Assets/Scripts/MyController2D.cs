using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class MyController2D : MonoBehaviour{

    private bool powerUp;
    private bool jumping;
    private Rigidbody2D rb;
    private Animator animator;

    public float speed = 5f;
    public float jumpForce = 8f;
    public Sprite spriteImage;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumping = false;
        powerUp = false;
    }

    
    void Update(){
        jump();
        animator.SetBool("jumping", jumping);
        if (Input.GetAxis("Horizontal") < 0) {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool("walking", true);
        } else if (Input.GetAxis("Horizontal") > 0) {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("walking", true);
        } else {
            animator.SetBool("walking", false);
        }
    }

    private void FixedUpdate() {
        if(!jumping)
            transform.Translate(new Vector2(Input.GetAxis("Horizontal") * speed, 0) * Time.deltaTime, Space.World);
        else
            transform.Translate(new Vector2(Input.GetAxis("Horizontal") * speed/3, 0) * Time.deltaTime, Space.World);

        
    }

    private void jump() {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow)) && !jumping && rb.velocity.y == 0) {
            jumping = true;
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("ground")) {
            jumping = false;
            rb.velocity = Vector2.zero;
        }else if (collision.gameObject.CompareTag("enemy")) {
            if (powerUp)
                Destroy(collision.gameObject);
            else
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("powerUp")) {
            powerUp = true;
            animator.SetBool("powerUp", powerUp);
            Destroy(collision.gameObject);
            GetComponent<SpriteRenderer>().sprite = spriteImage;
        }
    }
}
