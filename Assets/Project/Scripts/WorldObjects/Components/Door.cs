using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isSceneDoor = false;
    public bool isOpen = false;

    public int sceneNumber;
    public int doorNumber;

    public Animator animator;


    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    public void OpenDoor()
    {
        animator.SetTrigger("OpenDoor");
        isOpen = true;
    }

    public void CloseDoor()
    {
        animator.SetTrigger("CloseDoor");
        isOpen = false;
    }

    private void AnimationFinished()
    {
        animator.ResetTrigger("OpenDoor");
        animator.ResetTrigger("CloseDoor");
    }
}

