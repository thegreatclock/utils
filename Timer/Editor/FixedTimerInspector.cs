using UnityEditor;
using System;

namespace GreatClock.Common.Utils {

	[CustomEditor(typeof(FixedTimer))]
	public class FixedTimerInspector : TimerBaseInspector {

		protected override Type GetTimerType() {
			return typeof(FixedTimer);
		}
		protected override Type GetTimerBaseType() {
			return typeof(FixedTimer).BaseType;
		}

	}

}