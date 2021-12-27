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
    [BepInPlugin("org.J-Joe.monkeytag.Trails", "MonkeyTrails", "1.0.0.0")]
    public class MyPatcher : BaseUnityPlugin
    {
        public static ConfigEntry<bool> enabled;
        public static ConfigEntry<float> startSize;
        public static ConfigEntry<float> endSize;
        public static ConfigEntry<float> time;
        public static ConfigEntry<Color> trailColor;

        public void Awake()
        {
            var harmony = new Harmony("com.J-Joe.monkeytag.alwaysOn");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            ConfigFile config = new ConfigFile(Path.Combine(Paths.ConfigPath, "TrailMonke.cfg"), true);

            enabled = config.Bind<bool>("Config", "Enabled", true, "If the trail is turned on or not");
            startSize = config.Bind<float>("Config", "StartSize", 0.1f, "The size of the trail when it starts");
            endSize = config.Bind<float>("Config", "EndSize", 0.05f, "The size of the trail at the end");
            time = config.Bind<float>("Config", "Time", 5f, "How long the trail will be there");
            trailColor = config.Bind<Color>("Config", "TrailColor", Color.green, "The color of the trail");
        }

        public void LateUpdate()
        {
            // Player tag = PlayerOffset
            if (enabled.Value)
            {
                if (GorillaLocomotion.Player.Instance.bodyCollider.gameObject.GetComponent<TrailRenderer>() != null) return;
                else
                {
                    GorillaLocomotion.Player.Instance.bodyCollider.gameObject.AddComponent<TrailRenderer>();
                    TrailRenderer tr = GorillaLocomotion.Player.Instance.bodyCollider.gameObject.GetComponent<TrailRenderer>();

                    tr.enabled = true;
                    tr.emitting = true;

                    //changable
                    tr.startWidth = startSize.Value;
                    tr.endWidth = endSize.Value;
                    tr.material.color = trailColor.Value;
                    tr.time = time.Value;
                }
            }
        }
    }
}