using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PhysicsObject))]
public class Player : MonoBehaviour
{
    public float walkSpeed;
    public float jumpStrength;
    public Follow camera;
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
        if (p.down && V > 0)
        {
            p.vY = jumpStrength;
        }
        p.Simulate();
        camera.Simulate();
    }
}