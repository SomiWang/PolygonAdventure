using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //sword
  
    public Renderer rsword_outside;
    public Renderer rsword_inside;


    //smoothdamp:http://wiki.ceeger.com/script/unityengine/classes/vector3/vector3.smoothdamp
    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float gravity = -12;
    public float jumpPower = 1;
    [Range(0, 1)]
    public float airControlPercent;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.2f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;

    Animator animator;
    AnimationClip clip;
    AnimationClip[] clipList;
      Transform cameraT;
    CharacterController controller;


    //Combat Variable
    bool inCombat;
    bool canAttack;
    bool canMove;
    int noOfClicks;
    
    // Use this for initialization
    void Start()
    {

        animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        inCombat = false;
        canAttack = true;
        noOfClicks = 0;
        canMove = true;

        SetSwordIn();
    }

    // Update is called once per frame
    void Update()
    {



        //input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        bool running = Input.GetKey(KeyCode.LeftShift);
        if (canMove == true) { Move(inputDir, running); }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0) && !inCombat && !animator.GetCurrentAnimatorStateInfo(0).IsName("Moving"))
        {
            
            canMove = false;
            PutWeapon();
        }
        if (Input.GetMouseButtonDown(0))
        {
            
            canMove = false;
           
            TookWeapon();
            ComboStarter();
        }
        SetMoveable();
        
        //animator
        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
   
    }
    
    


 




    void Move(Vector2 inputDir, bool running)
    {
        if (inputDir != Vector2.zero)
        {

            //transform.eulerAngles = Vector3.up * Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            //SmoothRotate
            float playerRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;

            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, playerRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        float playerSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, playerSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        //transform.Translate(transform.forward * playerSpeed * Time.deltaTime, Space.World);

        velocityY += Time.deltaTime * gravity;//重力
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
        //controller撞到牆壁,xy=0,則currentSpeed=0
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;
        if (controller.isGrounded)
        {
            velocityY = 0;
        }


    }




    public void SetSwordOut()
    {
        rsword_outside.enabled = true;
        rsword_inside.enabled = false;
   
    }
    void SetSwordIn()
    {

        
        rsword_outside.enabled = false;
        rsword_inside.enabled = true;
    }


    public void PutWeapon()
    {
        animator.SetBool("WeaponOut", false);
    }
    public void TookWeapon()
    {

        animator.SetBool("WeaponOut", true);
    }
    //Combo
    void ComboStarter()
    {

        if (canAttack)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("CombatMoving") ||
                animator.GetCurrentAnimatorStateInfo(0).IsName("Attack31"))
            {
                noOfClicks++;
                Debug.Log(noOfClicks);
            }
            if (noOfClicks == 1)
            {
                animator.SetInteger("AttackCombo", 31);
            }
        }
    }

    public void SetMoveable()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SetMoveable") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("SetMoveable_Combat"))
        {
            canMove = true;
            
        }
    }

    public void ComboCheck()
    {
        canAttack = false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack31") && noOfClicks == 1)
        {//If the first animation is still playing and only 1 click has happened, return to idle
            animator.SetInteger("AttackCombo", 4);
            canAttack = true;
            canMove = true;
            noOfClicks = 0;
        }

        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack31") && noOfClicks >= 2)
        {//If the first animation is still playing and at least 2 clicks have happened, continue the combo
            animator.SetInteger("AttackCombo", 33);
            canAttack = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack33") && noOfClicks == 2)
        { //If the second animation is still playing and only 2 clicks have happened, return to idle
            animator.SetInteger("AttackCombo", 4);
            canAttack = true;
            canMove = true;
            noOfClicks = 0;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack33") && noOfClicks >= 3)
        { //If the second animation is still playing and at least 3 clicks have happened, continue the combo
            animator.SetInteger("AttackCombo", 6);
            canAttack = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack6"))
        { //Since this is the third and last animation, return to idle
            animator.SetInteger("AttackCombo", 4);
            canAttack = true;
            canMove = true;
            noOfClicks = 0;
        }


    }
    //motion.jump
    void Jump()
    {
        if (controller.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpPower);
            velocityY = jumpVelocity;
        }
    }
    //在空中修飾SmoothTime
    float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }

        return smoothTime / airControlPercent;
    }



}
