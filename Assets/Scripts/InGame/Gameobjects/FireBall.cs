using UnityEngine;

public class FireBall : Bullet
{
    [SerializeField]
    protected float attackRadius = 5;
    [SerializeField]
    protected LayerMask layerMask;

    public override float GetDamage()
    {
        AttackArea();
        Disintegrate();
        return 0;
    }

    public void AttackArea()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius, layerMask);
        foreach (Collider2D item in colliders)
        {
            Asteroid asteroid = item.gameObject.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                float distance = Vector3.Distance(transform.position, asteroid.transform.position);
                asteroid.SetDamage(damage - distance);
            }
        }
    }
}