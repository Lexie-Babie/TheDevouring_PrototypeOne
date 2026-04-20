using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class PlayerPiece : MonoBehaviour
{
    public static PlayerPiece Instance;

    public bool IsMoving { get; private set; }

    private Vector3 restPosition;

    void Awake()
    {
        Instance = this;
    }

    //snaps to position 
    public void SnapTo(Vector3 position)
    {
        restPosition = position;
        transform.position = position;
    }

    //animation of movement
    public void MoveTo(Vector3 target, System.Action onArrived = null)
    {
        if (IsMoving) StopAllCoroutines();
        StartCoroutine(MoveRoutine(this, target, onArrived));
    }
    IEnumerator MoveRoutine(PlayerPiece @this, Vector3 target, System.Action onArrived)
    {
        @this.IsMoving = true;

        Vector3 start = @this.transform.position;
        float duration = 0.45f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float smooth = t * t * (3f - 2f * t);
            yield return null;
        }

        @this.
                    transform.position = target;
        @this.IsMoving = false;

        onArrived?.Invoke();
    }
}



            