using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kiselov_HW_SearchTextFiles
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Action actionDelegateForm = null;

        int iCount = 0;

        FileOperator fOperator; 

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Checks for empty text boxes 
                if (textBox1.Text.Length == 0)
                {
                    MessageBox.Show("Incorrect value of searched word");
                }

                else if (textBox2.Text.Length == 0)
                {
                    MessageBox.Show("Incorrect value of new word");
                }

                else if (textBox3.Text.Length == 0)
                {
                    MessageBox.Show("Incorrect path to files");
                }
                else
                {
                    // creation of new fileoperator object 
                    fOperator = new FileOperator(textBox1.Text, textBox2.Text);

                    // method gets an amount of pathes to *.txt files  
                    List<string> lstPathes = new List<string>();
                    lstPathes = fOperator.GetPathes(textBox3.Text);

                    // method checks returned collection for amount of searched files  
                    if (lstPathes.Count < 1)
                    {
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                        MessageBox.Show("There are no needed files in current directory");
                    }

                    else
                    {
                        textBox4.AppendText(string.Format("Amount of found *.txt files - {0}\r\n", fOperator.Files));

                        // Async starts 
                        iCount = lstPathes.Count;

                        actionDelegateForm = CallBackFromThread;

                        foreach (string strPath in lstPathes)
                        {

                            Action<string> MainFunction = fOperator.ChangeWordsInFile;
                            IAsyncResult iaresult = MainFunction.BeginInvoke(strPath, Finish, MainFunction);

                            // Async ends 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            DialogResult result = folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                textBox3.Text = folderBrowser.SelectedPath+'\\';
            }
        }

        void Finish(IAsyncResult iar)
        {
            try
            {
                Action<string> MainFunction = (Action<string>)iar.AsyncState;
                MainFunction.EndInvoke(iar);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            actionDelegateForm();
        }

        void CallBackFromThread()
        {
            Interlocked.Decrement(ref iCount);
            if (iCount == 0)
            {
                // the recent part of executed task 
                textBox4.AppendText("List of edited files: \r\n");
                foreach (string strPath in fOperator.ListOfEditedFiles)
                {
                    textBox4.AppendText(string.Format("{0} \r\n", strPath));
                }

                textBox4.AppendText(string.Format("Amount of edited words - {0} \r\n", fOperator.Words));
            }
        }
    }
}
