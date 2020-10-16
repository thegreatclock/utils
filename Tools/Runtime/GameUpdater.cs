using GreatClock.Common.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreatClock.Common.Utils {

	public sealed class GameUpdater : MonoBehaviour {

		public delegate void UpdateDelegate(float deltaTime);

		public interface IUpdater {
			void Add(UpdateDelegate update);
			void Add(UpdateDelegate update, int priority);
			void Add(UpdateDelegate update, bool ignoreBehaviour);
			void Add(UpdateDelegate update, bool ignoreBehaviour, int priority);
			void AddUnScaled(UpdateDelegate update);
			void AddUnScaled(UpdateDelegate update, int priority);
			void AddUnScaled(UpdateDelegate update, bool ignoreBehaviour);
			void AddUnScaled(UpdateDelegate update, bool ignoreBehaviour, int priority);
			void Remove(UpdateDelegate update);
		}

		public static IUpdater updater {
			get {
				if (s_updater == null) {
					if (instance == null) {
						if (s_fake_updater == null) { s_fake_updater = new FakeUpdater(); }
						return s_fake_updater;
					}
					s_updater = new Updater();
				}
				return s_updater;
			}
		}

		public static IUpdater late_updater {
			get {
				if (s_late_updater == null) {
					if (instance == null) {
						if (s_fake_updater == null) { s_fake_updater = new FakeUpdater(); }
						return s_fake_updater;
					}
					s_late_updater = new Updater();
				}
				return s_late_updater;
			}
		}

		public static IUpdater fixed_updater {
			get {
				if (s_fixed_updater == null) {
					if (instance == null) {
						if (s_fake_updater == null) { s_fake_updater = new FakeUpdater(); }
						return s_fake_updater;
					}
					s_fixed_updater = new Updater();
				}
				return s_fixed_updater;
			}
		}

		private static Updater s_updater;
		private static Updater s_late_updater;
		private static Updater s_fixed_updater;
		private static FakeUpdater s_fake_updater;

		private sealed class FakeUpdater : IUpdater {
			public void Add(UpdateDelegate update) { }
			public void Add(UpdateDelegate update, int priority) { }
			public void Add(UpdateDelegate update, bool ignoreBehaviour) { }
			public void Add(UpdateDelegate update, bool ignoreBehaviour, int priority) { }
			public void AddUnScaled(UpdateDelegate update) { }
			public void AddUnScaled(UpdateDelegate update, int priority) { }
			public void AddUnScaled(UpdateDelegate update, bool ignoreBehaviour) { }
			public void AddUnScaled(UpdateDelegate update, bool ignoreBehaviour, int priority) { }
			public void Remove(UpdateDelegate update) { }
		}

		private class Updater : IUpdater {

			public void Add(UpdateDelegate update) {
				AddInternal(update, false, int.MaxValue, false);
			}

			public void Add(UpdateDelegate update, int priority) {
				AddInternal(update, false, priority, false);
			}

			public void Add(UpdateDelegate update, bool ignoreBehaviour) {
				AddInternal(update, ignoreBehaviour, int.MaxValue, false);
			}

			public void Add(UpdateDelegate update, bool ignoreBehaviour, int priority) {
				AddInternal(update, ignoreBehaviour, priority, false);
			}

			public void AddUnScaled(UpdateDelegate update) {
				AddInternal(update, false, int.MaxValue, true);
			}

			public void AddUnScaled(UpdateDelegate update, int priority) {
				AddInternal(update, false, priority, true);
			}

			public void AddUnScaled(UpdateDelegate update, bool ignoreBehaviour) {
				AddInternal(update, ignoreBehaviour, int.MaxValue, true);
			}

			public void AddUnScaled(UpdateDelegate update, bool ignoreBehaviour, int priority) {
				AddInternal(update, ignoreBehaviour, priority, true);
			}

			public void Remove(UpdateDelegate update) {
				RemoveInternal(update);
			}

			private Dictionary<int, UpdateItem> mUpdaters = new Dictionary<int, UpdateItem>();
			private List<UpdateItem> mSortedUpdaters = new List<UpdateItem>();
			private KeyedPriorityQueue<int, UpdateItem, int> mNewUpdaters = new KeyedPriorityQueue<int, UpdateItem, int>();
			private List<UpdateItem> mToAddUpdaters = new List<UpdateItem>();

			private bool mUpdating = false;
			private int mNewCount;
			private int mNewPriority;
			private int mInterior;

			private void AddInternal(UpdateDelegate update, bool ignoreBehaviour, int priority, bool unscaled) {
				if (update == null) { return; }
				int key = update.GetHashCode();
				if (mUpdaters.ContainsKey(key) || mNewUpdaters.Contains(key)) { return; }
				MonoBehaviour behaviour = ignoreBehaviour ? null : update.Target as MonoBehaviour;
				UpdateItem updater = UpdateItem.Get(key, priority, unscaled, behaviour, update);
				if (mUpdating) {
					mToAddUpdaters.Add(updater);
				} else {
					mNewUpdaters.Enqueue(key, updater, priority);
				}
			}

			private void RemoveInternal(UpdateDelegate update) {
				if (update == null) { return; }
				int key = update.GetHashCode();
				UpdateItem updater;
				int removeAt = -1;
				if (mUpdating) {
					if (mUpdaters.TryGetValue(key, out updater)) {
						mUpdaters.Remove(key);
						removeAt = updater.index;
						if (mInterior >= updater.index) {
							mInterior--;
						}
					} else if (mNewUpdaters.RemoveFromQueue(key)) {
						removeAt = mInterior + mNewCount;
						mNewCount--;
						if (mNewCount > 0) {
							mNewPriority = mNewUpdaters.Peek().priority;
						}
					}
				} else {
					if (mUpdaters.TryGetValue(key, out updater)) {
						mUpdaters.Remove(key);
						removeAt = updater.index;
					} else {
						UpdateItem u;
						int p;
						if (mNewUpdaters.RemoveFromQueue(key, out u, out p)) {
							UpdateItem.Cache(u);
						}
					}
				}
				if (removeAt >= 0) {
					UpdateItem.Cache(mSortedUpdaters[removeAt]);
					mSortedUpdaters.RemoveAt(removeAt);
					for (int i = mSortedUpdaters.Count - 1; i >= removeAt; i--) {
						UpdateItem u = mSortedUpdaters[i];
						u.index = i;
					}
				}
			}

			public void Update(float deltaTime, float unscaledDeltaTime) {
				mUpdating = true;
				int len = mSortedUpdaters.Count;
				mNewPriority = int.MaxValue;
				mNewCount = mNewUpdaters.Count;
				for (int i = 0; i < mNewCount; i++) {
					mSortedUpdaters.Add(null);
				}
				if (mNewCount > 0) {
					mNewPriority = mNewUpdaters.Peek().priority;
				}
				for (mInterior = len - 1; mInterior >= -1;) {
					UpdateItem updater = null;
					if (mInterior < 0) {
						if (mNewCount > 0) {
							mNewCount--;
							updater = mNewUpdaters.Dequeue();
							updater.index = mNewCount;
							mSortedUpdaters[mNewCount] = updater;
							mUpdaters.Add(updater.key, updater);
						} else {
							break;
						}
					} else {
						updater = mSortedUpdaters[mInterior];
						if (mNewCount <= 0) {
							mInterior--;
						} else if (mNewPriority < updater.priority) {
							UpdateItem newUpdater = mNewUpdaters.Dequeue();
							newUpdater.index = mInterior + mNewCount;
							mSortedUpdaters[newUpdater.index] = newUpdater;
							mUpdaters.Add(newUpdater.key, newUpdater);
							updater = newUpdater;
							mNewCount--;
						} else {
							updater.index = mInterior + mNewCount;
							mSortedUpdaters[updater.index] = updater;
							mSortedUpdaters[mInterior] = null;
							mInterior--;
						}
					}
					if (updater.behaviour == null || updater.behaviour.isActiveAndEnabled) {
						try {
							updater.update(updater.unscaled ? unscaledDeltaTime : deltaTime);
						} catch (System.Exception e) {
							Debug.LogException(e);
						}
					}
				}
				for (int i = 0, imax = mToAddUpdaters.Count; i < imax; i++) {
					UpdateItem updater = mToAddUpdaters[i];
					mNewUpdaters.Enqueue(updater.key, updater, updater.priority);
				}
				mToAddUpdaters.Clear();
				mUpdating = false;
			}

		}

		private static GameUpdater s_instance;
		private static bool s_instance_inited = false;
		private static GameUpdater instance {
			get {
				if (!s_instance_inited && Application.isPlaying) {
					s_instance_inited = true;
					GameObject go = new GameObject("GameUpdater");
					DontDestroyOnLoad(go);
					s_instance = go.AddComponent<GameUpdater>();
				}
				return s_instance;
			}
		}

		void Update() {
			if (s_updater != null) { s_updater.Update(Time.deltaTime, Time.unscaledDeltaTime); }
		}

		void LateUpdate() {
			if (s_late_updater != null) { s_late_updater.Update(Time.deltaTime, Time.unscaledDeltaTime); }
		}

		void FixedUpdate() {
			if (s_fixed_updater != null) { s_fixed_updater.Update(Time.fixedDeltaTime, Time.fixedUnscaledDeltaTime); }
		}

		private class UpdateItem {

			public int key { get; private set; }
			public int priority { get; private set; }
			public int index { get; set; }
			public bool unscaled { get; private set; }
			public MonoBehaviour behaviour { get; private set; }
			public UpdateDelegate update { get; private set; }

			private UpdateItem() { }

			private static UpdateItem[] cached_instances = new UpdateItem[20];
			private static int cached_start = 0;
			private static int cached_length = 0;

			public static UpdateItem Get(int key, int priority, bool unscaled, MonoBehaviour behaviour, UpdateDelegate update) {
				UpdateItem instance = null;
				if (cached_length > 0) {
					instance = cached_instances[cached_start];
					cached_instances[cached_start] = null;
					cached_start++;
					cached_length--;
					if (cached_start >= cached_instances.Length) {
						cached_start -= cached_instances.Length;
					}
				}
				if (instance == null) {
					instance = new UpdateItem();
				}
				instance.key = key;
				instance.priority = priority;
				instance.unscaled = unscaled;
				instance.behaviour = behaviour;
				instance.update = update;
				return instance;
			}

			public static void Cache(UpdateItem instance) {
				if (instance == null) { return; }
				instance.key = default(int);
				instance.priority = default(int);
				instance.index = default(int);
				instance.behaviour = default(MonoBehaviour);
				instance.update = default(UpdateDelegate);
				if (cached_length >= cached_instances.Length) { return; }
				int end = cached_start + cached_length;
				if (end >= cached_instances.Length) {
					end -= cached_instances.Length;
				}
				cached_instances[end] = instance;
				cached_length++;
			}

		}

	}

}