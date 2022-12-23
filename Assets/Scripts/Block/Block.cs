using UnityEngine;
using UnityEngine.Events;

public class Block : DestroyableObject
{
    [SerializeField] private Vector2Int _destroySizeRange;

    private int _destroySize;
    private int _filled;

    public int LeftToFill => _destroySize - _filled;
    public event UnityAction<int> SizeChanged;

    private void Awake()
    {
        _destroySizeRange.x += GameObject.FindObjectOfType<Spawner>().BlocksCreated;
        _destroySizeRange.y += GameObject.FindObjectOfType<Spawner>().BlocksCreated;
    }
    private void Start()
    {
        SetColor();
        _destroySize = Random.Range(_destroySizeRange.x, _destroySizeRange.y);
        SizeChanged?.Invoke(LeftToFill);
    }

    public void Fill()
    {
        _filled++;
        SizeChanged?.Invoke(LeftToFill);

        if (_filled >= _destroySize)
        {
            Destroy(gameObject);
        }
    }

    private void SetColor()
    {
        int[] rgb = {255,255,255};
        int constant = Random.Range(0,3);
        for (int i = 0; i < 3; i++)
        {
            if(i == constant){continue;}
            else{rgb[i] = Random.Range(150,256);}
        }

        GetComponent<SpriteRenderer>().color = new Color32((byte)rgb[0],
                                                  (byte)rgb[1],
                                                  (byte)rgb[2],
                                                    255);
    }
}
