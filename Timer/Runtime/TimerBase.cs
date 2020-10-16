using UnityEngine;
using GreatClock.Common.Collections;

namespace GreatClock.Common.Utils {

	public abstract class TimerBase<T> : MonoBehaviour where T : TimerBase<T> {

		public delegate void TimerDelegate();
		public delegate void TimeoutDelegate(float timeout);

		#region key, delay, interval
		public static void Register(string key, float delay, float interval, TimerDelegate onTimer) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, delay, interval, null, onTimer, null, 1);
		}

		public static void Register(string key, float delay, float interval, TimeoutDelegate onTimer) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, delay, interval, onTimer, null, null, 1);
		}

		public static void Register(string key, float delay, float interval, TimerDelegate onTimer, TimerDelegate onCanceled) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, delay, interval, null, onTimer, onCanceled, 1);
		}

		public static void Register(string key, float delay, float interval, TimeoutDelegate onTimer, TimerDelegate onCanceled) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, delay, interval, onTimer, null, onCanceled, 1);
		}

		public static void Register(string key, float delay, float interval, TimerDelegate onTimer, int times) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, delay, interval, null, onTimer, null, times);
		}

		public static void Register(string key, float delay, float interval, TimeoutDelegate onTimer, int times) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, delay, interval, onTimer, null, null, times);
		}

		public static void Register(string key, float delay, float interval, TimerDelegate onTimer, TimerDelegate onCanceled, int times) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, delay, interval, null, onTimer, onCanceled, times);
		}

		public static void Register(string key, float delay, float interval, TimeoutDelegate onTimer, TimerDelegate onCanceled, int times) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, delay, interval, onTimer, null, onCanceled, times);
		}
		#endregion

		#region key, timeout
		public static void Register(string key, float timeout, TimerDelegate onTimer) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, timeout, timeout, null, onTimer, null, 1);
		}

		public static void Register(string key, float timeout, TimeoutDelegate onTimer) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, timeout, timeout, onTimer, null, null, 1);
		}

		public static void Register(string key, float timeout, TimerDelegate onTimer, TimerDelegate onCanceled) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, timeout, timeout, null, onTimer, onCanceled, 1);
		}

		public static void Register(string key, float timeout, TimeoutDelegate onTimer, TimerDelegate onCanceled) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, timeout, timeout, onTimer, null, onCanceled, 1);
		}

		public static void Register(string key, float timeout, TimerDelegate onTimer, int times) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, timeout, timeout, null, onTimer, null, times);
		}

		public static void Register(string key, float timeout, TimeoutDelegate onTimer, int times) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, timeout, timeout, onTimer, null, null, times);
		}

		public static void Register(string key, float timeout, TimerDelegate onTimer, TimerDelegate onCanceled, int times) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, timeout, timeout, null, onTimer, onCanceled, times);
		}

		public static void Register(string key, float timeout, TimeoutDelegate onTimer, TimerDelegate onCanceled, int times) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return; }
			timer.RegisterInternal(key, timeout, timeout, onTimer, null, onCanceled, times);
		}
		#endregion

		#region delay, interval
		public static ulong Register(float delay, float interval, TimerDelegate onTimer) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, delay, interval, null, onTimer, null, 1);
		}

		public static ulong Register(float delay, float interval, TimeoutDelegate onTimer) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, delay, interval, onTimer, null, null, 1);
		}

		public static ulong Register(float delay, float interval, TimerDelegate onTimer, TimerDelegate onCanceled) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, delay, interval, null, onTimer, onCanceled, 1);
		}

		public static ulong Register(float delay, float interval, TimeoutDelegate onTimer, TimerDelegate onCanceled) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, delay, interval, onTimer, null, onCanceled, 1);
		}

		public static ulong Register(float delay, float interval, TimerDelegate onTimer, int times) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, delay, interval, null, onTimer, null, times);
		}

		public static ulong Register(float delay, float interval, TimeoutDelegate onTimer, int times) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, delay, interval, onTimer, null, null, times);
		}

		public static ulong Register(float delay, float interval, TimerDelegate onTimer, TimerDelegate onCanceled, int times) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, delay, interval, null, onTimer, onCanceled, times);
		}

		public static ulong Register(float delay, float interval, TimeoutDelegate onTimer, TimerDelegate onCanceled, int times) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, delay, interval, onTimer, null, onCanceled, times);
		}
		#endregion

		#region timeout
		public static ulong Register(float timeout, TimerDelegate onTimer) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, timeout, timeout, null, onTimer, null, 1);
		}

		public static ulong Register(float timeout, TimeoutDelegate onTimer) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, timeout, timeout, onTimer, null, null, 1);
		}

		public static ulong Register(float timeout, TimerDelegate onTimer, TimerDelegate onCanceled) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, timeout, timeout, null, onTimer, onCanceled, 1);
		}

		public static ulong Register(float timeout, TimeoutDelegate onTimer, TimerDelegate onCanceled) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, timeout, timeout, onTimer, null, onCanceled, 1);
		}

		public static ulong Register(float timeout, TimerDelegate onTimer, int times) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, timeout, timeout, null, onTimer, null, times);
		}

		public static ulong Register(float timeout, TimeoutDelegate onTimer, int times) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, timeout, timeout, onTimer, null, null, times);
		}

		public static ulong Register(float timeout, TimerDelegate onTimer, TimerDelegate onCanceled, int times) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, timeout, timeout, null, onTimer, onCanceled, times);
		}

		public static ulong Register(float timeout, TimeoutDelegate onTimer, TimerDelegate onCanceled, int times) {
			T timer = GetInstance();
			if (timer == null) { return 0L; }
			return timer.RegisterInternal(null, timeout, timeout, onTimer, null, onCanceled, times);
		}
		#endregion

		#region unregister
		public static bool Unregister(string key) {
			if (key == null) { throw new System.ArgumentNullException(key); }
			T timer = GetInstance();
			if (timer == null) { return false; }
			return timer.UnregisterInternal(key);
		}

		public static bool Unregister(ulong id) {
			T timer = GetInstance();
			if (timer == null) { return false; }
			return timer.UnregisterInternal((long)id);
		}
		#endregion

		private static T s_instance;
		private static bool s_instance_inited = false;
		private static T GetInstance() {
			if (!s_instance_inited) {
				s_instance_inited = true;
				GameObject go = new GameObject(typeof(T).Name);
				DontDestroyOnLoad(go);
				s_instance = go.AddComponent<T>();
				s_instance.enabled = false;
			}
			return s_instance;
		}

#region non static
		private ulong mIdGen = 0uL;
		private KeyedPriorityQueue<long, TimerData, double> mQueue = new KeyedPriorityQueue<long, TimerData, double>();

		protected abstract double GetNow();
		protected abstract void OnChanged();

		protected int GetTimers() { return mQueue.Count; }

		private ulong RegisterInternal(string key, float delay, float interval, TimeoutDelegate onTimer1, TimerDelegate onTimer2, TimerDelegate onCanceled, int times) {
			long id = 0L;
			if (key == null) {
				id = (long)(++mIdGen);
			} else {
				id += key.GetHashCode();
				id -= int.MaxValue;
				TimerData d;
				if (mQueue.TryGetItem(id, out d)) {
					mQueue.RemoveFromQueue(id);
					if (d.onCanceled != null) {
						try {
							d.onCanceled();
						} catch (System.Exception e) {
							Debug.LogException(e);
						}
					}
				}
			}
			TimerData data = new TimerData();
			data.id = id;
			data.key = key;
			data.interval = interval;
			data.onTimer1 = onTimer1;
			data.onTimer2 = onTimer2;
			data.onCanceled = onCanceled;
			data.times = times;
			mQueue.Enqueue(id, data, GetNow() + delay);
			if (!enabled) { enabled = true; }
			OnChanged();
#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
#endif
			return id <= 0L ? 0L : (ulong)id;
		}

		private bool UnregisterInternal(string key) {
			long id = key.GetHashCode();
			id -= int.MaxValue;
			return UnregisterInternal(id);
		}

		private bool UnregisterInternal(long id) {
			bool ret = mQueue.RemoveFromQueue(id);
			if (ret && mQueue.Count <= 0 && enabled) {
				enabled = false;
			}
			if (ret) {
				OnChanged();
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(this);
#endif
			}
			return ret;
		}

		protected void DoUpdate() {
			double now = GetNow();
			while (mQueue.Count > 0) {
				long id;
				double p;
				mQueue.Peek(out id, out p);
				if (p > now) { break; }
				TimerData data = mQueue.Dequeue(out id, out p);
				if (data.times != 1) {
					if (data.times > 0) { data.times--; }
					mQueue.Enqueue(id, data, p + data.interval);
				}
				if (data.onTimer1 != null) {
					try {
						data.onTimer1((float)(now - p));
					} catch (System.Exception e) {
						Debug.LogException(e);
					}
				}
				if (data.onTimer2 != null) {
					try {
						data.onTimer2();
					} catch (System.Exception e) {
						Debug.LogException(e);
					}
				}
				if (mQueue.Count <= 0 && enabled) {
					enabled = false;
				}
				OnChanged();
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(this);
#endif
			}
		}

		private struct TimerData {
			public long id;
			public string key;
			public float interval;
			public TimeoutDelegate onTimer1;
			public TimerDelegate onTimer2;
			public TimerDelegate onCanceled;
			public int times;
		}

#endregion

	}

}
