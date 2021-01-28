using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private const float MV_SPEED = 7f;
    private State state;
    private Transform gunAnchor;
    private Animator shotgunAnim;
    private Animator knifeAnim;
    private Animator flamethrowerAnim;

    private void Awake()
    {
        gunAnchor = transform.Find("gunAnchor");
        rigidbody2D = GetComponent<Rigidbody2D>();
        shotgunAnim = gunAnchor.Find("Shotgun").GetComponent<Animator>();
        flamethrowerAnim = gunAnchor.Find("Flambethrower").GetComponent<Animator>();
        knifeAnim = gunAnchor.Find("Knife").GetComponent<Animator>();
        aimGunEndPoint = gunAnchor.Find("GunEndPoint");
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
                    moveY = +1f;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    moveY = -1f;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    moveX = +1f;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    moveX = -1f;
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

        CheckAttack();
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

    /*
    private void LateUpdate()
    {
        updateCrosshair();
    }
    
    void updateCrosshair()
    {
        if (crosshair.transform.localPosition.magnitude >= 5)
        {
            crosshair.transform.localPosition = crosshair.transform.localPosition.normalized * 5;
            crosshair.GetComponent<Rigidbody2D>().velocity = transform.gameObject.GetComponent<Rigidbody2D>().velocity;
        }
        Vector2 crosshairVel = transform.gameObject.GetComponent<Rigidbody2D>().velocity + new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        crosshairVel.Normalize();
        (Physics2D.Raycast(transform.position, Vector3.up, 0.1f))
        {
            if(crosshairVel.y > 0)
            {
                crosshairVel.y = 0;
            }
        }
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f))
        {
            if (crosshairVel.y < 0)
            {
                crosshairVel.y = 0;
            }
        }
        if (Physics2D.Raycast(transform.position, Vector3.right, 0.1f))
        {
            if (crosshairVel.x > 0)
            {
                crosshairVel.x = 0;
            }
        }
        if (Physics2D.Raycast(transform.position, Vector3.left, 0.1f))
        {
            if (crosshairVel.x < 0)
            {
                crosshairVel.x = 0;
            }
        }
        crosshair.GetComponent<Rigidbody2D>().velocity = crosshairVel * 5;
    }
    */
    void weaponOne()
    {
        if (Input.GetMouseButtonDown(0))
        {
            aimGunEndPoint = gunAnchor.Find("Knife");
            knifeAnim.SetTrigger("Fire");
            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                gunEndPointPosition = aimGunEndPoint.position,
                shootPosition = GetMouseWorldPosition()
            });
        }
    }

    void weaponTwo()
    {
        if(Input.GetMouseButtonDown(0))
        {
            flamethrowerAnim.SetTrigger("Fire");
        }
        if(Input.GetMouseButton(0))
        {
            aimGunEndPoint = gunAnchor.Find("Flambethrower");
            flamethrowerAnim.SetBool("IsFiring", true);
            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                gunEndPointPosition = aimGunEndPoint.position,
                shootPosition = GetMouseWorldPosition()
            });
        }
        else
        {
            flamethrowerAnim.SetBool("IsFiring", false);
        }
        //flamethrowerAnim;
    }

    void weaponThree()
    {
        if (Input.GetMouseButtonDown(0))
        {
            aimGunEndPoint = gunAnchor.Find("Shotgun").Find("GunEndPoint");
            Vector3 shootPoint = aimGunEndPoint.position;
            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3 shootDir = (mousePosition - transform.position).normalized;
            ShotgunHandler.RayShoot(shootPoint, shootDir);
            shotgunAnim.SetTrigger("Shoot");
            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                gunEndPointPosition = aimGunEndPoint.position,
                shootPosition = GetMouseWorldPosition()
            });
        }
        //spawn pellets from gun end point, need to construct prefabs for projectiles
    }

    private void PlayerShootProjectiles_OnShoot(object sender, OnShootEventArgs e)
    {

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
        gunAnchor.eulerAngles = new Vector3(0, 0, angle);
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;
        return worldPosition;
    }

    public void CheckAttack(){
        Quaternion rotato = new Quaternion(0,0,0,0);
        Vector3 Offset = transform.position + new Vector3(1,0,0);
        if(Input.GetButtonDown("Use")){
            //Object.Instantiate(hitbox, Offset, rotato);
        }
    }

}
