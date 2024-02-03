using HarmonyLib;

namespace unlimited_radar.Paches
{
    internal class AddNewScrapFoundToDisplayPatch
    {
        [HarmonyPatch(typeof(HUDManager), "AddNewScrapFoundToDisplay")]
        [HarmonyPrefix]
        static public void AddNewScapFoundToDisplayOverride(HUDManager __instance, GrabbableObject GObject)
        {
            __instance.itemsToBeDisplayed.Add(GObject);
        }
    }
}
