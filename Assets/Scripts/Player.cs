using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using FMOD.Studio;

[RequireComponent(typeof(PhysicsObject))]
public class Player : MonoBehaviour
{
    public int max_health;
    public int health;
    public SpriteRenderer[] healthBar;
    public Sprite healthOn;
    public Sprite healthOff;
    public bool imune_from_damage = false;
    float immunityTimerDuration = 0.5f;
    float immunityTimerTimeLeft = 0;

    public float dashSpeed = 50;
    public float dashDuration = 0.75f;
    public float dashRotationSpeed = 6.14f; // In degrees
    float dashTimeLeft = 0;
    float dashDirection = 0;

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
    public SpriteRenderer flash;

    [Header("Sound Stuff")]
    string dashPath = "event:/Dash";
    string jumpPath = "event:/Jump";
    string hurtPath = "event:/Ouch";
    string deathPath = "event:/Death";

    EventInstance jump;
    //EventInstance dash;
    //EventInstance hurt;
    //EventInstance death;

    //EventReference jump;
    EventReference dash;
    EventReference hurt;
    EventReference death;

    bool jumpKeyPressed = false;



    void Start()
    {
        anim = GetComponent<Animator>();
        loadFMODReferences();
    }

    void loadFMODReferences()
    {
        jump = RuntimeManager.CreateInstance(jumpPath);
        //dash = RuntimeManager.CreateInstance(dashPath);
        //hurt = RuntimeManager.CreateInstance(hurtPath);
        //death = RuntimeManager.CreateInstance(deathPath);

        //jump = RuntimeManager.PathToEventReference(jumpPath);
        dash = RuntimeManager.PathToEventReference(dashPath);
        hurt = RuntimeManager.PathToEventReference(hurtPath);
        death = RuntimeManager.PathToEventReference(deathPath);

    }

    void update_imunity_timer(){
        if (immunityTimerTimeLeft > 0){
            immunityTimerTimeLeft -= Time.fixedDeltaTime;
        }
        flash.color = new Color(1.0f, 0.0f, 0.0f, immunityTimerTimeLeft / 6.0f);
    }
    void update_dash_timer(){
        if (dashTimeLeft > 0){
            dashTimeLeft -= Time.deltaTime;
            transform.Rotate(Vector3.forward * dashRotationSpeed * Time.deltaTime * dashDirection * -1);
            if (dashTimeLeft <= 0.0001){// end dash
                anim.SetTrigger("DashEnd");
                p.vX = dashDirection * walkSpeed;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
    public void Dash(InputAction.CallbackContext context){
        // If dash action and not already in dash
        if (context.performed && dashTimeLeft <=0){
            dashDirection = transform.localScale.x;
     
            dashTimeLeft = dashDuration;
            anim.SetTrigger("Dash");
            Debug.Log("Dash");

            RuntimeManager.PlayOneShot(dash);
            // play dash sound
        }
    }
    void FixedUpdate()
    {
        update_imunity_timer();
        update_dash_timer();
        float H = horizontal.ReadValue<float>();
        float V = vertical.ReadValue<float>();
        
        anim.SetFloat("DashTimeLeft", dashTimeLeft);
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




        if (dashTimeLeft > 0){
            p.vX = dashDirection * dashSpeed;
        }else{
            p.vX = H * walkSpeed;
        }
        
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
            
            // stop jump if its playing
            PLAYBACK_STATE state;
            jump.getPlaybackState(out state);
            if (state != PLAYBACK_STATE.STOPPED)
            {
                jump.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
            
            jump.getPlaybackState(out state);
            { 
            if (state == PLAYBACK_STATE.STOPPED)
                {
                    jump.start();
                }
            }
            


            // Play jump?
            // no time to fix this. In a one block hole, holding jump will
            // spam jump sfx. But issue is larger than a fix like this
            //if (V >= 1)
            //{
            //    if (!jumpKeyPressed)
            //    {
            //        RuntimeManager.PlayOneShot(jump);
            //        jumpKeyPressed = true;
            //    }
            //}
            //else
            //{
            //    jumpKeyPressed = false;
            //}

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

                RuntimeManager.PlayOneShot(hurt);

            }
            else
            {
                Debug.Log("Warning: Not taking damage since damge type unknown");
            }
        }
        if (health == 0)
        {

            RuntimeManager.PlayOneShot(death);

            health = 3;
            p.pX = 0;
            p.pY = 0;
        }
    }

    public void Win() // Call this when a win condition is met!
    {
        SceneManager.LoadScene("Win");
    }
}
