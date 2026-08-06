using UnityEngine;
using UnityEngine.EventSystems;

public class MobileMoveButton : MonoBehaviour, IPointerDownHandler
{
    public int dx = 0; // set -1/0/1 in Inspector
    public int dy = 0;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;
        var pc = player.GetComponent<PlayerController>();
        if (pc == null) return;
        pc.TryMove(new Vector2Int(dx, dy));
    }
}
