using UnityEngine;

namespace GreatClock.Common.Utils {

	public sealed class FixedTimer : TimerBase<FixedTimer> {

		private double mTimer = 0.0;

		protected override double GetNow() {
			return mTimer;
		}

		protected override void OnChanged() {
			if (GetTimers() <= 0) {
				mTimer = 0.0;
			}
		}

		void FixedUpdate() {
			mTimer += Time.fixedDeltaTime;
			DoUpdate();
		}

	}

}
