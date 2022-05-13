using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace ResmiYazisma2
{
    public partial class Form1 : Form
    {
        // DbConnection con;
        SqlConnection con;
        public Form1()
        {
            InitializeComponent();
           
        }

        private void FindAndReplace(Word.Application wordApp, object ToFindText, object replaceWithText)
        {
            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundLike = false;
            object nmatchAllforms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiactitics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;

            wordApp.Selection.Find.Execute(ref ToFindText,
                ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundLike,
                ref nmatchAllforms, ref forward,
                ref wrap, ref format, ref replaceWithText,
                ref replace, ref matchKashida,
                ref matchDiactitics, ref matchAlefHamza,
                ref matchControl);
        }

        private void CreateWordDocument(object filename, object SaveAs)
        {
          //  SaveAs= (object)System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + txtSyi.Text + ".docx";
            Word.Application wordApp = new Word.Application();
            object missing = Missing.Value;
            Word.Document myWordDoc = null;

            if (File.Exists((string)filename))
            {
                object readOnly = false;
                object isVisible = false;
                wordApp.Visible = false;

                myWordDoc = wordApp.Documents.Open(ref filename, ref missing, ref readOnly,
                                        ref missing, ref missing, ref missing,
                                        ref missing, ref missing, ref missing,
                                        ref missing, ref missing, ref missing,
                                        ref missing, ref missing, ref missing, ref missing);
                myWordDoc.Activate();

                //find and replace
                this.FindAndReplace(wordApp, "[Sayi]", txtSyi.Text);
                this.FindAndReplace(wordApp, "[Konu]", txtKonu.Text);
                this.FindAndReplace(wordApp, "[Temsilci]","Faruk Seçkin");
                this.FindAndReplace(wordApp, "[Tarih]", dtTarih.Value.ToShortDateString());
                this.FindAndReplace(wordApp, "[icerik]", txtIcerik.Text);
            }
            else
            {
                MessageBox.Show("File not Found!");
            }

            //Save as
            myWordDoc.SaveAs2(ref SaveAs, ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing);

            myWordDoc.Close();
            wordApp.Quit();
            MessageBox.Show("Dosya Oluşturuldu.!");
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            Object temlateFile = Application.StartupPath + "\\Antetli_Template.docx";
            object saveAsfile = (object)System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Resmi Yazışmalar\\" + txtKonu.Text+".docx";
            //CreateWordDocument(temlateFile, saveAsfile);
            if (Directory.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Resmi Yazışmalar"))
            {
                // Directory.CreateDirectory(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Resmi Yazışmalar");
                CreateWordDocument(temlateFile, saveAsfile);
            }
            else
            {
                //  MessageBox.Show("Yok");
                Directory.CreateDirectory(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Resmi Yazışmalar");

                CreateWordDocument(temlateFile, saveAsfile);
            }
        }

        private void Baglanti()
        {
            // con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.16.0;Data Source=Data\\dbResmiYazismalar.accde");
            //OleDbDataAdapter da;
            //OleDbCommand command;
            //con.Close();
            //con.Open();
            //OleDbCommand cmd = new OleDbCommand("SELECT top (1) sayi FROM Sayi order by id desc", (OleDbConnection)con);
            //string sonSayi = cmd.ExecuteScalar().ToString();
            //con.Close();
            con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = "DataSource=(LocalDB)\\MSSQLLocalDB;attachdbfilename=|DataDirectory|; AttachDbFilename =Data.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
            con.Open();
            MessageBox.Show("Connection opened");
            SqlCommand cmd = new SqlCommand("SELECT top (1) sayi FROM TblSayi order by id desc", con);
            string sonSayi = cmd.ExecuteScalar().ToString();
            con.Close();
            MessageBox.Show("Connection closed");
            MessageBox.Show(sonSayi);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Baglanti();
            DataEntities context = new DataEntities();

            //TblSayi sayi = new TblSayi { Sayi = 2 };
            //context.Entry(sayi);
            //context.SaveChanges();
            var  sayiCek=context.TblSayi.ToList().LastOrDefault();

            MessageBox.Show(sayiCek.Sayi.ToString());
        }
    }
}
