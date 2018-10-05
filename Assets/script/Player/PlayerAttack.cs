using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    //AttackCombo


    Animator animator;
    bool canAttack ;
    int noOfClicks ;
  
    void Start () {
        canAttack = true;
        noOfClicks = 0;
        animator = GetComponent < Animator >();

    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetMouseButtonDown(0))
            {
            
            ComboStarter();
                
            }
    
        Debug.Log(noOfClicks);
        Debug.Log("Can Attack:" + canAttack);
    }

    //motion.attack
    void ComboStarter()
    {

        if (canAttack)
        {
            noOfClicks++;
        }
        if (noOfClicks == 1)
        {
            animator.SetBool("WeaponOut", true);
            animator.SetInteger("AttackCombo", 31);
        }
    }
    public void ComboCheck()
    {

        canAttack = false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack31") && noOfClicks == 1)
        {//If the first animation is still playing and only 1 click has happened, return to idle
            animator.SetInteger("AttackCombo", 4);
            canAttack = true;
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
            noOfClicks = 0;
        }

     
    }
}
