using System;
using System.Linq;
using Assets.HeroEditor4D.Common.CommonScripts;
using HeroEditor4D.Common;
using HeroEditor4D.Common.Data;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.CharacterScripts
{
    [Serializable]
    public class CharacterAppearance
    {
        public string Hair = "BuzzCut";
        public string Ears = "Human";
        public string Eyebrows = "Default";
        public string Eyes = "Boy";
        public string Mouth = "Default";
        public string Body = "HumanPants";
        public string Underwear = "MaleUnderwear";
        
        public Color32 HairColor = new Color32(150, 50, 0, 255);
        public Color32 EyesColor = new Color32(0, 200, 255, 255);
        public Color32 BodyColor = new Color32(255, 200, 120, 255);
        public Color32 UnderwearColor = new Color32(120, 100, 80, 255);

        public bool ShowHelmet = true;

        public void Setup(Character4D character)
        {
            character.Parts.ForEach(i => Setup(i));
        }

        public void Setup(CharacterBase character, bool initialize = true)
        {
            if (character.SpriteCollection.Id != "FantasyHeroes") return; // Not supported yet.

            character.Hair = Hair.IsEmpty() ? null : character.HairRenderer.GetComponent<SpriteMapping>().FindSprite(character.SpriteCollection.Hair.FindSprites(Hair));
            character.HairRenderer.color = HairColor;
            character.Ears = Ears.IsEmpty() ? null : character.SpriteCollection.Ears.FindSprites(Ears);

            if (character.Expressions.Count > 0)
            {
                character.Expressions[0] = new Expression { Name = "Default" };

                if (character.name != "Back")
                {
                    character.Expressions[0].Eyebrows = Eyebrows.IsEmpty() ? null : character.EyebrowsRenderer.GetComponent<SpriteMapping>().FindSprite(character.SpriteCollection.Eyebrows.FindSprites(Eyebrows));
                    character.Expressions[0].Eyes = character.EyesRenderer.GetComponent<SpriteMapping>().FindSprite(character.SpriteCollection.Eyes.FindSprites(Eyes));
                    character.Expressions[0].Mouth = character.MouthRenderer.GetComponent<SpriteMapping>().FindSprite(character.SpriteCollection.Mouth.FindSprites(Mouth));
                }

                foreach (var expression in character.Expressions)
                {
                    if (expression.Name != "Dead") expression.EyesColor = EyesColor;
                }
            }

            if (character.EyesRenderer != null)
            {
                character.EyesRenderer.color = EyesColor;
            }

            character.BodyRenderers.ForEach(i => i.color = BodyColor);
            character.HeadRenderer.color = BodyColor;
            character.EarsRenderers.ForEach(i => i.color = BodyColor);

            var body = character.SpriteCollection.Body.Single(i => i.Name == Body);

            character.Body = body.Sprites;

            if (body.Tags.Contains("NoMouth"))
            {
                character.Expressions.ForEach(i => i.Mouth = null);
            }

            if (initialize) character.Initialize();
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public static CharacterAppearance FromJson(string json)
        {
            return JsonUtility.FromJson<CharacterAppearance>(json);
        }
    }
}