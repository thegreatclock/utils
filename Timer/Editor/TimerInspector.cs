using UnityEditor;
using System;

namespace GreatClock.Common.Utils {

	[CustomEditor(typeof(Timer))]
	public class TimerInspector : TimerBaseInspector {

		protected override Type GetTimerType() {
			return typeof(Timer);
		}
		protected override Type GetTimerBaseType() {
			return typeof(Timer).BaseType;
		}

	}

}