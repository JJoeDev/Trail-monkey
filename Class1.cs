using System;
using System.Collections.Generic;
using HarmonyLib;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using System.Reflection;
using UnityEngine.XR;
using Photon.Pun;
using System.IO;
using System.Net;
using Photon.Realtime;
using UnityEngine.Rendering;

namespace fly
{
    [BepInPlugin("org.JJoe.monkeytag.Trails", "MonkeyTrails", "1.0.0.0")]
    [HarmonyPatch]
    public class MyPatcher : BaseUnityPlugin
    {
        // Body
        public static ConfigEntry<bool> enabled;
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

        public void Awake()
        {
            var harmony = new Harmony("com.JJoe.monkeytag.Trails");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            ConfigFile config = new ConfigFile(Path.Combine(Paths.ConfigPath, "TrailMonke.cfg"), true);

            // body
            enabled = config.Bind<bool>("Config", "Enabled", true, "If the trail is turned on or not");
            startSize = config.Bind<float>("Config", "Start Size", 0.1f, "The size of the trail when it starts");
            endSize = config.Bind<float>("Config", "End Size", 0f, "The size of the trail at the end");
            time = config.Bind<float>("Config", "Time", 5f, "How long the trail is visible");
            trailColor = config.Bind<Color>("Config", "TrailColor", Color.green, "The color of the trail");

            // right hand
            rightHandTrailEnable = config.Bind<bool>("Config", "Right hand trail enable", true, "If the right hand trail is enabled");
            rightStartSize = config.Bind<float>("Config", "Right Start Size", 0.05f, "The start size of the right hand trail");
            rightEndSize = config.Bind<float>("Config", "Right End Size", 0f, "The end size of the left hand trail");
            rightTime = config.Bind<float>("Config", "Right Time", 5f, "The time the right hand trail is visible");
            rightTrailColor = config.Bind<Color>("Config", "Right trail color", Color.red, "The color of the right hand trail");

            // left hand
            leftHandTrailEnable = config.Bind<bool>("Config", "Left hand trail enable", true, "If the left hand trail is enabled");
            leftStartSize = config.Bind<float>("Config", "Left start size", 0.05f, "The start size of the left hand trail");
            leftEndSize = config.Bind<float>("Config", "Left end size", 0f, "The end size of the left hand trail");
            leftTime = config.Bind<float>("Config", "Left time", 5f, "the time the left hand trail is visible");
            leftTrailColor = config.Bind<Color>("Config", "Left trail color", Color.blue, "The color of the left hand trail");

            Debug.LogWarning("------- ALL SETTINGS HAS BEEN LOADED NOW -------");
        }

        [HarmonyPatch(typeof(GorillaLocomotion.Player))]
        [HarmonyPatch("Awake", MethodType.Normal)]
        static void Postfix(GorillaLocomotion.Player __instance)
        {
            Debug.LogWarning("------- APPLYING TRAIL(s) NOW -------");

            // Player tag = PlayerOffset
            if (enabled.Value)
            {
                TrailRenderer tr = __instance.bodyCollider.gameObject.AddComponent<TrailRenderer>();

                tr.enabled = true;
                tr.emitting = true;

                // changable
                tr.startWidth = startSize.Value;
                tr.endWidth = endSize.Value;
                tr.material.color = trailColor.Value;
                tr.time = time.Value;

                if (leftHandTrailEnable.Value)
                {
                    TrailRenderer ltr = __instance.leftHandTransform.gameObject.AddComponent<TrailRenderer>();

                    ltr.enabled = true;
                    ltr.emitting = true;

                    // changable
                    ltr.startWidth = leftStartSize.Value;
                    ltr.endWidth = leftEndSize.Value;
                    ltr.time = leftTime.Value;
                    ltr.material.color = leftTrailColor.Value;
                }

                if (rightHandTrailEnable.Value)
                {
                    TrailRenderer rtr = __instance.rightHandTransform.gameObject.AddComponent<TrailRenderer>();

                    rtr.enabled = true;
                    rtr.emitting = true;

                    // changable
                    rtr.startWidth = rightStartSize.Value;
                    rtr.endWidth = rightEndSize.Value;
                    rtr.time = rightTime.Value;
                    rtr.material.color = rightTrailColor.Value;
                }
            }

            Debug.LogWarning("------- ALL TRAILS HAS BEEN APPLIED -------");
        }
    }
}