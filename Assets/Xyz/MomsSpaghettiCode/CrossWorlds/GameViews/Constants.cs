using System.Collections.Generic;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameViews
{
    public static class Constants
    {
        public static Dictionary<int, string> numberDots = new Dictionary<int, string>
        {
            {1, "•"},
            {2, "<pos=20%>•\n\n<pos=-20%>•"},
            {3, "<pos=20%>•\n•\n<pos=-20%>•"},
            {4, "••\n\n••"},
            {5, "••\n•\n••"},
            {8, "••••\n\n••••"},
            {10, "•••••\n\n•••••"},
        };
    }
}