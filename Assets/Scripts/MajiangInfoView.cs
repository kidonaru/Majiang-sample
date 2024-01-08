public class MajiangInfoView : MajiangTextView
{
    public void UpdateInfo(Majiang.BoardModel model)
    {
        lock (_lock)
        {
            if (model == null)
            {
                this.info = "";
                return;
            }

            var info = model.jushu >= 4 ? "南" : "東";
            info += $"{model.jushu + 1}局 {model.changbang}本場\n";

            this.info = info;
        }
    }
}