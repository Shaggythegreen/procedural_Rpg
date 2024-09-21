using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player_Handler : MonoBehaviour
{
    public Rigidbody2D rb;
   [SerializeField] private float horizontal, vertical;
    public float speed = 2f;
    private bool isFacingRight = true;
    public Transform orientation;
    public Animator HeadAnimator,BodyAnimator;
   
    void Update()
    {



     

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (horizontal > .1f && Input.GetKeyDown(KeyCode.E))
        {
            Vector3 pos = transform.position;
            pos.z = 0;
            Vector3Int MapPos = new Vector3Int(Mathf.FloorToInt(pos.x) + 1, Mathf.FloorToInt(pos.y), 0);
            Tilemap Cliffs = GameObject.FindGameObjectWithTag("Cliff_Map").GetComponent<Tilemap>();
            if (Cliffs != null)
            {
                if (Cliffs.GetTile(MapPos) != null)
                {
                    Debug.Log("right");
                    transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                }
            }
        }

        if (horizontal < -.1f && Input.GetKeyDown(KeyCode.E))
        {
            Vector3 pos = transform.position;
            pos.z = 0;
            Vector3Int MapPos = new Vector3Int(Mathf.FloorToInt(pos.x) - 1, Mathf.FloorToInt(pos.y), 0);
            Tilemap Cliffs = GameObject.FindGameObjectWithTag("Cliff_Map").GetComponent<Tilemap>();
            if (Cliffs != null)
            {
                if (Cliffs.GetTile(MapPos) != null)
                {
                    Debug.Log("left");
                    transform.position = new Vector2(transform.position.x -1 , transform.position.y);
                }
            }
        }

        if (vertical < -.1f && Input.GetKeyDown(KeyCode.E))
        {
            Vector3 pos = transform.position;
            pos.z = 0;
            Vector3Int MapPos = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y) - 1, 0);
            Tilemap Cliffs = GameObject.FindGameObjectWithTag("Cliff_Map").GetComponent<Tilemap>();
            if (Cliffs != null)
            {
                if (Cliffs.GetTile(MapPos) != null)
                {
                    Debug.Log("down");
                    transform.position = new Vector2(transform.position.x , transform.position.y - 1);
                }
            }
        }
        if (vertical > .1f && Input.GetKeyDown(KeyCode.E))
        {
            Vector3 pos = transform.position;
            pos.z = 0;
            Vector3Int MapPos = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y) + 1, 0);
            Tilemap Cliffs = GameObject.FindGameObjectWithTag("Cliff_Map").GetComponent<Tilemap>();
            if (Cliffs != null)
            {
                if (Cliffs.GetTile(MapPos) != null)
                {
                    Debug.Log("up");
                    transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            HeadAnimator.SetTrigger("Tool_Use");
        }


        if (horizontal > .1f || horizontal < -.1f || vertical > .1f || vertical < -.1f)
        {
  

            BodyAnimator.SetBool("Walking", true);
            HeadAnimator.SetBool("Walking", false);
        }
        if (horizontal == 0 && vertical == 0)
        {
            
            BodyAnimator.SetBool("Walking", false);
            HeadAnimator.SetBool("Walking", false);
        }
        Flip();
    }

    private void FixedUpdate()
    {
        
            rb.velocity = new Vector2(horizontal * speed, vertical * speed);
       
        

    }

  
    public void Climb_Wall(Vector2 Location)
    {

    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
