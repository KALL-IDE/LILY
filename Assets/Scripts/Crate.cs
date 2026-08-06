using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Crate : MonoBehaviour
{
    public float moveDuration = 0.12f;
    private bool isMoving = false;

    public bool TryPush(Vector2Int dir)
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
        else
        {
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
