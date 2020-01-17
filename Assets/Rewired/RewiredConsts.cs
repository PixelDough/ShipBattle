// <auto-generated>
// Rewired Constants
// This list was generated on 1/16/2020 7:12:45 PM
// The list applies to only the Rewired Input Manager from which it was generated.
// If you use a different Rewired Input Manager, you will have to generate a new list.
// If you make changes to the exported items in the Rewired Input Manager, you will
// need to regenerate this list.
// </auto-generated>

namespace RewiredConsts {
    public static partial class Action {
        // Default
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Quit")]
        public const int Quit = 0;
        // Ship
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Ship", friendlyName = "Turn")]
        public const int Turn = 1;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Ship", friendlyName = "Shoot")]
        public const int Shoot = 2;
    }
    public static partial class Category {
        public const int Default = 0;
    }
    public static partial class Layout {
        public static partial class Joystick {
            public const int Default = 0;
        }
        public static partial class Keyboard {
            public const int Default = 0;
        }
        public static partial class Mouse {
            public const int Default = 0;
        }
        public static partial class CustomController {
            public const int Default = 0;
        }
    }
    public static partial class Player {
        [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "System")]
        public const int System = 9999999;
        [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "Player 1")]
        public const int Player1 = 0;
        [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "Player 2")]
        public const int Player2 = 1;
        [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "Player 3")]
        public const int Player3 = 2;
        [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "Player 4")]
        public const int Player4 = 3;
    }
    public static partial class CustomController {
    }
    public static partial class LayoutManagerRuleSet {
    }
    public static partial class MapEnablerRuleSet {
    }
}
