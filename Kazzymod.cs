using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Kazzymod
{
	public class Kazzymod : Mod
	{

        public Kazzymod()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true,
            };
		}

		public override void Load()
		{
			if (Main.netMode != NetmodeID.Server)
			{		
				Ref<Effect> dyeRef = new Ref<Effect>(GetEffect("Effects/CustomDyeShaders"));

				GameShaders.Armor.BindShader(ItemType<Items.RubyDye>(), new ArmorShaderData(dyeRef, "ArmorGem")).UseImage("Images/Misc/noise").UseColor(1.5f, 0.15f, 0f);
				GameShaders.Armor.BindShader(ItemType<Items.TopazDye>(), new ArmorShaderData(dyeRef, "ArmorGem")).UseImage("Images/Misc/noise").UseColor(1.7f, 0.75f, 0f);
				GameShaders.Armor.BindShader(ItemType<Items.EmeraldDye>(), new ArmorShaderData(dyeRef, "ArmorGem")).UseImage("Images/Misc/noise").UseColor(0f, 0.8f, 0f);
				GameShaders.Armor.BindShader(ItemType<Items.SapphireDye>(), new ArmorShaderData(dyeRef, "ArmorGem")).UseImage("Images/Misc/noise").UseColor(0f, 0f, 1.1f);
				GameShaders.Armor.BindShader(ItemType<Items.AmethystDye>(), new ArmorShaderData(dyeRef, "ArmorGem")).UseImage("Images/Misc/noise").UseColor(1f, 0f, 1.4f);
				GameShaders.Armor.BindShader(ItemType<Items.DiamondDye>(), new ArmorShaderData(dyeRef, "ArmorGem")).UseImage("Images/Misc/noise").UseColor(0.8f, 0.65f, 1.2f);

				GameShaders.Armor.BindShader(ItemType<Items.HazeDye>(), new ArmorShaderData(dyeRef, "ArmorHaze"));
				GameShaders.Armor.BindShader(ItemType<Items.MatrixDye>(), new ArmorShaderData(dyeRef, "MathTest")).UseColor(2f, 2f, 2f);
				GameShaders.Armor.BindShader(ItemType<Items.LysergicAcidDye>(), new ArmorShaderData(dyeRef, "ArmorLysergicAcid"));
				GameShaders.Armor.BindShader(ItemType<Items.AuroraDye>(), new ArmorShaderData(dyeRef, "ArmorAurora"));

				Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/CustomScreenShaders"));

				Filters.Scene["HydraShockwave"] = new Filter(new ScreenShaderData(screenRef, "HydraShockwave"), EffectPriority.VeryHigh);
				Filters.Scene["HydraShockwave"].Load();
				Filters.Scene["Confetti"] = new Filter(new ScreenShaderData(screenRef, "Confetti"), EffectPriority.VeryHigh);
				Filters.Scene["Confetti"].Load();

				screenRef = new Ref<Effect>(GetEffect("Effects/UnderworldFilter"));

				Filters.Scene["UnderworldFilter"] = new Filter(new ScreenShaderData(screenRef, "UnderworldFilter"), EffectPriority.VeryHigh);
				Filters.Scene["UnderworldFilter"].Load();

				screenRef = new Ref<Effect>(GetEffect("Effects/ShockwaveEffect"));

				Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
				Filters.Scene["Shockwave"].Load();
			}
        }

		public override void Unload()
		{
			
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.TargetDummy);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.SolarFlareHelmet);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.SolarFlareBreastplate);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.SolarFlareLeggings);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.WingsSolar);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.PumpkinMoonMedallion);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.CosmicCarKey);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.Binoculars);
            recipe.AddRecipe();
        }
    }
}