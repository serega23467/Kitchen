using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;

namespace BrightnessPlugin
{
	public class UseBrightnessPlugin : MonoBehaviour
	{

#if (UNITY_IOS || UNITY_TVOS || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
		[DllImport("BrightnessPlugin")]
#endif
		private static extern void SetBrightnessFromUnity(float t);


#if (UNITY_IOS || UNITY_TVOS || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
		[DllImport("BrightnessPlugin")]
#endif
		private static extern IntPtr GetRenderEventFunc();

#if UNITY_WEBGL && !UNITY_EDITOR
	[DllImport ("__Internal")]
	private static extern void RegisterPlugin();
#endif

		IEnumerator Start()
		{
#if UNITY_WEBGL && !UNITY_EDITOR
		RegisterPlugin();
#endif
			yield return StartCoroutine("CallPluginAtEndOfFrames");
		}


		public void SetBrightness(float val)
		{
			SetBrightnessFromUnity(val);
		}


		private IEnumerator CallPluginAtEndOfFrames()
		{
			while (true)
			{
				yield return new WaitForEndOfFrame();
				GL.IssuePluginEvent(GetRenderEventFunc(), 1);
			}
		}
	}
}