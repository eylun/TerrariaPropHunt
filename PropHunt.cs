using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace PropHunt
{
	public class PropHunt : Mod
	{
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
			bool anyPlayerTransformed = false;
            foreach (var player in Main.player)
            {
				if (player.GetModPlayer<PropHuntPlayer>().isTransformed)
                {
					anyPlayerTransformed = true;
                }
            }
			if (anyPlayerTransformed)
            {
				int healthBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Entity Health Bars"));
				int mouseItemIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Item / NPC Head"));
				if (healthBarIndex != -1)
				{
					layers.RemoveAt(healthBarIndex);
				}
				if (mouseItemIndex != -1)
				{
					layers.RemoveAt(mouseItemIndex);
				}
			}
        }

        public override void AddRecipeGroups()
		{
			RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Chest", new int[]
			{
				ItemID.Chest,
				ItemID.GoldChest,
				ItemID.ShadowChest,
				ItemID.EbonwoodChest,
				ItemID.RichMahoganyChest,
				ItemID.PearlwoodChest,
				ItemID.IvyChest,
				ItemID.IceChest,
				ItemID.LivingWoodChest,
				ItemID.SkywareChest,
				ItemID.ShadewoodChest,
				ItemID.WebCoveredChest,
				ItemID.LihzahrdChest,
				ItemID.WaterChest,
				ItemID.JungleChest,
				ItemID.CorruptionChest,
				ItemID.CrimsonChest,
				ItemID.HallowedChest,
				ItemID.FrozenChest,
				ItemID.DynastyChest,
				ItemID.HoneyChest,
				ItemID.SteampunkChest,
				ItemID.PalmWoodChest,
				ItemID.MushroomChest,
				ItemID.BorealWoodChest,
				ItemID.SlimeChest,
				ItemID.GreenDungeonChest,
				ItemID.PinkDungeonChest,
				ItemID.BlueDungeonChest,
				ItemID.BoneChest,
				ItemID.CactusChest,
				ItemID.FleshChest,
				ItemID.ObsidianChest,
				ItemID.PumpkinChest,
				ItemID.SpookyChest,
				ItemID.GlassChest,
				ItemID.MartianChest,
				ItemID.GraniteChest,
				ItemID.MeteoriteChest,
				ItemID.MarbleChest,
				ItemID.CrystalChest,
				ItemID.GoldenChest


			});
			RecipeGroup.RegisterGroup("PropHunt: Chests", group);
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			PropHuntModMessageType msgType = (PropHuntModMessageType)reader.ReadByte();
			switch (msgType)
			{
				case PropHuntModMessageType.triggerProp:
					int playerId = reader.ReadInt32();
					int mouseX = reader.ReadInt32();
					int mouseY = reader.ReadInt32();
					Player triggerPlayer = Main.player[playerId];
					triggerPlayer.GetModPlayer<PropHuntPlayer>().mouseX = mouseX;
					triggerPlayer.GetModPlayer<PropHuntPlayer>().mouseY = mouseY;
					triggerPlayer.GetModPlayer<PropHuntPlayer>().isTransformed = true;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)PropHuntModMessageType.triggerProp);
						packet.Write(playerId);
						packet.Write(mouseX);
						packet.Write(mouseY);
						packet.Send(-1);
					}
					break;
				case PropHuntModMessageType.removeProp:
					playerId = reader.ReadInt32();
					triggerPlayer = Main.player[playerId];
					triggerPlayer.GetModPlayer<PropHuntPlayer>().isTransformed = false;
					triggerPlayer.GetModPlayer<PropHuntPlayer>().mouseX = null;
					triggerPlayer.GetModPlayer<PropHuntPlayer>().mouseY = null;
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)PropHuntModMessageType.removeProp);
						packet.Write(playerId);
						packet.Send(-1);
					}
					break;

			}
		}
	}
	internal enum PropHuntModMessageType : byte
	{
		triggerProp,
		removeProp
	}
}