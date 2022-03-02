using System;
using System.Collections.Generic;
using UnityEngine;
using Xyz.MomsSpaghettiCode.CrossWorlds.GameViews;

namespace Xyz.MomsSpaghettiCode.UI
{
    public class MenuButton : MonoBehaviour
    {
        public GameObject menuRef;
        public Camera cameraRef;
        public float menuEaseOutTransitionDuration;

        private Color currentBackgroundColor;

        private Dictionary<Loader.Scene, Color> sceneBackgroundColors = new Dictionary<Loader.Scene, Color>
        {
            {Loader.Scene.Menu, new Color(0xF5 / 255f, 0x8E / 255f, 0x74 / 255f)},
            {Loader.Scene.Game, new Color(0xEA / 255f, 0xDD / 255f, 0xC7 / 255f)}
        };

        private void GoToScene(Loader.Scene target)
        {
            if (menuRef is null)
            {
                Loader.Load(Loader.Scene.Game);
                return;
            }


            LeanTween.scale(
                    menuRef,
                    Vector3.zero,
                    menuEaseOutTransitionDuration
                )
                .setEaseInOutBack()
                .setOnComplete(() => { Loader.Load(target); });

            currentBackgroundColor = cameraRef.backgroundColor;
            // cameraRef.backgroundColor = sceneBackgroundColors[target];
            LeanTween.value(0, 1, menuEaseOutTransitionDuration)
                .setEaseInOutBack()
                .setOnUpdate(t =>
                {
                    var color =
                        ((int) Math.Floor(cameraRef.backgroundColor.r * 255)).ToString("X") +
                        ((int) Math.Floor(cameraRef.backgroundColor.g * 255)).ToString("X") +
                        ((int) Math.Floor(cameraRef.backgroundColor.b * 255)).ToString("X");
                    Debug.Log($"<color=#{color}>COLOR</color>");
                    // Debug.Log(cameraRef.backgroundColor);
                    cameraRef.backgroundColor = Color.Lerp(
                        currentBackgroundColor,
                        sceneBackgroundColors[target],
                        t
                    );
                    // cameraRef.
                    // Debug.Log(cameraRef.backgroundColor);
                });
        }

        public void GoToMenuScene()
        {
            GoToScene(Loader.Scene.Menu);
        }

        public void GoToGameScene()
        {
            GoToScene(Loader.Scene.Game);
        }
    }
}