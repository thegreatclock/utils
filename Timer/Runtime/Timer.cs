using UnityEngine;

namespace GreatClock.Common.Utils {

	public sealed class Timer : TimerBase<Timer> {

		private double mTimer = 0.0;

		protected override double GetNow() {
			return mTimer;
		}

		protected override void OnChanged() {
			if (GetTimers() <= 0) {
				mTimer = 0.0;
			}
		}

		void Update() {
			mTimer += Time.deltaTime;
			DoUpdate();
		}

	}

}
