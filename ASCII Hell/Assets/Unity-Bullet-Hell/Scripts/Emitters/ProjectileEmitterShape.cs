﻿using UnityEngine;
using System.Collections.Generic;

namespace BulletHell
{
    // Emitter to fire pre-defined shape patterns
    public class ProjectileEmitterShape : ProjectileEmitterBase
    {
        [SerializeField]
        public GameObject ShapeTemplate;
        private List<Vector3> TemplatePositions;

        public override void Awake()
        {
            base.Awake();

            if (ShapeTemplate == null)
            {
                ShapeTemplate = Resources.Load<GameObject>(@"ShapeTemplates\CircleShape");
            }

            TemplatePositions = new List<Vector3>();
            foreach (Transform child in ShapeTemplate.transform)
            {
                TemplatePositions.Add(child.transform.position);
            }
        }

        public override void Start()
        {
            base.Start();
        }

        public override void FireProjectile(Vector2 direction, float leakedTime)
        {
            if (Projectiles.AvailableCount >= TemplatePositions.Count)
            {
                for (int n = 0; n < TemplatePositions.Count; n++)
                {
                    Pool<ProjectileData>.Node node = Projectiles.Get();

                    node.Item.Position = transform.position + TemplatePositions[n];
                    node.Item.Scale = Scale;
                    node.Item.TimeToLive = TimeToLive - leakedTime;
                    node.Item.Direction = direction.normalized;
                    node.Item.Velocity = Speed * Direction.normalized;// * (UseSlowDownTime ? (m_slowTime ? m_speedSlowPercent : 1f) : 1f);
                    node.Item.Position += node.Item.Velocity * leakedTime;
                    node.Item.Color = new Color(0.6f, 0.7f, 0.6f, 1);
                    node.Item.Acceleration = Acceleration;
                    node.Item.UseSlowDownTime = UseSlowDownTime;
                }

                Direction = Rotate(Direction, RotationSpeed);
            }
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