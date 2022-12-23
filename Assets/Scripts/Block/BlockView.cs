using UnityEngine;
using TMPro;


[RequireComponent(typeof(Block))]
public class BlockView : MonoBehaviour
{
    [SerializeField] private TMP_Text _view;

    private Block _block;

    private void Awake()
    {
        _block = GetComponent<Block>();
    }

    private void OnEnable()
    {
        _block.SizeChanged += OnFillingUpdate;
    }

    private void OnDisable()
    {
        _block.SizeChanged -= OnFillingUpdate;
    }

    private void OnFillingUpdate(int leftToFill)
    {
        _view.text = leftToFill.ToString();
    }


}
