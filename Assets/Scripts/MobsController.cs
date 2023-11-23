using System.Collections;
using UnityEngine;

public class MobsController : MonoBehaviour
{
    public Transform playerTransform;
    public CharacterController player;
    public CharacterController mob;
    public float playerSpeed;
    public float alcance;
    private bool rangeDetected;
    public Animator PrisonerAnimator;
    private bool isDamaging = false;
    private bool isLive = true;

    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mob"))
        {
            rangeDetected = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mob"))
        {
            rangeDetected = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rangeDetected && !isDamaging && isLive)
        {
            PrisonerAnimator.SetBool("FlagPlayerDetected", true);
            Vector3 direction = player.transform.position - mob.transform.position;
            direction.y = 0f;
            direction.Normalize();
            mob.Move(direction * (playerSpeed * Time.deltaTime) * 2.5f);
            mob.transform.LookAt(player.transform.position);

            float distance = Vector3.Distance(mob.transform.position, player.transform.position);
            if (distance < alcance)
            {
                PrisonerAnimator.SetBool("FlagTouchPlayer", true);
                StartCoroutine(DamagePlayer());
            }
            else
            {
                PrisonerAnimator.SetBool("FlagTouchPlayer", false);
            }
        }
        else
        {
            PrisonerAnimator.SetBool("FlagPlayerDetected", false);
        }

        if (playerController.health <= 0 && isLive)
        {
            playerController.healthSlider.value = playerController.health;
            isLive = false;
            playerController.playerAnimatorController.SetBool("FlagIsLive", false);
            PrisonerAnimator.SetBool("FlagIsLive", false);
            player.enabled = false;
            playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y - 2.5f, playerTransform.position.z);
            playerController.enabled = false;
            this.enabled = false;
        }
    }

    IEnumerator DamagePlayer()
    {
        if (playerController != null)
        {
            isDamaging = true;
            playerController.health -= 20;
            yield return new WaitForSeconds(1.6f);
            isDamaging = false;
        }
    }

    private void OnAnimatorMove()
    {
        // Puedes agregar lógica adicional para el movimiento del mob aquí.
    }
}
