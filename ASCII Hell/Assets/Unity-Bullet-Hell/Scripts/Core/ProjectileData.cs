using UnityEngine;

namespace BulletHell
{
    public class ProjectileData
    {
        public Vector2 Direction;

        private Vector2 m_velocity;
        public Vector2 Velocity
        {
            get { return m_velocity * (UseSlowDownTime ? (m_slowTime ? m_speedSlowPercent : 1f) : 1f); }
            set { m_velocity = value; }
        }
        
        public float Acceleration;
        public Vector2 Gravity;
        public Vector2 Position;
        public float Rotation;
        public Color Color;
        public float Scale;
        public float TimeToLive;
        
        public bool UseSlowDownTime = true;

        private float m_speedSlowPercent = 0.5f;
        private bool m_slowTime = false;


        public ProjectileData()
        {
            m_speedSlowPercent = GameplayParameters.instance.SlowDownPercent;
            CustomEvents.EventUtil.AddListener(CustomEventList.PARAMETER_CHANGE, OnParameterChange);
            CustomEvents.EventUtil.AddListener(CustomEventList.SLOW_TIME, OnSlowTime);
        }

        public void UpdateVelocity(float tick)
        {
            // apply acceleration
            m_velocity *= (1 + Acceleration * tick);

            // apply gravity
            m_velocity += Gravity * tick;
        }

        protected void OnParameterChange(CustomEvents.EventArgs evt)
        {
            m_speedSlowPercent = GameplayParameters.instance.SlowDownPercent;
        }

        protected void OnSlowTime(CustomEvents.EventArgs evt)
        {
            m_slowTime = (bool)evt.args.GetValue(0);
        }

        ~ProjectileData()
        {
            CustomEvents.EventUtil.RemoveListener(CustomEventList.PARAMETER_CHANGE, OnParameterChange);
            CustomEvents.EventUtil.RemoveListener(CustomEventList.SLOW_TIME, OnSlowTime);
        }
    }
}