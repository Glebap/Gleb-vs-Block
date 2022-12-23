using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TailGenerator))]
[RequireComponent(typeof(SnakeMove))]

public class Snake : MonoBehaviour
{
    [SerializeField] private Restart _restart;
    [SerializeField] private float _speed;
    [SerializeField] private SnakeHead _head;
    [SerializeField] private int _tailSize;

    private List<Segment> _tail;
    private TailGenerator _tailGenerator;
    private SnakeMove _snakeMove;
    private float _tailSpringiness => _speed * 3.6f;
    private bool _move = true;

    public event UnityAction<int> SizeUpdated;
    public event UnityAction<Vector2> SnakeDestroyed;


    public void Awake()
    {
        _tailGenerator = GetComponent<TailGenerator>();
        _snakeMove = GetComponent<SnakeMove>();

        _tail = _tailGenerator.Generate(_tailSize);
        SizeUpdated?.Invoke(_tail.Count);
    }

    private void OnEnable()
    {
        _head.BlockCollided += OnBlockCollided;
        _head.BonusCollected += OnBonusCollected;
    }

    private void OnDisable()
    {
        _head.BlockCollided -= OnBlockCollided;
         _head.BonusCollected -= OnBonusCollected;
    }

    private void FixedUpdate()
    {
        if (_move)
        {
            _head.transform.up = _snakeMove.GetDirectionClickPosition(_head.transform.position);
            Move(_head.transform.position + _head.transform.up * _speed * Time.fixedDeltaTime);
        }
    }

    private void Move(Vector3 nextPosition)
    {
        Vector3 previousPosition = _head.transform.position;

        foreach(var segment in _tail)
        {
            Vector3 tempPosition = segment.transform.position;
            segment.transform.position = Vector2.Lerp(segment.transform.position, previousPosition, _tailSpringiness * Time.deltaTime);
            previousPosition = tempPosition;
        }

        _head.Move(nextPosition);
    }

    private void OnBlockCollided(Vector2 position)
    {
        if (_tail.Count == 0)
        {
            SnakeDestroyed?.Invoke(new Vector2(_head.transform.position.x, _head.transform.position.y));
            _move = false;
            Destroy(_head.gameObject);
            _restart.LoadGame();
            return;
        }

        Segment destroyedSegment = _tail[_tail.Count - 1];
        _tail.Remove(destroyedSegment);
        Destroy(destroyedSegment.gameObject);

        SizeUpdated?.Invoke(_tail.Count);
    }

    private void OnBonusCollected(int bonusSize)
    {
        _tail.AddRange(_tailGenerator.Generate(bonusSize));
        SizeUpdated?.Invoke(_tail.Count);
    }
}
