using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace IT28G2022_SkoricVanja_Pozoriste.Forme
{
    /// <summary>
    /// Interaction logic for Predstava.xaml
    /// </summary>
    public partial class Predstava : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;
        private static string glumac = @"select ime + ' ' + prezime as Glumac, glumacID from [Glumac/ica]";
        private static string reditelj = @"select ime + ' ' + prezime as Reditelj, rediteljID from Reditelj";
        public Predstava()
        {
            InitializeComponent();
            Popunjavanje.Popuni(cmbGlumac, glumac);
            cmbGlumac.Focus();
            Popunjavanje.Popuni(cmbReditelj, reditelj);
            cmbReditelj.Focus();
            konekcija = kon.KreirajKonekciju();
        }

        public Predstava(bool azuriraj, DataRowView red)
        {
            this.azuriraj = azuriraj;
            this.red = red;
            InitializeComponent();
            Popunjavanje.Popuni(cmbGlumac, glumac);
            Popunjavanje.Popuni(cmbReditelj, reditelj);
            konekcija = kon.KreirajKonekciju();
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@glumacID", SqlDbType.Int).Value = (int)cmbGlumac.SelectedValue;
                cmd.Parameters.Add("@rediteljID", SqlDbType.Int).Value = (int)cmbReditelj.SelectedValue;
                cmd.Parameters.Add("@naslov", SqlDbType.NVarChar).Value = txtNaslov.Text;
                cmd.Parameters.Add("@autor", SqlDbType.NVarChar).Value = txtAutor.Text;
                cmd.Parameters.Add("@zanr", SqlDbType.NVarChar).Value = txtZanr.Text;

                if (azuriraj)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"update Predstava
                                        set glumacID = @glumacID, rediteljID = @rediteljID, naslov = @naslov, autor = @autor, zanr = @zanr 
                                        where predstavaID = @id";
                    red = null;
                }
                else
                {
                    cmd.CommandText = @"insert into Predstava
                                               values (@glumacID, @rediteljID, @naslov, @autor, @zanr)";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unos odredjenih vrednosti nije validan.");
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
