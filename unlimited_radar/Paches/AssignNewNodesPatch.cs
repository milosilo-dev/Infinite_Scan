using System;
using System.Collections.Generic;
using System.Reflection;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace unlimited_radar.Paches
{
    internal class AssignNewNodesPatch
    {
        [HarmonyPatch(typeof(HUDManager), "AssignNewNodes")]
        [HarmonyPrefix]
        private static bool AssignNewNodesOverride(HUDManager __instance, PlayerControllerB playerScript, ref RectTransform[] ___scanElements, ref List<ScanNodeProperties> ___nodesOnScreen, ref int ___scannedScrapNum, ref RaycastHit[] ___scanNodesHit)
        {
            int num = Physics.SphereCastNonAlloc(new Ray(((Component)playerScript.gameplayCamera).transform.position + ((Component)playerScript.gameplayCamera).transform.forward * 20, ((Component)playerScript.gameplayCamera).transform.forward), 20f, ___scanNodesHit, 100000f, 4194304);
            if (num > ___scanElements.Length)
            {
                num = ___scanElements.Length;
            }
            ___nodesOnScreen.Clear();
            ___scannedScrapNum = 0;
            MethodInfo methodInfo = AccessTools.Method(typeof(HUDManager), "AttemptScanNode", (Type[])null, (Type[])null);
            if (num > ___scanElements.Length)
            {
                for (int i = 0; i < num; i++)
                {
                    ScanNodeProperties component = ((Component)((RaycastHit)(___scanNodesHit[i])).transform).gameObject.GetComponent<ScanNodeProperties>();
                    if (component.nodeType == 1 || component.nodeType == 2)
                    {
                        methodInfo?.Invoke(__instance, new object[3] { component, i, playerScript });
                    }
                }
            }
            if (___nodesOnScreen.Count < ___scanElements.Length)
            {
                for (int j = 0; j < num; j++)
                {
                    ScanNodeProperties component2 = ((Component)((RaycastHit)(___scanNodesHit[j])).transform).gameObject.GetComponent<ScanNodeProperties>();
                    methodInfo?.Invoke(__instance, new object[3] { component2, j, playerScript });
                }
            }
            return false;
        }
    }
}
