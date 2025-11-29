using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using PotionCraft.Content.UI.CraftUI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static PotionCraft.Content.System.LanguageHelper;

namespace PotionCraft.Content.UI.DialogueUI;

public class DialogueQuestion:PotionElement<DialogueState>
{
    private ItemIcon<DialogueState> _preview;

    private ItemIcon<DialogueState> _submit;

    private SlotState _state = SlotState.Preview;

    private enum SlotState
    {
        Preview,
        Submit,
    }
    
    public override void OnInitialize()
    {
        
    }

    private void InitPreItem(int level)
    {
        var question =  CreatQuestion(level);
        var item = new Item();
        item.SetDefaults(ModContent.ItemType<BasePotion>());
        var preitem = AsPotion(item);
        preitem._Name = question;
    }

    public static Item CreatePotionByName()
    {
        var item = new Item();
        item.SetDefaults(ModContent.ItemType<BasePotion>());
        var modItem = AsPotion(item);
        var list = modItem._Name.Split(' ');
        var material = new Item();
        foreach (var craft in list)
        {
            switch (craft)
            {
                case "+":
                   BrewPotionState.MashUp(modItem,material);
                    break;
                case "@":
                   BrewPotionState.Putify(modItem,material); 
                    break;
                default:
                    material = new Item();
                    material.SetDefaults(ItemID.Search.GetId(craft));
                    break;
            }
        }
        return item;
    }
}