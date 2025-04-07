using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public SpriteRenderer SpriteRenderer;
    public Animator an;
    Rigidbody2D rb;

    private Vector2 movement;
    private Vector2 bounds;
    private float pHW;
    private float xPos;
    // Start is called before the first frame update
    void Start()
    {
        pHW = SpriteRenderer.bounds.extents.x;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        doMovement();
        Flip();
    }

    private void Flip()
    {
        if (transform.position.x > xPos){
            SpriteRenderer.flipX = false;
        }
        else if (transform.position.x < xPos){
            SpriteRenderer.flipX = true;
        }

        xPos = transform.position.x;
    }

    private void doMovement(){
        float inputx = Input.GetAxis("Horizontal");
        float inputy = Input.GetAxis("Vertical");
        movement.x = inputx * speed * Time.deltaTime;
        movement.y = inputy * speed * Time.deltaTime;
        transform.Translate(movement);
        if(inputx != 0 || inputy != 0)
        {
            an.SetBool("IsRunning", true);
        }
        else{
                an.SetBool("IsRunning", false);
        }
    }
}


/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float speed = 5f;
    public SpriteRenderer SpriteRenderer;
    public Animator an;
    RigidBody2D rb;
     private bool isFacingRight = true;

    private Vector movement;
    private Vector2 bounds;
    private float pHW;
    private float xPos;

    // Start is called before the first frame update
    void Start()
    {
        pHW = SpriteRenderer.bounds.extents.x;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        doMovement();
        Flip();
    }

     private void Flip()
     {
        if (transform.position.x > xPos){
            SpriteRenderer.flipX = false;
        }
        else if (transform.position.x < xPos){
            SpriteRenderer.flipX = true;
        }

        xPos = tansform.position.x;
     }

     private void doMovement(){
        float input = Input.GetAxis("Horizontal");
        movement.x = input * speed * Time.deltaTime;
        transform.Translate(movement);
        if(input != 0 )
        {
            an.SetBool("isRunning", true);
        }
        else{
             an.SetBool("isRunning", false);
        }
     }
}
*/