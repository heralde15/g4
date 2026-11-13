using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.Visual;

public sealed class HitboxDisplayCheat : ToggleCheat<HitboxDisplayCheat.CheatSettings>
{
	public enum HitboxDisplayMode
	{
		Outline,
		OutlineAndOverlay,
		Overlay
	}

	public sealed class CheatSettings : USettings
	{
		public bool Players;

		public bool NPCs;

		public bool Projectiles;

		public bool WorldItems;

		public Color PlayerColor;

		public Color FriendlyNPCColor;

		public Color ImmortalNPCColor;

		public Color UnfriendlyNotDamagingNPCColor;

		public Color HostileNPCColor;

		public Color HostileInvincibleNPCColor;

		public Color FriendlyOwnedProjectileColor;

		public Color FriendlyOtherProjectileColor;

		public Color HostileProjectileColor;

		public Color WorldItemColor;

		[JsonConverter(typeof(StringEnumConverter))]
		public HitboxDisplayMode DisplayMode;

		public bool LocalPlayerIsRGB;

		public bool TeamBasedPlayerColor;

		public bool WorldItemRarityBasedColor;

		public CheatSettings()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			PlayerColor = Color.Turquoise;
			FriendlyNPCColor = Color.Lime;
			ImmortalNPCColor = Color.Gold;
			UnfriendlyNotDamagingNPCColor = Color.Orange;
			HostileNPCColor = Color.Red;
			HostileInvincibleNPCColor = new Color(70, 0, 0);
			FriendlyOwnedProjectileColor = Color.LimeGreen;
			FriendlyOtherProjectileColor = Color.Gold;
			HostileProjectileColor = new Color(255, 0, 100);
			WorldItemColor = new Color(130, 0, 255);
			base._002Ector();
		}
	}

	public override string Name => "Hitbox Display";

	public override CheatCategory Category => CheatCategory.Visual;
}
