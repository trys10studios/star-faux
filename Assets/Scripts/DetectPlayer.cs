using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public Animator enemy1Anim;
    public bool enemyAttack = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            enemyAttack = true;
        }
    }
    //NOTE: code snippet below is for animator boolean for Enemy1
    //enemy1Anim.SetBool("enemyAttack", true); 
}
