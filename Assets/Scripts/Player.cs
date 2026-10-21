using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using System.Collections;


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
    public PhysicsObject p;

    private Animator anim;
    public SpriteRenderer flash;

    public float spawnX;
    public float spawnY;

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

    bool delayedR;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(loadFMODReferences());
        spawnX = p.pX;
        spawnY = p.pY;
    }

    IEnumerator loadFMODReferences()
    {
        bool loaded;
        yield return new WaitUntil(() => loaded = RuntimeManager.HaveAllBanksLoaded);
        
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
        if (dashTimeLeft >= 0){
            dashTimeLeft -= Time.fixedDeltaTime;
            transform.Rotate(Vector3.forward * dashRotationSpeed * Time.deltaTime * dashDirection * -1);
            if (dashTimeLeft <= 0.0001){// end dash
                anim.SetTrigger("DashEnd");
                p.vX = dashDirection * walkSpeed;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
    void FixedUpdate()
    {
        update_imunity_timer();
        update_dash_timer();
        float H = Input.GetAxisRaw("Horizontal");
        float V = Input.GetAxisRaw("Vertical");
        
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
        if (Input.GetKey(KeyCode.Q) && dashTimeLeft <= 0)
        {
            dashDirection = transform.localScale.x;
    
            dashTimeLeft = dashDuration;
            anim.SetTrigger("Dash");
            Debug.Log("Dash");

            RuntimeManager.PlayOneShot(dash);
        }
        if (Input.GetKey(KeyCode.R) && !delayedR)
        {
            RuntimeManager.PlayOneShot(death);

            health = 3;
            p.pX = spawnX;
            p.pY = spawnY;
        }
        delayedR = Input.GetKey(KeyCode.R);
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
            p.pX = spawnX;
            p.pY = spawnY;
        }
    }

    public void Win() // Call this when a win condition is met!
    {
       print("win called");
        SceneManager.LoadScene("Win");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            spawnX = p.pX;
            spawnY = p.pY;
            collision.gameObject.GetComponent<check>().turnon();
        }
    }
}
