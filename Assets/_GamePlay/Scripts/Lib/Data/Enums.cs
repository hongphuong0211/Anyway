using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Lib
{
    public class Enums
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static List<T> SortOrder<T>(List<T> list)
        {
            return list.OrderBy(d => System.Guid.NewGuid()).Take(list.Count).ToList();
        }
    }

    public enum GameMode { Burger = 0, Candy = 1, Sandwich = 2, Rotten = 3 }

    public enum GamePlay { Stack, Queue }

    public enum GameState { Shop, MainMenu, Pause, GamePlay }

    public enum GameResult { Win, Lose }

    public enum ButtonState { Buy, Equip, Equipped, Ads, CommingSoon }

    public enum PriceType { Ads, Gem }

    public enum BurgerState { Good, CutOff, Spicy, Flat, Roten, Shit }

    [System.Serializable]
    public class SkinPrice
    {
        public PriceType priceType;
        public int value;
    }
}


