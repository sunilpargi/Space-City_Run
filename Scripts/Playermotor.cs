using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermotor : MonoBehaviour
{
    private const float lane_Distance = 2.5f;
    private const float turnSpeed = 0.05f;
    private bool isStartRunning = false;
    [SerializeField] private float slidingHeight;
    [SerializeField] private float slidingCenter ;

    //Animation
    private Animator anim;

    //Movement
    public CharacterController controller;
    [SerializeField] private float jumpForce = 7f;
    private float gravity = 12f;
    private float verticalVelocity ;
    public int desiredLane = 1; // 0 =left, 1 = middle, 2 = right
    public bool isDead;
    
    //SPEED modifier
   public float originalSpeed = 3f;
   [SerializeField] private float speed ;
   [SerializeField] private float speedIncreaseLastTIck ;
   [SerializeField] private float speedIncreaseTime = 5f; 
   [SerializeField] private float speedIncreaseAmount = .1f;

    [SerializeField] public AudioSource audioSOurce, bgAudioSOurce, ambienceAudioSOurce;
    [SerializeField] public AudioClip  hit,jump,slide,death, gameoverClip;
    void Start()
    {
        speed = originalSpeed;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStartRunning) return;

        if(Time.time - speedIncreaseLastTIck > speedIncreaseTime)
        {
            speedIncreaseLastTIck = Time.time;
            speed += speedIncreaseAmount;

            GameManager.instance.UpdateModifier(speed - originalSpeed);
        }
        //gather the input on which lane we should be

        if (MobileScript.instance.SwipeLeft)
        {
            Movelane(false);
            anim.SetTrigger("LeftStraf");
            //audioSOurce.clip = jump;
            //audioSOurce.Play();
            AudioSource.PlayClipAtPoint(jump, Camera.main.transform.position);
        }
        if (MobileScript.instance.SwipeRight)
        {
            Movelane(true);
            anim.SetTrigger("RightStraf");
            //audioSOurce.clip = jump;
            //audioSOurce.Play();
            AudioSource.PlayClipAtPoint(jump, Camera.main.transform.position);
        }
        //if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        //    Movelane(false);

        //if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        //    Movelane(true);

        //calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if(desiredLane == 0)
        {
            targetPosition += Vector3.left * lane_Distance;
        }
        else if(desiredLane == 2) 
        {
            targetPosition += Vector3.right * lane_Distance;
        }

        //lets calcuate our move delta
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).x * speed ; // give direction left or right


        //calculate y
        bool isGrounded = IsGrounded();
        anim.SetBool("Grounded", isGrounded);

        if (isGrounded) // if grounded
        {
            verticalVelocity = -5f;

            if (MobileScript.instance.SwipeUp)
            {
               
                anim.SetTrigger("Jump");
                //audioSOurce.clip = jump;
                //audioSOurce.Play();
                AudioSource.PlayClipAtPoint(jump, Camera.main.transform.position);
                verticalVelocity = jumpForce;
            }

            else if (MobileScript.instance.SwipeDown)
            {
                //sliding
                StartSliding();
                //audioSOurce.clip = slide;
                //audioSOurce.Play();
                AudioSource.PlayClipAtPoint(slide, Camera.main.transform.position);
                Invoke("StopSliding", 1f); 
            }
        }

        else
        {
            verticalVelocity -= gravity * Time.deltaTime;

            //fast falling machanic
            if (MobileScript.instance.SwipeDown)
            {
                verticalVelocity = -jumpForce;
            }
        }
        moveVector.y = verticalVelocity;
        moveVector.z = speed;

        controller.Move(moveVector * Time.deltaTime * 1.5f);

        //rotate the player
        Vector3 dir = controller.velocity;
        if(dir != Vector3.zero)
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, turnSpeed);
        }
       

    }

    private void StartSliding()
    {
        anim.SetBool("Sliding", true);
        controller.height = slidingHeight;
        controller.center = new Vector3(controller.center.x, controller.center.x, slidingCenter);
    }
    private void StopSliding()
    {
        anim.SetBool("Sliding", false);
        controller.height = 1.8f;
        controller.center = new Vector3(controller.center.x, 1f, 0f);
    }

    private void Movelane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    private bool IsGrounded()
    {
        Ray groundRay = new Ray(new Vector3(controller.bounds.center.x, (controller.bounds.center.y - controller.bounds.extents.y + .1f), controller.bounds.center.z), Vector3.down);

        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.red, 1);

        return Physics.Raycast(groundRay, .1f + .1f);
    }

    public void StartRunning()
    {
        isStartRunning = true;
        anim.SetTrigger("StartRunning");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                {
                    Crash();
                    break;
                }
        }
    }

    private void Crash()
    {
        isDead = true;
        anim.SetTrigger("Death");
        audioSOurce.clip = death;
        audioSOurce.Play();
        ambienceAudioSOurce.Stop();
        bgAudioSOurce.Stop();
        bgAudioSOurce.clip = gameoverClip;
        bgAudioSOurce.Play();
        isStartRunning = false;
        GameManager.instance.OnDeath();
    }
}
