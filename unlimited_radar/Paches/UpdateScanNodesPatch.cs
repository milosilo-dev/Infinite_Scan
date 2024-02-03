using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace unlimited_radar.Paches
{
    internal class UpdateScanNodesPatch
    {
        [HarmonyPatch(typeof(HUDManager), "UpdateScanNodes")]
        [HarmonyPrefix]
        static private void UpdateScanNodesOverride(HUDManager __instance, PlayerControllerB playerScript, ref float ___updateScanInterval, ref RectTransform[] ___scanElements, ref Dictionary<RectTransform, ScanNodeProperties> ___scanNodes, ref TextMeshProUGUI[] ___scanElementText, ref int ___totalScrapScannedDisplayNum, ref float ___addToDisplayTotalInterval, ref int ___scannedScrapNum)
        {
            Vector3 zero = Vector3.zero;
            Type type = __instance.GetType();
            if (___updateScanInterval <= 0f)
            {
                ___updateScanInterval = 0.25f;
                type.InvokeMember("AssignNewNodes", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, __instance, new object[] { playerScript });
            }
            ___updateScanInterval -= Time.deltaTime;
            bool flag = false;
            for (int i = 0; i < ___scanElements.Length; i++)
            {
                if (___scanNodes.Count > 0 && ___scanNodes.TryGetValue(___scanElements[i], out var value) && value != null)
                {
                    try
                    {
                        if (!___scanElements[i].gameObject.activeSelf)
                        {
                            ___scanElements[i].gameObject.SetActive(value: true);
                            ___scanElements[i].GetComponent<Animator>().SetInteger("colorNumber", value.nodeType);
                            if (value.creatureScanID != -1)
                            {
                                type.InvokeMember("AttemptScanNewCreature", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, __instance, new object[] { value.creatureScanID });
                            }
                        }
                        goto IL_00f7;
                    }
                    catch (Exception arg)
                    {
                        Debug.LogError($"Error in updatescanNodes A: {arg}");
                        goto IL_00f7;
                    }
                }
                ___scanNodes.Remove(___scanElements[i]);
                ___scanElements[i].gameObject.SetActive(value: false);
                continue;
                IL_00f7:
                try
                {
                    ___scanElementText = ___scanElements[i].gameObject.GetComponentsInChildren<TextMeshProUGUI>();
                    if (___scanElementText.Length > 1)
                    {
                        ___scanElementText[0].text = value.headerText;
                        ___scanElementText[1].text = value.subText;
                    }
                    if (value.nodeType == 2)
                    {
                        flag = true;
                    }
                    zero = playerScript.gameplayCamera.WorldToScreenPoint(value.transform.position);
                    ___scanElements[i].anchoredPosition = new Vector2(zero.x - 439.48f, zero.y - 244.8f);
                }
                catch (Exception arg2)
                {
                    Debug.LogError($"Error in updatescannodes B: {arg2}");
                }
            }
            try
            {
                if (!flag)
                {
                    __instance.totalScrapScanned = 0;
                    ___totalScrapScannedDisplayNum = 0;
                    ___addToDisplayTotalInterval = 0.35f;
                }
                __instance.scanInfoAnimator.SetBool("display", ___scannedScrapNum >= 2 && flag);
            }
            catch (Exception arg3)
            {
                Debug.LogError($"Error in updatescannodes C: {arg3}");
            }
        }
    }
}
