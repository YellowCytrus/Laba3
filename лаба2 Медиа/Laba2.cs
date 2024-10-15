using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib.WavPack;

namespace лаба2_Медиа
{
    public partial class Laba2 : Form
    {
        public Laba2()
        {
            InitializeComponent();
        }

        private void open_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();

            dialog.InitialDirectory = "c:\\";
            dialog.Filter = "Windows Media Video (*.wmv)|*.wmv|All files (*.*)|*.*";
            dialog.FilterIndex = 1;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var filename = dialog.FileName;
                TagLib.File file = TagLib.File.Create(filename);

                var db = new AeroDataContext();
                var data = new Videos
                {
                    FileName = filename,
                    Width = file.Properties.VideoWidth,
                    Heigth = file.Properties.VideoHeight,

                };
                db.Videos.InsertOnSubmit(data);
                db.SubmitChanges();

                files.Items.Add(filename);

                player.URL = filename;
                player.close();
            }
        }

        private void play_Click(object sender, EventArgs e)
        {
            var playlist = player.newPlaylist("Playlist", "");

            foreach (var selected in files.Items)
            {
                string s = selected.ToString();
                var temp = player.newMedia(s);
                playlist.appendItem(temp);
            }
        }
    }
}
