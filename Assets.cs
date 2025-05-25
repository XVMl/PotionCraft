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
        public class UI
        {
            public static Asset<Texture2D> UITexture(string path)=> ModContent.Request<Texture2D>("PotionCraft/Assets/UI/" + path);

            public static Texture2D ItemSlotBackgroud => UITexture("Scoreboard").Value;

            public static Texture2D ItemSlotUIStateBackGroud => UITexture("LisaoQuestion").Value;

            public static Texture2D Panel => UITexture("CategoryPanel").Value;

            public static Texture2D Button=>UITexture("Button_Filtering").Value;

            public static Texture2D Slot => UITexture("Slot_Back").Value;

            public static Texture2D BackGround => UITexture("Background").Value;
         
            public static Texture2D BackGround1 => UITexture("Background1").Value;
        }
        public class Items
        { 
        
        }

    }

    
}
