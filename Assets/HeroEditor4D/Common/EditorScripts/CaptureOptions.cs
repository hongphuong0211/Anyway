using System.Collections.Generic;
using Assets.HeroEditor4D.Common.CharacterScripts;
using HeroEditor4D.Common.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor4D.Common.EditorScripts
{
    public class CaptureOptions : MonoBehaviour
    {
        public Toggle Left;
        public Toggle Right;
        public Toggle Front;
        public Toggle Back;

        public Toggle Idle;
        public Toggle Walk;
        public Toggle Run;
        public Toggle Slash;
        public Toggle Jab;
        public Toggle Shot;
        public Toggle Cast;
        public Toggle Death;

        public Toggle Shadow;
        public InputField FrameSize;
        public InputField FrameCount;

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Capture()
        {
            var direction = Vector2.right;

            if (Right.isOn) direction = Vector2.right;
            if (Left.isOn) direction = Vector2.left;
            if (Front.isOn) direction = Vector2.down;
            if (Back.isOn) direction = Vector2.up;

            var options = new List<CaptureOption>();

            if (Idle.isOn) options.Add(new CaptureOption(CharacterState.Idle, null));
            if (Walk.isOn) options.Add(new CaptureOption(CharacterState.Walk, null));
            if (Run.isOn) options.Add(new CaptureOption(CharacterState.Run, null));
            if (Slash.isOn) options.Add(new CaptureOption(CharacterState.Idle, FindObjectOfType<Character4D>().WeaponType == WeaponType.Melee2H ? "Slash2H" : "Slash1H"));
            if (Jab.isOn) options.Add(new CaptureOption(CharacterState.Idle, "Jab"));

            if (Shot.isOn)
            {
                var character = FindObjectOfType<Assets.HeroEditor4D.Common.CharacterScripts.Character>();

                switch (character.WeaponType)
                {
                    case WeaponType.Bow:
                        options.Add(new CaptureOption(CharacterState.Idle, "SimpleBowShot"));
                        break;
                    case WeaponType.Paired:
                        options.Add(new CaptureOption(CharacterState.Idle, "SecondaryShot"));
                        break;
                }
            }

            if (Cast.isOn) options.Add(new CaptureOption(CharacterState.Idle, "Cast"));
            if (Death.isOn) options.Add(new CaptureOption(CharacterState.Death, null));

            FindObjectOfType<SpriteSheetCapture>().Capture(direction, options, int.Parse(FrameSize.text), int.Parse(FrameCount.text), Shadow.isOn);
            Close();
        }

        public void OnFrameSizeChanged(string value)
        {
            if (FrameSize.text == "") return;

            var valueInt = int.Parse(value);

            if (valueInt < 128) valueInt = 128;
            if (valueInt > 512) valueInt = 512;

            FrameSize.SetTextWithoutNotify(valueInt.ToString());
        }

        public void OnFrameCountChanged(string value)
        {
            if (FrameCount.text == "") return;

            var valueInt = int.Parse(value);

            if (valueInt < 4) valueInt = 4;
            if (valueInt > 16) valueInt = 16;

            FrameCount.SetTextWithoutNotify(valueInt.ToString());
        }
    }
}