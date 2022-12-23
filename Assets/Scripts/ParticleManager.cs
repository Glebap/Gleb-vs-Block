using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private SnakeHead _head;
    [SerializeField] private Snake _snake;
    [SerializeField] private ParticleSystem _segmentDestroyParticle;
    [SerializeField] private ParticleSystem _headDestroyParticle;
    [SerializeField] private ParticleSystem _finishParticle;

    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
    }


    private void OnEnable()
    {
        _head.BlockCollided += OnBlockCollided;
        _head.FinishCrossed += OnFinishCrossed;
        _snake.SnakeDestroyed += OnSnakeDestroyed;
    }

    private void OnDisable()
    {
        _head.BlockCollided -= OnBlockCollided;
        _head.FinishCrossed -= OnFinishCrossed;
        _snake.SnakeDestroyed += OnSnakeDestroyed;
    }

    private void OnBlockCollided(Vector2 position)
    {
        PlayParticle(_segmentDestroyParticle, position, 1.0f);
    }

    private void OnSnakeDestroyed(Vector2 position)
    {
        PlayParticle(_headDestroyParticle, position, 1.0f);
    }

    private void OnFinishCrossed()
    {
        StartCoroutine(FinishParticlesPlay(12));
    }

    private IEnumerator FinishParticlesPlay(int quantity)
    {
        for (int i = 1; i < quantity; i++)
        {
            PlayParticle(_finishParticle, _camera.ViewportToWorldPoint(new Vector2(Random.Range(20, 80)/100.0f, 0.8f)), 3.0f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void PlayParticle(ParticleSystem particle, Vector2 position, float lifeTime)
    {
        ParticleSystem particleInstance = Instantiate(particle,
                                                      new Vector3(position.x, position.y, transform.position.z),
                                                      Quaternion.identity, transform);
        Destroy(particleInstance.gameObject, lifeTime);
    }
}
