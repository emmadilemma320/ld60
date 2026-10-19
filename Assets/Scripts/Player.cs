using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhysicsObject))]
public class Player : MonoBehaviour
{
    public int max_health;
    public int health;
    public SpriteRenderer[] healthBar;
    public Sprite healthOn;
    public Sprite healthOff;
    public bool imune_from_damage = false;
    public float immunityTimerDuration = 5.5f;
    public float immunityTimerTimeLeft = 0;
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
    public SpriteRenderer flash;
    void Start()
    {
        
    }
    void update_imunity_timer(){
        if (immunityTimerTimeLeft > 0){
            immunityTimerTimeLeft -= Time.fixedDeltaTime;
        }
        flash.color = new Color(1.0f, 0.0f, 0.0f, immunityTimerTimeLeft / 6.0f);
    }

    void FixedUpdate()
    {
        update_imunity_timer();
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
        if (fan)
        {
            p.vY += 2f;
        }
        healthBar[0].sprite = health > 0 ? healthOn : healthOff;
        healthBar[1].sprite = health > 1 ? healthOn : healthOff;
        healthBar[2].sprite = health > 2 ? healthOn : healthOff;
        p.Simulate();
        camera.Simulate();
    }

    public void take_damage(string damage_type){
        if (immunityTimerTimeLeft <= 0){
            if (damage_type == "sparks"){
                health -=1;
                Debug.Log("Take damage from sparks");
                immunityTimerTimeLeft = immunityTimerDuration;
            }
            else{
                Debug.Log("Warning: Not taking damage since damge type unknown");
            }
        }
    }

    public void Win() // Call this when a win condition is met!
    {
        SceneManager.LoadScene("Win");
    }
}
