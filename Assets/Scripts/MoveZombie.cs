using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MoveZombie : MonoBehaviour
{
    public CharacterController mob;
    public Transform mobTransform;
    public Transform playerTransform;
    public Slider healthZombieSlider;
    public float health = 1000;
    public float gravity = 9.8f;
    public float fallVelocity;
    public float playerSpeed;
    public float jumpForce;
    public float playerRunVelocity;
    public float respawnTime = 10.0f;

    public bool rangeDetected;

    private Vector3 PrincipalPosition;
    private Vector3 PrincipalPositionCH;
    private bool isDead = false;
    private float respawnTimer = 0.0f;

    public MobsController mobsController;

    // Start is called before the first frame update
    void Start()
    {
        PrincipalPosition = mobTransform.position;
        PrincipalPositionCH = mob.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        healthZombieSlider.value = health;

        if (isDead)
        {
            respawnTimer += Time.deltaTime;
            if (respawnTimer >= respawnTime)
            {
                Respawn();
            }
            return;
        }

        SetGravity();

        if (health <= 0)
        {
            mobsController.PrisonerAnimator.SetBool("FlagZombieIsLive", false);
            isDead = true;
            mobsController.enabled = false;
            PrincipalPosition = mobTransform.position;
            PrincipalPositionCH = mob.transform.position;
        }
        else
        {
            mobsController.PrisonerAnimator.SetBool("FlagZombieIsLive", true);
        }
    }

    void SetGravity()
    {
        if (mob.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
        }

        mob.Move(Vector3.up * fallVelocity * Time.deltaTime);
    }

    void Respawn()
    {
        health = 1000;
        mobsController.PrisonerAnimator.SetBool("FlagZombieIsLive", true);
        mob.transform.position = PrincipalPositionCH;
        mobTransform.position = PrincipalPosition;
        mobsController.enabled = true;
        isDead = false;
        respawnTimer = 0.0f; 
    }
}
