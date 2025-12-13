using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using PotionCraft.Content.UI.CraftUI;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;

namespace PotionCraft.Content.UI.DialogueUI;
public class ExamplePotion : AutoUIState
{
    public override bool Isload() => true;

    public override string LayersFindIndex => "Vanilla: Mouse Text";

    private UIGrid List;

    public override void OnInitialize()
    {
        Width.Set(626, 0);
        Height.Set(450, 0);
        Top.Set(300, 0);
        HAlign = .5f;
        Active = false;

        List = new UIGrid();
        List.Width.Set(600, 0);
        List.Height.Set(420, 0);
        List.Left.Set(50, 0);
        List.Top.Set(50, 0);

        List.ListPadding = 80f;
        Append(List);

    }

    private string FileStreamText(string filename)
    {
        Mod Mod = ModContent.GetInstance<PotionCraft>();
        using var reader = new StreamReader(Mod.GetFileStream(filename));
        return reader.ReadToEnd();
    }

    public void Init(ExamplePotion examplePotion)
    {
        try
        {
            var js = FileStreamText("Assets/JSON/ExamplePotion.json");
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(js);
            var array = jsonObject["Potions"] as JArray;
            foreach (var item in array)
            {
                var data = JsonConvert.DeserializeObject<ExamplePotionData>(item.ToString());
                var potion = new Item();   
                potion = DialogueQuestion.CreatePotionByName(data._Name);
                data.SetData(potion);
                List.Add(new ItemIcon<ExamplePotion>(examplePotion, potion)
                {
                    ExtraInformation = true,
                    Name = "PotionStyle",
                });
            }
        }
        catch
        {

        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X, (int)GetDimensions().Y, 14, 14), new Rectangle(0, 0, 14, 14), Color.White);
        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X + 14, (int)GetDimensions().Y, (int)Width.Pixels - 28, 14), new Rectangle(14, 0, 14, 14), Color.White);
        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X + (int)Width.Pixels - 14, (int)GetDimensions().Y, 14, 14), new Rectangle(612, 0, 14, 14), Color.White);

        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X, (int)GetDimensions().Y + 14, 14, (int)Height.Pixels - 28), new Rectangle(0, 14, 14, 14), Color.White);
        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X + (int)Width.Pixels - 14, (int)GetDimensions().Y + 14, 14, (int)Height.Pixels - 28), new Rectangle(612, 14, 14, 14), Color.White);

        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X, (int)GetDimensions().Y + (int)Height.Pixels - 14, 14, 14), new Rectangle(0, 392, 14, 14), Color.White);
        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X + 14, (int)GetDimensions().Y + (int)Height.Pixels - 14, (int)Width.Pixels - 28, 14), new Rectangle(14, 392, 14, 14), Color.White);
        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X + (int)Width.Pixels - 14, (int)GetDimensions().Y + (int)Height.Pixels - 14, 14, 14), new Rectangle(612, 392, 14, 14), Color.White);

        spriteBatch.Draw(Assets.UI.Question, new Rectangle((int)GetDimensions().X + 14, (int)GetDimensions().Y + 14, (int)Width.Pixels - 28, (int)Height.Pixels - 28), new Rectangle(14, 14, 14, 14), Color.White);

        base.Draw(spriteBatch);

    }

    private class ExamplePotionData
    {
        public string _Name = "";

        public string IconName = "";

        public string CustomName = "";

        public bool CanCustomName;

        public string Signatures = "";

        public bool CanEditor;

        public void SetData(Item item)
        {
            if (item.ModItem is not BasePotion potion)
                return;
            potion.CustomName = CustomName;
            potion.CanCustomName = CanCustomName;
            potion.Signatures = Signatures;
            potion.CanEditor = CanEditor;
            potion.IconName = IconName;

        }
    }

}

