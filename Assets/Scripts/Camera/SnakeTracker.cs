using UnityEngine;

public class SnakeTracker : MonoBehaviour
{
    [SerializeField] private SnakeHead _snakeHead;
    [SerializeField] private float _speed;
    [SerializeField] private float _offsetMultiplier;

    private float _offset;

    private void Start()
    {
        _offset = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, _offsetMultiplier)).y;
    }


    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, GetTargetPosition(), _speed * Time.fixedDeltaTime);
    }

    private Vector3 GetTargetPosition()
    {
        if (_snakeHead != null)
        {
            return new Vector3(transform.position.x,
                               _snakeHead.transform.position.y - _offset,
                               transform.position.z);
        }
        return transform.position;
        
    }
}
