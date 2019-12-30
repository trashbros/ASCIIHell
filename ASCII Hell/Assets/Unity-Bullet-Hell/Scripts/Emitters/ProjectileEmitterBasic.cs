using UnityEngine;

namespace BulletHell
{
    // Most basic emitter implementation
    public class ProjectileEmitterBasic : ProjectileEmitterBase
    {
        public new void Start()
        {
            base.Start();
        }

        public override void FireProjectile(Vector2 direction, float leakedTime)
        {
            Pool<ProjectileData>.Node node = Projectiles.Get();

            node.Item.Position = transform.position;
            node.Item.Scale = Scale;
            node.Item.TimeToLive = TimeToLive - leakedTime;
            node.Item.Direction = direction.normalized;
            node.Item.Velocity = Speed * Direction.normalized * (UseSlowDownTime ? (m_slowTime ? m_speedSlowPercent : 1f) : 1f);
            node.Item.Position += node.Item.Velocity * leakedTime;
            node.Item.Color = new Color(0.6f, 0.7f, 0.6f, 1);
            node.Item.Acceleration = Acceleration;

            Direction = Rotate(Direction, RotationSpeed);
        }

        public override void ProjectileHitEvent(RaycastHit2D[] RaycastHitBuffer)
        {
            foreach (var rayhit in RaycastHitBuffer)
            {
                var coll = rayhit.collider.gameObject.GetComponent<ICollidable>();
                if (coll != null)
                {
                    coll.OnHit(this.gameObject);
                }
            }
        }

        public new void UpdateEmitter()
        {
            base.UpdateEmitter();
        }
    }
}