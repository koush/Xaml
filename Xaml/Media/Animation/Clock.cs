using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

namespace System.Windows.Media.Animation
{
    public enum ClockState
    {
        Active,
        Stopped,
        Paused,
    }

    public class Clock 
    {
        AnimationSystem mySystem = AnimationSystem.CurrentAnimationSystem;

        internal void StepTime(int milliseconds)
        {
            if (myClockState != ClockState.Active)
                return;
            myCurrentGlobalTime += milliseconds;
            OnClockChanged();
            if (myCurrentGlobalTime >= myTotalTime)
            {
                myClockState = ClockState.Stopped;
                mySystem.RemoveClock(this);
            }
        }

        protected virtual void OnClockChanged()
        {
        }

        public int? CurrentIteration
        {
            get
            {
                if (myClockState == ClockState.Stopped)
                    return null;
                return (int)Math.Floor(myCurrentGlobalTime / myNaturalDuration.TimeSpan.TotalMilliseconds);
            }
        }

        bool myAutoReverse;
        public float? CurrentProgress
        {
            get
            {
                if (myClockState == ClockState.Stopped)
                    return null;
                if (myNaturalDuration == Duration.Forever)
                    return 0;
                System.Diagnostics.Debug.Assert(myNaturalDuration != Duration.Automatic);
                int totalMilliseconds = (int)myNaturalDuration.TimeSpan.TotalMilliseconds;
                float time = myCurrentGlobalTime;
                // cap the time to the maximum length of the animation
                if (time > myTotalTime)
                    time = myTotalTime;
                int rollOver = (int)time % totalMilliseconds;
                if (myAutoReverse)
                {
                    if (CurrentIteration.Value % 2 != 0)
                        rollOver = totalMilliseconds - rollOver;
                }
                else
                {
                    if (rollOver == 0)
                        rollOver = totalMilliseconds;
                }
                return (float)rollOver / (float)totalMilliseconds;
            }
        }

        public TimeSpan? CurrentTime
        {
            get
            {
                if (myClockState == ClockState.Stopped)
                    return null;
                if (myNaturalDuration == Duration.Forever)
                    return TimeSpan.FromMilliseconds(myCurrentGlobalTime);
                return TimeSpan.FromMilliseconds(Math.IEEERemainder(myCurrentGlobalTime, myNaturalDuration.TimeSpan.TotalMilliseconds));
            }
        }

        float myCurrentGlobalTime = 0;
        public TimeSpan CurrentGlobalTime
        {
            get
            {
                return TimeSpan.FromMilliseconds(myCurrentGlobalTime);
            }
        }

        ClockState myClockState = ClockState.Active;
        public ClockState ClockState
        {
            get
            {
                return myClockState;
            }
        }

        Timeline myTimeline;
        float myTotalTime;
        protected internal Clock(Timeline timeline)
        {
            myAutoReverse = timeline.AutoReverse;
            myTimeline = timeline;
            myNaturalDuration = myTimeline.Duration;
            if (timeline.RepeatBehavior == RepeatBehavior.Forever)
                myTotalTime = float.MaxValue;
            else
            {
                if (timeline.RepeatBehavior.HasCount)
                    myTotalTime = (float)(myTimeline.Duration.TimeSpan.TotalMilliseconds * myTimeline.RepeatBehavior.Count);
                else
                    myTotalTime = (float)myTimeline.RepeatBehavior.Duration.TimeSpan.TotalMilliseconds;
            }
            mySystem.AddClock(this);
        }

        Duration myNaturalDuration;
        public Duration NaturalDuration
        {
            get
            {
                return myNaturalDuration;
            }
        }
    }
}
