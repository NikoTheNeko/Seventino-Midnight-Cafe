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


    public int health = 10;
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
        audio = gameObject.AddComponent<AudioSource>(); //adds an AudioSource to the game object this script is attached to
        audio.playOnAwake = false;
        audio.clip = walkSound;
        audio.volume = (0.5f);
        audio.Stop();
        currentStam = maxStamina;
        currentFlameStam = maxFlameStamina;
        currentGunStam = maxGunStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
        flameBar.maxValue = maxFlameStamina;
        flameBar.value = maxFlameStamina;
        gunBar.maxValue = maxGunStamina;
        gunBar.value = maxGunStamina;
        //Cursor.lockState = CursorLockMode.Locked;

    }

    public enum State
    {
        Normal,
        Rolling
    }

    private Rigidbody2D rigidbody2D;
    private Vector3 moveDirection;
    private Vector3 rollDirection;
    private Vector3 lastMovedDirection;
    private float rollSpeed;
    [SerializeField] private float MV_SPEED = 7f;
    public State state;
    private Transform gunAnchor;
    private Animator shotgunAnim;
    private Animator knifeAnim;
    private Animator flamethrowerAnim;
    public ParticleSystem flameParticles;
    public ParticleSystem smokeParticles;
    public LayerMask enemyLayer;
    private FlameHandler flameo;
    public ShotgunHandler shuggun;
    public KnifeHandler knifey;


    public AudioClip walkSound;
    public AudioClip dashSound;
    public AudioClip hitSound;
    private AudioSource audio;

    public bool facingRight = true;

    public Slider staminaBar;
    public Slider flameBar;
    public Slider gunBar;
    private int maxStamina = 1000;
    private int maxFlameStamina = 1001;
    private int maxGunStamina = 1002;
    private int currentStam;
    private int currentFlameStam;
    private int currentGunStam;

    public int fireDelay;

    public Collider2D triggerCollider;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;
    private Coroutine regenFlame;
    private Coroutine regenGun;

    private void Awake()
    {
        gunAnchor = transform.Find("gunAnchor");
        rigidbody2D = GetComponent<Rigidbody2D>();
        shotgunAnim = gunAnchor.Find("Shotgun").GetComponent<Animator>();
        flamethrowerAnim = gunAnchor.Find("Flambethrower").GetComponent<Animator>();
        flameo = gunAnchor.Find("Flambethrower").GetComponent<FlameHandler>();
        knifeAnim = gunAnchor.Find("Knife").GetComponent<Animator>();
        knifey = gunAnchor.Find("Knife").GetComponent<KnifeHandler>();
        shuggun = gunAnchor.Find("Shotgun").GetComponent<ShotgunHandler>();
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
                if (Input.GetKey(KeyCode.W) && CanMove)
                {
                    moveY = +.3f;
                }
                if (Input.GetKey(KeyCode.S) && CanMove)
                {
                    moveY = -.3f;
                }
                if (Input.GetKey(KeyCode.D) && CanMove)
                {
                    if(!facingRight)
                    {
                        Flip();
                    }
                    moveX = +.3f;
                }
                if (Input.GetKey(KeyCode.A) && CanMove)
                {
                    if(facingRight)
                    {
                        Flip();
                    }
                    moveX = -.3f;
                }
                if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && CanMove)
                {
                    playerAnim.SetBool("Idle", false);
                    playerAnim.SetBool("Run", true);
                    if (!audio.isPlaying)
                    {
                        audio.Play();
                    }
                }
                else
                {
                    playerAnim.SetBool("Idle", true);
                    playerAnim.SetBool("Run", false);
                    audio.Stop();
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
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    audio.clip = dashSound;
                    audio.loop = false;
                    audio.Play();
                    StartCoroutine(ResetToWalk(.1f));
                    playerAnim.SetTrigger("Dash");
                    rollDirection = lastMovedDirection;
                    rollSpeed = 25f;
                    state = State.Rolling;
                }
                break;
            // Currently in a rolling state.
            case State.Rolling:
                // Decays speed over time.
                //triggerCollider.enabled = false;
                float rollSpeedDropMult = 3.1f;
                rollSpeed -= rollSpeed * rollSpeedDropMult * Time.deltaTime;

                // Once Dodge roll speed goes below running speed, state changes to Normal.
                float rollSpeedMin = MV_SPEED;
                if (rollSpeed < rollSpeedMin)
                {
                    state = State.Normal;
                    //triggerCollider.enabled = true;
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
        if (fireDelay > 0)
        {
            --fireDelay;
        }
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
        if (Input.GetMouseButtonDown(0) && currentStam > 120 && fireDelay == 0)
        {
            UseStamina(120, ref currentStam, ref staminaBar);
            fireDelay = 20;
            aimGunEndPoint = gunAnchor.Find("Knife").Find("AttackPoint");
            Vector3 shootPoint = aimGunEndPoint.position;
            knifey.Swing(shootPoint, 0.25f, enemyLayer);
            aimGunEndPoint = gunAnchor.Find("Knife");
            if (knifeAnim.GetCurrentAnimatorStateInfo(0).IsName("knifeUp"))
            {
                knifeAnim.SetTrigger("goDown");
            }
            else if(knifeAnim.GetCurrentAnimatorStateInfo(0).IsName("knifeDown"))
            {
                knifeAnim.SetTrigger("goUp");
            }
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
        if(Input.GetMouseButton(0) && currentFlameStam > 2 && fireDelay == 0)
        {
            UseStamina(2, ref currentFlameStam, ref flameBar);
            fireDelay = 0;
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
        if (Input.GetMouseButtonDown(0) && currentGunStam > 140 && fireDelay == 0)
        {
            UseStamina(140, ref currentGunStam, ref gunBar);
            fireDelay = 30;
            aimGunEndPoint = gunAnchor.Find("Shotgun").Find("GunEndPoint");
            Vector3 shootPoint = aimGunEndPoint.position;
            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3 shootDir = (mousePosition - transform.position).normalized;
            shuggun.RayShoot(shootPoint, shootDir);
        }
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
        audio.clip = hitSound;
        audio.loop = false;
        audio.Play();
        StartCoroutine(ResetToWalk(.05f));
        StartCoroutine(FlashColor());

        health -= amount;

        checkDead();
    }

    IEnumerator ResetToWalk(float delay)
    {
        yield return new WaitForSeconds(delay);
        audio.clip = walkSound;
        audio.loop = true;
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
        SceneManager.LoadScene(sceneName: "deathMenu");
    }

    private void checkDead()
    {
        if (health <= 0)
        {
            //playerAnim.SetTrigger("Death");
            InventoryTracker tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
            tracker.ClearInventory();
            StartCoroutine("LeaveScene", 1.5f);
        }
    }

    public bool UseStamina(int amount, ref int currentStam, ref Slider staminaBar)
    {
        if(currentStam - amount >= 0)
        {
            currentStam -= amount;
            staminaBar.value = currentStam;
            if(regen != null && staminaBar.maxValue == 1000)
            {
                StopCoroutine(regen);
            }
            if (regenFlame != null && staminaBar.maxValue == 1001)
            {
                StopCoroutine(regenFlame);
            }
            if (regenGun != null && staminaBar.maxValue == 1002)
            {
                StopCoroutine(regenGun);
            }
            if (staminaBar.maxValue == 1000)
            {
                regen = StartCoroutine(RegenStamKnife());
            }
            else if (staminaBar.maxValue == 1001)
            {
                regenFlame = StartCoroutine(RegenStamFlame());
            }
            else if (staminaBar.maxValue == 1002)
            {
                regenGun = StartCoroutine(RegenStamGun());
            }
            return true;
        }
        else
        {
            // Debug.Log("nostam");
            return false;
        }
    }

    IEnumerator RegenStamKnife()
    {
        yield return new WaitForSeconds(1.5f);
        while(currentStam < maxStamina)
        {
            currentStam += 20;
            staminaBar.value = currentStam;
            yield return regenTick;
        }
        regen = null;
    }
    IEnumerator RegenStamGun()
    {
        yield return new WaitForSeconds(1.5f);
        while (currentGunStam < maxGunStamina)
        {
            currentGunStam += 20;
            gunBar.value = currentGunStam;
            yield return regenTick;
        }
        regenGun = null;
    }
    IEnumerator RegenStamFlame()
    {
        yield return new WaitForSeconds(1.5f);
        while (currentFlameStam < maxFlameStamina)
        {
            currentFlameStam += 20;
            flameBar.value = currentFlameStam;
            yield return regenTick;
        }
        
        regenFlame = null;
    }

    /*
    IEnumerator RegenFlameStam()
    {
        yield return new WaitForSeconds(1.5f);
        while (currentFlameStam < maxFlameStamina)
        {
            currentFlameStam += 20;
            flameBar.value = currentFlameStam;
            yield return regenTick;
        }
        regenFlame = null;
    }
    
    IEnumerator RegenGunStam()
    {
        yield return new WaitForSeconds(1.5f);
        while (currentGunStam < maxGunStamina)
        {
            currentGunStam += 20;
            gunBar.value = currentGunStam;
            yield return regenTick;
        }
        regenGun = null;
    }
    */

    /*public bool UseFlameStamina(int amount)
    {
        if (currentFlameStam - amount >= 0)
        {
            currentFlameStam -= amount;
            flameBar.value = currentFlameStam;
            if (regen != null)
            {
                StopCoroutine(regen);
            }
            regen = StartCoroutine(RegenStam());
            return true;
        }
        else
        {
            // Debug.Log("nostam");
            return false;
        }
    }

    public bool UseGunStamina(int amount)
    {
        if (currentGunStam - amount >= 0)
        {
            currentGunStam -= amount;
            gunBar.value = currentGunStam;
            if (regen != null)
            {
                StopCoroutine(regen);
            }
            regen = StartCoroutine(RegenStam());
            return true;
        }
        else
        {
            // Debug.Log("nostam");
            return false;
        }
    }*/

}
