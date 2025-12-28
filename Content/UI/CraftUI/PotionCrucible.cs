using Microsoft.Xna.Framework.Graphics;
using PotionCraft.Content.Items;
using PotionCraft.Content.System;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using static PotionCraft.Content.System.AutoLoaderSystem.LoaderPotionOrMaterial;
using System;
using System.Collections.Generic;
namespace PotionCraft.Content.UI.CraftUI
{
    public class PotionCrucible : PotionElement<BrewPotionState>
    {
        private UIElement Crucible;

        public Button<BrewPotionState> MashUp;

        public Button<BrewPotionState> Putity;

        private BrewPotionState BrewPotionState;

        private List<MovementElement> movementList = new();

        public PotionCrucible(BrewPotionState brewPotionState)
        {
            BrewPotionState = brewPotionState;
            PotionCraftState = brewPotionState;
            Height.Set(400, 0);
            Width.Set(400, 0);
            Crucible = new UIElement();
            Crucible.Width.Set(384, 0);
            Crucible.Height.Set(234, 0);
            Crucible.Top.Set(250, 0);
            Crucible.HAlign = .5f;
            Append(Crucible);

            MashUp = new(Assets.UI.HelpIcon, Color.White, brewPotionState)
            {
                Name = "MashUp",
                Active = false
            };
            MashUp.Width.Set(32, 0);
            MashUp.Height.Set(32, 0);
            MashUp.Top.Set(160, 0);
            MashUp.Left.Set(360, 0);
            MashUp.OnClike = () =>
            {
                BrewPotionState.MashUp(brewPotionState.CreatPotion, brewPotionState.currentItem);
                brewPotionState.Craft = null;
                brewPotionState.PotionMaterial = null;
                Putity.Active = false;
                MashUp.Active = false;
                brewPotionState.Refresh();
            };
            MashUp.TransitionAnimation = () =>
            {
                MashUp.A = MathHelper.Lerp(MashUp.A, MashUp.Active ? 1 : .7f, .05f);
            };
            MashUp.HoverTexture = Assets.UI.HelpIconActive;
            Append(MashUp);

            Putity = new(Assets.UI.HelpIcon, Color.White, brewPotionState)
            {
                Name = "Putity",
                Active = false
            };
            Putity.Width.Set(32, 0);
            Putity.Height.Set(32, 0);
            Putity.Top.Set(200, 0);
            Putity.Left.Set(360, 0);
            Putity.OnClike = () =>
            {
                BrewPotionState.Putify(brewPotionState.CreatPotion, brewPotionState.currentItem);
                brewPotionState.Craft = null;
                brewPotionState.PotionMaterial = null;
                Putity.Active = false;
                MashUp.Active = false;
                brewPotionState.Refresh();
            };
            Putity.TransitionAnimation = () =>
            {
                Putity.A = MathHelper.Lerp(Putity.A, Putity.Active ? 1 : .7f, .05f);
            };
            Putity.HoverTexture = Assets.UI.HelpIconActive;
            Append(Putity);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MashUp.Update(gameTime);
            Putity.Update(gameTime);
            for (int i = 0; i < movementList.Count; i++)
            {
                MovementElement element = movementList[i];
                if (element.CheckEnd)
                {
                    movementList.Remove(element);
                    continue;
                }
                element.Update();
            }
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (!PotionCraftState.Active) return;
            if (Main.mouseItem.IsAir)
                return;
            movementList.Add(new MovementElement(Main.MouseScreen, GetDimensions().Center()
                , Main.mouseItem));
            if (!PotionList.ContainsKey(Main.mouseItem.Name) && Main.mouseItem.ModItem is not MagicPanacea)
                return;

            BrewPotionState.AddPotion(Main.mouseItem);
            //Main.LocalPlayer.HeldItem.TurnToAir();
            //Main.mouseItem.TurnToAir();
            Main.LocalPlayer.HeldItem.stack--;
            Main.mouseItem.stack--;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var element in movementList)
            {
                element.Draw(spriteBatch);
            }
            spriteBatch.Draw(Assets.UI.Crucible, Crucible.GetDimensions().ToRectangle(), Color.White);
            
            base.Draw(spriteBatch);
        }


        public class MovementElement
        {
            private Vector2 startPos;

            private Vector2 endPos;

            private Vector2 position;

            private Func<float, float> movement;

            public Item item;

            private int time = 0;

            public MovementElement(Vector2 startPos, Vector2 endPos, Item _item)
            {
                this.startPos = startPos;
                position = startPos;
                this.endPos = endPos;
                item = _item.Clone();
                item.stack = 1;
                Main.NewText((int)Math.Sqrt((endPos.Y - startPos.Y) / 0.03f));
            }

            public bool CheckEnd => time == (int)Math.Sqrt((endPos.Y - startPos.Y) /0.03f);

            public void Update()
            {
                time++;
                position.Y = startPos.Y + time * time * .03f;
            }

            public static (float a, float b, float c) CalculateParabolaCoefficients(Vector2 p1, Vector2 p2, Vector2 p3)
            {
                var b = ((p1.Y - p3.Y) * (p1.X * p1.X - p2.X * p2.X) - (p1.Y - p2.Y) * (p1.X * p1.X - p3.X * p3.X)) /
                        ((p1.X - p3.X) * (p1.X * p1.X - p2.X * p2.X) - (p1.X - p2.X) * (p1.X * p2.X - p3.X * p3.X));

                var a = ((p1.Y - p2.Y) - b * (p1.X - p2.X)) / (p1.X * p1.X - p2.X * p2.X);

                var c = p1.Y - a * p1.X * p1.X - b * p1.X;
                return (a, b, c);
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                Main.inventoryScale = 2.030f;
                ItemSlot.Draw(spriteBatch, ref item, 21, position);
            }

        }


    }

}
