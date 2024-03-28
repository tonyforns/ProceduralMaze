using UnityEditor.Animations;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private const string IS_OPEN = "IsOpen";

    
    [SerializeField] private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Open()
    {
        animator.SetBool(IS_OPEN, true);
    }

    public void Close()
    {
        animator.SetBool(IS_OPEN, false);
    }
}
