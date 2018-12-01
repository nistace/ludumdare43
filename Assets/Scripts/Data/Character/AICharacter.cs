using UnityEngine;

[RequireComponent(typeof(Shooter))]
public class AICharacter : MonoBehaviour
{
	public IBulletDamageable target;
	public CharacterVisibleArea sight;
	private Shooter shooter;

	public void Awake()
	{
		this.shooter = this.GetComponent<Shooter>();
	}

	public void Update()
	{
		if (this.target != null && this.target.IsDestroyed())
			this.target = null;
		this.target = this.sight.Closest;
		if (this.target != null)
			this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, ((Vector2)this.target.Transform().position - (Vector2)this.transform.position).AngleWithVector2Up()));
		this.shooter.shooting = this.target != null &&
			Physics2D.Raycast(this.shooter.bulletSpawnPosition.position,
				  this.target.Transform().position - this.shooter.bulletSpawnPosition.position,
				  Vector2.Distance(this.target.Transform().position, this.transform.position), 1 << LayerMask.NameToLayer("TeamMember")).collider == null;
	}

}