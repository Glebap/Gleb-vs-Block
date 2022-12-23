using UnityEngine;

public class RoundEnd : MonoBehaviour
{
    private GameObject _parent;
    private float defaultScaleY;

    private void Start()
    {
        _parent = transform.parent.gameObject;
        defaultScaleY = transform.localScale.y;
    }
    private void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x,
                                           defaultScaleY / _parent.transform.localScale.y,
                                           transform.localScale.z);
    }
}
