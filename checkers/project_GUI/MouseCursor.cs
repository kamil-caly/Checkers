using project_logic;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace project_GUI
{
    public static class MouseCursor
    {
        public static Cursor GetCursor(Player player)
        {
            string url = "";

            if (player == Player.White)
            {
                url = "assets/CursorW.cur";
            }
            else
            {
                url = "assets/CursorB.cur";
            }

            Stream stream = Application.GetResourceStream(new Uri(url, UriKind.Relative)).Stream;
            return new Cursor(stream, true);
        }
    }
}
