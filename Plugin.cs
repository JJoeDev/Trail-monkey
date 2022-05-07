using BepInEx;
using BepInEx.Configuration;
using System.IO;
using System.Reflection;
using UnityEngine;
using HarmonyLib;

namespace GorillaTrails
{
    [BepInPlugin(pInfo.GUID, pInfo.Name, pInfo.Version)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        // Body
        public static ConfigEntry<bool> bodyEnabled;
        public static ConfigEntry<float> startSize;
        public static ConfigEntry<float> endSize;
        public static ConfigEntry<float> time;
        public static ConfigEntry<Color> trailColor;

        // Right hand
        public static ConfigEntry<bool> rightHandTrailEnable;
        public static ConfigEntry<float> rightStartSize;
        public static ConfigEntry<float> rightEndSize;
        public static ConfigEntry<float> rightTime;
        public static ConfigEntry<Color> rightTrailColor;

        // Left hand
        public static ConfigEntry<bool> leftHandTrailEnable;
        public static ConfigEntry<float> leftStartSize;
        public static ConfigEntry<float> leftEndSize;
        public static ConfigEntry<float> leftTime;
        public static ConfigEntry<Color> leftTrailColor;


        void Awake()
        {
            

            var h = new Harmony(pInfo.GUID);
            h.PatchAll(Assembly.GetExecutingAssembly());

            ConfigFile config = new ConfigFile(Path.Combine(Paths.ConfigPath, "TrailMonke.cfg"), true);

            // Body
            bodyEnabled = config.Bind<bool>("Config", "Is mod enabled", true, "If the body trail is enabled");
            startSize = config.Bind<float>("Config", "Size at body pos", 0.1f, "Change the size at the body position");
            endSize = config.Bind<float>("Config", "Size at end pos", 0f, "Change the size of the trails end point");
            time = config.Bind<float>("Config", "How long the trail is alive", 5f, "Change how long the trail is alive");
            trailColor = config.Bind<Color>("Config", "The color of the trail", Color.cyan, "Change the color the trail will have");

            // right hand
            rightHandTrailEnable = config.Bind<bool>("Config", "Is the right trail enabled", true, "If the right hand trail is enabled");
            rightStartSize = config.Bind<float>("Config", "Size at right hand pos", 0.05f, "Change the size at the right hand position");
            rightEndSize = config.Bind<float>("Config", "Size at right trail end", 0f, "Change the size of the trails end point");
            rightTime = config.Bind<float>("Config", "How long right trail is alive", 5f, "Change how long right hand trail is alive");
            rightTrailColor = config.Bind("Config", "Color of the right trail", Color.blue, "Change the color of right hand trail");

            // left hand
            leftHandTrailEnable = config.Bind<bool>("Config", "Is the left trail enabled", true, "If the left hand trail is enabled");
            leftStartSize = config.Bind<float>("Config", "Size at left hand pos", 0.05f, "Change the size at the left hand position");
            leftEndSize = config.Bind<float>("Config", "Size at left trail end", 0f, "Change the size of the trails end point");
            leftTime = config.Bind<float>("Config", "How long left trail is alive", 5f, "Change how long left hand trail is alive");
            leftTrailColor = config.Bind("Config", "Color of the left trail", Color.blue, "Change the color of left hand trail");
        }

        [HarmonyPatch(typeof(GorillaLocomotion.Player))]
        [HarmonyPatch("Awake", MethodType.Normal)]
        static void Postfix(GorillaLocomotion.Player __instance)
        {
            if (bodyEnabled.Value)
            {
                TrailRenderer tr = __instance.bodyCollider.gameObject.AddComponent<TrailRenderer>();

                tr.enabled = true;
                tr.emitting = true;

                tr.startWidth = startSize.Value;
                tr.endWidth = endSize.Value;
                tr.material.color = trailColor.Value;
                tr.time = time.Value;
            }

            if (rightHandTrailEnable.Value)
            {
                TrailRenderer rtr = __instance.rightHandTransform.gameObject.AddComponent<TrailRenderer>();

                rtr.enabled = true;
                rtr.emitting = true;

                rtr.startWidth = rightStartSize.Value;
                rtr.endWidth = rightEndSize.Value;
                rtr.material.color = rightTrailColor.Value;
                rtr.time = rightTime.Value;
            }

            if (leftHandTrailEnable.Value)
            {
                TrailRenderer ltr = __instance.leftHandTransform.gameObject.AddComponent<TrailRenderer>();

                ltr.enabled = true;
                ltr.emitting = true;

                ltr.startWidth = leftStartSize.Value;
                ltr.endWidth = leftEndSize.Value;
                ltr.material.color = leftTrailColor.Value;
                ltr.time = leftTime.Value;
            }
        }
    }
}
