using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace ApexStats
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StringBuilder sb = new StringBuilder();
        ArrayList seasons = new ArrayList();
        string[] ranks = new string[] {"Bronze 4", "Bronze 3", "Bronze 2", "Bronze 1",
                                       "Silver 4", "Silver 3", "Silver 2", "Silver 1",
                                       "Gold 4", "Gold 3", "Gold 2", "Gold 1",
                                       "Platinum 4", "Platinum 3", "Platinum 2", "Platinum 1",
                                       "Diamond 4", "Diamond 3", "Diamond 2", "Diamond 1",
                                       "Master", "Apex Predator" };
        string[] gamemodes = new string[] { "Battle Royale", "Arenas" };

        public MainWindow()
        {
            InitializeComponent();

            foreach (string mode in gamemodes)
            {
                txtGamemode.Items.Add(mode);
            }

            foreach (string rank in ranks)
            {
                txtSplitOneRank.Items.Add(rank);
                txtSplitTwoRank.Items.Add(rank);
            }
        }


        private void ButtonAddSeason_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSeason.Text) && !lstSeasons.Items.Contains(txtSeason.Text))
            {
                Season newS = new Season(txtGamemode.Text.ToString(), txtSeason.Text.ToString(),
                    txtSplitOneRank.Text.ToString(), txtSplitTwoRank.Text.ToString(),
                    txtAvgDamage.Text.ToString(),
                    txtKdr.Text.ToString(), txtRankedAvgDamage.Text.ToString(),
                    txtRankedAvgDamage.Text.ToString());
                lstSeasons.Items.Add(newS.toString());
                seasons.Add(newS);
                seasons.Add(Environment.NewLine);
                txtGamemode.SelectedItem = null;
                txtSeason.Clear();
                txtSplitOneRank.SelectedItem = null;
                txtSplitTwoRank.SelectedItem = null;
                txtAvgDamage.Clear();
                txtKdr.Clear();
                txtRankedAvgDamage.Clear();
                txtRankedKdr.Clear();
            }

        }


        // private void ButtonAddName_Click(object sender, RoutedEventArgs e)
        // {
        //     if (!string.IsNullOrWhiteSpace(txtName.Text) && !lstNames.Items.Contains(txtName.Text))
        //     {
        //         lstNames.Items.Add(txtName.Text);
        //         sb.Append(txtName.Text.ToString());
        //         sb.Append(Environment.NewLine);
        //         txtName.Clear();
        //     }
        // }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                foreach (string season in lstSeasons.Items) // needs work to get working with seasons list
                {
                    File.AppendAllText(saveFileDialog.FileName, season + Environment.NewLine);
                }
            }
        }


        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            lstSeasons.Items.Clear();
            Season newS = new Season();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
                foreach (string season in File.ReadAllLines(openFileDialog.FileName))
                {
                    newS.parse(season);
                    lstSeasons.Items.Add(newS.toString());
                    seasons.Add(newS);
                }
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            Season selected = new Season();
            selected.parse(lstSeasons.SelectedItem.ToString());
            txtSeason.Text = selected.getSeason();
            txtAvgDamage.Text = selected.getAvgDamage();
            txtKdr.Text = selected.getKdr();
            txtRankedAvgDamage.Text = selected.getRankedAvgDamage();
            txtRankedKdr.Text = selected.getRankedKdr();
            txtSplitOneRank.SelectedItem = selected.getSplitOneRank();
            txtSplitTwoRank.SelectedItem = selected.getSplitTwoRank();
        }


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtGamemode.SelectedItem = null;
            txtSeason.Clear();
            txtSplitOneRank.SelectedItem = null;
            txtSplitTwoRank.SelectedItem = null;
            txtAvgDamage.Clear();
            txtKdr.Clear();
            txtRankedAvgDamage.Clear();
            txtRankedKdr.Clear();
        }
    }

    public class Season
    {
        private string gamemode;
        private string season;
        private string splitOneRank;
        private string splitTwoRank;
        private string avgDamage;
        private string kdr;
        private string rankedAvgDamage;
        private string rankedKdr;


        public Season()
        {

        }


        public Season(string gamemode, string season, string splitOneRank, string splitTwoRank, string avgDamage,
                            string kdr, string rankedAvgDamage, string rankedKdr)
        {
            this.gamemode = gamemode;
            this.season = season;
            this.splitOneRank = splitOneRank;
            this.splitTwoRank = splitTwoRank;
            this.avgDamage = avgDamage;
            this.kdr = kdr;
            this.rankedAvgDamage = rankedAvgDamage;
            this.rankedKdr = rankedKdr;
        }

        public string getGamemode()
        {
            return gamemode;
        }

        public void setGamemode()
        {
            this.gamemode = gamemode;
        }

        public string getSeason()
        {
            return season;
        }


        public void setSeason(string season)
        {
            this.season = season;
        }


        public string getSplitOneRank()
        {
            return splitOneRank;
        }


        public void setSplitOneRank(string splitOneRank)
        {
            this.splitOneRank = splitOneRank;
        }


        public string getSplitTwoRank()
        {
            return splitTwoRank;
        }


        public void setSplitTwoRank(string splitTwoRank)
        {
            this.splitTwoRank = splitTwoRank;
        }


        public string getAvgDamage()
        {
            return avgDamage;
        }


        public void setAvgDamage(string avgDamage)
        {
            this.avgDamage = avgDamage;
        }


        public string getKdr()
        {
            return kdr;
        }


        public void setKdr(string kdr)
        {
            this.kdr = kdr;
        }


        public string getRankedAvgDamage()
        {
            return rankedAvgDamage;
        }


        public void getRankedAvgDamage(string rankedAvgDamage)
        {
            this.rankedAvgDamage = rankedAvgDamage;
        }


        public string getRankedKdr()
        {
            return rankedKdr;
        }


        public void setRankedKdr(string rankedKdr)
        {
            this.rankedKdr = rankedKdr;
        }


        public string toString()
        {
            return "" + gamemode + " | " + season + " | " + splitOneRank + " | " + splitTwoRank + " | " + avgDamage + " | " +
                kdr + " | " + rankedAvgDamage + " | " + rankedKdr;
        }


        public void parse(String rivi)
        {
            StringBuilder sb = new StringBuilder(rivi);
            gamemode = Separate(sb, '|', gamemode);
            season = Separate(sb, '|', season);
            splitOneRank = Separate(sb, '|', splitOneRank);
            splitTwoRank = Separate(sb, '|', splitTwoRank);
            avgDamage = Separate(sb, '|', avgDamage);
            kdr = Separate(sb, '|', kdr);
            rankedAvgDamage = Separate(sb, '|', rankedAvgDamage);
            rankedKdr = Separate(sb, '|', rankedKdr);
        }


        public static String Separate(StringBuilder sb, char character, bool searchBackwards)
        {
            if (sb == null) return "";
            int p;
            if (searchBackwards) p = sb.ToString().LastIndexOf("" + character);
            else p = sb.ToString().IndexOf("" + character);
            String beginning;
            if (p < 0)
            {
                beginning = sb.ToString();
                sb.Remove(0, sb.Length);
                return beginning;
            }
            beginning = sb.ToString().Substring(0, p);
            sb.Remove(0, p + 1);
            return beginning;
        }


        public static String Separate(StringBuilder sb, char character, String defaultS)
        {
            String piece = Separate(sb, character, false);
            if (piece.Length == 0) piece = defaultS;
            if (piece == null) piece = "";
            return Remove_2_Empty(piece.Trim());
        }


        public static String Remove_2_Empty(String s)
        {
            if (s == null) return "";
            return Remove_2_Empty(new StringBuilder(s)).ToString();
        }


        public static StringBuilder Remove_2_Empty(StringBuilder s)
        {
            int empty = 0;
            int l = 0, k = 0;
            if (s == null) return new StringBuilder("");
            int leng = s.Length;

            while (l < leng)
            {
                char c = s[l];
                if (c == ' ') empty++;
                else leng = 0;
                if (leng <= 1) s[k++] = c;
                l++;
            }
            //s.Remove(k, leng);
            return s;
        }
    }
}
