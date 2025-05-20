using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

public class Cube : Block
{
    [Header("Sprites")]
    public Sprite DefaultSprite;
    public Sprite RocketSprite;

    private SpriteRenderer Sr;

    [Header("Particle")]
    public GameObject Particle;
    

    void Awake()
    {
        Sr = GetComponent<SpriteRenderer>();
    }

    public void SetRocketMode(bool IsRocketState)
    {
        if (Sr == null) Sr = GetComponent<SpriteRenderer>();
        Sr.sprite = IsRocketState ? RocketSprite : DefaultSprite;
    }


    public void PlayDestroySequence()
    {
        float duration = 0.1f;

        Tween.Scale(transform, Vector3.zero, duration)
            .OnComplete(() =>
            {
                SpawnParticles();
                Destroy(gameObject);
            });
    }

    private void SpawnParticles()
    {
        Vector3 origin = transform.position;

        for (int ParticleCount = 0; ParticleCount < 3; ParticleCount++)
        {
            GameObject P = Instantiate(Particle, origin, Quaternion.identity);
            Transform Pt = P.transform;

            Pt.localScale = Vector3.one * 0.3f;

            Vector2 RandomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-0.5f, -1.5f)).normalized;
            float Distance = Random.Range(0.5f, 1f);
            Vector3 TargetPos = Pt.position + (Vector3)(RandomDir * Distance);

            Tween.Position(Pt, TargetPos, 0.4f, Ease.Linear);
            Tween.Scale(Pt, Vector3.zero, 0.3f, Ease.Linear, (int)0f, (CycleMode)0.4f).OnComplete(() => Destroy(P));
        }
    }
    

}