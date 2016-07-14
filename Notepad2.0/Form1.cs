using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Notepad2._0
{
    public partial class Window : Form
    {
        public int PageCounter;
        public IDictionary<string, string> TextDataDictionary = new Dictionary<string, string>();

        public Window()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tabsPage = new TabPage("new" + PageCounter)
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom
            };

            var richTextBox = new RichTextBox
            {
                Width = tabsPage.Width,
                Height = tabsPage.Height,
                Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom
            };

            tabsPage.Controls.Add(richTextBox);

            TabControl.TabPages.Insert(TabControl.TabCount, tabsPage);
            PageCounter++;
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveFileDialog.ShowDialog() != DialogResult.OK)
            {
                SaveFileDialog.Dispose();
                return;
            }

            var textBox = (RichTextBox) TabControl.SelectedTab.Controls[0];

            TabControl.SelectedTab.Text = Path.GetFileNameWithoutExtension(SaveFileDialog.FileName);
            textBox.SaveFile(SaveFileDialog.FileName, Path.GetExtension(SaveFileDialog.FileName) == ".rtf"
                ? RichTextBoxStreamType.RichText
                : RichTextBoxStreamType.PlainText);

            if (!TextDataDictionary.ContainsKey(Path.GetFileNameWithoutExtension(SaveFileDialog.FileName)))
                TextDataDictionary.Add(Path.GetFileNameWithoutExtension(SaveFileDialog.FileName), textBox.Text);
            else
                TextDataDictionary[Path.GetFileNameWithoutExtension(SaveFileDialog.FileName)] = textBox.Text;

            if (TabControl.SelectedTab.Text != null && TabControl.SelectedTab.Text.EndsWith(@"*"))
            {
                var trimEnd = TabControl.SelectedTab.Text.TrimEnd('*');
                TabControl.SelectedTab.Text = trimEnd;
            }

            SaveFileDialog.Dispose();
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var textBox = (RichTextBox) TabControl.SelectedTab.Controls[0];
            textBox.TextChanged += TextBoxOnTextChanged;
        }

        private void TextBoxOnTextChanged(object sender, EventArgs eventArgs)
        {
            var textBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            if (!TextDataDictionary.ContainsKey(TabControl.SelectedTab.Text)) return;

            if (TextDataDictionary[TabControl.SelectedTab.Text] == textBox.Text)
            {
                if (TabControl.SelectedTab.Text == null || !TabControl.SelectedTab.Text.EndsWith(@"*")) return;

                var trimEnd = TabControl.SelectedTab.Text.TrimEnd('*');
                TabControl.SelectedTab.Text = trimEnd;
            }
            else
                TabControl.SelectedTab.Text = TabControl.SelectedTab.Text + @"*";
        }

        private void TabControl_ControlAdded(object sender, ControlEventArgs e)
        {
            var textBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            textBox.TextChanged += TextBoxOnTextChanged;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() != DialogResult.OK)
            {
                OpenFileDialog.Dispose();
                return;
            }

            TabControl.SelectedTab.Text = Path.GetFileNameWithoutExtension(OpenFileDialog.FileName);

            var stream = new StreamReader(new FileStream(OpenFileDialog.FileName, FileMode.Open));
            var textBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            textBox.Text = stream.ReadToEnd();

            if (!TextDataDictionary.ContainsKey(Path.GetFileNameWithoutExtension(OpenFileDialog.FileName)))
                TextDataDictionary.Add(Path.GetFileNameWithoutExtension(OpenFileDialog.FileName), textBox.Text);
            else
                TextDataDictionary[Path.GetFileNameWithoutExtension(OpenFileDialog.FileName)] = textBox.Text;

            OpenFileDialog.Dispose();
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabControl.TabPages.Remove(TabControl.SelectedTab);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}