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
using Terraria.ObjectData;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

using static Terraria.ModLoader.ModContent;
using System.Runtime.InteropServices;

namespace PropHunt
{
    class PropHuntPlayer : ModPlayer
    {
        public bool isTransformed = false;
        public Nullable<int> mouseX;
        public Nullable<int> mouseY;
        public bool isUsePropWand = false;
        public PlayerLayer PropEffect;
        public bool isMousingOver = false;

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)PropHuntModMessageType.propSync);
            packet.Write(player.whoAmI);
            int playerX = player.GetModPlayer<PropHuntPlayer>().mouseX.GetValueOrDefault();
            int playerY = player.GetModPlayer<PropHuntPlayer>().mouseY.GetValueOrDefault();
            packet.Write(playerX);
            packet.Write(playerY);
            packet.Write(player.GetModPlayer<PropHuntPlayer>().isTransformed);
            packet.Send(toWho, fromWho);
        }
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            if (isTransformed && mouseX != null && mouseY != null)
            {
                Tile mouseOverTile = Main.tile[mouseX.GetValueOrDefault(), mouseY.GetValueOrDefault()];
                TileObjectData tileObject = TileObjectData.GetTileData(mouseOverTile);
                PlayerLayer propEffect = new PlayerLayer("PropHuntMod", "PropEffect", delegate (PlayerDrawInfo drawInfo)
                {
                    Player drawPlayer = drawInfo.drawPlayer;
                    Texture2D selectedTexture = Main.tileTexture[Main.tile[mouseX.GetValueOrDefault(), mouseY.GetValueOrDefault()].type];
                    int styleColumn = 0;
                    int alternate = 0;
                    TileObjectData.GetTileInfo(mouseOverTile, ref styleColumn, ref alternate);
                    int styleRow = 0;
                    if (tileObject.StyleWrapLimit > 0)
                    {
                        styleRow = styleColumn / tileObject.StyleWrapLimit * tileObject.StyleLineSkip;
                        styleColumn %= tileObject.StyleWrapLimit;
                    }
                    int xPad;
                    int yPad;
                    if (tileObject.StyleHorizontal)
                    {
                        xPad = tileObject.CoordinateFullWidth * styleColumn;
                        yPad = tileObject.CoordinateFullHeight * styleRow;
                    }
                    else
                    {
                        xPad = tileObject.CoordinateFullWidth * styleRow;
                        yPad = tileObject.CoordinateFullHeight * styleColumn;
                    }
                    int width = tileObject.CoordinateFullWidth / 18;
                    int widthOffSetTexture = 0;
                    int[] heights = tileObject.CoordinateHeights;
                    int playerX;
                    int playerY;
                    int extraPlayerVerticalPadding;
                    for (int x = 0; x < width; x++)
                    {
                        int heightOffSetTexture = 0;
                        for (int y = 0; y < heights.Length; y++)

                        {
                            if (heights.Length > 1)
                            {
                                extraPlayerVerticalPadding = 1;
                            }
                            else
                            {
                                extraPlayerVerticalPadding = 0;
                            }
                            playerX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X) + x * 16 - 12 - tileObject.CoordinatePadding * width;
                            playerY = (int)(drawInfo.position.Y + drawPlayer.height / 2f - Main.screenPosition.Y + y * 16 + 20 + heights.Length - tileObject.Height * 16 - extraPlayerVerticalPadding);
                            Rectangle rect = new Rectangle(xPad + widthOffSetTexture, yPad + heightOffSetTexture, tileObject.CoordinateWidth, tileObject.CoordinateHeights[y]);
                            Main.playerDrawData.Add(
                                new DrawData(selectedTexture, new Vector2(playerX, playerY), rect, Color.White)
                            );
                            heightOffSetTexture += heights[y] + tileObject.CoordinatePadding;
                        }
                        widthOffSetTexture += 16 + tileObject.CoordinatePadding;
                    }
                });
                PropEffect = propEffect;
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
                layers.Remove(PropEffect);
                PropEffect = null;
            }
        }
        public override void ModifyDrawHeadLayers(List<PlayerHeadLayer> layers)
        {
            if (isTransformed)
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
