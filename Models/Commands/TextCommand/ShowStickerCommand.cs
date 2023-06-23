namespace ArknightsResources.Stories.Models.Commands
{
    /// <summary>
    /// 表示一个展示Sticker的命令,Sticker可以持久性的展示格式化文本内容
    /// </summary>
    public class ShowStickerCommand : TextCommand
    {
        /// <summary>
        /// 使用指定的参数构造<seealso cref="ShowStickerCommand"/>的新实例
        /// </summary>
        /// <param name="id">Sticker的ID,其确定Sticker的显示顺序</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="alignment"></param>
        /// <param name="size"></param>
        /// <param name="delay"></param>
        /// <param name="width"></param>
        /// <param name="text">剧情文本</param>
        public ShowStickerCommand(string id, double x, double y, string alignment, double size, double delay,
                                  double width, string text) : base(text)
        {
            Id = id;
            X = x;
            Y = y;
            Alignment = alignment;
            Size = size;
            Delay = delay;
            Width = width;
        }

        /// <summary>
        /// Sticker的ID,其确定Sticker的显示顺序
        /// </summary>
        public string Id { get; }

        public double X { get; }

        public double Y { get; }

        public string Alignment { get; }

        public double Size { get; }

        public double Delay { get; }

        public double Width { get; }
    }
}
