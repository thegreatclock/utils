using System;

namespace GreatClock.Common.Utils {

	public class RealTimeTimer : TimerBase<RealTimeTimer> {

		private bool mStartTimeInited = false;
		private DateTime mStartTime;

		protected override double GetNow() {
			if (mStartTimeInited) {
				return (DateTime.Now - mStartTime).TotalSeconds;
			}
			mStartTime = DateTime.Now;
			mStartTimeInited = true;
			return 0.0;
		}

		protected override void OnChanged() {
			if (GetTimers() <= 0) {
				mStartTimeInited = false;
			}
		}

		void Update() {
			DoUpdate();
		}

	}

}
