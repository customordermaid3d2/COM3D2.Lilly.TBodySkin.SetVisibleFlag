using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using UnityEngine;

namespace COM3D2.Lilly.TBodySkinPatch
{
    [BepInPlugin("COM3D2.Lilly.TBodySkinPatch", "COM3D2.Lilly.TBodySkinPatch", "21.3.28")]// 버전 규칙 잇음. 반드시 2~4개의 숫자구성으로 해야함. 미준수시 못읽어들임

    public class TBodySkinPatch : BaseUnityPlugin
    {
        Harmony harmony;
		static ManualLogSource log = BepInEx.Logging.Logger.CreateLogSource("TBodySkin");

		public void Awake()
        {
            harmony = Harmony.CreateAndPatchAll(typeof(TBodySkinPatch), null);
        }

		[HarmonyPrefix,HarmonyPatch(typeof(TBodySkin), "SetVisibleFlag")]
		public static void SetVisibleFlag(TBodySkin __instance, bool __runOriginal, bool boSetFlag, string name, Transform t = null, bool boTgt = false)
		{
			if (t.name.IndexOf(name) >= 0)
			{
				boTgt = true;
			}
			if (name == "_ALL_")
			{
				boTgt = true;
			}
			if (boTgt)
			{
				__instance.m_dicDelNodeBody[t.name] = boSetFlag;
			}
			IEnumerator enumerator = t.GetEnumerator();
			//try
			{
				while (enumerator.MoveNext())
                {
                    try
                    {
                        object obj = enumerator.Current;
                        Transform t2 = (Transform)obj;
                        __instance.SetVisibleFlag(boSetFlag, name, t2, boTgt);
                    }
                    catch (Exception e)
                    {
						log.LogFatal(e.ToString());
                    }
				}
			}
			/*
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			*/
			__runOriginal = false;
		}
	}
}
