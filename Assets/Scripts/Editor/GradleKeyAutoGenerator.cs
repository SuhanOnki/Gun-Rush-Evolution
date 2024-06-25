using System;
using UnityEditor;
using UnityEngine;
namespace DragonTools.Editor.Utils
{
	[InitializeOnLoad]
	public class GradleKeyAutoGenerator
	{
		static GradleKeyAutoGenerator()
		{
			if (!PlayerSettings.Android.useCustomKeystore) return;

			string pass = GetPass("KEY");
			if (string.IsNullOrEmpty(pass))
			{
				Debug.LogError($"Couldn't load project password");
				return;
			}


			PlayerSettings.Android.keyaliasPass = pass;
			PlayerSettings.Android.keystorePass = pass;
		}

		private static string GetPass(string name)
		{
			var value = Environment.GetEnvironmentVariable(name);

			if (string.IsNullOrEmpty(value))
			{
				Debug.LogError($"Variable '{name}' not set.");
				return null;
			}
			else
			{
				return value;
			}
		}
	}
}