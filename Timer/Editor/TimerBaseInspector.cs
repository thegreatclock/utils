using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

namespace GreatClock.Common.Utils {
	
	public abstract class TimerBaseInspector : Editor {

		private object mQueue;
		private IList mHeap;
		private IList mHeapCopy;

		private MethodInfo mMethodCompare;

		private PropertyInfo mPropCount;
		private FieldInfo mFieldValue;
		private FieldInfo mFieldPriority;

		private FieldInfo mFieldId;
		private FieldInfo mFieldKey;
		private FieldInfo mFieldOnTimer1;
		private FieldInfo mFieldOnTimer2;
		private FieldInfo mFieldOnCancel;
		private FieldInfo mFieldTimes;
		private FieldInfo mFieldInterval;

		private GUIStyle mStyleArea;
		private GUIStyle mStyleBold;

		private GUILayoutOption mTitleWidth = GUILayout.Width(80f);

		private Vector2 mScroll;

		protected abstract System.Type GetTimerType();
		protected abstract System.Type GetTimerBaseType();

		void Awake() {
			System.Type timerType = GetTimerType();
            System.Type baseType = GetTimerBaseType();
			FieldInfo fQueue = baseType.GetField("mQueue", BindingFlags.Instance | BindingFlags.NonPublic);
			mQueue = fQueue.GetValue(target);

			string sTimerData = string.Format("GreatClock.Common.Utils.TimerBase`1+TimerData[{0}]", timerType);
			System.Type tTimerData = System.Type.GetType(string.Format("{0},Assembly-CSharp", sTimerData));
			mFieldId = tTimerData.GetField("id");
			mFieldKey = tTimerData.GetField("key");
			mFieldOnTimer1 = tTimerData.GetField("onTimer1");
			mFieldOnTimer2 = tTimerData.GetField("onTimer2");
			mFieldOnCancel = tTimerData.GetField("onCanceled");
			mFieldTimes = tTimerData.GetField("times");
			mFieldInterval = tTimerData.GetField("interval");

			string sKVP = string.Format("GreatClock.Common.Collections.KeyedPriorityQueue`3[System.Int64,{0},System.Double]", sTimerData);
			System.Type tKVP = System.Type.GetType(string.Format("{0},Assembly-CSharp", sKVP));
			mMethodCompare = tKVP.GetMethod("Compare", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo fHeap = tKVP.GetField("mHeap", BindingFlags.Instance | BindingFlags.NonPublic);
			mPropCount = tKVP.GetProperty("Count", BindingFlags.Instance | BindingFlags.Public);
			mHeap = fHeap.GetValue(mQueue) as IList;
			mHeapCopy = System.Activator.CreateInstance(mHeap.GetType()) as IList;

			string sKVPNode = string.Format("GreatClock.Common.Collections.KeyedPriorityQueue`3+Node[System.Int64,{0},System.Double]", sTimerData);
			System.Type tKVPNode = System.Type.GetType(string.Format("{0},Assembly-CSharp", sKVPNode));
			mFieldValue = tKVPNode.GetField("value");
			mFieldPriority = tKVPNode.GetField("priority");
		}

		public override void OnInspectorGUI() {
			if (mStyleArea == null) { mStyleArea = GUI.skin.FindStyle("TextArea") ?? GUI.skin.FindStyle("AS TextArea"); }
			if (mStyleBold == null) { mStyleBold = "BoldLabel"; }
			Color cachedColor;
            GUILayout.Space(4f);
			int count = (int)mPropCount.GetValue(mQueue, null);
			cachedColor = GUI.color;
			GUI.color = Color.white;
            EditorGUILayout.LabelField(string.Format("{0} Timers", count), mStyleBold);
			GUI.color = cachedColor;
            GUILayout.Space(4f);
			mHeapCopy.Clear();
			for (int i = 0; i <= count; i++) {
				mHeapCopy.Add(mHeap[i]);
			}
			mScroll = EditorGUILayout.BeginScrollView(mScroll, false, false);
			int ii = 0;
			for (int i = count; i > 0; i--) {
				Heapify(1, i);
				object n = mHeapCopy[1];
				mHeapCopy[1] = mHeapCopy[i];
				cachedColor = GUI.backgroundColor;
				GUI.backgroundColor = (++ii & 1) == 0 ? Color.white : Color.gray;
				EditorGUILayout.BeginVertical(mStyleArea, GUILayout.MinHeight(10f));
				GUI.backgroundColor = cachedColor;
				object data = mFieldValue.GetValue(n);
				string key = mFieldKey.GetValue(data) as string;
				if (key != null) {
					EditorGUILayout.LabelField(string.Format("Key : {0}", key), mStyleBold);
				} else {
					EditorGUILayout.LabelField(string.Format("Id : {0}", mFieldId.GetValue(data)), mStyleBold);
				}
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Target Time", mTitleWidth);
				EditorGUILayout.LabelField(string.Format("{0}", mFieldPriority.GetValue(n)), mStyleArea);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("OnTimer", mTitleWidth);
				if (!DrawDelegate(mFieldOnTimer1.GetValue(data) as System.Delegate) &&
					!DrawDelegate(mFieldOnTimer2.GetValue(data) as System.Delegate)) {
					EditorGUILayout.LabelField("null", mStyleArea);
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("OnCancel", mTitleWidth);
				if (!DrawDelegate(mFieldOnCancel.GetValue(data) as System.Delegate)) {
					EditorGUILayout.LabelField("null", mStyleArea);
				}
				EditorGUILayout.EndHorizontal();
				int times = (int)mFieldTimes.GetValue(data);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Times", mTitleWidth);
				EditorGUILayout.LabelField(times.ToString(), mStyleArea);
				EditorGUILayout.EndHorizontal();
				if (times != 1) {
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Interval", mTitleWidth);
					EditorGUILayout.LabelField(string.Format("{0}", mFieldInterval.GetValue(data)), mStyleArea);
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.EndVertical();
				GUILayout.Space(2f);
			}
			EditorGUILayout.EndScrollView();
		}

		private string[] mDelegateParas = new string[10];
		private bool DrawDelegate(System.Delegate d) {
			if (d == null) { return false; }
			MethodInfo method = d.Method;
			if (method == null) { return false; }
			Component component = d.Target as Component;
			if (component != null) {
				EditorGUILayout.BeginVertical();
			}
			ParameterInfo[] paras = method.GetParameters();
			for (int i = 0, imax = paras == null ? 0 : paras.Length; i < imax; i++) {
				ParameterInfo para = paras[i];
				string typeName = null;
				System.Type pType = para.ParameterType;
				if (pType == typeof(float)) {
					typeName = "float";
				} else {
					typeName = pType.FullName;
				}
				mDelegateParas[i] = string.Format("{0} {1}", typeName, para.Name);
			}
			EditorGUILayout.LabelField(string.Format("{0}.{1}({2})", method.DeclaringType.FullName, method.Name,
				string.Join(", ", mDelegateParas, 0, paras.Length)), mStyleArea);
			if (component != null) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Object :", GUILayout.Width(50f));
				EditorGUILayout.ObjectField(component, typeof(Component), true);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}
			return true;
		}

		private object[] mCmpParas = new object[2];
		private int Compare(object l, object r) {
			mCmpParas[0] = l;
			mCmpParas[1] = r;
			return (int)mMethodCompare.Invoke(mQueue, mCmpParas);
		}

		private void Heapify(int i, int count) {
			int l = i << 1;
			int r = l + 1;
			int h = i;
			if (l <= count && Compare(mFieldPriority.GetValue(mHeapCopy[l]), mFieldPriority.GetValue(mHeapCopy[h])) < 0) {
				h = l;
			}
			if (r <= count && Compare(mFieldPriority.GetValue(mHeapCopy[r]), mFieldPriority.GetValue(mHeapCopy[h])) < 0) {
				h = r;
			}
			if (h != i) {
				object n = mHeapCopy[h];
				mHeapCopy[h] = mHeapCopy[i];
				mHeapCopy[i] = n;
				Heapify(h, count);
			}
		}

	}

}