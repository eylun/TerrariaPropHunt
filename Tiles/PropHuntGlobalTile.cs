using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.UI;
using Terraria.ObjectData;
using Terraria.UI;
using Terraria.UI.Chat;
namespace PropHunt.Tiles
{
    class PropHuntGlobalTile : GlobalTile
    {
        public override void MouseOver(int i, int j, int type)
        {
            Main.LocalPlayer.GetModPlayer<PropHuntPlayer>().isMousingOver = true;
        }
    }
}
