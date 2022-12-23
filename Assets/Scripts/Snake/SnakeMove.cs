using UnityEngine;

public class SnakeMove : MonoBehaviour
{
    [SerializeField] private float _goalHeadDistance;

    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
    }

    public Vector2 GetDirectionClickPosition(Vector2 headPosition)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = _camera.ScreenToViewportPoint(touch.position);
            touchPosition.y = _camera.WorldToViewportPoint(headPosition).y + _goalHeadDistance;
            touchPosition = _camera.ViewportToWorldPoint(touchPosition);

            Vector2 direction = new Vector2(touchPosition.x - headPosition.x, touchPosition.y - headPosition.y);

            return direction;
        }

        return Vector3.up;
    }
}
