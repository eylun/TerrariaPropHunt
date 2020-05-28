using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace PropHunt
{
    class PropHuntPlayer : ModPlayer
    {
        public bool isUsePropWand = false;
        public PlayerLayer PropEffect;
        public bool isMousingOver = false;
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            if (PropEffect != null)
            {
                for (var i = 0; i < layers.Count; i++)
                {
                    layers[i].visible = false;
                }
                PropEffect.visible = true;
                layers.Add(PropEffect);
            }
            else
            {
                for (var i = 0; i < layers.Count; i++)
                {
                    layers[i].visible = true;
                }
                layers.RemoveAt(layers.Count - 1);
                PropEffect = null;
            }
        }
        public override void ModifyDrawHeadLayers(List<PlayerHeadLayer> layers)
        {
            if (PropEffect != null)
            {
                for (var i = 0; i < layers.Count; i++)
                {
                    layers[i].visible = false;
                }
            }
        }

        public override void ResetEffects()
        {
            isMousingOver = false;
        }
    }
}
