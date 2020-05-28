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
            if (Main.LocalPlayer.GetModPlayer<PropHuntPlayer>().isUsePropWand)
            {
                Tile mouseOverTile = Main.tile[i, j];
                TileObjectData tileObject = TileObjectData.GetTileData(mouseOverTile);
                if (tileObject != null)
                {
                    PlayerLayer propEffect = new PlayerLayer("PropHuntMod", "PropEffect", delegate (PlayerDrawInfo drawInfo) {
                        Player drawPlayer = drawInfo.drawPlayer;
                        Texture2D selectedTexture = Main.tileTexture[Main.tile[i, j].type];
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
                                if (heights.Length > 1) {
                                    extraPlayerVerticalPadding = 1;
                                } else
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
                    }
                    );
                    Main.LocalPlayer.GetModPlayer<PropHuntPlayer>().PropEffect = propEffect;
                } 
                Main.LocalPlayer.GetModPlayer<PropHuntPlayer>().isUsePropWand = false;
            }
        }
    }
}
