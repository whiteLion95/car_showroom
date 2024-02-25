using UnityEngine;

namespace Utils
{
    public class TimeManager : Singleton<TimeManager>
    {
        [SerializeField] private TimeData _data;
        [SerializeField] private float timeScale;

        public bool TimeBack { get; set; }

        private void Update()
        {
            if (TimeBack)
            {
                Time.timeScale += 1f / _data.SlowDownLength * Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Clamp01(Time.timeScale);

                if (Time.timeScale == 1f)
                    TimeBack = false;
            }

            timeScale = Time.timeScale;
        }

        public void DoSlowMotion(bool andBack)
        {
            Time.timeScale = _data.SlowDownFactor;
            Time.fixedDeltaTime = Time.timeScale * .02f;

            if (andBack)
                TimeBack = true;
        }
    }
}