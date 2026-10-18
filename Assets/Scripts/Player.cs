using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PhysicsObject))]
public class Player : MonoBehaviour
{
    public int max_health;
    public int health;
    public bool imune_from_damage = false;
    float imunityTimerDuration = 5.5f;
    float imunityTimerTimeLeft = 0;


    public float walkSpeed;
    public float jumpStrength;
    public int jumpLength;
    public Follow camera;
    public bool fan;
    private int coyote = 0;
    private int jumpCounter;
    readonly InputAction horizontal = InputSystem.actions.FindAction("Horizontal");
    readonly InputAction vertical = InputSystem.actions.FindAction("Vertical");
    public PhysicsObject p;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void update_imunity_timer(){
        if (imunityTimerTimeLeft > 0){
            imunityTimerTimeLeft -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        update_imunity_timer();
        float H = horizontal.ReadValue<float>();
        float V = vertical.ReadValue<float>();
        
        anim.SetFloat("yVelocity", p.vY);
        anim.SetFloat("Magnitude", Mathf.Abs(p.vX));

        Vector3 localScale = transform.localScale;
        if (p.vX >= 0.1  && localScale.x == -1f){ // Flip to right
            localScale.x = 1f;
            transform.localScale = localScale;
        }else if (p.vX < -0.1  && localScale.x == 1f){ // Flip to left
            localScale.x = -1f;
            transform.localScale = localScale;
        }
        
        

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
        if (fan)
        {
            p.vY += 2f;
        }
        p.Simulate();
        camera.Simulate();
    }

    public void take_damage(string damage_type){
        if (imunityTimerTimeLeft <= 0){
            if (damage_type == "sparks"){
                health -=1;
                Debug.Log("Take damage from sparks");
                imunityTimerTimeLeft = imunityTimerDuration;
            }
            else{
                Debug.Log("Warning: Not taking damage since damge type unknown");
            }
        }
    }
}
