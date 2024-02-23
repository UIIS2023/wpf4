using IT28G2022_SkoricVanja_Pozoriste.Forme;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IT28G2022_SkoricVanja_Pozoriste
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string ucitanaTabela;
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;

        #region Select upiti
        private static string gledalacSelect = @"select gledalacID as ID, ime as Ime, prezime as Prezime, pol as Pol from Gledalac";
        private static string glumacSelect = @"select glumacID as ID, ime as Ime, prezime as Prezime, godine as Godine, pol as Pol from [Glumac/ica]";
        private static string rediteljSelect = @"select rediteljID as ID, ime as Ime, prezime as Prezime, godine as Godine from Reditelj";
        private static string zaposleniSelect = @"select zaposleniID as ID, ime as Ime, prezime as Prezime, pozicija as Pozicija, radnoVreme as 'Radno vreme' from Zaposleni";
        private static string izvedbaSelect = @"select izvedbaID as ID, vremePocetka as 'Vreme pocetka' from Izvedba join Predstava on Izvedba.predstavaID=Predstava.predstavaID";
        private static string pozoristeSelect = @"select pozoristeID as ID, naziv as Naziv, lokacija as Lokacija, direktor as Direktor from Pozoriste join Zaposleni on Pozoriste.zaposleniID=Zaposleni.zaposleniID";
        private static string predstavaSelect = @"select predstavaID as ID, naslov as Naslov, autor as Autor, zanr as Zanr from Predstava join [Glumac/ica] on Predstava.glumacID=[Glumac/ica].glumacID
                                                                                                                                          join Reditelj on Predstava.rediteljID=Reditelj.rediteljID";
        private static string ulaznicaSelect = @"select ulaznicaID as ID, cena as Cena, brojSedista as 'Broj sedista' from Ulaznica join Gledalac on Ulaznica.gledalacID=Gledalac.gledalacID
                                                                                                                                    join Pozoriste on Ulaznica.pozoristeID=Pozoriste.pozoristeID
                                                                                                                                    join Izvedba on Ulaznica.izvedbaID=Izvedba.izvedbaID";
        #endregion

        #region Select sa uslovom
        private static string selectUslovGledalac = @"select * from Gledalac where gledalacID=";
        private static string selectUslovGlumac = @"select * from [Glumac/ica] where glumacID=";
        private static string selectUslovReditelj = @"select * from Reditelj where rediteljID=";
        private static string selectUslovZaposleni = @"select * from Zaposleni where zaposleniID=";
        private static string selectUslovIzvedba = @"select * from Izvedba where izvedbaID=";
        private static string selectUslovPozoriste = @"select * from Pozoriste where pozoristeID=";
        private static string selectUslovPredstava = @"select * from Predstava where predstavaID=";
        private static string selectUslovUlaznica = @"select * from Ulaznica where ulaznicaID=";
        #endregion

        #region Delete naredbe
        private static string gledalacDelete = @"delete from Gledalac where gledalacID=";
        private static string glumacDelete = @"delete from Glumac where glumacID=";
        private static string rediteljDelete = @"delete from Reditelj where rediteljID=";
        private static string zaposleniDelete = @"delete from Zaposleni where zaposleniID=";
        private static string izvedbaDelete = @"delete from Izvedba where izvedbaID=";
        private static string pozoristeDelete = @"delete from Pozoriste where pozoristeID=";
        private static string predstavaDelete = @"delete from Predstava where predstavaID=";
        private static string ulaznicaDelete = @"delete from Ulaznica where ulaznicaID=";

        #endregion


        public MainWindow()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            UcitajPodatke(gledalacSelect);

        }

        private void UcitajPodatke(string selectUpit)
        {
            try
            {
                konekcija.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(selectUpit, konekcija);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                if (dataGridCentralni != null)
                {
                    dataGridCentralni.ItemsSource = dataTable.DefaultView;
                }
                ucitanaTabela = selectUpit;
                dataAdapter.Dispose();
                dataTable.Dispose();
            }
            catch (SqlException)
            {
                MessageBox.Show("Neuspesno ucitani podaci", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }

        }

        private void btnPozoriste_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(pozoristeSelect);
        }

        private void btnPredstava_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(predstavaSelect);
        }

        private void btnIzvedba_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(izvedbaSelect);
        }

        private void btnGlumac_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(glumacSelect);
        }

        private void btnReditelj_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(rediteljSelect);
        }

        private void btnGledalac_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(gledalacSelect);
        }

        private void btnZaposleni_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(zaposleniSelect);
        }

        private void btnUlaznica_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(ulaznicaSelect);
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window prozor;

            if (ucitanaTabela.Equals(zaposleniSelect))
            {
                prozor = new Zaposleni();
                prozor.ShowDialog();
                UcitajPodatke(zaposleniSelect);

            }
            else if (ucitanaTabela.Equals(rediteljSelect))
            {
                prozor = new Reditelj();
                prozor.ShowDialog();
                UcitajPodatke(rediteljSelect);

            }
            else if (ucitanaTabela.Equals(gledalacSelect))
            {
                prozor = new Gledalac();
                prozor.ShowDialog();
                UcitajPodatke(gledalacSelect);

            }
            else if (ucitanaTabela.Equals(glumacSelect))
            {
                prozor = new Glumac();
                prozor.ShowDialog();
                UcitajPodatke(glumacSelect);

            }
            else if (ucitanaTabela.Equals(pozoristeSelect))
            {
                prozor = new Pozoriste();
                prozor.ShowDialog();
                UcitajPodatke(pozoristeSelect);

            }
            else if (ucitanaTabela.Equals(izvedbaSelect))
            {
                prozor = new Izvedba();
                prozor.ShowDialog();
                UcitajPodatke(izvedbaSelect);

            }
            else if (ucitanaTabela.Equals(ulaznicaSelect))
            {
                prozor = new Ulaznica();
                prozor.ShowDialog();
                UcitajPodatke(ulaznicaSelect);

            }
            else if (ucitanaTabela.Equals(predstavaSelect))
            {
                prozor = new Predstava();
                prozor.ShowDialog();
                UcitajPodatke(predstavaSelect);

            }

        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(zaposleniSelect))
            {
                PopuniFormu(selectUslovZaposleni);
                UcitajPodatke(zaposleniSelect);

            }
            else if (ucitanaTabela.Equals(rediteljSelect))
            {
                PopuniFormu(selectUslovReditelj);
                UcitajPodatke(rediteljSelect);

            }
            else if (ucitanaTabela.Equals(gledalacSelect))
            {
                PopuniFormu(selectUslovGledalac);
                UcitajPodatke(gledalacSelect);

            }
            else if (ucitanaTabela.Equals(glumacSelect))
            {
                PopuniFormu(selectUslovGlumac);
                UcitajPodatke(glumacSelect);

            }
            else if (ucitanaTabela.Equals(pozoristeSelect))
            {
                PopuniFormu(selectUslovPozoriste);
                UcitajPodatke(pozoristeSelect);

            }
            else if (ucitanaTabela.Equals(izvedbaSelect))
            {
                PopuniFormu(selectUslovIzvedba);
                UcitajPodatke(izvedbaSelect);

            }
            else if (ucitanaTabela.Equals(ulaznicaSelect))
            {
                PopuniFormu(selectUslovUlaznica);
                UcitajPodatke(ulaznicaSelect);

            }
            else if (ucitanaTabela.Equals(predstavaSelect))
            {
                PopuniFormu(selectUslovPredstava);
                UcitajPodatke(predstavaSelect);

            }
        }

        private void PopuniFormu(string selectUslov)
        {
            try
            {
                konekcija.Open();
                azuriraj = true;
                red = (DataRowView)dataGridCentralni.SelectedItems[0];
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                cmd.CommandText = selectUslov + "@id";
                SqlDataReader citac = cmd.ExecuteReader();
                if (citac.Read())
                {
                    if (ucitanaTabela.Equals(gledalacSelect))
                    {
                        Gledalac prozorGledalac = new Gledalac(azuriraj, red);
                        prozorGledalac.txtIme.Text = citac["ime"].ToString();
                        prozorGledalac.txtPrezime.Text = citac["prezime"].ToString();
                        prozorGledalac.txtPol.Text = citac["pol"].ToString();
                        prozorGledalac.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(glumacSelect))
                    {
                        Glumac prozorGlumac = new Glumac(azuriraj, red);
                        prozorGlumac.txtIme.Text = citac["ime"].ToString();
                        prozorGlumac.txtPrezime.Text = citac["prezime"].ToString();
                        prozorGlumac.txtGodine.Text = citac["godine"].ToString();
                        prozorGlumac.txtPol.Text = citac["pol"].ToString();
                        prozorGlumac.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(izvedbaSelect))
                    {
                        Izvedba prozorIzvedba = new Izvedba(azuriraj, red);
                        prozorIzvedba.cmbPredstava.SelectedValue = citac["predstavaID"];
                        prozorIzvedba.dtVremePocetka.SelectedDate = (DateTime)citac["vremePocetka"];
                        prozorIzvedba.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(pozoristeSelect))
                    {
                        Pozoriste prozorPozoriste = new Pozoriste(azuriraj, red);
                        prozorPozoriste.cmbZaposleni.SelectedValue = citac["zaposleniID"];
                        prozorPozoriste.txtNaziv.Text = citac["naziv"].ToString();
                        prozorPozoriste.txtLokacija.Text = citac["lokacija"].ToString();
                        prozorPozoriste.txtDirektor.Text = citac["direktor"].ToString();
                        prozorPozoriste.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(predstavaSelect))
                    {
                        Predstava prozorPredstava = new Predstava(azuriraj, red);
                        prozorPredstava.cmbGlumac.SelectedValue = citac["glumacID"];
                        prozorPredstava.cmbReditelj.SelectedValue = citac["rediteljID"];
                        prozorPredstava.txtNaslov.Text = citac["naslov"].ToString();
                        prozorPredstava.txtAutor.Text = citac["autor"].ToString();
                        prozorPredstava.txtZanr.Text = citac["zanr"].ToString();
                        prozorPredstava.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(rediteljSelect))
                    {
                        Reditelj prozorReditelj = new Reditelj(azuriraj, red);
                        prozorReditelj.txtIme.Text = citac["ime"].ToString();
                        prozorReditelj.txtPrezime.Text = citac["prezime"].ToString();
                        prozorReditelj.txtGodine.Text = citac["godine"].ToString();
                        prozorReditelj.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(ulaznicaSelect))
                    {
                        Ulaznica prozorUlaznica = new Ulaznica(azuriraj, red);
                        prozorUlaznica.cmbGledalac.SelectedValue = citac["gledalacID"];
                        prozorUlaznica.cmbPozoriste.SelectedValue = citac["pozoristeID"];
                        prozorUlaznica.cmbIzvedba.SelectedValue = citac["izvedbaID"];
                        prozorUlaznica.txtCena.Text = citac["cena"].ToString();
                        prozorUlaznica.txtBrojSedista.Text = citac["brojSedista"].ToString();
                        prozorUlaznica.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(zaposleniSelect))
                    {
                        Zaposleni prozorZaposleni = new Zaposleni(azuriraj, red);
                        prozorZaposleni.txtIme.Text = citac["ime"].ToString();
                        prozorZaposleni.txtPrezime.Text = citac["prezime"].ToString();
                        prozorZaposleni.txtPozicija.Text = citac["pozicija"].ToString();
                        prozorZaposleni.txtRadnoVreme.Text = citac["radnoVreme"].ToString();
                        prozorZaposleni.ShowDialog();
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

        private void Brisanje(string selectUslov)
        {
            try
            {
                konekcija.Open();
                var selectedRow = (DataRowView)dataGridCentralni.SelectedItem;
                object a = selectedRow.Row.ItemArray[0];
                int? id = (int?)a; //upitnik znaci da moze biti i null vrednost
                SqlCommand cmd = new SqlCommand { Connection = konekcija };
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.CommandText = selectUslov + "@id";
                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (ArgumentOutOfRangeException){
                MessageBox.Show("Niste selektovali red!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SqlException)
            {
                MessageBox.Show("Ne mozete obrisati element koji se koristi u drugoj tabeli kao strani kljuc!",
                "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCentralni.SelectedItems.Count == 0)
            {
                MessageBox.Show("Morate selektovati red koji zelite da izmenite!", 
                    "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (dataGridCentralni.SelectedItems.Count > 1)
            {
                MessageBox.Show("Selektujte samo 1 red!", "Greska", MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
            else
            {

                if (ucitanaTabela.Equals(zaposleniSelect))
                {
                    Brisanje(zaposleniDelete);
                    UcitajPodatke(zaposleniSelect);


                }
                else if (ucitanaTabela.Equals(pozoristeSelect))
                {
                    Brisanje(pozoristeDelete);
                    UcitajPodatke(pozoristeSelect);

                }
                else if (ucitanaTabela.Equals(predstavaSelect))
                {
                    Brisanje(predstavaDelete);
                    UcitajPodatke(predstavaSelect);

                }
                else if (ucitanaTabela.Equals(rediteljSelect))
                {
                    Brisanje(rediteljDelete);
                    UcitajPodatke(rediteljSelect);

                }
                else if (ucitanaTabela.Equals(gledalacSelect))
                {
                    Brisanje(gledalacDelete);
                    UcitajPodatke(gledalacSelect);

                }
                else if (ucitanaTabela.Equals(glumacSelect))
                {

                    Brisanje(glumacDelete);
                    UcitajPodatke(glumacSelect);

                }
                else if (ucitanaTabela.Equals(ulaznicaSelect))
                {
                    Brisanje(ulaznicaDelete);
                    UcitajPodatke(ulaznicaSelect);

                }
                else if (ucitanaTabela.Equals(izvedbaSelect))
                {
                    Brisanje(izvedbaDelete);
                    UcitajPodatke(izvedbaSelect);

                }

            }
        }

        
    }
}