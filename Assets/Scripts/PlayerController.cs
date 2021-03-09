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

    private BoxCollider meleeZone;

    private Rigidbody playerRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject.transform;
        meleeZone = this.gameObject.transform.Find("MeleeZone").GetComponent<BoxCollider>();

        playerRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float xTranslation = Input.GetAxis("Horizontal");
        float yTranslation = Input.GetAxis("Vertical");
        if (xTranslation != 0 || yTranslation != 0)
        {
            // We put yTranslation in the z value because in our game, y means going into the sky.
            Vector3 movement = new Vector3(xTranslation, 0, yTranslation).normalized;
            //movement = Vector3.Normalize(movement);

            //player.position += movement * speed * Time.deltaTime;
            movement *= speed;
            playerRigidbody.AddForce(movement, ForceMode.VelocityChange);

        }

        LookAtMouse();

        PollAttack();
        AttackTimer();
    }

    void LookAtMouse()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    void PollAttack()
    {
        //meleeZone.gameObject.SetActive(false);
        if (Input.GetMouseButtonDown(0))
        { 
            meleeZone.gameObject.SetActive(true);
            isAttacking = true;
        }
    }


    // Even though attacks will be pretty much instant. I'm not sure if setting a collider to true and false within the same update frame will be registed by things so I've made a timer of like
    // .1 of a second just to make sure things register the attack.
    void AttackTimer()
    {
        if (isAttacking)
        {
            attackCounter += Time.deltaTime;
            if (attackCounter >= 0.3f)
            {
                isAttacking = false;
                meleeZone.gameObject.SetActive(false);
                attackCounter = 0;
            }
        }
    }

	


}
