using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using static PotionCraft.Assets;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria.GameContent;
using Terraria.GameInput;
using static PotionCraft.Content.System.AutoLoaderSystem.LoaderPotionOrMaterial;
using static PotionCraft.Content.System.LanguageHelper;
using Luminance.Common.Utilities;
using Terraria.GameContent.UI.Elements;
using Terraria.Audio;
using Terraria.ID;
using ReLogic.Content;
namespace PotionCraft.Content.UI.CraftUI
{
    public class BrewPotionState : AutoUIState
    {
        public override bool Active() => ActiveState && CraftState == CraftUiState.BrewPotion;

        public override bool Isload() => true;

        public override string LayersFindIndex => "Vanilla: Mouse Text";

        private PotionSlot<BrewPotionState> PotionSlot;
        
        private Item currentItem;

        private BrewPotionButton BrewPotionButton;

        public int PotionCount = 20;

        public BasePotion CreatPotion = ModContent.GetInstance<BasePotion>();
        
        private UIElement Area;
        
        public Queue<Action> Crafts = new();

        private PotionSynopsis PotionSynopsis;

        private PotionComponent potionComponent;

        private PotionSetting potionSetting;

        private ColorSelector colorSelector;

        private Slider brewPotionSlider;

        private bool changecraft;
        public override void OnInitialize()
        {
            Area = new UIElement()
            {
                HAlign = 0.2f,
                VAlign = 0.3f,
            };
            Area.Width.Set(270f, 0);
            Area.Height.Set(270f, 0);
            Append(Area);

            PotionSynopsis = new PotionSynopsis(this);
            PotionSynopsis.Top.Set(300, 0);
            PotionSynopsis.Left.Set(500, 0);
            PotionSynopsis.SourcePotion = new Vector2(PotionSynopsis.Left.Pixels, PotionSynopsis.Top.Pixels);
            Append(PotionSynopsis);

            colorSelector = new(this);
            colorSelector.Top.Set(300, 0);
            colorSelector.Left.Set(100, 0);
            Append(colorSelector);

            potionSetting = new PotionSetting(this)
            {
                HAlign = .5f,
                VAlign = .7f
            };
            Append(potionSetting);

        }

        public static bool QuicklyCheckPotion(Item item1,BasePotion item2)
        {
            if (item1.ModItem is not BasePotion)
                return false;
            return AsPotion(item1)._Name == item2._Name;
        }
        
        private void AddPotion(Item item)
        {
            if (PotionList.ContainsKey(item) && item.ModItem is not BasePotion)
                return;
            
            currentItem = item;
            if (!QuicklyCheckPotion(item, CreatPotion) && item.type != ModContent.ItemType<MagicPanacea>())
            {
                Crafts.Enqueue(()=>MashUp(item));
                Crafts.Last()?.Invoke();
                return;
            }
            changecraft = false;
            Crafts.Enqueue(() => Putify(item));
            Crafts.Last()?.Invoke();
        }
        
        private void Putify(Item item)
        {
            
            foreach (var buff in CreatPotion.PotionDictionary)
            {
                buff.Value.BuffTime *= 2;
                buff.Value.Counts *= 2;
            }
            for (var i = 0; i < CreatPotion.DrawPotionList.Count; i++)
            {
                CreatPotion.DrawCountList[i] *= 2;
            }
            CreatPotion.PurifyingCount++;
            CreatPotion._Name += "@ ";
        }

        private void ChangeCraft()
        { 
            Crafts.Dequeue();
            if (!changecraft)
            {
                Crafts.Enqueue(()=>MashUp(currentItem));
            }
            else
            {
                Crafts.Enqueue(()=>Putify(currentItem));
            }
            changecraft = !changecraft;;
            Refresh();
        }
        
        private void MashUp(Item item)
        {
            var count = 0;
            if(item.ModItem is BasePotion)
                goto BasePotion;
            
            var name = Lang.GetBuffName(item.buffType);
            CreatPotion.PotionDictionary.TryAdd(name, new PotionData(
                name,
                item.type,
                0,
                0,
                item.buffType
            ));
            CreatPotion.PotionDictionary[name].BuffTime+= item.buffTime;
            CreatPotion.PotionDictionary[name].Counts++;
            CreatPotion.DrawPotionList.Add(item.type);
            CreatPotion.DrawCountList.Add(1);
            CreatPotion._Name +=
                $"{Lang.GetBuffName(item.buffType).Replace(" ", "")} + ";    
            goto End;
            
            BasePotion:
            var material = AsPotion(item);
            foreach (var buff in material.PotionDictionary)
            {
                CreatPotion.PotionDictionary.TryAdd(buff.Key,buff.Value);
                CreatPotion.PotionDictionary[buff.Key].BuffTime+= buff.Value.BuffTime;
                CreatPotion.PotionDictionary[buff.Key].Counts+= buff.Value.Counts;
            }
            
            for (var i = 0; i < material.DrawCountList.Count; i++)
            {
                CreatPotion.DrawPotionList.Add(material.DrawPotionList[i]);
                CreatPotion.DrawCountList.Add(material.DrawCountList[i]);
            }
            CreatPotion.MashUpCount += material.MashUpCount+1;
            count += material.MashUpCount;
            CreatPotion._Name +=
                $"{material._Name} +";    
            
            End:
            CreatPotion.MashUpCount++;
            item.stack--;
            if (CreatPotion.MashUpCount == 0|| CreatPotion.MashUpCount == count + 1)
                CreatPotion._Name = $"{Lang.GetBuffName(item.buffType).Replace(" ","")} ";
        }

        public void Refresh()
        {
            Potion = new Item();
            Potion.SetDefaults(ModContent.ItemType<BasePotion>());
            foreach (var action in Crafts)
            {
                action.Invoke();
            }
        }
        
        private void ClearAll()
        {
            CreatPotion = ModContent.GetInstance<BasePotion>();
            Crafts.Clear();
            Potion = new Item();
            Potion.SetDefaults(ModContent.ItemType<BasePotion>());
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            PotionSynopsis.Update(gameTime);
            potionSetting.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            
        }

    }

    public class PotionComponent : PotionElement<BrewPotionState>
    {
        public PotionComponent(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            Width.Set(342f, 0);
            Height.Set(388f, 0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            var tex = UITexture("Ui3").Value;
            spriteBatch.Draw(tex, GetDimensions().Position() + new Vector2(0,-24),new Rectangle(0,0,342,24), Color.White);
            spriteBatch.Draw(tex, GetDimensions().Position(), Color.White);
            spriteBatch.Draw(tex, GetDimensions().Position() + new Vector2(0, +24), new Rectangle(0, 164, 342, 24), Color.White);
        }
    }

    public class PotionSetting : PotionElement<BrewPotionState>
    {
        private Button delete;

        private Button brew;

        private Slider slider;

        private Button autouse;

        public PotionSetting(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            Width.Set(384f, 0);
            Height.Set(212f, 0);
            delete = new Button(UITexture("Delete"), Color.White, "Delete");
            delete.Height.Set(34, 0);
            delete.Width.Set(96, 0);
            delete.Left.Set(50, 0);
            delete.Top.Set(170, 0);
            Append(delete);
            brew = new Button(UITexture("Brew"), Color.White, "Brew");
            brew.Height.Set(34, 0);
            brew.Width.Set(96, 0);
            brew.Left.Set(250, 0);
            brew.Top.Set(170, 0);
            Append(brew);
            slider = new Slider(brewPotionState);
            slider.Left.Set(90, 0);
            slider.Top.Set(122, 0);
            Append(slider);
            autouse = new Button(Assets.UI.Icon, Color.White, new Rectangle(40,0,18,18));
            autouse.Height.Set(18, 0);
            autouse.Width.Set(18, 0);
            autouse.Left.Set(122, 0);
            autouse.Top.Set(46, 0);
            autouse.iconcolor = Deafult;
            Append(autouse);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            slider.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.UI.UI4, GetDimensions().ToRectangle(), Color.White);
            base.Draw(spriteBatch);
        }

    }

    public class Button : PotionElement<BrewPotionState>
    {
        private Asset<Texture2D> Texture;

        private Rectangle Rectangle =Rectangle.Empty;

        public Action onClike;

        public string text;

        public float scale;

        public Color color = Color.White;

        public Color iconcolor = Color.White;
        public Button(Asset<Texture2D> texture2D ,Color color,string text ="",float scale=1)
        {
            Texture  = texture2D;
            this.text = text;
            this.color = color;
            this.scale = scale;
        }
        public Button(Asset<Texture2D> texture2D, Color color, Rectangle rectangle, string text = "", float scale = 1)
        {
            Texture = texture2D;
            Rectangle = rectangle;
            this.text = text;
            this.color = color;
            this.scale = scale;
        }
        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            onClike?.Invoke();
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            SoundEngine.PlaySound(SoundID.MenuClose);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!Rectangle.IsEmpty)
                spriteBatch.Draw(Texture.Value, GetDimensions().ToRectangle(),Rectangle ,iconcolor);
            else
                spriteBatch.Draw(Texture.Value, GetDimensions().ToRectangle(), iconcolor);
            Utils.DrawBorderString(spriteBatch, text,GetDimensions().Position()+new Vector2(30,Height.Pixels/2) , color, 0.75f);
            base.Draw(spriteBatch);
        }

    }

    public class BrewPotionButton : PotionElement<BrewPotionState>
    {
        public static MethodInfo SetModItem = typeof(Item).GetProperty(nameof(Item.ModItem))!.GetSetMethod();
        public BrewPotionButton(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            Width.Set(100f, 0);
            Height.Set(32f, 0);
        }

        private void BrewPotion(Item potion)
        {
            if (PotionCraftState.Crafts.Count != 0)
                foreach (var craft in PotionCraftState.Crafts) craft.Invoke();
             
            SetModItem.Invoke(PotionCraftState.Potion, [PotionCraftState.CreatPotion]);
            Main.LocalPlayer.BuyItem(50 * PotionCraftState.PotionCount);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (PotionCraftState.Potion.IsAir) return;
            BrewPotion((PotionCraftState.Potion));
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var rectangle = GetDimensions().ToRectangle();
            spriteBatch.Draw(Assets.UI.Button, rectangle, Color.White);
            ItemSlot.DrawSavings(spriteBatch,rectangle.X, rectangle.Y);
        }

    }

    public class BrewPotionMashUpButton : PotionElement<BrewPotionState>
    {
        public bool CouldClick;
        public BrewPotionMashUpButton(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            Width.Set(100f, 0);
            Height.Set(50f, 0);
        }

        

    }

    public class BrewPotionInput:PotionElement<BrewPotionState>
    {

        public bool Inputting;
        
        public (string,string) Currentvalue= ("","");

        public List<(string,string)> Recordvalue=new();
        
        public string Showstring="";

        private string SelectColor = Deafult_Hex;
        
        public Action Onchange;
        public BrewPotionInput(BrewPotionState brewPotionState,string currentValue=null)
        {
            PotionCraftState = brewPotionState;
            Width.Set(180f, 0f);
            Height.Set(50f, 0f);
            Recordvalue = ParseTextToList(currentValue);
        }

        public BrewPotionInput(Action onchange, BrewPotionState brewPotionState,string currentvalue=null )
        {
            PotionCraftState = brewPotionState;
            Recordvalue = ParseTextToList(currentvalue);
            Onchange = onchange;
        }

        private void InputText()
        {
            Inputting = true;
            Main.blockInput= true;
            
        }

        private void HandleInputText()
        {
            if (Main.keyState.IsKeyDown(Keys.Escape))
            {
                EndInputText();
            }

            PlayerInput.WritingText = true;
            Main.instance.HandleIME();
            if (Currentvalue.Item2.Length == 0)
                Currentvalue = StListText();
            Currentvalue = (Currentvalue.Item1,Main.GetInputText(Currentvalue.Item2));
        }

        private void EndInputText()
        {
            Main.blockInput = false;
            Inputting = false;
            Recordvalue.Add(Currentvalue);
            Showstring = string.Join("", Recordvalue.Select(s=>string.IsNullOrEmpty(s.Item1) ? Deafult_Hex : s.Item1.Insert(10,s.Item2)));
            Onchange?.Invoke();
        }

        public override void Update(GameTime gameTime)
        {
            if (!PotionCraftState.Active())
                return;
            if (Main.mouseLeft && !IsMouseHovering)
            {
                EndInputText();
            }
            if (Inputting)
            {
                HandleInputText();
            }
        }

        private (string,string) StListText()
        {
            if (Recordvalue.Count == 0)
                return ("","");
            var ss = Recordvalue.Last();
            Recordvalue.Remove(Recordvalue.Last());
            return ss;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (!PotionCraftState.Active())
                return;
            InputText();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Utils.DrawBorderString(spriteBatch, Showstring, GetDimensions().Position(), Color.White, 0.75f, 0f, 0f, -1);
            Vector2 vector2 = GetDimensions().Position() + 
                              FontAssets.MouseText.Value.MeasureString(string.Join("",Recordvalue.Select(s=>s.Item2))) ;
            if (Inputting)
            {
                spriteBatch.Draw(Assets.UI.PanelGrayscale, GetDimensions().ToRectangle(), new Color(49, 84, 141));
                Main.instance.DrawWindowsIMEPanel(GetDimensions().Position());
                Utils.DrawBorderString(spriteBatch, Currentvalue.Item1.Insert(10, Currentvalue.Item2), vector2,
                    Color.White, 0.75f, 0f, 0f, -1);
            }
            if (Main.GameUpdateCount % 20U >= 10U)
                return;
            Utils.DrawBorderString(spriteBatch, "|", vector2+FontAssets.MouseText.Value.MeasureString(Currentvalue.Item2), Color.White, 0.75f, 0.0f, 0.0f, -1);

        }

    }
    
    public class BrewPotionPutifyButton : PotionElement<BrewPotionState>
    {
        public bool CouldClick;
        public BrewPotionPutifyButton(BrewPotionState brewPotionState)
        {
            PotionCraftState = brewPotionState;
            Width.Set(100f, 0);
            Height.Set(50f, 0);
        }
        
        
        
    }
    
    
}
