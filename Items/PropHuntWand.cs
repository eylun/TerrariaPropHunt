using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace PropHunt.Items
{
	public class PropHuntWand : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Transform into furniture!" + "\nLeft Click to transform, right click to cancel transformation");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.value = 100;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.UseSound = SoundID.Item30;
			item.noMelee = true;
		}

		public override bool AltFunctionUse(Player player)
        {
			return true;
        }

        public override bool CanUseItem(Player player)
        {
			if (player.altFunctionUse == 2)
			{
				if (player.GetModPlayer<PropHuntPlayer>().PropEffect != null)
				{
					if (Main.netMode != NetmodeID.SinglePlayer)
					{
						Main.NewText(player.GetModPlayer<PropHuntPlayer>().PropEffect);
						ModPacket packet = mod.GetPacket();
						packet.Write((byte)PropHuntModMessageType.removeProp);
						packet.Write(player.whoAmI);
						packet.Send();
					}
				}
			} else
            {
				if (player.GetModPlayer<PropHuntPlayer>().isMousingOver && Main.netMode != NetmodeID.SinglePlayer)
				{
					int worldX = (int)Main.MouseWorld.X / 16;
					int worldY = (int)Main.MouseWorld.Y / 16;
					if (TileObjectData.GetTileData(Main.tile[worldX, worldY]) != null)
					{
						ModPacket packet = mod.GetPacket();
						packet.Write((byte)PropHuntModMessageType.triggerProp);
						packet.Write(player.whoAmI);
						packet.Write(worldX);
						packet.Write(worldY);
						packet.Send();
					}
				}
			}
			return true;
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("Wood", 3);
			recipe.AddRecipeGroup("PropHunt: Chests", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}