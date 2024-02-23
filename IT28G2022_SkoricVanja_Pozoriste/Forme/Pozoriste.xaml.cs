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
    /// Interaction logic for Pozoriste.xaml
    /// </summary>
    public partial class Pozoriste : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;
        private static string zaposleni = @"select ime + ' ' + prezime as Zaposleni , zaposleniID from Zaposleni";
        public Pozoriste()
        {
            InitializeComponent();
            Popunjavanje.Popuni(cmbZaposleni, zaposleni);
            cmbZaposleni.Focus();
            konekcija = kon.KreirajKonekciju();
        }

        public Pozoriste(bool azuriraj, DataRowView red)
        {
            konekcija = kon.KreirajKonekciju();
            this.azuriraj = azuriraj;
            this.red = red;
            InitializeComponent();
            Popunjavanje.Popuni(cmbZaposleni, zaposleni);
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
                cmd.Parameters.Add("@zaposleniID", SqlDbType.Int).Value = (int)cmbZaposleni.SelectedValue;
                cmd.Parameters.Add("@naziv", SqlDbType.NVarChar).Value = txtNaziv.Text;
                cmd.Parameters.Add("@lokacija", SqlDbType.NVarChar).Value = txtLokacija.Text;
                cmd.Parameters.Add("@direktor", SqlDbType.NVarChar).Value = txtDirektor.Text;

                if (azuriraj)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"update Pozoriste
                                        set zaposleniID = @zaposleniID, naziv = @naziv, lokacija = @lokacija, direktor = @direktor
                                        where pozoristeID = @id";
                    red = null;
                }
                else
                {
                    cmd.CommandText = @"insert into Pozoriste
                                               values (@zaposleniID, @naziv, @lokacija, @direktor)";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
