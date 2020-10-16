using UnityEngine;
using UnityEditor;
using System.Text;
using System.Security.Cryptography;
using System;
using System.IO;

namespace GreatClock.Common.Utils {

	public static class GameLayersGenerator {

		private const string SCRIPT_PATH = "Assets/GameLayers.cs";
		private static MD5 md5_calc = MD5.Create();

		[MenuItem("GreatClock/Generate GameLayers Script")]
		static void GenerateGameLayersScript() {
			StringBuilder code = new StringBuilder();
			code.AppendLine("/* ********** Auto Generated ********** */");
			code.AppendLine();
			code.AppendLine("using System.Collections.Generic;");
			code.AppendLine("using UnityEngine;");
			code.AppendLine();
			code.AppendLine("public static class GameLayers {");
			code.AppendLine();
			for (int i = 0; i < 32; i++) {
				string ln = LayerMask.LayerToName(i);
				if (string.IsNullOrEmpty(ln)) { continue; }
				code.AppendLine(string.Format("\tpublic const int {0} = {1};", ln.Replace(' ', '_'), i));
				code.AppendLine();
			}
			code.AppendLine("\tprivate static List<Transform> temp_transforms = new List<Transform>();");
			code.AppendLine("\tpublic static void SetGameObjectsLayer(GameObject root, int layer) {");
			code.AppendLine("\t\tif (root == null || root.Equals(null)) { return; }");
			code.AppendLine("\t\ttemp_transforms.Clear();");
			code.AppendLine("\t\troot.GetComponentsInChildren<Transform>(true, temp_transforms);");
			code.AppendLine("\t\tfor (int i = temp_transforms.Count - 1; i >= 0; i--) {");
			code.AppendLine("\t\t\ttemp_transforms[i].gameObject.layer = layer;");
			code.AppendLine("\t\t}");
			code.AppendLine("\t\ttemp_transforms.Clear();");
			code.AppendLine("\t}");
			code.AppendLine();
			code.AppendLine("}");
			byte[] bytes = Encoding.UTF8.GetBytes(code.ToString());
			if (File.Exists(SCRIPT_PATH)) {
				string md5o = null;
				try {
					using (FileStream fs = File.OpenRead(SCRIPT_PATH)) {
						md5o = BitConverter.ToString(md5_calc.ComputeHash(fs));
					}
				} catch (Exception e) {
					Debug.LogException(e);
				}
				string md5n = BitConverter.ToString(md5_calc.ComputeHash(bytes));
				if (md5n == md5o) {
					return;
				}
			}
			File.WriteAllBytes(SCRIPT_PATH, bytes);
			AssetDatabase.Refresh();
		}

	}

}
