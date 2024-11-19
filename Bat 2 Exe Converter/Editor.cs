using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace Bat_2_Exe_Converter
{
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();
        }
        public string BatchFilePath;
        public string BatchCode1;
        public string BatchCode2;
        private void Editor_Load(object sender, EventArgs e)
        {
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont.FontFamily, 11.0F);
            richTextBox1.AppendText("::This is a example batch file.\n@echo off\ntitle hello!\necho yo\npause");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Font = new Font("Fira Code Light", 11.0F);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont.FontFamily, 11.0F);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ss = new OpenFileDialog())
            {
                ss.Filter = "Batch file (*.bat)|*.bat";
                if (ss.ShowDialog() == DialogResult.OK)
                {
                    BatchFilePath = ss.FileName;
                }
                else
                {
                    BatchFilePath = null;
                }
            }
            using (StreamReader sr = new StreamReader(BatchFilePath.Replace(@"\", @"\\")))
            {
                string code = sr.ReadToEnd();
                richTextBox1.Clear();
                richTextBox1.AppendText(code);
                BatchCode1 = code;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BatchCode2 = richTextBox1.Text;
            if (!String.Equals(BatchCode2, BatchCode1))
            {
                if (BatchFilePath == null)
                {
                    MessageBox.Show("You have not selected any batch file to save the content!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (File.Exists(BatchFilePath.Replace(@"\", @"\\")))
                    {
                        File.Delete(BatchFilePath.Replace(@"\", @"\\"));
                    }
                    using (FileStream fs = File.Create(BatchFilePath.Replace(@"\", @"\\")))
                    {
                        byte[] writeArr = Encoding.UTF8.GetBytes(BatchCode2);
                        fs.Write(writeArr, 0, writeArr.Length);
                        fs.Close();
                    }
                    MessageBox.Show("Successfully saved the updated batch file!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("No updated code was found hence it was not saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Batch File|*.bat";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string FilePath = sfd.FileName;
                    using (FileStream fs = File.Create(FilePath.Replace(@"\", @"\\")))
                    {
                        byte[] writeArray = Encoding.UTF8.GetBytes(richTextBox1.Text);
                        fs.Write(writeArray, 0, writeArray.Length);
                        fs.Close();
                    }
                    MessageBox.Show("Successfully created and saved the contents of the file!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BatchFilePath = sfd.FileName;
                }
            }
        }
    }
}
