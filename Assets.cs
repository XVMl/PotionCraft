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
            public static Asset<Texture2D> UITexture(string path)=> ModContent.Request<Texture2D>(Path.UI + path);

            public static Texture2D ItemSlotBackgroud => UITexture("Scoreboard").Value;

            public static Texture2D ItemSlotUIStateBackGroud => UITexture("LisaoQuestion").Value;

            public static Texture2D Panel => UITexture("CategoryPanel").Value;

            public static Texture2D Button=>UITexture("Button_Filtering").Value;

            public static Texture2D Slot => UITexture("Slot_Back").Value;

            public static Texture2D BackGround => UITexture("Background").Value;
         
            public static Texture2D BackGround1 => UITexture("Background1").Value;

            public static Texture2D Tooltip => UITexture("Tooltip").Value;

            public static Texture2D PanelGrayscale => UITexture("PanelGrayscale").Value;
            
        }
        public class Items
        {
            public static Asset<Texture2D> ItemsTexture(string path) => ModContent.Request<Texture2D>(Path.Items+ path);

            public static Texture2D BasePotion => ItemsTexture("BasePotion").Value;


        }
        public class NPCs
        {
            public static Asset<Texture2D> NPCsTexture(string path) => ModContent.Request<Texture2D>(Path.NPCs + path);

            public static Texture2D DendriticNoiseZoomedOut => NPCsTexture("DendriticNoiseZoomedOut").Value;

            public static Texture2D BloomCircleSmall => NPCsTexture("BloomCircleSmall").Value;
            
            public static Texture2D LaserChannel => NPCsTexture("LaserChannel").Value;

            public static Texture2D Energy => NPCsTexture("Energy").Value;


        }
    }

    
}
