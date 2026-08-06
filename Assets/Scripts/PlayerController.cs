using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    public float moveDuration = 0.12f; // movement speed between grid cells
    private bool isMoving = false;

    void Update()
    {
        if (isMoving) return;

        Vector2Int dir = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) dir = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) dir = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) dir = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) dir = Vector2Int.right;
        else return;

        TryMove(dir);
    }

    // Make this public so UI buttons / touch can call it too
    public bool TryMove(Vector2Int dir)
    {
        if (isMoving) return false;

        Vector2Int currentGrid = Vector2Int.RoundToInt(transform.position);
        Vector2Int targetGrid = currentGrid + dir;
        Vector2 targetPos = (Vector2)targetGrid;

        Collider2D hit = Physics2D.OverlapPoint(targetPos);
        if (hit == null)
        {
            StartCoroutine(MoveTo(targetPos));
            return true;
        }
        else if (hit.CompareTag("Crate"))
        {
            Crate crate = hit.GetComponent<Crate>();
            if (crate != null)
            {
                bool pushed = crate.TryPush(dir);
                if (pushed)
                {
                    StartCoroutine(MoveTo(targetPos));
                    return true;
                }
                else
                {
                    GameManager.Instance.TriggerExplosion();
                    return false;
                }
            }
            else
            {
                GameManager.Instance.TriggerExplosion();
                return false;
            }
        }
        else
        {
            GameManager.Instance.TriggerExplosion();
            return false;
        }
    }

    IEnumerator MoveTo(Vector2 targetPos)
    {
        isMoving = true;
        Vector2 start = transform.position;
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            transform.position = Vector2.Lerp(start, targetPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;

        GameManager.Instance.CheckWinCondition();
    }
}
