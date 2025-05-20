using UnityEngine;

public class Vase : Obstacle
{
    [Header("Broken Vase")]
    public Sprite BrokenVaseSprite;

    private SpriteRenderer Sr;
    public bool Broken = false;

    void Start()
    {
        Sr = GetComponent<SpriteRenderer>();
    }

    public void UpdateBrokenVase()
    {
        Broken = true;
        Sr.sprite = BrokenVaseSprite;
    }
}
