using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;

namespace TheMovieDB
{
    public partial class formMain : Form
    {
        public int number = 1;
        public string url;
        public string address;

        public formMain()
        {
            InitializeComponent();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            dataGridView.Rows.Clear();
            string year = treeView.SelectedNode.Text;
            url = "https://api.themoviedb.org/3/discover/movie?api_key=d63d007af8c7f4d8beb5206b14594b47&language=fr&include_adult=false&include_video=true&page=1&primary_release_year=" + year + "&page=" + number.ToString();

            if (year != "Films" && year != "Séries")
            {
                RootObject JSON = ChargerJSON();

                // on grise la page précédente si on est à la première page
                if (JSON.page == 1) { buttonPrevious.Enabled = false; }
            }
        }

        private RootObject ChargerJSON()
        {
            // on instancie pour aller chercher l'url sur internet
            WebClient client = new WebClient();

            // encodage du fichier client
            client.Encoding = Encoding.UTF8;

            address = client.DownloadString(url);
            RootObject JSON = JsonConvert.DeserializeObject<RootObject>(address);

            for (int i = 0; i < JSON.results.Count; i++)
            {
                string[] film = new string[5];
                
                film[0] = JSON.results[i].overview;
                film[1] = JSON.results[i].original_title;
                film[2] = JSON.results[i].original_language;
                film[3] = JSON.results[i].title;
                film[4] = JSON.results[i].backdrop_path;
                //film[] = JSON.results[i].poster_path;
                //film[] = JSON.results[i].adult;
                //film[] = JSON.results[i].release_date;
                //film[] = JSON.results[i].popularity;
                //film[] = JSON.results[i].vote_count;
                //film[] = JSON.results[i].video;
                //film[] = JSON.results[i].vote_average;

                dataGridView.Rows.Add(film);
            }
            return JSON;
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            string i = treeView.SelectedNode.Text;

            if (i != "Films" && i != "TV")
            {
                buttonNext.Enabled = true;
                
                if (number > 1)
                {
                    dataGridView.Rows.Clear();
                    number--;
                    url += "&page=" + number;

                    ChargerJSON();
                }

                if (number == 1)
                {
                    buttonPrevious.Enabled = false;
                }
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            string i = treeView.SelectedNode.Text;

            if (i != "Films" && i != "TV")
            {
                RootObject JSON = ChargerJSON();

                if (JSON.page < JSON.total_pages)
                {
                    dataGridView.Rows.Clear();
                    number++;
                    url += "&page=" + number;
                    ChargerJSON();
                }

                if (number != 1) { buttonPrevious.Enabled = true; }
                if (number == JSON.total_pages) { buttonNext.Enabled = false; }
            } 
        }
    }
}
