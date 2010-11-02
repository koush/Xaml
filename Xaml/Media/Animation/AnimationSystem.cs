using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;
using System.Threading;

namespace System.Windows.Media.Animation
{
    class AnimationSystem : DispatcherObject
    {
        List<Clock> myClocks = new List<Clock>();
        DispatcherTimer myTimer;

        void TimerHandler(object sender, EventArgs e)
        {
            Animate();
        }

        public void AddClock(Clock clock)
        {
            myClocks.Add(clock);
            if (myClocks.Count == 1)
            {
                myLastTick = Environment.TickCount;
                myTimer.Start();
            }
        }

        int myLastTick = Environment.TickCount;
        void Animate()
        {
            int tickCount = Environment.TickCount;
            int timeElapsed = tickCount - myLastTick;
            myLastTick = tickCount;

            var clocks = myClocks.ToArray();
            foreach (Clock clock in clocks)
            {
                clock.StepTime(timeElapsed);
            }

            if (myClocks.Count == 0)
                myTimer.Stop();
        }

        public void RemoveClock(Clock clock)
        {
            myClocks.Remove(clock);
        }

        public static AnimationSystem CurrentAnimationSystem
        {
            get
            {
                return ThreadStatic<AnimationSystem>.GetOrCreateThreadStatic(Create);
            }
        }

        private AnimationSystem()
        {
            myTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(40), TimerHandler, Dispatcher.CurrentDispatcher);
        }

        static AnimationSystem Create()
        {
            return new AnimationSystem();
        }
    }
}