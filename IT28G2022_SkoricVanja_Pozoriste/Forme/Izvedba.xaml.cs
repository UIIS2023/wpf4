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
    /// Interaction logic for Izvedba.xaml
    /// </summary>
    public partial class Izvedba : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;
        private static string predstava = @"select * from Predstava";
       
        public Izvedba()
        {
            InitializeComponent();
            Popunjavanje.Popuni(cmbPredstava, predstava);
            cmbPredstava.Focus();
            konekcija = kon.KreirajKonekciju();
        }

        public Izvedba(bool azuriraj, DataRowView red)
        {
            this.azuriraj = azuriraj;
            this.red = red;
            InitializeComponent();
            Popunjavanje.Popuni(cmbPredstava, predstava);
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
                cmd.Parameters.Add("@predstavaID", SqlDbType.Int).Value = (int)cmbPredstava.SelectedValue;
                cmd.Parameters.Add("@vremePocetka", SqlDbType.DateTime).Value = DateTime.Parse(dtVremePocetka.Text);
             
                if (azuriraj)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"update Izvedba
                                        set predstavaID = @predstavaID, vremePocetka = @vremePocetka
                                        where izvedbaID = @id";
                    red = null;
                }
                else
                {
                    cmd.CommandText = @"insert into Izvedba
                                               values (@predstavaID, @vremePocetka)";
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
