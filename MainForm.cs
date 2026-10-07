using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace STALZONERegionSwitcher
{
    public partial class MainForm : Form
    {
        private string _gameFolder;

        public MainForm()
        {
            InitializeComponent();
            ApplyDarkTheme();
            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Generate simple "S" icon if no external icon
            try
            {
                this.Icon = CreateLetterIcon('S');
            }
            catch { }

            string path = SteamHelper.FindGameFolder();
            if (!string.IsNullOrEmpty(path))
            {
                _gameFolder = path;
                UpdateFolderLabel(path, true);
            }
            else
            {
                UpdateFolderLabel("Папка не найдена", false);
            }
            UpdateButtons();
        }

        private void ApplyDarkTheme()
        {
            this.BackColor = Color.FromArgb(15, 15, 26);
            this.ForeColor = Color.FromArgb(224, 216, 255);

            panelFolder.BackColor = Color.FromArgb(26, 21, 41);
            panelFolder.BorderStyle = BorderStyle.FixedSingle;

            lblTitle.ForeColor = Color.FromArgb(176, 168, 255);
            lblRegionCaption.ForeColor = Color.FromArgb(224, 216, 255);
            lblRegion.ForeColor = Color.FromArgb(224, 216, 255);
            lblFolderPath.ForeColor = Color.FromArgb(224, 216, 255);
            lblSignature.ForeColor = Color.FromArgb(108, 92, 231);

            StyleButton(btnChangeFolder);
            StyleButton(btnChangeRegion);
            StyleButton(btnLaunch);
        }

        private void StyleButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.FromArgb(106, 90, 205);
            btn.FlatAppearance.BorderSize = 1;
            btn.BackColor = Color.FromArgb(72, 61, 139);
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(90, 79, 168);
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(72, 61, 139);
            };
        }

        private Icon CreateLetterIcon(char letter)
        {
            int size = 64;
            using (var bmp = new Bitmap(size, size))
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.FromArgb(72, 61, 139));

                using (var brush = new SolidBrush(Color.White))
                using (var font = new Font("Segoe UI", 36f, FontStyle.Bold, GraphicsUnit.Pixel))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(letter.ToString(), font, brush, new RectangleF(0, 0, size, size), sf);
                }

                IntPtr hIcon = bmp.GetHicon();
                return Icon.FromHandle(hIcon);
            }
        }

        private void UpdateFolderLabel(string pathOrText, bool success)
        {
            if (success)
            {
                string s = pathOrText;
                if (s.Length > 70)
                {
                    string[] parts = s.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 3)
                        s = "…\\" + string.Join("\\", parts, parts.Length - 3, 3);
                    else
                        s = "…\\" + Path.GetFileName(s);
                }
                lblFolderPath.Text = s;
                toolTip.SetToolTip(lblFolderPath, pathOrText);
                lblFolderPath.ForeColor = Color.FromArgb(224, 216, 255);
            }
            else
            {
                lblFolderPath.Text = pathOrText;
                toolTip.SetToolTip(lblFolderPath, "");
                lblFolderPath.ForeColor = Color.FromArgb(255, 153, 153);
            }
        }

        private void UpdateButtons()
        {
            if (string.IsNullOrEmpty(_gameFolder))
            {
                lblRegion.Text = "";
                btnChangeRegion.Enabled = false;
                btnLaunch.Enabled = false;
                return;
            }

            string current = SteamHelper.GetCurrentRegion(_gameFolder);
            string display = current == "не задан" ? "автоматически" : current;
            lblRegion.Text = display;

            btnChangeRegion.Enabled = true;
            btnLaunch.Enabled = true;

            btnChangeRegion.Text = current == "не задан" ? "Выбрать регион" : "Сменить регион";
        }

        private void btnChangeFolder_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Выберите папку STALZONE";
                dlg.ShowNewFolderButton = false;
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                string path = dlg.SelectedPath;
                if (!SteamHelper.IsValidGameFolder(path))
                {
                    MessageBox.Show(
                        "Отсутствует файл steam_appid.txt или неверный AppID.\nУбедитесь, что выбрали корневую папку игры.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _gameFolder = Path.GetFullPath(path);
                UpdateFolderLabel(_gameFolder, true);
                UpdateButtons();
            }
        }

        private void btnChangeRegion_Click(object sender, EventArgs e)
        {
            var menu = new ContextMenuStrip();
            menu.BackColor = Color.FromArgb(40, 30, 60);
            menu.ForeColor = Color.White;
            menu.Renderer = new DarkMenuRenderer();

            var autoItem = new ToolStripMenuItem("Выбирать автоматически");
            autoItem.Click += (s, ev) => SetRegion("AUTO");
            menu.Items.Add(autoItem);

            menu.Items.Add(new ToolStripSeparator());

            var ruItem = new ToolStripMenuItem("Россия (RU)");
            ruItem.Click += (s, ev) => SetRegion("RU");
            menu.Items.Add(ruItem);

            var globalItem = new ToolStripMenuItem("EU/NA/ASIA (GLOBAL)");
            globalItem.Click += (s, ev) => SetRegion("GLOBAL");
            menu.Items.Add(globalItem);

            menu.Show(btnChangeRegion, new Point(0, btnChangeRegion.Height));
        }

        private void SetRegion(string code)
        {
            try
            {
                SteamHelper.SetRegion(_gameFolder, code);
                UpdateButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось сохранить регион\n" + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            SteamHelper.StartGame();
        }

        // Simple dark renderer for context menu
        private class DarkMenuRenderer : ToolStripProfessionalRenderer
        {
            public DarkMenuRenderer() : base(new DarkColorTable()) { }

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                if (e.Item.Selected)
                {
                    using (var b = new SolidBrush(Color.FromArgb(90, 79, 168)))
                        e.Graphics.FillRectangle(b, e.Item.ContentRectangle);
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
            }
        }

        private class DarkColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected
            {
                get { return Color.FromArgb(90, 79, 168); }
            }
            public override Color MenuItemBorder
            {
                get { return Color.FromArgb(106, 90, 205); }
            }
            public override Color MenuBorder
            {
                get { return Color.FromArgb(106, 90, 205); }
            }
            public override Color ToolStripDropDownBackground
            {
                get { return Color.FromArgb(40, 30, 60); }
            }
            public override Color ImageMarginGradientBegin
            {
                get { return Color.FromArgb(40, 30, 60); }
            }
            public override Color ImageMarginGradientMiddle
            {
                get { return Color.FromArgb(40, 30, 60); }
            }
            public override Color ImageMarginGradientEnd
            {
                get { return Color.FromArgb(40, 30, 60); }
            }
            public override Color SeparatorDark
            {
                get { return Color.FromArgb(80, 70, 120); }
            }
            public override Color SeparatorLight
            {
                get { return Color.FromArgb(80, 70, 120); }
            }
        }
    }
}
