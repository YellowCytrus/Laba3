using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Windows.Media;

namespace Лаба3_Медиа
{
    public partial class Form1 : Form
    {
        private string filename;
        private MediaPlayer player = new MediaPlayer();
        private AeroDataContext db = new AeroDataContext();

        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter;
        DataTable table;

        public Form1()
        {
            InitializeComponent();

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void open_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "c:\\";
            dialog.Filter = "Windows Media Audio (*.wma)|*.wma|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.ShowDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filename = dialog.FileName;
                TagLib.File file = TagLib.File.Create(filename);

                var db = new AeroDataContext();
                var data = new Audio
                {
                    Filename = filename,
                    Title = file.Tag.Title,
                    Album = file.Tag.Album,
                    Year = (int)file.Tag.Year,
                    Comment = file.Tag.Comment
                };
                db.Audio.InsertOnSubmit(data);
                db.SubmitChanges();

                title.Text = file.Tag.Title;
                artist.Text = file.Tag.FirstPerformer;
                album.Text = file.Tag.Album;
                year.Text = file.Tag.Year.ToString();
                comment.Text = file.Tag.Comment;
            }
        }

        private void play_Click(object sender, EventArgs e)
        {
            Uri uri = new Uri("file://" + filename);
            player.Open(uri);
            player.Play();
        }

        private void stop_Click(object sender, EventArgs e)
        {
            player.Stop();
            player.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "aeroDataSet.Audio". При необходимости она может быть перемещена или удалена.
            this.audioTableAdapter.Fill(this.aeroDataSet.Audio);

        }
    }
}
