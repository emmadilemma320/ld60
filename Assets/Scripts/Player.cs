using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PhysicsObject))]
public class Player : MonoBehaviour
{
    public InputAction horizontal = InputSystem.actions.FindAction("Horizontal");
    public InputAction vertical = InputSystem.actions.FindAction("Vertical");
    public PhysicsObject p;
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        float H = horizontal.ReadValue<float>();
        float V = vertical.ReadValue<float>();
        p.vX = H * 2.5f;
        if (p.down && V > 0)
        {
            p.vY = 5.5f;
        }
    }
}