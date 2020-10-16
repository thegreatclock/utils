using System.Collections;
using UnityEngine;

namespace GreatClock.Common.Utils {

	public class CoroutineRunner : MonoBehaviour {

		public static void Start(IEnumerator routine) {
			CoroutineRunner ins = instance;
			if (ins == null) { return; }
			ins.StartCoroutine(routine);
		}

		private static CoroutineRunner s_instance;
		private static bool s_inited = false;
		private static CoroutineRunner instance {
			get {
				if (!s_inited) {
					GameObject go = new GameObject("CoroutineRunner");
					DontDestroyOnLoad(go);
					s_instance = go.AddComponent<CoroutineRunner>();
					s_instance.enabled = false;
				}
				return s_instance == null || s_instance.Equals(null) ? null : s_instance;
			}
		}

	}

}
