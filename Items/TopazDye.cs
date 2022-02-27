using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace Kazzymod.Items
{
    class TopazDye : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.maxStack = 99;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(this, 3);
            recipe.AddRecipe();

        }
    }
}
