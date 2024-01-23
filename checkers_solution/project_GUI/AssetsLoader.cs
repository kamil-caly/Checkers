using project_logic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace project_GUI
{
    public static class AssetsLoader
    {
        private const string pawnBlackUrl = "assets/PawnB.png";
        private const string pawnWhiteUrl = "assets/PawnW.png";
        private const string LadyBlackUrl = "assets/QueenB.png";
        private const string LadyWhiteUrl = "assets/QueenW.png";

        private readonly static Dictionary<FieldContent, string> whitePieces = new()
        {
            { FieldContent.Pawn, pawnWhiteUrl },
            { FieldContent.Lady, LadyWhiteUrl }
        };

        private readonly static Dictionary<FieldContent, string> blackPieces = new()
        {
            { FieldContent.Pawn, pawnBlackUrl },
            { FieldContent.Lady, LadyBlackUrl }
        };

        public static ImageSource GetImage(FieldContent content, Player color) =>
            new BitmapImage(new Uri(color == Player.White ? whitePieces[content] : blackPieces[content], UriKind.Relative));
    }
}
