using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Drawing;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Bat_2_Exe_Converter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /*Public variables for making things work*/
        public string __code__;
        public static string iconpath;
        public string filepath;
        public bool fileselected = false;
        public static string ExeName;
        public static bool Hidden = false;
        public static bool ShowWindow = false;
        public static string ManifestFilePath;

        public class Compiler
        {
            public static void Compile_Code(string src) //Compiled the C# code into a EXE file
            {
                try
                {
                    Dictionary<string, string> providerOptions = new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } };
                    CSharpCodeProvider codeProvider = new CSharpCodeProvider(providerOptions);
                    ICodeCompiler icc = codeProvider.CreateCompiler();
                    string executablename = "Compiled_Code.exe";
                    System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
                    parameters.GenerateExecutable = true;
                    if (ExeName != null)
                    {
                        parameters.OutputAssembly = ExeName;
                    }
                    else
                    {
                        parameters.OutputAssembly = executablename;
                    }
                    parameters.ReferencedAssemblies.Add("System.dll");
                    parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                    parameters.ReferencedAssemblies.Add("System.Core.dll");
                    parameters.ReferencedAssemblies.Add("System.Runtime.InteropServices.dll");
                    parameters.ReferencedAssemblies.Add("System.Security.dll");
                    parameters.ReferencedAssemblies.Add("System.Management.dll");
                    parameters.ReferencedAssemblies.Add("System.Diagnostics.Process.dll");
                    parameters.ReferencedAssemblies.Add("System.Runtime.dll");

                    parameters.TreatWarningsAsErrors = false;
                    string[] source = new string[] { src };
                    if (ManifestFilePath != null)
                    {
                        parameters.CompilerOptions = @"/win32manifest:" + ManifestFilePath.Replace(@"\", @"\\");
                    }
                    else if (iconpath != null)
                    {
                        parameters.CompilerOptions = @"/win32icon:" + iconpath.Replace(@"\", @"\\");
                    }
                    CompilerResults results = icc.CompileAssemblyFromSourceBatch(parameters, source);
                    if (results.Errors.Count > 0)
                    {
                        foreach (CompilerError CompErr in results.Errors)
                        {
                            MessageBox.Show($"{CompErr.ErrorText}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Successfully Compiled your source code!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An Error Occured While Compiling the File\n\rError Stack:\n\r{ex.ToString()}", "Error While Compiling");
                }
            }
            public static void Extract(string nameSpace, string outDirectory, string internalFilePath, string resourceName)
            {
                Assembly assembly = Assembly.GetCallingAssembly();
                using (Stream s = assembly.GetManifestResourceStream(nameSpace + "." + (internalFilePath == "" ? "" : internalFilePath + ".") + resourceName))
                using (BinaryReader r = new BinaryReader(s))
                using (FileStream fs = new FileStream(outDirectory + "\\" + resourceName, FileMode.OpenOrCreate))
                using (BinaryWriter w = new BinaryWriter(fs))
                    w.Write(r.ReadBytes((int)s.Length));
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (fileselected == false)
            {
                MessageBox.Show("You have to select a batch file to compile the code!", "Batch File not Selected.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (StreamReader sr = new StreamReader(filepath.Replace(@"\", @"\\")))
                {
                    string line = sr.ReadToEnd();
                    var MainSource = Bat_2_Exe_Converter.Properties.Resources.Compile;
                    string replaced = Regex.Replace(line, Environment.NewLine, " && ");
                    MainSource = MainSource.Replace("%code%", replaced);
                    if (Hidden == true && ShowWindow == false)
                    {
                        MainSource = MainSource.Replace("%mode%", "Hidden");
                    }
                    else
                    {
                        MainSource = MainSource.Replace("%mode%", "Normal");
                    }
                    Compiler.Compile_Code(MainSource);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            About ab = new About();
            ab.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog x = new OpenFileDialog())
            {
                x.Filter = "ico file (*.ico)|*.ico";
                if (x.ShowDialog() == DialogResult.OK)
                {
                    iconpath = x.FileName;
                    pictureBox1.ImageLocation = x.FileName;
                    label3.Text = x.FileName;
                }
                else
                {
                    pictureBox1.Image = null;
                    iconpath = null;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog file = new OpenFileDialog())
            {
                file.Filter = "Batch file (*.bat)|*.bat";
                if (file.ShowDialog() == DialogResult.OK)
                {
                    fileselected = true;
                    button5.Enabled = false;
                    label4.Text = "Files Selected? = Yes.";
                    filepath = file.FileName;
                    button6.Enabled = true;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog sel = new OpenFileDialog())
            {
                sel.Filter = "Batch file (*.bat)|*.bat";
                if (sel.ShowDialog() == DialogResult.OK)
                {
                    button5.Enabled = false;
                    label4.Text = "Files Selected? = Yes.";
                    filepath = sel.FileName;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button6.Enabled = false;
            checkBox2.Checked = true;
            button8.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                ExeName = $"{textBox1.Text}.exe";
                label6.Text = ExeName;
            }
            else
            {
                ExeName = "Compiled_Code.exe";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*
             Editor edit = new Editor();
             edit.ShowDialog();
             */
            MessageBox.Show("This feature is currently being developed by Agniva, Will be done in few days!", "Infomration", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true && checkBox2.Checked == true)
            {
                checkBox2.Checked = false;
                Hidden = true;
                ShowWindow = false;
            }
            else
            {
                if (checkBox1.Checked == true)
                {
                    Hidden = true;
                    ShowWindow = false;
                }
                else
                {
                    Hidden = false;
                    ShowWindow = true;
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true && checkBox1.Checked == true)
            {
                checkBox1.Checked = false;
                Hidden = false;
                ShowWindow = true;
            }
            else
            {
                if (checkBox2.Checked == true)
                {
                    Hidden = false;
                    ShowWindow = true;
                }
                else
                {
                    Hidden = false;
                    ShowWindow = true;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog yo = new OpenFileDialog())
            {
                yo.Filter = "Manifest file (*.manifest)|*.manifest";
                if (yo.ShowDialog() == DialogResult.OK)
                {
                    label7.Text = "Manifest Selected? = Yes.";
                    ManifestFilePath = yo.FileName.Replace(@"\", @"\\");
                    button8.Enabled = true;
                    button7.Enabled = false;
                }
                else
                {
                    ManifestFilePath = null;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog yo = new OpenFileDialog())
            {
                yo.Filter = "Manifest file (*.manifest)|*.manifest";
                if (yo.ShowDialog() == DialogResult.OK)
                {
                    label7.Text = "Manifest Selected? = Yes.";
                    ManifestFilePath = yo.FileName.Replace(@"\", @"\\");
                    button8.Enabled = true;
                }
                else
                {
                    ManifestFilePath = null;
                }
            }
        }
    }
}