using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PhysicsObject))]
public class Player : MonoBehaviour
{
    public float walkSpeed;
    public float jumpStrength;
    public int jumpLength;
    public Follow camera;
    private int coyote = 0;
    private int jumpCounter;
    readonly InputAction horizontal = InputSystem.actions.FindAction("Horizontal");
    readonly InputAction vertical = InputSystem.actions.FindAction("Vertical");
    public PhysicsObject p;
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        float H = horizontal.ReadValue<float>();
        float V = vertical.ReadValue<float>();
        p.vX = H * walkSpeed;
        coyote++;
        jumpCounter--;
        if (p.down)
        {
            coyote = 0;
        }
        if (coyote <= 4 && V > 0 && p.vY <= 0.25f)
        {
            p.vY = jumpStrength;
            coyote = 5;
            jumpCounter = jumpLength;
        }
        else if (V > 0)
        {
            if (jumpCounter > 0)
            {
                p.vY = jumpStrength;
            }
            p.gravity = -0.75f;
        }
        else
        {
            jumpCounter = 0;
            p.gravity = -1.25f;
        }
        p.Simulate();
        camera.Simulate();
    }
}