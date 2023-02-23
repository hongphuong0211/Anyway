using Assets.HeroEditor4D.Common.CharacterScripts;
using HeroEditor4D.Common.Enums;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.ExampleScripts
{
    /// <summary>
    /// An example of how to change character's appearance.
    /// </summary>
    public class CharacterAppearance : MonoBehaviour
    {
        public Character4D Character;

        public void SetRandomAppearance()
        {
            SetRandomHair();
            SetRandomEyebrows();
            SetRandomEyes();
            SetRandomMouth();
        }

        public void ResetAppearance()
        {
            new CharacterScripts.CharacterAppearance().Setup(Character); // CharacterScripts.CharacterAppearance is another way to set appearance.
        }

        public void SetRandomHair()
        {
            var randomIndex = Random.Range(0, Character.SpriteCollection.Hair.Count);
            var randomItem = Character.SpriteCollection.Hair[randomIndex];
            var randomColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

            Character.SetBody(randomItem, BodyPart.Hair, randomColor);
        }

        public void SetRandomEyebrows()
        {
            var randomIndex = Random.Range(0, Character.SpriteCollection.Eyebrows.Count);
            var randomItem = Character.SpriteCollection.Eyebrows[randomIndex];

            Character.SetBody(randomItem, BodyPart.Eyebrows);
        }

        public void SetRandomEyes()
        {
            var randomIndex = Random.Range(0, Character.SpriteCollection.Eyes.Count);
            var randomItem = Character.SpriteCollection.Eyes[randomIndex];
            var randomColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

            Character.SetBody(randomItem, BodyPart.Eyes, randomColor);
        }

        public void SetRandomMouth()
        {
            var randomIndex = Random.Range(0, Character.SpriteCollection.Mouth.Count);
            var randomItem = Character.SpriteCollection.Mouth[randomIndex];

            Character.SetBody(randomItem, BodyPart.Mouth);
        }
    }
}