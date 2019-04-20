using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isSceneDoor = false;
    public bool isOpen = false;

    public int sceneNumber;
    public int doorNumber;

//    public Transform doorPivot;
    public Animator animator;


    private void Awake()
    {
        //doorPivot = this.transform.parent.gameObject.transform;
        animator = GetComponentInParent<Animator>();
    }

    public void OpenDoor()
    {
        animator.SetTrigger("OpenDoor");
        isOpen = true;
        Debug.Log("sluiten");
    }

    public void CloseDoor()
    {
        animator.SetTrigger("CloseDoor");
        isOpen = false;
        Debug.Log("openen");

    }

    private void AnimationFinished()
    {
        //deur gaat wel open, maar blijft of altijd open staan als je geen terugjkeer naar de idle state in 
        //de animator hebt, óf hij klapt meteen dicht als je dat wel doet.

        animator.ResetTrigger("OpenDoor");
        animator.ResetTrigger("CloseDoor");
    }
}

