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
    /// Interaction logic for Ulaznica.xaml
    /// </summary>
    public partial class Ulaznica : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;
        private static string gledalac = @"select ime + ' ' + prezime as Gledalac,gledalacID from Gledalac";
        private static string pozoriste = @"select * from Pozoriste";
        private static string izvedba = @"select * from Izvedba";

        public Ulaznica()
        {
            InitializeComponent();
            Popunjavanje.Popuni(cmbGledalac, gledalac);
            cmbGledalac.Focus();
            Popunjavanje.Popuni(cmbPozoriste, pozoriste);
            cmbPozoriste.Focus();
            Popunjavanje.Popuni(cmbIzvedba, izvedba);
            konekcija = kon.KreirajKonekciju();
        }

        public Ulaznica(bool azuriraj, DataRowView red)
        {
            this.azuriraj = azuriraj;
            this.red = red;
            InitializeComponent();
            Popunjavanje.Popuni(cmbGledalac, gledalac);
            Popunjavanje.Popuni(cmbPozoriste, pozoriste);
            Popunjavanje.Popuni(cmbIzvedba, izvedba);
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
                cmd.Parameters.Add("@gledalacID", SqlDbType.Int).Value = (int)cmbGledalac.SelectedValue;
                cmd.Parameters.Add("@pozoristeID", SqlDbType.Int).Value = (int)cmbPozoriste.SelectedValue;
                cmd.Parameters.Add("@izvedbaID", SqlDbType.Int).Value = (int)cmbIzvedba.SelectedValue;
                cmd.Parameters.Add("@cena", SqlDbType.Int).Value = int.Parse(txtCena.Text);
                cmd.Parameters.Add("@brojSedista", SqlDbType.Int).Value = int.Parse(txtBrojSedista.Text);

                if (azuriraj)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"update Ulaznica
                                        set gledalacID = @gledalacID, pozoristeID = @pozoristeID, izvedbaID = @izvedbaID, cena = @cena, brojSedista = @brojSedista 
                                        where ulaznicaID = @id";
                    red = null;
                }
                else
                {
                    cmd.CommandText = @"insert into Ulaznica
                                               values (@gledalacID, @pozoristeID, @izvedbaID, @cena, @brojSedista)";
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

