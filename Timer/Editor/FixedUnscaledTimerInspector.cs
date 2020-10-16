using UnityEditor;
using System;

namespace GreatClock.Common.Utils {

	[CustomEditor(typeof(FixedUnscaledTimer))]
	public class FixedUnscaledTimerInspector : TimerBaseInspector {

		protected override Type GetTimerType() {
			return typeof(FixedUnscaledTimer);
		}
		protected override Type GetTimerBaseType() {
			return typeof(FixedUnscaledTimer).BaseType;
		}

	}

}