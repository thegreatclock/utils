using UnityEditor;
using System;

namespace GreatClock.Common.Utils {

	[CustomEditor(typeof(RealTimeTimer))]
	public class RealTimeTimerInspector : TimerBaseInspector {

		protected override Type GetTimerType() {
			return typeof(RealTimeTimer);
		}
		protected override Type GetTimerBaseType() {
			return typeof(RealTimeTimer).BaseType;
		}

	}

}