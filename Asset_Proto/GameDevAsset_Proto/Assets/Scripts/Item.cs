using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item
{
    public enum ItemType
    {
        HealthPotion,
        MajorHealthPotion,
        SpeedPotion,
        InvincibilityPotion
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion:
                return ItemAssets.instance.healthPotionSprite;

            case ItemType.MajorHealthPotion:
                return ItemAssets.instance.majorHealthPotionSprite;

            case ItemType.SpeedPotion:
                return ItemAssets.instance.speedPotionSprite;

            case ItemType.InvincibilityPotion:
                return ItemAssets.instance.invincibilityPotionSprite;

        }
    }
    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion:
            case ItemType.MajorHealthPotion:
            case ItemType.SpeedPotion:
            case ItemType.InvincibilityPotion:
                return true;
        }
    }
}
