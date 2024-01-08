using System.Collections.Generic;
using System.Linq;

public class MajiangHuleView : MajiangTextView
{
    public void UpdateInfo(Majiang.BoardModel board, Majiang.Hule hule)
    {
        lock (_lock)
        {
            if (hule == null)
            {
                this.info = "";
                return;
            }

            var player_id = board.player_id[hule.l];
            var player = board.player[player_id];
            var info = $"{player}: ";

            info += hule.baojia != null ? "ロン\n\n" : "ツモ\n\n";

            foreach (var yaku in hule.hupai)
            {
                if (yaku.fanshu[0] == '*')
                {
                    info += $"{yaku.name}\n";
                }
                else
                {
                    info += $"{yaku.name} {yaku.fanshu}翻\n";
                }
            }

            if (hule.fanshu != null)
            {
                info += $"\n{hule.fanshu}翻 {hule.fu}符\n";
            }
            else
            {
                info += $"\n役満\n";
            }


            var fenpei = hule.fenpei
                .Where(x => x < 0)
                .OrderBy(x => x)
                .Distinct()
                .ToList();
            if (fenpei.Count >= 2)
            {
                info += $"{-fenpei[0]}、{-fenpei[1]}点\n";
            }
            else if (fenpei.Count == 1)
            {
                info += $"{-fenpei[0]}点\n";
            }

            this.info = info;
        }
    }

    public void UpdateInfoOnPingju(Majiang.Pingju pingju)
    {
        lock (_lock)
        {
            if (pingju == null)
            {
                this.info = "";
                return;
            }

            var info = "流局\n\n";

            info += $"{pingju.name}\n\n";

            this.info = info;
        }
    }

    class PlayerInfo
    {
        public int l;
        public int rank;
        public string name;
        public int defen;
        public string point;
    }

    public void UpdateInfoOnJieju(Majiang.Paipu paipu)
    {
        lock (_lock)
        {
            if (paipu == null)
            {
                this.info = "";
                return;
            }

            var info = "終局\n\n";

            var playerInfoList = new List<PlayerInfo>();
            for (var l = 0; l < 4; l++)
            {
                var playerInfo = new PlayerInfo();
                playerInfo.l = l;
                playerInfo.rank = paipu.rank[l];
                playerInfo.name = paipu.player[l];
                playerInfo.defen = paipu.defen[l];
                playerInfo.point = paipu.point[l];
                playerInfoList.Add(playerInfo);
            }

            playerInfoList = playerInfoList.OrderBy(x => x.rank).ToList();

            foreach (var playerInfo in playerInfoList)
            {
                info += $"{playerInfo.rank}位 {playerInfo.name} {playerInfo.defen}点 ({playerInfo.point})\n";
            }

            this.info = info;
        }
    }
}