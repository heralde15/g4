using Terraria.Social.Base;

namespace Terraria.ZazaMod;

public class FuckingJoinRequest : UserJoinToServerRequest
{
	private string _text;

	public FuckingJoinRequest(string userDisplayName, string fullIdentifier)
		: base(userDisplayName, fullIdentifier)
	{
		_text = base.UserDisplayName + "\n";
		switch (Main.rand.Next(5))
		{
		default:
			_text += "This Bloke Wants to Join you";
			break;
		case 1:
			_text += "This Fucker Wants to Join you";
			break;
		case 2:
			_text += "This Weirdo Wants to Join you";
			break;
		case 3:
			_text += "This Great Gal Wants to Join you";
			break;
		case 4:
			_text += "The one guy who beat you up 30 years ago Wants to Join you";
			break;
		}
	}

	public override bool IsValid()
	{
		return true;
	}

	public override string GetUserWrapperText()
	{
		return _text;
	}
}
