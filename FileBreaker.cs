using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;

[assembly: AssemblyTitle("文件工具箱")]
[assembly: AssemblyProduct("文件工具箱")]
[assembly: AssemblyDescription("本地文件处理工具：生成损坏副本、修改文件时间戳")]
[assembly: AssemblyCompany("Nanyin520")]
[assembly: AssemblyCopyright("Copyright (c) 2026 Nanyin520")]
[assembly: AssemblyFileVersion("1.1.0.0")]
[assembly: AssemblyInformationalVersion("1.1.0")]
[assembly: AssemblyVersion("1.1.0.0")]

namespace FileBreaker
{
    // ================= 主题 =================
    internal static class T
    {
        public static readonly Color Window = Color.FromArgb(244, 246, 250);
        public static readonly Color Card = Color.White;
        public static readonly Color Line = Color.FromArgb(232, 235, 242);
        public static readonly Color Text = Color.FromArgb(24, 31, 45);
        public static readonly Color TextSub = Color.FromArgb(96, 107, 128);
        public static readonly Color TextMute = Color.FromArgb(148, 158, 174);
        public static readonly Color Soft = Color.FromArgb(246, 248, 252);
        public static readonly Color SoftHover = Color.FromArgb(240, 243, 249);
        public static readonly Color SoftPress = Color.FromArgb(232, 236, 245);
        public static readonly Color SegTrack = Color.FromArgb(238, 241, 247);
        public static readonly Color Primary = Color.FromArgb(43, 92, 224);
        public static readonly Color PrimaryHover = Color.FromArgb(36, 79, 200);
        public static readonly Color PrimaryDown = Color.FromArgb(30, 66, 170);
        public static readonly Color AccentTint = Color.FromArgb(232, 238, 253);
        public static readonly Color Danger = Color.FromArgb(219, 68, 76);
        public static readonly Color DangerHover = Color.FromArgb(196, 52, 60);
        public static readonly Color DangerDown = Color.FromArgb(170, 42, 50);
        public static readonly Color BtnOff = Color.FromArgb(243, 245, 249);
        public static readonly Color BtnOffLine = Color.FromArgb(231, 234, 241);
        public static readonly Color Track = Color.FromArgb(238, 241, 246);
    }

    internal static class F
    {
        public const string Family = "微软雅黑";
        public const string Mono = "Consolas";
        public static readonly Font H1 = new Font(Family, 17f, FontStyle.Bold);
        public static readonly Font H2 = new Font(Family, 11.5f, FontStyle.Bold);
        public static readonly Font Body = new Font(Family, 9.75f);
        public static readonly Font Small = new Font(Family, 9f);
        public static readonly Font Tiny = new Font(Family, 8.25f);
        public static readonly Font Btn = new Font(Family, 10f, FontStyle.Bold);
        public static readonly Font Digit = new Font(Mono, 12f);
        public static readonly Font MonoData = new Font(Mono, 10.5f);
        public static readonly Font Tab = new Font(Family, 10.5f, FontStyle.Bold);
    }

    // ================= 多语言 =================
    internal static class L
    {
        public static bool En = false;

        private static string S(string zh, string en) { return En ? en : zh; }

        private static readonly string[] ZhUnits = { "年", "月", "日", "时", "分", "秒" };
        private static readonly string[] EnSeps = { "-", "-", " ", ":", ":", "" };

        public static string DtUnit(int i) { return En ? EnSeps[i] : ZhUnits[i]; }

        // 窗口与页签
        public static string Title { get { return S("文件工具箱", "File Toolkit"); } }
        public static string Sub { get { return S("本地处理 · 不联网 · 原始文件不被改动", "Local only · offline · originals untouched"); } }
        public static string TabBreak { get { return S("文件破坏", "Corrupt"); } }
        public static string TabTime { get { return S("修改时间", "Timestamps"); } }
        public static string LangBtn { get { return S("EN", "中文"); } }

        // 选择文件卡片
        public static string PickTitle { get { return S("选择目标文件", "Select target file"); } }
        public static string HintBreak { get { return S("生成一个与原件同大小、同后缀的损坏副本，原文件保持不动", "Same size, same extension — the original file stays untouched"); } }
        public static string HintTime { get { return S("只修改时间戳，文件内容与大小完全不变", "Only timestamps change — content and size stay identical"); } }
        public static string Placeholder { get { return S("尚未选择文件 · 可点击右侧按钮，或把文件拖进窗口", "No file selected — browse or drag a file here"); } }
        public static string Browse { get { return S("浏览文件", "Browse"); } }
        public static string OutLabel { get { return S("生成后保存到", "Save output to"); } }
        public static string OutPlaceholder { get { return S("默认与原文件同一文件夹", "Default: same folder as original"); } }
        public static string OutBtn { get { return S("选择文件夹", "Folder…"); } }
        public static string OutHint { get { return S("不选则输出到原文件所在文件夹；同名文件会自动追加序号，不会覆盖", "Leave empty to save beside the original; duplicates get a number, never overwritten"); } }
        public static string DropHint { get { return S("也可以直接把文件拖到窗口里", "You can also drag a file into this window"); } }

        // 进度卡片
        public static string ProgressTitle { get { return S("处理进度", "Progress"); } }
        public static string Ready { get { return S("就绪 · 等待选择文件", "Ready · waiting for a file"); } }
        public static string RunBtn { get { return S("开始破坏", "Start"); } }
        public static string OutNote { get { return S("输出文件命名为「原名_broken.后缀」；若同名已存在会自动追加序号", "Output is named \"name_broken.ext\"; a number is appended if it already exists"); } }
        public static string LocalTip { get { return S("所有处理都在本机完成，不会上传任何文件", "Everything runs locally — nothing is uploaded"); } }
        public static string Selected(string name) { return S("已选择 " + name, "Selected " + name); }
        public static string ReadyTail { get { return S(" · 可以开始", " · ready"); } }
        public static string Working { get { return S("正在处理……", "Working…"); } }
        public static string DonePrefix { get { return S("完成 · 已生成 ", "Done · created "); } }
        public static string FailPrefix { get { return S("失败：", "Failed: "); } }

        // 使用步骤
        public static string HelpTitle { get { return S("使用步骤", "How to use"); } }
        public static string HelpText { get { return S("① 选择目标文件　→　② 点击「开始破坏」　→　③ 生成损坏副本，原文件不受影响", "① Pick a file  →  ② Click \"Start\"  →  ③ A copy is produced, original untouched"); } }
        public static string HelpNote { get { return S("适合给损坏样本做脱敏、测试异常文件处理流程；副本大小与原文件保持一致。", "Handy for sanitising damaged samples and testing bad-file handling; the copy keeps the exact size."); } }

        // 时间页
        public static string CurTitle { get { return S("当前时间", "Current timestamps"); } }
        public static string KeyCreation { get { return S("创建时间", "Creation"); } }
        public static string KeyWrite { get { return S("修改时间", "Modified"); } }
        public static string KeyAccess { get { return S("访问时间", "Accessed"); } }
        public static string TargetTitle { get { return S("目标时间", "Target time"); } }
        public static string CopyCurrent { get { return S("取当前修改时间", "Use modified time"); } }
        public static string ApplyBtn { get { return S("应用修改", "Apply"); } }
        public static string FillCurrent { get { return S("已填入当前修改时间", "Filled with the current modified time"); } }
        public static string ChangedTo { get { return S("已修改 ", "Changed "); } }

        // 对话框
        public static string MsgTitle { get { return S("提示", "Notice"); } }
        public static string MsgDoneTitle { get { return S("破坏完成", "Corruption complete"); } }
        public static string MsgTimeDone { get { return S("完成", "Done"); } }
        public static string MsgErr { get { return S("错误", "Error"); } }
        public static string NeedFile { get { return S("请先选择一个有效的文件。", "Please choose a valid file first."); } }
        public static string NeedTick { get { return S("请至少勾选一项要修改的时间。", "Tick at least one timestamp to change."); } }
        public static string OpenBreakTitle { get { return S("选择要破坏的文件", "Pick a file to corrupt"); } }
        public static string OpenTimeTitle { get { return S("选择要修改时间的文件", "Pick a file to retime"); } }
        public static string AllFiles { get { return S("所有文件 (*.*)|*.*", "All files (*.*)|*.*"); } }
        public static string FolderDesc { get { return S("选择生成后的保存位置", "Choose where to save the copy"); } }
        public static string ReadFail { get { return S("读取文件失败：", "Cannot read file: "); } }
        public static string ReadTimeFail { get { return S("读取时间失败：", "Cannot read timestamps: "); } }
        public static string ModifyFail { get { return S("修改失败：", "Failed: "); } }

        public static string BreakDoneBody(string path, long oldSize, long newSize, string ext)
        {
            if (En)
                return "Corrupted copy created:\n" + path +
                       "\n\nOriginal size: " + oldSize + " bytes" +
                       "\nNew size: " + newSize + " bytes" +
                       "\nExtension: " + ext;
            return "已生成破坏后的文件：\n" + path +
                   "\n\n原大小：" + oldSize + " 字节" +
                   "\n新大小：" + newSize + " 字节" +
                   "\n后缀名：" + ext;
        }

        public static string TimeDoneBody(string path, string items, string target)
        {
            if (En)
                return "Timestamps changed:\n" + path + "\n\nFields: " + items + "\nTarget: " + target;
            return "已修改文件时间：\n" + path + "\n\n已修改项：" + items + "\n目标时间：" + target;
        }

        public static string TimeErrBody(string msg)
        {
            return En ? "Failed to change timestamps:\n" + msg : "修改时间失败：\n" + msg;
        }
    }

    // ================= 语言偏好持久化 =================
    internal static class Cfg
    {
        private static string FilePath
        {
            get
            {
                string d = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FileBreaker");
                try { Directory.CreateDirectory(d); } catch { }
                return Path.Combine(d, "settings.txt");
            }
        }

        public static void Load()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string s = File.ReadAllText(FilePath).Trim().ToLowerInvariant();
                    if (s.StartsWith("lang=")) { L.En = s.Substring(5).Trim().StartsWith("en"); return; }
                }
            }
            catch { }

            // 没有保存过偏好时，跟随系统语言
            try
            {
                string two = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                L.En = (two != "zh");
            }
            catch { L.En = false; }
        }

        public static void Save()
        {
            try { File.WriteAllText(FilePath, "lang=" + (L.En ? "en" : "zh")); } catch { }
        }
    }

    // ================= 绘制工具 =================
    internal static class Draw
    {
        public static GraphicsPath Round(RectangleF r, float radius)
        {
            var path = new GraphicsPath();
            if (radius < 1f) { path.AddRectangle(r); return path; }
            float d = radius * 2f;
            if (d > r.Width) d = r.Width;
            if (d > r.Height) d = r.Height;
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static void Box(Graphics g, RectangleF r, float radius, Color fill)
        {
            using (var p = Round(r, radius))
            using (var b = new SolidBrush(fill))
                g.FillPath(b, p);
        }

        // 用「外框填色 + 内框填色」画 1px 边框，避免画笔描边被裁剪出黑边
        public static void Box(Graphics g, RectangleF r, float radius, Color fill, Color border)
        {
            using (var p = Round(r, radius))
            using (var b = new SolidBrush(border))
                g.FillPath(b, p);
            Box(g, RectangleF.Inflate(r, -1f, -1f), Math.Max(0.5f, radius - 1f), fill);
        }

        public static void Clear(Graphics g, Rectangle r, Color c)
        {
            using (var b = new SolidBrush(c))
                g.FillRectangle(b, r);
        }

        public static void Txt(Graphics g, string s, Font f, Color c, Rectangle r, TextFormatFlags align)
        {
            TextRenderer.DrawText(g, s, f, r, c,
                align | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
        }

        public static int Measure(string s, Font f)
        {
            return TextRenderer.MeasureText(s, f, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding).Width;
        }
    }

    // ================= 圆角按钮 =================
    internal enum BtnKind { Primary, Danger, Ghost, Subtle }

    internal sealed class UiButton : Control
    {
        public BtnKind Kind = BtnKind.Primary;
        public float Corner = 10f;

        private bool _hover;
        private bool _down;

        public UiButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                   | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = T.Card;
            Font = F.Btn;
            Cursor = Cursors.Hand;
            TabStop = false;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Cursor = Enabled ? Cursors.Hand : Cursors.Default;
            _hover = false; _down = false;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hover = true; Invalidate(); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hover = false; _down = false; Invalidate(); }
        protected override void OnMouseDown(MouseEventArgs e) { base.OnMouseDown(e); if (e.Button == MouseButtons.Left) { _down = true; Invalidate(); } }
        protected override void OnMouseUp(MouseEventArgs e) { base.OnMouseUp(e); _down = false; Invalidate(); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            Draw.Clear(g, ClientRectangle, BackColor);

            var r = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f);
            Color fill, border, text;

            if (!Enabled)
            {
                fill = T.BtnOff; border = T.BtnOffLine; text = T.TextMute;
            }
            else
            {
                switch (Kind)
                {
                    case BtnKind.Danger:
                        fill = _down ? T.DangerDown : (_hover ? T.DangerHover : T.Danger);
                        border = fill; text = Color.White; break;
                    case BtnKind.Ghost:
                        fill = _down ? T.SoftPress : T.Card;
                        border = _hover ? T.Primary : T.Line;
                        text = _hover ? T.Primary : T.TextSub; break;
                    case BtnKind.Subtle:
                        fill = _down ? T.SoftPress : (_hover ? T.SoftHover : T.Soft);
                        border = fill; text = T.Text; break;
                    default:
                        fill = _down ? T.PrimaryDown : (_hover ? T.PrimaryHover : T.Primary);
                        border = fill; text = Color.White; break;
                }
            }

            Draw.Box(g, r, Corner, fill, border);
            Draw.Txt(g, Text, Font, text, ClientRectangle,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }

    // ================= 卡片 =================
    internal sealed class UiCard : Panel
    {
        public float Corner = 14f;

        public UiCard()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                   | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = T.Window;
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            Draw.Clear(g, ClientRectangle, BackColor);

            // 极轻的投影
            Draw.Box(g, new RectangleF(1f, 2f, Width - 2f, Height - 2f), Corner, Color.FromArgb(9, 30, 45, 80));

            var r = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 2f);
            Draw.Box(g, r, Corner, T.Card, T.Line);
        }
    }

    // ================= 输入框 =================
    internal sealed class UiField : Panel
    {
        public readonly TextBox Box;

        private string _placeholder = "";
        public string Placeholder
        {
            get { return _placeholder; }
            set
            {
                _placeholder = value ?? "";
                _ph.Text = _placeholder;
                _ph.Visible = (Box.TextLength == 0) && _placeholder.Length > 0;
            }
        }

        private bool _focus;
        private readonly Label _ph;

        public UiField()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                   | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = T.Card;
            Box = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = T.Soft,
                ForeColor = T.Text,
                Font = F.Body,
                ReadOnly = true,
            };
            Box.Enter += delegate { _focus = true; Invalidate(); };
            Box.Leave += delegate { _focus = false; Invalidate(); };

            _ph = new Label
            {
                BackColor = T.Soft,
                ForeColor = T.TextMute,
                Font = F.Small,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Visible = false
            };
            _ph.Click += delegate { Box.Focus(); };

            Controls.Add(Box);
            Controls.Add(_ph);
            _ph.BringToFront();

            Box.TextChanged += delegate
            {
                _ph.Visible = (Box.TextLength == 0) && Placeholder.Length > 0;
                Invalidate();
            };
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            int h = Box.PreferredHeight;
            int y = (Height - h) / 2 + 1;
            Box.SetBounds(14, y, Width - 28, h);

            int phH = Box.PreferredHeight + 4;
            _ph.SetBounds(15, y - 2, Width - 30, phH);
            _ph.Text = _placeholder;
            _ph.Visible = (Box.TextLength == 0) && _placeholder.Length > 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            Draw.Clear(g, ClientRectangle, BackColor);
            Draw.Box(g, new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f), 9f,
                T.Soft, _focus ? T.Primary : Color.FromArgb(221, 226, 236));
        }
    }

    // ================= 进度条 =================
    internal sealed class UiProgress : Control
    {
        private int _value;

        public int Value
        {
            get { return _value; }
            set
            {
                int v = value;
                if (v < 0) v = 0;
                if (v > 100) v = 100;
                if (v != _value) { _value = v; Invalidate(); }
            }
        }

        public UiProgress()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                   | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = T.Card;
            Height = 24;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            Draw.Clear(g, ClientRectangle, BackColor);

            int barW = Width - 56;
            float h = 8f;
            float top = (Height - h) / 2f;

            Draw.Box(g, new RectangleF(0.5f, top, barW, h), h / 2f, T.Track);

            if (_value > 0)
            {
                float w = Math.Max(h, barW * (_value / 100f));
                if (w > barW) w = barW;
                Draw.Box(g, new RectangleF(0.5f, top, w, h), h / 2f,
                    _value >= 100 ? Color.FromArgb(34, 160, 110) : T.Primary);
            }

            Draw.Txt(g, _value + "%", F.Small, T.TextMute,
                new Rectangle(barW + 10, 0, Width - barW - 10, Height), TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
        }
    }

    // ================= 分段切换 =================
    internal sealed class SegmentTabs : Control
    {
        public string[] Items = new string[0];
        public int SelectedIndex;
        public event EventHandler SelectedIndexChanged;

        private int _hover = -1;

        public SegmentTabs()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                   | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = T.Window;
            Font = F.Tab;
            Cursor = Cursors.Hand;
        }

        private Rectangle SegmentRect(int i)
        {
            int n = Math.Max(1, Items.Length);
            int gap = 4;
            int w = (Width - gap * 2) / n;
            return new Rectangle(gap + i * w, gap, w, Height - gap * 2);
        }

        private int IndexAt(Point p)
        {
            for (int i = 0; i < Items.Length; i++)
                if (SegmentRect(i).Contains(p)) return i;
            return -1;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int i = IndexAt(e.Location);
            if (i != _hover) { _hover = i; Invalidate(); }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_hover != -1) { _hover = -1; Invalidate(); }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            int i = IndexAt(e.Location);
            if (i >= 0 && i != SelectedIndex)
            {
                SelectedIndex = i;
                Invalidate();
                if (SelectedIndexChanged != null) SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            Draw.Clear(g, ClientRectangle, BackColor);

            Draw.Box(g, new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f), 12f, T.SegTrack);

            for (int i = 0; i < Items.Length; i++)
            {
                var r = SegmentRect(i);
                bool sel = i == SelectedIndex;

                if (sel)
                    Draw.Box(g, new RectangleF(r.X + 0.5f, r.Y + 0.5f, r.Width - 1f, r.Height - 1f), 9f,
                        T.Card, Color.FromArgb(226, 230, 238));
                else if (i == _hover)
                    Draw.Box(g, new RectangleF(r.X + 0.5f, r.Y + 0.5f, r.Width - 1f, r.Height - 1f), 9f,
                        Color.FromArgb(250, 251, 254));

                Draw.Txt(g, Items[i], Font, sel ? T.Primary : (_hover == i ? T.Text : T.TextSub), r,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }
    }

    // ================= 开关 =================
    internal sealed class UiSwitch : Control
    {
        private bool _checked;
        public string Label;

        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (value != _checked)
                {
                    _checked = value;
                    Invalidate();
                    if (CheckedChanged != null) CheckedChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler CheckedChanged;

        public UiSwitch()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                   | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = T.Card;
            Font = F.Body;
            Cursor = Cursors.Hand;
            Height = 26;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Checked = !Checked;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            Draw.Clear(g, ClientRectangle, BackColor);

            string label = Label ?? Text;
            int tw = 46;
            Draw.Txt(g, label, Font, T.Text, new Rectangle(0, 0, Math.Max(1, Width - tw), Height),
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            float th = 22f, tw2 = 44f;
            float x = Width - tw2;
            float y = (Height - th) / 2f;

            Draw.Box(g, new RectangleF(x, y, tw2, th), th / 2f,
                _checked ? T.Primary : Color.FromArgb(214, 220, 230));

            float knob = th - 4f;
            float kx = _checked ? x + tw2 - knob - 2f : x + 2f;
            using (var b = new SolidBrush(Color.White))
                g.FillEllipse(b, kx, y + 2f, knob, knob);
        }
    }

    // ================= 日期时间输入 =================
    internal sealed class DateTimeBox : Control
    {
        private static readonly string[] Units = { "年", "月", "日", "时", "分", "秒" };

        private int _y = DateTime.Now.Year;
        private int _mo = DateTime.Now.Month;
        private int _d = DateTime.Now.Day;
        private int _h = DateTime.Now.Hour;
        private int _mi = DateTime.Now.Minute;
        private int _s = DateTime.Now.Second;

        private int _seg = 3;
        private string _buf = "";
        private bool _focus;
        private readonly Rectangle[] _rects = new Rectangle[6];

        public DateTime Value
        {
            get { return Compose(); }
            set { SetValue(value); }
        }

        public DateTimeBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                   | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = T.Card;
            Cursor = Cursors.IBeam;
            TabStop = true;
        }

        private int MaxDay { get { return DateTime.DaysInMonth(_y, _mo); } }

        private DateTime Compose()
        {
            int day = _d;
            if (day > MaxDay) day = MaxDay;
            if (day < 1) day = 1;
            return new DateTime(_y, _mo, day, _h, _mi, _s);
        }

        private void SetValue(DateTime v)
        {
            _y = v.Year; _mo = v.Month; _d = v.Day;
            _h = v.Hour; _mi = v.Minute; _s = v.Second;
            _buf = "";
            Invalidate();
        }

        private int GetSeg(int i)
        {
            switch (i)
            {
                case 0: return _y;
                case 1: return _mo;
                case 2: return _d;
                case 3: return _h;
                case 4: return _mi;
                default: return _s;
            }
        }

        private void SetSeg(int i, int v)
        {
            switch (i)
            {
                case 0:
                    if (v < 1900) v = 1900;
                    if (v > 2999) v = 2999;
                    _y = v;
                    if (_d > MaxDay) _d = MaxDay;
                    break;
                case 1:
                    if (v < 1) v = 1;
                    if (v > 12) v = 12;
                    _mo = v;
                    if (_d > MaxDay) _d = MaxDay;
                    break;
                case 2:
                    if (v < 1) v = 1;
                    if (v > MaxDay) v = MaxDay;
                    _d = v;
                    break;
                case 3:
                    if (v < 0) v = 23;
                    if (v > 23) v = 0;
                    _h = v;
                    break;
                case 4:
                    if (v < 0) v = 59;
                    if (v > 59) v = 0;
                    _mi = v;
                    break;
                default:
                    if (v < 0) v = 59;
                    if (v > 59) v = 0;
                    _s = v;
                    break;
            }
            Invalidate();
        }

        protected override void OnEnter(EventArgs e) { base.OnEnter(e); _focus = true; Invalidate(); }
        protected override void OnLeave(EventArgs e) { base.OnLeave(e); _focus = false; _buf = ""; Invalidate(); }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
            _buf = "";
            for (int i = 0; i < 6; i++)
            {
                var hit = _rects[i];
                hit.Inflate(6, 6);
                if (hit.Contains(e.Location)) { _seg = i; break; }
            }
            Invalidate();
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
                return true;
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if (c >= '0' && c <= '9')
            {
                e.Handled = true;
                int maxLen = _seg == 0 ? 4 : 2;
                if (_buf.Length >= maxLen) _buf = "";
                _buf += c;
                SetSeg(_seg, int.Parse(_buf));
                if (_buf.Length >= maxLen)
                {
                    _buf = "";
                    if (_seg < 5) _seg++;
                }
                Invalidate();
            }
            else if (c == '\b')
            {
                e.Handled = true;
                _buf = "";
                Invalidate();
            }

            base.OnKeyPress(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Up) { _buf = ""; SetSeg(_seg, GetSeg(_seg) + 1); e.Handled = true; }
            else if (e.KeyCode == Keys.Down) { _buf = ""; SetSeg(_seg, GetSeg(_seg) - 1); e.Handled = true; }
            else if (e.KeyCode == Keys.Left) { _buf = ""; if (_seg > 0) _seg--; Invalidate(); e.Handled = true; }
            else if (e.KeyCode == Keys.Right) { _buf = ""; if (_seg < 5) _seg++; Invalidate(); e.Handled = true; }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            SetSeg(_seg, GetSeg(_seg) + (e.Delta > 0 ? 1 : -1));
        }

        private string Digits(int i)
        {
            switch (i)
            {
                case 0: return _y.ToString("0000");
                case 1: return _mo.ToString("00");
                case 2: return _d.ToString("00");
                case 3: return _h.ToString("00");
                case 4: return _mi.ToString("00");
                default: return _s.ToString("00");
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            Draw.Clear(g, ClientRectangle, BackColor);

            Draw.Box(g, new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f), 9f,
                T.Soft, _focus ? T.Primary : T.Line);

            int x = 14;
            int boxH = 30;
            int boxY = (Height - boxH) / 2;

            for (int i = 0; i < 6; i++)
            {
                string dg = Digits(i);
                int dw = Draw.Measure(dg, F.Digit);
                var box = new Rectangle(x, boxY, dw + 12, boxH);

                if (_focus && _seg == i)
                    Draw.Box(g, new RectangleF(box.X + 0.5f, box.Y + 0.5f, box.Width - 1f, box.Height - 1f), 6f, T.AccentTint);

                Draw.Txt(g, dg, F.Digit, T.Text, box,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                _rects[i] = box;

                x = box.Right + (L.En ? 2 : 5);
                string u = L.DtUnit(i);
                if (u.Length > 0)
                {
                    int uw = Draw.Measure(u, F.Small);
                    Draw.Txt(g, u, F.Small, T.TextMute, new Rectangle(x, 0, uw + 4, Height),
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                    x += uw + (L.En ? 2 : (i == 2 ? 14 : 6));
                }
            }
        }
    }

    // ================= 主窗体 =================
    internal sealed class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    internal sealed class MainForm : Form
    {
        // ===== 破坏页控件 =====
        private TextBox _breakPathBox;
        private UiButton _breakBrowseBtn;
        private UiButton _breakOutBrowseBtn;
        private TextBox _breakOutPathBox;
        private UiButton _breakRunBtn;
        private UiProgress _breakProgress;
        private Label _breakStatusLabel;

        // ===== 时间页控件 =====
        private TextBox _timePathBox;
        private UiButton _timeBrowseBtn;
        private Label _curCreationLabel;
        private Label _curWriteLabel;
        private Label _curAccessLabel;
        private DateTimeBox _targetTimePicker;
        private UiSwitch _modCreationCheck;
        private UiSwitch _modWriteCheck;
        private UiSwitch _modAccessCheck;
        private UiButton _copyCurrentBtn;
        private UiButton _applyTimeBtn;
        private Label _timeStatusLabel;

        private SegmentTabs _tabs;
        private Panel _pageBreak;
        private Panel _pageTime;
        private UiButton _langBtn;

        private string _breakFile = "";
        private string _timeFile = "";
        private string _outDir = "";
        private int _tabIndex = 0;

        private const int Pad = 24;              // 卡片内边距
        private static readonly int CardW = 704; // 卡片宽度

        public MainForm()
        {
            Cfg.Load();

            Text = L.Title;
            ClientSize = new Size(760, 740);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = T.Window;
            Font = F.Body;
            AllowDrop = true;
            KeyPreview = true;
            DoubleBuffered = true;

            try
            {
                var ico = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                if (ico != null) Icon = ico;
            }
            catch { }

            BuildAll();
        }

        // ---------- 整体构建 / 语言切换后重建 ----------
        private void BuildAll()
        {
            SuspendLayout();

            while (Controls.Count > 0)
            {
                Control c = Controls[0];
                Controls.RemoveAt(0);
                try { c.Dispose(); } catch { }
            }

            Text = L.Title;

            BuildHeader();
            BuildTabs();
            BuildBreakPage();
            BuildTimePage();

            _tabs.SelectedIndex = (_tabIndex == 1) ? 1 : 0;
            _tabs.SelectedIndexChanged += delegate { _tabIndex = _tabs.SelectedIndex; ApplyTab(_tabIndex); };
            ApplyTab(_tabs.SelectedIndex);

            RestoreState();

            ResumeLayout(true);
            Invalidate(true);
        }

        // 语言切换后按已选文件恢复界面状态
        private void RestoreState()
        {
            if (!string.IsNullOrEmpty(_outDir))
                _breakOutPathBox.Text = _outDir;

            if (!string.IsNullOrEmpty(_breakFile))
            {
                _breakPathBox.Text = _breakFile;
                _breakRunBtn.Enabled = true;
                _breakStatusLabel.Text = L.Selected(Path.GetFileName(_breakFile)) + L.ReadyTail;
            }

            if (!string.IsNullOrEmpty(_timeFile))
            {
                _timePathBox.Text = _timeFile;
                _applyTimeBtn.Enabled = true;
                _copyCurrentBtn.Enabled = true;
                RefreshCurrentTimes();
                _timeStatusLabel.Text = L.Selected(Path.GetFileName(_timeFile));
            }
        }

        private void Lang_Click(object sender, EventArgs e)
        {
            L.En = !L.En;
            Cfg.Save();
            BuildAll();
        }

        // ---------- 头部 ----------
        private void BuildHeader()
        {
            var header = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(ClientSize.Width, 100),
                BackColor = T.Card
            };
            header.Paint += delegate (object s, PaintEventArgs e)
            {
                using (var pen = new Pen(T.Line))
                    e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
            };

            try
            {
                var ico = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                if (ico != null)
                {
                    var logo = new PictureBox
                    {
                        Image = new Icon(ico, 40, 40).ToBitmap(),
                        Location = new Point(26, 28),
                        Size = new Size(40, 40),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        BackColor = Color.Transparent
                    };
                    header.Controls.Add(logo);
                }
            }
            catch { }

            var title = new Label
            {
                Text = L.Title,
                Font = F.H1,
                ForeColor = T.Text,
                AutoSize = true,
                Location = new Point(82, 24),
                BackColor = Color.Transparent
            };

            var sub = new Label
            {
                Text = L.Sub,
                Font = F.Small,
                ForeColor = T.TextMute,
                AutoSize = true,
                Location = new Point(84, 58),
                BackColor = Color.Transparent
            };

            var ver = new Label
            {
                Text = "V1.1",
                Font = F.Tiny,
                ForeColor = T.TextMute,
                AutoSize = false,
                Size = new Size(46, 16),
                Location = new Point(ClientSize.Width - 24 - 60 - 52, 63),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            _langBtn = new UiButton
            {
                Text = L.LangBtn,
                Kind = BtnKind.Subtle,
                Font = F.Tiny,
                Location = new Point(ClientSize.Width - 24 - 60, 57),
                Size = new Size(60, 26)
            };
            _langBtn.Click += Lang_Click;

            header.Controls.AddRange(new Control[] { title, sub, ver, _langBtn });
            Controls.Add(header);
        }

        // ---------- 分段切换 ----------
        private void BuildTabs()
        {
            _tabs = new SegmentTabs
            {
                Location = new Point(28, 116),
                Size = new Size(CardW, 46),
                Items = new[] { L.TabBreak, L.TabTime }
            };
            Controls.Add(_tabs);
        }

        private void ApplyTab(int index)
        {
            _pageBreak.Visible = index == 0;
            _pageTime.Visible = index == 1;
        }

        // ---------- 通用：选择文件卡片 ----------
        private TextBox BuildPickCard(Panel host, string hintText, bool withOutput,
            out UiButton browseBtn, out UiButton outBrowseBtn, out TextBox outBox)
        {
            int cardH = withOutput ? 194 : 150;

            var card = new UiCard
            {
                Location = new Point(0, 0),
                Size = new Size(CardW, cardH)
            };

            var title = new Label
            {
                Text = L.PickTitle,
                Font = F.H2,
                ForeColor = T.Text,
                AutoSize = true,
                Location = new Point(Pad, 18),
                BackColor = T.Card
            };

            var hint = new Label
            {
                Text = hintText,
                Font = F.Small,
                ForeColor = T.TextMute,
                AutoSize = true,
                Location = new Point(Pad, 44),
                BackColor = T.Card
            };

            int row1Y = withOutput ? 66 : 74;

            var field = new UiField
            {
                Location = new Point(Pad, row1Y),
                Size = new Size(508, 42),
                Placeholder = L.Placeholder
            };

            var browse = new UiButton
            {
                Text = L.Browse,
                Kind = BtnKind.Ghost,
                Location = new Point(Pad + 520, row1Y),
                Size = new Size(136, 42)
            };

            card.Controls.AddRange(new Control[] { title, hint, field, browse });

            outBrowseBtn = null;
            outBox = null;

            if (withOutput)
            {
                // 标签宽度按当前语言实际文字宽度计算，避免英文被截断
                int lw = Math.Max(92, Draw.Measure(L.OutLabel, F.Small) + 12);

                var outLabel = new Label
                {
                    Text = L.OutLabel,
                    Font = F.Small,
                    ForeColor = T.TextSub,
                    AutoSize = false,
                    Location = new Point(Pad, 114),
                    Size = new Size(lw, 42),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = T.Card
                };

                var outField = new UiField
                {
                    Location = new Point(Pad + lw, 114),
                    Size = new Size(508 - lw, 42),
                    Placeholder = L.OutPlaceholder
                };

                var outBtn = new UiButton
                {
                    Text = L.OutBtn,
                    Kind = BtnKind.Ghost,
                    Location = new Point(Pad + 520, 114),
                    Size = new Size(136, 42)
                };

                var bottomHint = new Label
                {
                    Text = L.OutHint,
                    Font = F.Tiny,
                    ForeColor = T.TextMute,
                    AutoSize = true,
                    Location = new Point(Pad + 2, 164),
                    BackColor = T.Card
                };

                card.Controls.AddRange(new Control[] { outLabel, outField, outBtn, bottomHint });

                outBrowseBtn = outBtn;
                outBox = outField.Box;
            }
            else
            {
                var dropHint = new Label
                {
                    Text = L.DropHint,
                    Font = F.Tiny,
                    ForeColor = T.TextMute,
                    AutoSize = true,
                    Location = new Point(Pad + 2, 126),
                    BackColor = T.Card
                };
                card.Controls.Add(dropHint);
            }

            host.Controls.Add(card);

            browseBtn = browse;
            return field.Box;
        }

        private static Label MakeValueLabel(string text, int x, int y, int w)
        {
            var lb = new Label
            {
                Text = text,
                Font = F.MonoData,
                ForeColor = T.Text,
                AutoSize = false,
                Location = new Point(x, y),
                Size = new Size(w, 22),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = T.Card
            };
            return lb;
        }

        private static Label MakeKeyLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = F.Small,
                ForeColor = T.TextSub,
                AutoSize = true,
                Location = new Point(x, y + 3),
                BackColor = T.Card
            };
        }

        // ---------- 文件破坏页 ----------
        private void BuildBreakPage()
        {
            _pageBreak = new Panel
            {
                Location = new Point(28, 176),
                Size = new Size(CardW, 548),
                BackColor = T.Window
            };

            _breakPathBox = BuildPickCard(_pageBreak,
                L.HintBreak,
                true,
                out _breakBrowseBtn, out _breakOutBrowseBtn, out _breakOutPathBox);
            _breakBrowseBtn.Kind = BtnKind.Ghost;
            _breakBrowseBtn.Click += BreakBrowse_Click;
            _breakOutBrowseBtn.Click += BreakOutBrowse_Click;

            var card = new UiCard { Location = new Point(0, 210), Size = new Size(CardW, 210) };

            var title = new Label
            {
                Text = L.ProgressTitle,
                Font = F.H2,
                ForeColor = T.Text,
                AutoSize = true,
                Location = new Point(Pad, 18),
                BackColor = T.Card
            };

            _breakProgress = new UiProgress
            {
                Location = new Point(Pad, 46),
                Size = new Size(CardW - Pad * 2, 24),
                Value = 0
            };

            _breakStatusLabel = new Label
            {
                Text = L.Ready,
                Font = F.Body,
                ForeColor = T.TextSub,
                AutoSize = false,
                Location = new Point(Pad, 78),
                Size = new Size(CardW - Pad * 2, 22),
                BackColor = T.Card
            };

            _breakRunBtn = new UiButton
            {
                Text = L.RunBtn,
                Kind = BtnKind.Danger,
                Enabled = false,
                Location = new Point(CardW - Pad - 168, 148),
                Size = new Size(168, 44)
            };
            _breakRunBtn.Click += BreakRun_Click;

            var note = new Label
            {
                Text = L.OutNote,
                Font = F.Tiny,
                ForeColor = T.TextMute,
                AutoSize = false,
                Location = new Point(Pad, 108),
                Size = new Size(CardW - Pad * 2, 18),
                BackColor = T.Card
            };

            var tip = new Label
            {
                Text = L.LocalTip,
                Font = F.Tiny,
                ForeColor = T.TextMute,
                AutoSize = true,
                Location = new Point(Pad, 162),
                BackColor = T.Card
            };

            card.Controls.AddRange(new Control[] { title, _breakProgress, _breakStatusLabel, note, tip, _breakRunBtn });
            _pageBreak.Controls.Add(card);

            // --- 使用步骤 ---
            var helpCard = new UiCard { Location = new Point(0, 436), Size = new Size(CardW, 96) };

            var helpTitle = new Label
            {
                Text = L.HelpTitle,
                Font = F.H2,
                ForeColor = T.Text,
                AutoSize = true,
                Location = new Point(Pad, 14),
                BackColor = T.Card
            };

            var helpText = new Label
            {
                Text = L.HelpText,
                Font = F.Small,
                ForeColor = T.TextSub,
                AutoSize = false,
                Location = new Point(Pad, 42),
                Size = new Size(CardW - Pad * 2, 20),
                BackColor = T.Card
            };

            var helpNote = new Label
            {
                Text = L.HelpNote,
                Font = F.Tiny,
                ForeColor = T.TextMute,
                AutoSize = false,
                Location = new Point(Pad, 66),
                Size = new Size(CardW - Pad * 2 - 96, 18),
                BackColor = T.Card
            };

            var sign = new Label
            {
                Text = "By Nanyin",
                Font = F.Tiny,
                ForeColor = T.TextMute,
                AutoSize = false,
                Location = new Point(CardW - Pad - 90, 66),
                Size = new Size(90, 18),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = T.Card
            };

            helpCard.Controls.AddRange(new Control[] { helpTitle, helpText, helpNote, sign });
            _pageBreak.Controls.Add(helpCard);

            _pageBreak.AllowDrop = true;
            _pageBreak.DragEnter += BreakDragEnter;
            _pageBreak.DragDrop += BreakDragDrop;

            Controls.Add(_pageBreak);
        }

        private void BreakDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop)) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void BreakDragDrop(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files == null || files.Length == 0) return;
            if (!File.Exists(files[0])) return;
            _breakFile = files[0];
            _breakPathBox.Text = _breakFile;
            _breakRunBtn.Enabled = true;
            _breakStatusLabel.Text = L.Selected(Path.GetFileName(_breakFile)) + L.ReadyTail;
            _breakProgress.Value = 0;
        }

        private void BreakOutBrowse_Click(object sender, EventArgs e)
        {
            string start = string.IsNullOrEmpty(_outDir)
                ? (Path.GetDirectoryName(_breakFile) ?? "")
                : _outDir;

            using (var dlg = new FolderBrowserDialog
            {
                Description = L.FolderDesc,
                ShowNewFolderButton = true,
                SelectedPath = start
            })
            {
                if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(dlg.SelectedPath))
                {
                    _outDir = dlg.SelectedPath;
                    _breakOutPathBox.Text = _outDir;
                }
            }
        }

        // ---------- 修改时间页 ----------
        private void BuildTimePage()
        {
            _pageTime = new Panel
            {
                Location = new Point(28, 176),
                Size = new Size(CardW, 548),
                BackColor = T.Window
            };

            UiButton dummyBrowse;
            TextBox dummyBox;
            _timePathBox = BuildPickCard(_pageTime,
                L.HintTime,
                false,
                out _timeBrowseBtn, out dummyBrowse, out dummyBox);
            _timeBrowseBtn.Kind = BtnKind.Ghost;
            _timeBrowseBtn.Click += TimeBrowse_Click;

            // --- 当前时间 ---
            var infoCard = new UiCard { Location = new Point(0, 166), Size = new Size(CardW, 140) };

            var infoTitle = new Label
            {
                Text = L.CurTitle,
                Font = F.H2,
                ForeColor = T.Text,
                AutoSize = true,
                Location = new Point(Pad, 18),
                BackColor = T.Card
            };

            int valueW = 300;
            int valueX = CardW - Pad - valueW;

            _curCreationLabel = MakeValueLabel("—", valueX, 50, valueW);
            _curWriteLabel = MakeValueLabel("—", valueX, 78, valueW);
            _curAccessLabel = MakeValueLabel("—", valueX, 106, valueW);

            infoCard.Controls.AddRange(new Control[]
            {
                infoTitle,
                MakeKeyLabel(L.KeyCreation, Pad, 50), _curCreationLabel,
                MakeKeyLabel(L.KeyWrite, Pad, 78), _curWriteLabel,
                MakeKeyLabel(L.KeyAccess, Pad, 106), _curAccessLabel
            });
            _pageTime.Controls.Add(infoCard);

            // --- 目标时间 ---
            var setCard = new UiCard { Location = new Point(0, 314), Size = new Size(CardW, 204) };

            var setTitle = new Label
            {
                Text = L.TargetTitle,
                Font = F.H2,
                ForeColor = T.Text,
                AutoSize = true,
                Location = new Point(Pad, 18),
                BackColor = T.Card
            };

            _targetTimePicker = new DateTimeBox
            {
                Location = new Point(Pad, 44),
                Size = new Size(470, 44),
                Value = DateTime.Now
            };

            _copyCurrentBtn = new UiButton
            {
                Text = L.CopyCurrent,
                Kind = BtnKind.Ghost,
                Enabled = false,
                Location = new Point(Pad + 470 + 12, 44),
                Size = new Size(150, 44),
                Font = F.Small
            };
            _copyCurrentBtn.Click += CopyCurrent_Click;

            // 三个开关按最长文字实测宽度布局
            int swW = Math.Max(Draw.Measure(L.KeyCreation, F.Body),
                      Math.Max(Draw.Measure(L.KeyWrite, F.Body), Draw.Measure(L.KeyAccess, F.Body))) + 56;
            int swGap = 30;

            _modCreationCheck = new UiSwitch
            {
                Label = L.KeyCreation,
                Location = new Point(Pad, 104),
                Size = new Size(swW, 26)
            };
            _modWriteCheck = new UiSwitch
            {
                Label = L.KeyWrite,
                Location = new Point(Pad + swW + swGap, 104),
                Size = new Size(swW, 26),
                Checked = true
            };
            _modAccessCheck = new UiSwitch
            {
                Label = L.KeyAccess,
                Location = new Point(Pad + (swW + swGap) * 2, 104),
                Size = new Size(swW, 26)
            };

            _applyTimeBtn = new UiButton
            {
                Text = L.ApplyBtn,
                Kind = BtnKind.Primary,
                Enabled = false,
                Location = new Point(CardW - Pad - 168, 138),
                Size = new Size(168, 44)
            };
            _applyTimeBtn.Click += ApplyTime_Click;

            _timeStatusLabel = new Label
            {
                Text = "",
                Font = F.Small,
                ForeColor = T.TextMute,
                AutoSize = false,
                Location = new Point(Pad, 150),
                Size = new Size(380, 22),
                BackColor = T.Card
            };
            setCard.Controls.AddRange(new Control[]
            {
                setTitle, _targetTimePicker, _copyCurrentBtn,
                _modCreationCheck, _modWriteCheck, _modAccessCheck, _applyTimeBtn, _timeStatusLabel
            });
            _pageTime.Controls.Add(setCard);

            _pageTime.AllowDrop = true;
            _pageTime.DragEnter += BreakDragEnter;
            _pageTime.DragDrop += TimeDragDrop;

            Controls.Add(_pageTime);
        }

        private void TimeDragDrop(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files == null || files.Length == 0) return;
            if (!File.Exists(files[0])) return;
            _timeFile = files[0];
            _timePathBox.Text = _timeFile;
            _applyTimeBtn.Enabled = true;
            _copyCurrentBtn.Enabled = true;
            RefreshCurrentTimes();
            _timeStatusLabel.Text = L.Selected(Path.GetFileName(_timeFile));
        }

                // ================= 逻辑部分（未改动） =================


        private void BreakBrowse_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog
            {
                Title = L.OpenBreakTitle,
                Filter = L.AllFiles,
                CheckFileExists = true
            })
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    _breakFile = dlg.FileName;
                    _breakPathBox.Text = _breakFile;
                    _breakRunBtn.Enabled = true;
                    _breakStatusLabel.Text = L.Selected(Path.GetFileName(_breakFile)) + L.ReadyTail;
                    _breakProgress.Value = 0;
                }
            }
        }

        private void BreakRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_breakFile) || !File.Exists(_breakFile))
            {
                MessageBox.Show(this, L.NeedFile, L.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dir = (!string.IsNullOrEmpty(_outDir) && Directory.Exists(_outDir))
                ? _outDir
                : (Path.GetDirectoryName(_breakFile) ?? "");
            string name = Path.GetFileNameWithoutExtension(_breakFile);
            string ext = Path.GetExtension(_breakFile);
            string outName = name + "_broken" + ext;
            string outPath = Path.Combine(dir, outName);

            int idx = 1;
            while (File.Exists(outPath))
            {
                outName = name + "_broken" + idx + ext;
                outPath = Path.Combine(dir, outName);
                idx++;
            }

            _breakRunBtn.Enabled = false;
            _breakBrowseBtn.Enabled = false;
            _breakProgress.Value = 0;
            _breakStatusLabel.Text = L.Working;

            long fileSize;
            try
            {
                fileSize = new FileInfo(_breakFile).Length;
            }
            catch (Exception ex)
            {
                _breakStatusLabel.Text = L.ReadFail + ex.Message;
                _breakRunBtn.Enabled = true;
                _breakBrowseBtn.Enabled = true;
                return;
            }

            string input = _breakFile;
            string output = outPath;

            var thread = new Thread(() =>
            {
                bool ok;
                string msg;
                try
                {
                    CorruptFile(input, output, fileSize);
                    ok = true;
                    msg = L.DonePrefix + Path.GetFileName(output);
                }
                catch (Exception ex)
                {
                    ok = false;
                    msg = L.FailPrefix + ex.Message;
                }

                BeginInvoke((Action)(() =>
                {
                    _breakRunBtn.Enabled = true;
                    _breakBrowseBtn.Enabled = true;
                    _breakStatusLabel.Text = msg;
                    _breakProgress.Value = ok ? 100 : 0;
                    if (ok)
                    {
                        MessageBox.Show(this,
                            L.BreakDoneBody(output, fileSize, new FileInfo(output).Length, ext),
                            L.MsgDoneTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }));
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void CorruptFile(string input, string output, long fileSize)
        {
            const int bufferSize = 64 * 1024;
            byte[] buffer = new byte[bufferSize];

            using (var rng = new RNGCryptoServiceProvider())
            using (var fs = new FileStream(output, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize))
            {
                long remaining = fileSize;
                long written = 0;

                while (remaining > 0)
                {
                    int toWrite = (int)Math.Min(buffer.Length, remaining);
                    rng.GetBytes(buffer, 0, toWrite);
                    fs.Write(buffer, 0, toWrite);
                    remaining -= toWrite;
                    written += toWrite;

                    int pct = (int)((written * 100L) / Math.Max(1L, fileSize));
                    if (pct > 100) pct = 100;
                    BeginInvoke((Action)(() =>
                    {
                        if (_breakProgress.Value < pct) _breakProgress.Value = pct;
                    }));
                }
            }
        }

        private void TimeBrowse_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog
            {
                Title = L.OpenTimeTitle,
                Filter = L.AllFiles,
                CheckFileExists = true
            })
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    _timeFile = dlg.FileName;
                    _timePathBox.Text = _timeFile;
                    _applyTimeBtn.Enabled = true;
                    _copyCurrentBtn.Enabled = true;
                    RefreshCurrentTimes();
                    _timeStatusLabel.Text = "已选择 " + Path.GetFileName(_timeFile);
                }
            }
        }

        private void RefreshCurrentTimes()
        {
            if (string.IsNullOrEmpty(_timeFile) || !File.Exists(_timeFile)) return;
            try
            {
                var fi = new FileInfo(_timeFile);
                _curCreationLabel.Text = fi.CreationTime.ToString("yyyy-MM-dd HH:mm:ss");
                _curWriteLabel.Text = fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                _curAccessLabel.Text = fi.LastAccessTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                _timeStatusLabel.Text = L.ReadTimeFail + ex.Message;
            }
        }

        private void CopyCurrent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_timeFile) || !File.Exists(_timeFile)) return;
            try
            {
                var fi = new FileInfo(_timeFile);
                _targetTimePicker.Value = fi.LastWriteTime;
                _timeStatusLabel.Text = L.FillCurrent;
            }
            catch { }
        }

        private void ApplyTime_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_timeFile) || !File.Exists(_timeFile))
            {
                MessageBox.Show(this, L.NeedFile, L.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_modCreationCheck.Checked && !_modWriteCheck.Checked && !_modAccessCheck.Checked)
            {
                MessageBox.Show(this, L.NeedTick, L.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime target = _targetTimePicker.Value;
            try
            {
                if (_modCreationCheck.Checked) File.SetCreationTime(_timeFile, target);
                if (_modWriteCheck.Checked) File.SetLastWriteTime(_timeFile, target);
                if (_modAccessCheck.Checked) File.SetLastAccessTime(_timeFile, target);

                RefreshCurrentTimes();

                var items = new System.Collections.Generic.List<string>();
                if (_modCreationCheck.Checked) items.Add(L.KeyCreation);
                if (_modWriteCheck.Checked) items.Add(L.KeyWrite);
                if (_modAccessCheck.Checked) items.Add(L.KeyAccess);

                string joined = string.Join(" / ", items);

                _timeStatusLabel.Text = L.ChangedTo + joined + " → " + target.ToString("yyyy-MM-dd HH:mm:ss");
                MessageBox.Show(this,
                    L.TimeDoneBody(_timeFile, joined, target.ToString("yyyy-MM-dd HH:mm:ss")),
                    L.MsgTimeDone, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _timeStatusLabel.Text = L.ModifyFail + ex.Message;
                MessageBox.Show(this, L.TimeErrBody(ex.Message), L.MsgErr, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


