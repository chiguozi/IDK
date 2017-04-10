public static class Events
{
    static public class GameEvent
    {
        public const string OnSelfModelLoaded = "OnSelfModelLoaded";
    }

    static public class SelfControlEvent
    {
        public const string OnJoyStickMove = "OnJoyStickMove";
        public const string OnUseSkill = "OnUseSkill";
    }
	
	static public class UIEvent
	{
		public const string OnWndClose = "OnWndClose";
		public const string OnWndOpen = "OnWndOpen";
		public const string OnWndLoaded = "OnWndLoaded";
		public const string OnWndHide = "OnWndHide";
	}
	
}