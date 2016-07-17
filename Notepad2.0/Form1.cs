using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Notepad2._0.Properties;

namespace Notepad._0
{
    public partial class Window : Form
    {
        public int PageCounter;

        public IDictionary<string, string> SaveDictionary = new Dictionary<string, string>();

        public Window()
        {
            InitializeComponent();

            SaveFileDialog.Filter = Resources.SaveFileDialogFilter;
            SaveFileDialog.Title = Resources.SaveFileDialogTitle;
        }

        // When the new menu strip item is pressed.
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Creates the tab with RichTextBox.
            var tabPage = new TabPage("new" + PageCounter)
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom
            };

            var richTextBox = new RichTextBox
            {
                Width = tabPage.Width,
                Height = tabPage.Height,
                Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom
            };

            tabPage.Controls.Add(richTextBox);

            TabControl.TabPages.Insert(TabControl.TabCount, tabPage);
            PageCounter = TabControl.TabPages.Count;
        }

        // When the save menu strip item is pressed.
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var textBox = (RichTextBox)TabControl.SelectedTab.Controls[0];

            if (!SaveDictionary.ContainsKey(TabControl.SelectedTab.Text))
            {
                if (SaveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    SaveFileDialog.Dispose();
                    return;
                }

                // Changes the tabs title to that filname without extension and then saves the file.
                TabControl.SelectedTab.Text = Path.GetFileNameWithoutExtension(SaveFileDialog.FileName);
                textBox.SaveFile(SaveFileDialog.FileName, Path.GetExtension(SaveFileDialog.FileName) == ".rtf"
                    ? RichTextBoxStreamType.RichText
                    : RichTextBoxStreamType.PlainText);

                if (!SaveDictionary.ContainsKey(TabControl.SelectedTab.Text))
                    SaveDictionary.Add(TabControl.SelectedTab.Text, SaveFileDialog.FileName);
                else
                    SaveDictionary[TabControl.SelectedTab.Text] = SaveFileDialog.FileName;

                SaveFileDialog.Dispose();
            }
            else
            {
                textBox.SaveFile(SaveDictionary[TabControl.SelectedTab.Text],
                    Path.GetExtension(SaveDictionary[TabControl.SelectedTab.Text]) == ".rtf"
                    ? RichTextBoxStreamType.RichText
                    : RichTextBoxStreamType.PlainText);

                SaveDictionary[TabControl.SelectedTab.Text] = SaveFileDialog.FileName;
            }
        }

        // When the save menu strip item is pressed.
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var textBox = (RichTextBox)TabControl.SelectedTab.Controls[0];

            if (SaveFileDialog.ShowDialog() != DialogResult.OK)
            {
                SaveFileDialog.Dispose();
                return;
            }

            // Changes the tabs title to that filname without extension and then saves the file.
            TabControl.SelectedTab.Text = Path.GetFileNameWithoutExtension(SaveFileDialog.FileName);
            textBox.SaveFile(SaveFileDialog.FileName, Path.GetExtension(SaveFileDialog.FileName) == ".rtf"
                ? RichTextBoxStreamType.RichText
                : RichTextBoxStreamType.PlainText);

            if (!SaveDictionary.ContainsKey(Path.GetFileNameWithoutExtension(SaveFileDialog.FileName)))
                SaveDictionary.Add(TabControl.SelectedTab.Text, SaveFileDialog.FileName);
            else
                SaveDictionary[TabControl.SelectedTab.Text] = SaveFileDialog.FileName;

            SaveFileDialog.Dispose();
        }

        // When the open menu strip item is pressed.
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() != DialogResult.OK)
            {
                OpenFileDialog.Dispose();
                return;
            }

            CreateTab();

            if (!SaveDictionary.ContainsKey(Path.GetFileNameWithoutExtension(OpenFileDialog.FileName)))
                SaveDictionary.Add(TabControl.SelectedTab.Text, OpenFileDialog.FileName);
            else
                SaveDictionary[TabControl.SelectedTab.Text] = OpenFileDialog.FileName;

            if (TabControl.SelectedTab != null)
            {
                var textBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
                textBox.LoadFile(OpenFileDialog.FileName);
            }

            OpenFileDialog.Dispose();
        }
        
        // This is used for when the program opens a file and needs a tab.
        private void CreateTab()
        {
            // Creates the tab with RichTextBox for when the program opens a file.
            var tabPage = new TabPage("new" + PageCounter)
            {
                Text = Path.GetFileNameWithoutExtension(OpenFileDialog.FileName),
                Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom
            };

            var richTextBox = new RichTextBox
            {
                Width = tabPage.Width,
                Height = tabPage.Height,
                Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom
            };

            tabPage.Controls.Add(richTextBox);

            TabControl.TabPages.Insert(TabControl.TabCount, tabPage);
            TabControl.SelectedTab = tabPage;
            PageCounter = TabControl.TabPages.Count;
        }

        // When the close menu strip item is pressed.
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            TabControl.TabPages.Remove(TabControl.SelectedTab);
        }

        // When the exit menu strip item is pressed.
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        // When the undo menu strip item is pressed.
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var richTextBox = (RichTextBox) TabControl.SelectedTab.Controls[0];
            richTextBox.Undo();
        }

        // When the redo menu strip item is pressed.
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var richTextBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            richTextBox.Redo();
        }

        // When the cut menu strip item is pressed.
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var richTextBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            richTextBox.Cut();
        }

        // When the copy menu strip item is pressed.
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var richTextBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            richTextBox.Copy();
        }

        // When the paste menu strip item is pressed.
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var richTextBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            richTextBox.Paste();
        }

        // When the delete menu strip item is pressed.
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var richTextBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            var remove = richTextBox.SelectedText.Remove(0, richTextBox.SelectedText.Length);
            richTextBox.SelectedText = remove;
        }

        // When the select menu strip item is pressed.
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var richTextBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            richTextBox.SelectAll();
        }

        // When the word wrap menu strip item is pressed.
        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var richTextBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            richTextBox.WordWrap = !richTextBox.WordWrap;
        }

        // When the font menu strip item is pressed.
        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var richTextBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            if (FontDialog.ShowDialog() != DialogResult.OK)
            {
                FontDialog.Dispose();
                return;
            }

            if (richTextBox.SelectionLength > 0)
                richTextBox.SelectionFont = FontDialog.Font;
            else
                richTextBox.Font = FontDialog.Font;

            FontDialog.Dispose();
        }

        // When the font color menu strip item is pressed.
        private void fontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var richTextBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            if (FontColorDialog.ShowDialog() != DialogResult.OK)
            {
                FontColorDialog.Dispose();
                return;
            }

            if (richTextBox.SelectionLength > 0)
                richTextBox.SelectionColor = FontColorDialog.Color;
            else
                richTextBox.ForeColor = FontColorDialog.Color;

            FontColorDialog.Dispose();
        }

        private void fontBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var richTextBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            if (FontColorDialog.ShowDialog() != DialogResult.OK)
            {
                FontColorDialog.Dispose();
                return;
            }

            richTextBox.SelectionBackColor = FontDialog.Color;

            FontColorDialog.Dispose();
        }

        private void selectionBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var richTextBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            if (FontColorDialog.ShowDialog() != DialogResult.OK)
            {
                FontColorDialog.Dispose();
                return;
            }

            richTextBox.BackColor = FontColorDialog.Color;

            FontColorDialog.Dispose();
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var richTextBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            richTextBox.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void centerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var richTextBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            richTextBox.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == null) return;
            var richTextBox = (RichTextBox)TabControl.SelectedTab.Controls[0];
            richTextBox.SelectionAlignment = HorizontalAlignment.Right;
        }
    }
}