using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace PotionCraft       
{
    public class Assets
    {
        public static Asset<Texture2D> UITexture(string path) => ModContent.Request<Texture2D>(Path.UI + path);
        public static Asset<Texture2D> ItemsTexture(string path) => ModContent.Request<Texture2D>(Path.Items + path);

        public class Path
        {
            public static readonly string Assets = "PotionCraft/Assets/";

            public static readonly string UI = Assets + "UI/";
            
            public static readonly string Items = Assets + "Items/";

            public static readonly string NPCs = Assets + "NPCs/";
            
            public static readonly string Noise = Assets + "Noise/";

        }
        public class UI
        {
            
            public static Texture2D ItemSlotBackgroud => UITexture("Scoreboard").Value;

            public static Texture2D ItemSlotUIStateBackGroud => UITexture("LisaoQuestion").Value;

            public static Texture2D Panel => UITexture("CategoryPanel").Value;

            public static Texture2D Button=>UITexture("Button_Filtering").Value;

            public static Texture2D Slot => UITexture("Slot_Back").Value;

            public static Texture2D BackGround => UITexture("Background").Value;
         
            public static Texture2D BackGround1 => UITexture("Background1").Value;

            public static Texture2D Tooltip => UITexture("Tooltip").Value;

            public static Texture2D PanelGrayscale => UITexture("PanelGrayscale").Value;

            public static Texture2D Delete => UITexture("Delete").Value;

            public static Texture2D Brew => UITexture("Brew").Value;

            public static Texture2D UI4 => UITexture("UI4").Value;

            public static Asset<Texture2D> UI1 => UITexture("UI1");

            public static Texture2D UI2 => UITexture("UI2").Value;
            
            public static Texture2D UI5 => UITexture("UI5").Value;

            public static Texture2D ColorUI => UITexture("ColorUI").Value;

            public static Texture2D Circular => UITexture("Circular").Value;

            public static Texture2D Crucible => UITexture("Crucible").Value;

            public static Asset<Texture2D> Icon => UITexture("Icon");

            public static Asset<Texture2D> ColorSelector => UITexture("ColorSelector");

            public static Asset<Texture2D> Input => UITexture("Input");

            public static Asset<Texture2D> HajimiIcon = UITexture("HajimiIcon");
            
            public static Asset<Texture2D> HajimiIconHover = UITexture("HajimiIconHover");
            
            public static Asset<Texture2D> HelpIcon=>UITexture("HelpIcon");

            public static Asset<Texture2D> HelpIconActive => UITexture("HelpIconActive");
            
            public static Asset<Texture2D> Dialogue => UITexture("Dialogue");

            public static string PotionCraftBG => Path.UI + "PotionCraftBG";
        }
        public class Items
        {
            
            public static Texture2D BasePotion => ItemsTexture("BasePotion").Value;

        }
        public class NPCs
        {
            public static Asset<Texture2D> NPCsTexture(string path) => ModContent.Request<Texture2D>(Path.NPCs + path);

            public static Texture2D DendriticNoiseZoomedOut => NPCsTexture("DendriticNoiseZoomedOut").Value;

            public static Texture2D BloomCircleSmall => NPCsTexture("BloomCircleSmall").Value;
            
            public static Texture2D LaserChannel => NPCsTexture("LaserChannel").Value;

            public static Texture2D Energy => NPCsTexture("Energy").Value;

            public static Texture2D Hajimi => NPCsTexture("Hajimi").Value;

            public static Texture2D Emoji1 => NPCsTexture("Emoji1").Value;

            public static Texture2D Emoji2 => NPCsTexture("Emoji2").Value;

            public static Texture2D Emoji3 => NPCsTexture("Emoji3").Value;

            public static Texture2D Emoji4 => NPCsTexture("Emoji4").Value;

        }
    }

    
}
