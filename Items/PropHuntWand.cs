using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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
					player.GetModPlayer<PropHuntPlayer>().PropEffect = null;
				}
			} else
            {
				if (player.GetModPlayer<PropHuntPlayer>().isMousingOver)
				{
					player.GetModPlayer<PropHuntPlayer>().isUsePropWand = true;
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