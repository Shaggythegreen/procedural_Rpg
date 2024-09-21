using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class PathfindingAgent : MonoBehaviour
{
    public Transform target, sprite;
    private Pathfinding pathfinding;
    private List<Pathfinding.Node> path;
    private int targetIndex;
    private bool isMoving = false;
    public BossState state;
    public float speed = 5;
    public LayerMask playerlayer;
    private bool isFacingRight = true; // To track the facing direction
    public Animator animator;
    public Tilemap floorMap;

    void Start()
    {
        pathfinding = FindObjectOfType<Pathfinding>();
    }

    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, 5, playerlayer))
        {
            target = Physics2D.OverlapCircle(transform.position, 5, playerlayer).gameObject.transform;
            StartPathfinding(target.position);
        }

        if (isMoving)
        {
            FollowPath();
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    void StartPathfinding(Vector2 targetPos)
    {
        path = pathfinding.FindPath(transform.position, targetPos);
        if (path != null && path.Count > 0)
        {
            targetIndex = 0;
            isMoving = true;
        }
    }

    void FollowPath()
    {
        if (targetIndex < path.Count)
        {
            Vector3Int nextTilePos = floorMap.WorldToCell(path[targetIndex].worldPosition);

            if (pathfinding.IsTileWalkable(nextTilePos))
            {
                Vector2 targetPos = path[targetIndex].worldPosition;
                transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);

                if (Vector2.Distance(transform.position, targetPos) < 0.1f)
                {
                    targetIndex++;
                }
            }
            else
            {
                // Path is blocked, stop movement
                isMoving = false;
            }
        }
        else
        {
            isMoving = false;
        }
    }

    // Flip the sprite based on movement direction
    private void Flip(float horizontal)
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = sprite.localScale;
            localScale.x *= -1f;
            sprite.localScale = localScale;
        }
    }
}