public class MajiangScoreView : MajiangTextView
{
    public void UpdateInfo(Majiang.IBoard board)
    {
        lock (_lock)
        {
            if (board == null)
            {
                this.info = "";
                return;
            }

            var lunban_id = board.lunban >= 0 ? board.player_id[board.lunban] : -1;

            var info = "";
            for (int i = 0; i < board.defen.Count; i++)
            {
                var defen = board.defen[i];
                var name = board.player[i];
                var menfeng = board.menfeng(i) == 0 ? "(親)" : "";
                var lunban = lunban_id == i ? "*" : " ";
                info += $"{lunban}{name}{menfeng}: {defen}\n";
            }

            this.info = info;
        }
    }
}