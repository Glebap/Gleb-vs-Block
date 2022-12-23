using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SnakeHead _head;

    private void OnEnable()
    {
        _head.FinishCrossed += OnFinishCrossed;
    }

    private void OnDisable()
    {
        _head.FinishCrossed -= OnFinishCrossed;
    }

    private void OnFinishCrossed()
    {
        StartCoroutine(RestartGame());
    }

    public void LoadGame()
    {
        _animator.SetTrigger("FadeOut");
    }

    private void OnFadeComplete()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2.0f);
        LoadGame();
    }
}
