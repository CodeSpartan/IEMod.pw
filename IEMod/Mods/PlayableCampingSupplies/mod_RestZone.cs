using IEMod.Mods.Options;
using Patchwork;
using UnityEngine;

namespace IEMod.Mods.PlayableCampingSupplies
{
	[ModifiesType]
	public class mod_RestZone : RestZone
	{
		[ModifiesMember("Camp")]
		private void Camp()
		{
            PlayerInventory inventory = GameState.s_playerCharacter.Inventory;

            if (IEModOptions.MaxCampingSupplies != IEModOptions.MaxCampingSuppliesOptions.Disabled) {
                if (inventory)
                {
                    inventory.CampingSuppliesTotal--;
                }
            }

            RestZone.Rest(RestZone.Mode.Camp);
		}

        [ModifiesMember("WhyCannotCamp")]
        public static RestZone.CannotCampReason WhyCannotCamp()
        {
            RestZone.CannotCampReason cannotCampReason = 0;
            if (GameState.s_playerCharacter == null)
            {
                return RestZone.CannotCampReason.Error;
            }
            if (GameState.InCombat)
            {
                cannotCampReason |= RestZone.CannotCampReason.InCombat;
            }

            if (IEModOptions.MaxCampingSupplies != IEModOptions.MaxCampingSuppliesOptions.Disabled)
            {
                PlayerInventory inventory = GameState.s_playerCharacter.Inventory;
                if (inventory == null || inventory.CampingSuppliesTotal == 0)
                {
                    cannotCampReason |= RestZone.CannotCampReason.NoSupplies;
                }
            }

            if (GameState.Instance.CurrentMap != null && !GameState.Instance.CurrentMap.GetCanCamp())
            {
                cannotCampReason |= RestZone.CannotCampReason.NoCampMap;
            }
            if (PartyHelper.PartyCanSeeEnemy())
            {
                cannotCampReason |= RestZone.CannotCampReason.PartyCanSeeEnemy;
            }
            return cannotCampReason;
        }
    }
}
