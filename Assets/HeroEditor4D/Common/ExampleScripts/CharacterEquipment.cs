using Assets.HeroEditor4D.Common.CharacterScripts;
using HeroEditor4D.Common.Enums;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.ExampleScripts
{
    /// <summary>
    /// An example of how to change character's equipment.
    /// </summary>
    public class CharacterEquipment : MonoBehaviour
    {
        public Character4D Character;

        public void EquipRandomArmor()
        {
            var randomIndex = Random.Range(0, Character.SpriteCollection.Armor.Count);
            var randomItem = Character.SpriteCollection.Armor[randomIndex];

            Character.Equip(randomItem, EquipmentPart.Armor);
        }

        public void RemoveArmor()
        {
            Character.UnEquip(EquipmentPart.Armor);
        }

        public void EquipRandomHelmet()
        {
            var randomIndex = Random.Range(0, Character.SpriteCollection.Armor.Count);
            var randomItem = Character.SpriteCollection.Armor[randomIndex];

            Character.Equip(randomItem, EquipmentPart.Helmet);
        }

        public void RemoveHelmet()
        {
            Character.UnEquip(EquipmentPart.Helmet);
        }

        public void EquipRandomShield()
        {
            var randomIndex = Random.Range(0, Character.SpriteCollection.Shield.Count);
            var randomItem = Character.SpriteCollection.Shield[randomIndex];

            Character.Equip(randomItem, EquipmentPart.Shield);
        }

        public void RemoveShield()
        {
            Character.UnEquip(EquipmentPart.Shield);
        }

        public void EquipRandomWeapon()
        {
            var randomIndex = Random.Range(0, Character.SpriteCollection.MeleeWeapon1H.Count);
            var randomItem = Character.SpriteCollection.MeleeWeapon1H[randomIndex];

            Character.Equip(randomItem, EquipmentPart.MeleeWeapon1H);
        }

        public void RemoveWeapon()
        {
            Character.UnEquip(EquipmentPart.MeleeWeapon1H);
        }
    }
}