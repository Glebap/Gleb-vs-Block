using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]

public class SnakeHead : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private bool _isAlive = true;

    public event UnityAction<Vector2> BlockCollided;
    public event UnityAction<int> BonusCollected;
    public event UnityAction FinishCrossed;

    public bool IsAlive => _isAlive;


    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    public void Move(Vector3 newPosition)
    {
        _rigidbody2D.position  = newPosition;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Block block))
        {
            BlockCollided?.Invoke(new Vector2(transform.position.x, transform.position.y));
            block.Fill();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bonus bonus))
        {
            BonusCollected?.Invoke(bonus.Collect());
        }

        if (collision.gameObject.tag == "Finish")
         {
            FinishCrossed?.Invoke();
         }
    }

    private void OnDestroy()
    {
        _isAlive = false;
    }
}