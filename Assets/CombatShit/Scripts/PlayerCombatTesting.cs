using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCombatTesting : MonoBehaviour{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
    }

    #region Public Variables

    [Header("Rigidbody and Basic Stuff for Unity")]
    [Tooltip("Rigidbody of the Object")]
    public Rigidbody2D rbody; //Rigidbody of the Object

    [Header("Combat Stuff")]
    [Tooltip("crosshair")]
    public GameObject crosshair;
    [Tooltip("This is the hit box thing")]
    public GameObject hitbox;
    [Tooltip("Slash")]
    public GameObject slashBox;
    [Tooltip("Fire Shit")]
    public GameObject flameShit;
    [Tooltip("Seasoning Shot")]
    public GameObject sShot;

    public Animator playerAnim;


    public int health = 100;
    public SpriteRenderer[] sprites;
    public Color hurtColor;

    #endregion

    #region Private Varirables
    public bool CanMove = true;
    private List<GameObject> shotList;
    private Transform aimGunEndPoint;
    private int weaponSelect = 0;
                //weapon select 1: Knife
                //              2: Flambethrower
                //              3: Pepper shotgun

    #endregion
    private void Start()
    {
        currentStam = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private enum State
    {
        Normal,
        Rolling
    }

    private Rigidbody2D rigidbody2D;
    private Vector3 moveDirection;
    private Vector3 rollDirection;
    private Vector3 lastMovedDirection;
    private float rollSpeed;
    [SerializeField] private const float MV_SPEED = 7f;
    private State state;
    private Transform gunAnchor;
    private Animator shotgunAnim;
    private Animator knifeAnim;
    private Animator flamethrowerAnim;
    public ParticleSystem flameParticles;
    public ParticleSystem smokeParticles;
    public LayerMask enemyLayer;
    private FlameHandler flameo;
    public ShotgunHandler shuggun;

    public bool facingRight = true;

    public Slider staminaBar;
    private int maxStamina = 1000;
    private int currentStam;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    private void Awake()
    {
        gunAnchor = transform.Find("gunAnchor");
        rigidbody2D = GetComponent<Rigidbody2D>();
        shotgunAnim = gunAnchor.Find("Shotgun").GetComponent<Animator>();
        flamethrowerAnim = gunAnchor.Find("Flambethrower").GetComponent<Animator>();
        flameo = gunAnchor.Find("Flambethrower").GetComponent<FlameHandler>();
        knifeAnim = gunAnchor.Find("Knife").GetComponent<Animator>();
        state = State.Normal;
    }

    // Update is called once per frame
    void Update(){
        
        /* The switch statement determines whether the player
           is in a running state or rolling state. */
        switch (state)
        {
            // currently in a running state.
            case State.Normal:
                float moveX = 0f;
                float moveY = 0f;

                // WASD movement implementation.
                if (Input.GetKey(KeyCode.W))
                {
                    moveY = +.3f;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    moveY = -.3f;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    if(!facingRight)
                    {
                        Flip();
                    }
                    moveX = +.3f;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    if(facingRight)
                    {
                        Flip();
                    }
                    moveX = -.3f;
                }
                if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    playerAnim.SetBool("Idle", false);
                    playerAnim.SetBool("Run", true);
                }
                else
                {
                    playerAnim.SetBool("Idle", true);
                    playerAnim.SetBool("Run", false);
                }
                // converting WASD input into a vector3, normalized.
                moveDirection = new Vector3(moveX, moveY).normalized;

                // Stored for dodge rolling in the last moved direction when idle.
                if (moveX != 0 || moveY != 0)
                {
                    lastMovedDirection = moveDirection;
                }

                // Dodge roll can only start if the player is currently not in a dodge roll.
                // Dodge roll starts here.
                if (Input.GetKeyDown(KeyCode.LeftShift) && UseStamina(15))
                {
                    playerAnim.SetTrigger("Dash");
                    rollDirection = lastMovedDirection;
                    rollSpeed = 20f;
                    state = State.Rolling;
                }
                break;
            // Currently in a rolling state.
            case State.Rolling:
                // Decays speed over time.
                float rollSpeedDropMult = 3.1f;
                rollSpeed -= rollSpeed * rollSpeedDropMult * Time.deltaTime;

                // Once Dodge roll speed goes below running speed, state changes to Normal.
                float rollSpeedMin = MV_SPEED;
                if (rollSpeed < rollSpeedMin)
                {
                    state = State.Normal;
                }
                break;
        }

        UpdateWeapon();
        //if (Input.GetMouseButtonDown(0))
        weaponSelect = gunAnchor.GetComponent<weaponBehaviour>().index;
        switch (weaponSelect) {
            case 0:
                weaponOne();
                break;
            case 1:
                weaponTwo();
                break;
            case 2:
                weaponThree();
                break;
        }
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void FixedUpdate()
    {
        // handles the rolling state in a fixed update with a switch statement.
        switch (state)
        {
            // enters this case if the state is normal
            case State.Normal:
                rigidbody2D.velocity = moveDirection * MV_SPEED;
                break;
            // enters this case if the state is rolling
            case State.Rolling:
                rigidbody2D.velocity = rollDirection * rollSpeed;
                break;
        }
    }

    void weaponOne()
    {
        if (Input.GetMouseButtonDown(0) && UseStamina(25))
        {
            aimGunEndPoint = gunAnchor.Find("Knife").Find("AttackPoint");
            Vector3 shootPoint = aimGunEndPoint.position;
            KnifeHandler.Swing(shootPoint, 0.25f, enemyLayer);
            aimGunEndPoint = gunAnchor.Find("Knife");
            knifeAnim.SetTrigger("Fire");
        }
    }

    void weaponTwo()
    {
        var emission = flameParticles.emission;
        if (Input.GetMouseButtonDown(0))
        {
            flamethrowerAnim.SetTrigger("Fire");
            flameo.ActivateFlame();
        }
        if(Input.GetMouseButton(0) && UseStamina(1))
        {
            aimGunEndPoint = gunAnchor.Find("Flambethrower");
            flamethrowerAnim.SetBool("IsFiring", true);
            var em = flameParticles.emission;
            em.enabled = true;
            var em2 = smokeParticles.emission;
            em2.enabled = true;
            if (!flameParticles.isPlaying)
            {
                
            }
        }
        else
        {
            flamethrowerAnim.SetBool("IsFiring", false);
            flameo.DeactivateFlame();
            var em = flameParticles.emission;
            em.enabled = false;
            var em2 = smokeParticles.emission;
            em2.enabled = false;
            //flameParticles.Stop();
        }
        //flamethrowerAnim;
    }

    void weaponThree()
    {
        if (Input.GetMouseButtonDown(0) && UseStamina(33))
        {
            aimGunEndPoint = gunAnchor.Find("Shotgun").Find("GunEndPoint");
            Vector3 shootPoint = aimGunEndPoint.position;
            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3 shootDir = (mousePosition - transform.position).normalized;
            shuggun.RayShoot(shootPoint, shootDir);
        }
        //spawn pellets from gun end point, need to construct prefabs for projectiles
    }

    
    

    /**
        Getters and Setters for Can Move
        Get CanMove checks for Can Move
        Set CanMove sets it to the desired value
    **/
    public bool GetCanMove(){
        return CanMove;
    }

    public void SetCanMove(bool val){
        CanMove = val;
    }
    public void UpdateWeapon()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if(!facingRight)
        {
            angle += 180f;
        }
        gunAnchor.eulerAngles = new Vector3(0, 0, angle);
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;
        return worldPosition;
    }

    public void PlayerHit(int amount)
    {

        StartCoroutine(FlashColor());

        health -= amount;

        checkDead();
    }


    IEnumerator FlashColor()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = hurtColor;
        }
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = Color.white;
        }
    }

    IEnumerator LeaveScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName: "TitleScreen");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "beanProjectile")
        {
            health -= 3;
            CameraShake.instance.ShakeCamera(.25f, .05f);
            //Debug.Log(health);
            other.gameObject.SetActive(false);
        }
    }

    private void checkDead()
    {
        if (health < 0)
        {
            //playerAnim.SetTrigger("Death");
            StartCoroutine("LeaveScene", 1.5f);
        }
    }

    public bool UseStamina(int amount)
    {
        if(currentStam - amount >= 0)
        {
            currentStam -= amount;
            staminaBar.value = currentStam;
            if(regen != null)
            {
                StopCoroutine(regen);
            }
            regen = StartCoroutine(RegenStam());
            return true;
        }
        else
        {
            Debug.Log("nostam");
            return false;
        }
    }

    IEnumerator RegenStam()
    {
        yield return new WaitForSeconds(1.5f);
        while(currentStam < maxStamina)
        {
            currentStam++;
            staminaBar.value = currentStam;
            yield return regenTick;
        }
        regen = null;
    }
}
