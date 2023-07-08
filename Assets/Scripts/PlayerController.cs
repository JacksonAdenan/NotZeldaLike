using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.4f;
    private Transform player;


    [HideInInspector]
    public bool isAttacking = false;

    private float attackCounter = 0.0f;

    public BoxCollider meleeZone;

    private CharacterController characterController;

    private PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject.transform;
        //meleeZone = this.gameObject.transform.Find("MeleeZone").GetComponent<BoxCollider>();
        
        characterController = gameObject.GetComponent<CharacterController>();

        playerManager = PlayerManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        float xTranslation = Input.GetAxis("Horizontal");
        float yTranslation = Input.GetAxis("Vertical");

        if (Input.GetMouseButton(1))
        {
            Vector3 movement = transform.forward;
            //movement = Vector3.Normalize(movement);

            //player.position += movement * speed * Time.deltaTime;
            movement *= speed;
            //playerRigidbody.AddForce(movement, ForceMode.Impulse);
            characterController.Move(movement * speed * Time.deltaTime);

            // Do running animation
            playerManager.SetAnimation(PlayerManager.PlayerAnimation.Running);
        }
        /*if (xTranslation != 0 || yTranslation != 0)
        {
            // We put yTranslation in the z value because in our game, y means going into the sky.
            Vector3 movement = new Vector3(xTranslation, 0, yTranslation).normalized;
            //movement = Vector3.Normalize(movement);

            //player.position += movement * speed * Time.deltaTime;
            movement *= speed;
            //playerRigidbody.AddForce(movement, ForceMode.Impulse);
            player.transform.position += movement * speed * Time.deltaTime;

            // Do running animation
            playerManager.SetAnimation(PlayerManager.PlayerAnimation.Running);
            //playerManager.playerAnimator.SetBool("Running", true);
            //playerManager.playerAnimator.SetBool("Idle", false);

        }*/
        else
        {
            if (playerManager.health <= 3)
            {
                playerManager.SetAnimation(PlayerManager.PlayerAnimation.Idle_Low);
            }
            else
                playerManager.SetAnimation(PlayerManager.PlayerAnimation.Idle);
            
        }

        LookAtMouse();

        PollAttack();
        AttackTimer();
    }

    void LookAtMouse()
    {
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        Vector3 testPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //testPoint = Camera.main.transform.worldToLocalMatrix * Camera.main.projectionMatrix.inverse * testPoint;
        //Ray cameraRay = new Ray(testPoint, Camera.main.transform.forward);
        //Gizmos.DrawRay(cameraRay);

        Ray cameraRay = ViewportPointToRay(testPoint);

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Vector3 cachedRot = transform.rotation.eulerAngles;
            transform.LookAt(new Vector3(pointToLook.x, pointToLook.y + 1, pointToLook.z));
            transform.eulerAngles = new Vector3(cachedRot.x, transform.eulerAngles.y, cachedRot.z);
        }
    }

    void PollAttack()
    {
        //meleeZone.gameObject.SetActive(false);
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        { 
            meleeZone.gameObject.SetActive(true);
            isAttacking = true;

            playerManager.SetAnimation(PlayerManager.PlayerAnimation.Slash);
        }
    }


    // Even though attacks will be pretty much instant. I'm not sure if setting a collider to true and false within the same update frame will be registed by things so I've made a timer of like
    // .1 of a second just to make sure things register the attack.
    void AttackTimer()
    {
        if (isAttacking)
        {
            attackCounter += Time.deltaTime;
            if (attackCounter >= playerManager.attackAnimLength)
            {
                isAttacking = false;
                meleeZone.gameObject.SetActive(false);
                attackCounter = 0;
            }
        }
    }

	private void OnDrawGizmos()
	{
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        Vector3 testPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //testPoint = Camera.main.transform.worldToLocalMatrix * Camera.main.projectionMatrix.inverse * testPoint;
        //Ray cameraRay = new Ray(testPoint, Camera.main.transform.forward);
        //Gizmos.DrawRay(cameraRay);

        Ray cameraRay = ViewportPointToRay(testPoint);

        /*var viewportToWorldMatrix = (Camera.main.projectionMatrix * Camera.main.worldToCameraMatrix).inverse;
        var rayOrigin = viewportToWorldMatrix.MultiplyPoint(Input.mousePosition);

        var endPosition = Input.mousePosition;
        endPosition.z = 1f;
        var rayEnd = viewportToWorldMatrix.MultiplyPoint(endPosition);

        Ray cameraRay = new Ray(rayOrigin, rayEnd - rayOrigin);

        Gizmos.matrix = viewportToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);*/



        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pointToLook, 0.25f);
        }
    }
    public Ray ViewportPointToRay(Vector3 position)
    {
        position.x = (position.x - 0.5f) * 2f;
        position.y = (position.y - 0.5f) * 2f;
        position.z = -1f;

        var viewportToWorldMatrix = (Camera.main.projectionMatrix * Camera.main.worldToCameraMatrix).inverse;

        var rayOrigin = viewportToWorldMatrix.MultiplyPoint(position);

        var endPosition = position;
        endPosition.z = 1f;
        var rayEnd = viewportToWorldMatrix.MultiplyPoint(endPosition);

        return new Ray(rayOrigin, rayEnd - rayOrigin);
    }
}
