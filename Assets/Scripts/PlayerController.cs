using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float health = 100;
    public float horizontalMove;//x
    public float verticalMove;//y
    public CharacterController player;
    private Vector3 movePlayer;

    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    private Vector3 playerInput;

    public float gravity = 9.8f;
    public float fallVelocity;

    public float playerSpeed;
    public float jumpForce;
    public float playerRunVelocity;

    private bool aimPlayer = false;
    private float zoomNormality;
    public CinemachineFreeLook FreeLookCam;
    public GameObject aimCenterRed;

    public Animator playerAnimatorController;
    public GameObject gunPlayer;

    public float alcance = 200;

    public MobsController mobsController;
    public MoveZombie moveZombie;

    public GameObject shootGun;

    public Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        playerAnimatorController = GetComponent<Animator>();
        aimCenterRed.active = false;
        zoomNormality = FreeLookCam.m_Lens.FieldOfView;
        Cursor.lockState = CursorLockMode.Locked;//Ocultar el cursor al iniciar el juego
    }

    // Update is called once per frame
    void Update()
    {

        if (playerAnimatorController.GetBool("FlagShootRifle") && !Input.GetMouseButton(0))
        {
            playerAnimatorController.SetBool("FlagShootRifle", false);
        }

        healthSlider.GetComponent<Slider>().value = health;
        moveZombie.healthZombieSlider.value = moveZombie.health;

        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);


        camDirection();

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;

        movePlayer *= playerSpeed;

        player.transform.LookAt(player.transform.position + movePlayer);

        SetGravity();

        SetJump();


        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(1))
        {
            FreeLookCam.m_Lens.FieldOfView = zoomNormality - 16;
            aimPlayer = true;
            playerAnimatorController.SetBool("FlagAimPlayer", true);
            aimCenterRed.active = true;
            playerAnimatorController.SetBool("FlagRunRifle", true);
            playerAnimatorController.SetBool("FlagWalkRifle", false);
            float cameraRotationY = mainCamera.transform.eulerAngles.y;
            player.transform.rotation = Quaternion.Euler(0f, cameraRotationY, 0f);

            if (Input.GetMouseButton(0))
            {
                gunPlayer.transform.localPosition = new Vector3(-0.02138964f, 0.2930888f, 0.02545299f);
                gunPlayer.transform.localRotation = Quaternion.Euler(108.983f, 10.048f, 90.603f);
                playerAnimatorController.SetBool("FlagShootRifle", true);
                setAimPlayer();
            }
            else
            {
                gunPlayer.transform.localPosition = new Vector3(-0.021f, 0.293f, 0.03f);
                gunPlayer.transform.localRotation = Quaternion.Euler(93.02499f, 90f, 180f);
                playerAnimatorController.SetBool("FlagShootRifle", false);
            }

        }
        else if ((Input.GetButton("Vertical") || Input.GetButton("Horizontal")) && Input.GetMouseButton(1))
        {
            FreeLookCam.m_Lens.FieldOfView = zoomNormality - 16;
            aimPlayer = true;
            playerAnimatorController.SetBool("FlagAimPlayer", aimPlayer);
            aimCenterRed.active = true;
            playerAnimatorController.SetBool("FlagRunRifle", false);
            playerAnimatorController.SetBool("FlagWalkRifle", true);
            float cameraRotationY = mainCamera.transform.eulerAngles.y;
            player.transform.rotation = Quaternion.Euler(0f, cameraRotationY, 0f);

            if (Input.GetMouseButton(0))
            {
                gunPlayer.transform.localPosition = new Vector3(-0.02138964f, 0.2930888f, 0.02545299f);
                gunPlayer.transform.localRotation = Quaternion.Euler(108.983f, 10.048f, 90.603f);
                playerAnimatorController.SetBool("FlagShootRifle", true);
                setAimPlayer();
            }
            else
            {
                gunPlayer.transform.localPosition = new Vector3(-0.021f, 0.293f, 0.03f);
                gunPlayer.transform.localRotation = Quaternion.Euler(93.02499f, 90f, 180f);
                playerAnimatorController.SetBool("FlagShootRifle", false);
            }

        }
        else if (Input.GetMouseButton(1))
        {
            FreeLookCam.m_Lens.FieldOfView = zoomNormality - 16;
            aimPlayer = true;
            playerAnimatorController.SetBool("FlagAimPlayer", aimPlayer);
            aimCenterRed.active = true;
            playerAnimatorController.SetBool("FlagRunRifle", false);
            playerAnimatorController.SetBool("FlagWalkRifle", false);
            float cameraRotationY = mainCamera.transform.eulerAngles.y;
            player.transform.rotation = Quaternion.Euler(0f, cameraRotationY, 0f);

            if (Input.GetMouseButton(0))
            {
                gunPlayer.transform.localPosition = new Vector3(-0.02138964f, 0.2930888f, 0.02545299f);
                gunPlayer.transform.localRotation = Quaternion.Euler(108.983f, 10.048f, 90.603f);
                playerAnimatorController.SetBool("FlagShootRifle", true);
                setAimPlayer();
                setShootPlayer();
            }
            else
            {
                gunPlayer.transform.localPosition = new Vector3(-0.021f, 0.293f, 0.03f);
                gunPlayer.transform.localRotation = Quaternion.Euler(93.02499f, 90f, 180f);
                playerAnimatorController.SetBool("FlagShootRifle", false);
            }
            {

            }

        }
        else
        {
            FreeLookCam.m_Lens.FieldOfView = zoomNormality;
            aimPlayer = false;
            playerAnimatorController.SetBool("FlagAimPlayer", aimPlayer);
            playerAnimatorController.SetBool("FlagRunRifle", false);
            playerAnimatorController.SetBool("FlagWalkRifle", false);
            aimCenterRed.active = false;
        }


        if (Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Vertical"))
        {
            playerAnimatorController.SetFloat("PlayerRunVelocity", (playerInput.magnitude * playerRunVelocity));
            playerAnimatorController.SetFloat("PlayerWalkVelocity", 0);
            playerAnimatorController.SetBool("FlagRun", true);
            playerAnimatorController.SetBool("FlagWalkRelax", false);
            player.Move(new Vector3((movePlayer.x * Time.deltaTime) * 3, (movePlayer.y * Time.deltaTime * 1.5f), (movePlayer.z * Time.deltaTime) * 3));
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Horizontal"))
        {
            playerAnimatorController.SetFloat("PlayerRunVelocity", (playerInput.magnitude * playerRunVelocity));
            playerAnimatorController.SetFloat("PlayerWalkVelocity", 0);
            playerAnimatorController.SetBool("FlagRun", true);
            playerAnimatorController.SetBool("FlagWalkRelax", false);
            player.Move(new Vector3((movePlayer.x * Time.deltaTime) * 3, (movePlayer.y * Time.deltaTime * 1.5f), (movePlayer.z * Time.deltaTime) * 3));
        }
        else if (Input.GetKey(KeyCode.LeftAlt) && Input.GetButton("Vertical"))
        {
            playerAnimatorController.SetFloat("PlayerRunVelocity", 0);
            playerAnimatorController.SetFloat("PlayerWalkVelocity", playerInput.magnitude * playerSpeed);
            playerAnimatorController.SetBool("FlagWalkRelax", true);
            playerAnimatorController.SetBool("FlagRun", false);
            player.Move(new Vector3(movePlayer.x * Time.deltaTime, movePlayer.y * Time.deltaTime, movePlayer.z * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.LeftAlt) && Input.GetButton("Horizontal"))
        {
            playerAnimatorController.SetFloat("PlayerRunVelocity", 0);
            playerAnimatorController.SetFloat("PlayerWalkVelocity", playerInput.magnitude * playerSpeed);
            playerAnimatorController.SetBool("FlagWalkRelax", true);
            playerAnimatorController.SetBool("FlagRun", false);
            player.Move(new Vector3(movePlayer.x * Time.deltaTime, movePlayer.y * Time.deltaTime, movePlayer.z * Time.deltaTime));
        }
        else
        {
            playerAnimatorController.SetFloat("PlayerRunVelocity", 0);
            playerAnimatorController.SetFloat("PlayerWalkVelocity", playerInput.magnitude * playerSpeed);
            playerAnimatorController.SetBool("FlagWalkRelax", false);
            playerAnimatorController.SetBool("FlagRun", false);
            player.Move(new Vector3(movePlayer.x * Time.deltaTime, movePlayer.y * Time.deltaTime, movePlayer.z * Time.deltaTime));
        }


    }

    //Funcion del movimiento segun la camara
    void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;

    }

    //Funcion de la gravedad
    void SetGravity()
    {


        if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;

        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;

            playerAnimatorController.SetFloat("PlayerVerticalVelocity", player.velocity.y);
        }

        playerAnimatorController.SetBool("isGrounded", player.isGrounded);

    }

    //Funcion para saltar
    void SetJump()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = jumpForce;
            playerAnimatorController.SetTrigger("PlayerJump");
            playerAnimatorController.SetBool("isGrounded", player.isGrounded);
            playerAnimatorController.SetFloat("PlayerJumpForce", jumpForce);

        }
    }

    //Funcion para apuntar
    void setAimPlayer()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, alcance*2))
        {
            if (hit.collider.CompareTag("Mob"))
            {
                moveZombie.health -= 2;
            }
        }
    }

    void setShootPlayer()
    {

       

    }


    private void OnAnimatorMove()
    {

    }

}
