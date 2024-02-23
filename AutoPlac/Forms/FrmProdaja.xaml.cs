using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace AutoPlac.Forms
{
    /// <summary>
    /// Interaction logic for FrmProdaja.xaml
    /// </summary>
    public partial class FrmProdaja : Window
    {
        Konekcija con = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        DataRowView pomocniRed;
        bool azuriraj;
        public FrmProdaja()
        {
            InitializeComponent();
            konekcija = con.KreirajKonekciju();
            PopuniPadajuceListe();
            txtBrojRata.Focus();
        }
        public FrmProdaja(bool azuriraj, DataRowView red) : this()
        {
            this.azuriraj = azuriraj;
            this.pomocniRed = red;
        }

        private void FillComboBox(ComboBox cb, string query)
        {
            using (DataTable dt = new DataTable())
            {
                using (SqlDataAdapter da = new SqlDataAdapter(query, konekcija))
                {
                    da.Fill(dt);
                }
                cb.ItemsSource = dt.DefaultView;
            }
        }
        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();
                string vratiVozilo;
                if(this.azuriraj)
                    vratiVozilo = @"select VoziloID, BrojSasije from tbl_Vozilo where Prodato=0";
                else
                    vratiVozilo = @"select VoziloID, BrojSasije from tbl_Vozilo";

                FillComboBox(cbVozilo, vratiVozilo);

                string vratiKupca = @"select KupacID, Ime from tbl_Kupac";
                FillComboBox(cbKupac, vratiKupca);

                string vratiZaposlenog = @"select ZaposleniID, Ime from tbl_Zaposleni";
                FillComboBox(cbZaposleni, vratiZaposlenog);

                string vratiNacinPlacanja = @"select NacinPlacanjaID, Naziv from tbl_NacinPlacanja";
                FillComboBox(cbNacinPlacanja, vratiNacinPlacanja);

              
            }
            catch (SqlException)
            {
                MessageBox.Show("Padajuce liste nisu popunjene", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                konekcija.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = konekcija;

                    if (string.IsNullOrWhiteSpace(txtBrojRata.Text) || dpDatum.SelectedDate == null ||
                      cbVozilo.SelectedValue == null || cbKupac.SelectedValue == null || cbZaposleni.SelectedValue == null ||
                      cbNacinPlacanja.SelectedValue == null)
                    {
                        throw new FormatException("Polja ne smeju biti prazna!");
                    }

                    DateTime date = (DateTime)dpDatum.SelectedDate;
                    string datum = date.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add("@brojRata", SqlDbType.Int).Value = int.Parse(txtBrojRata.Text);
                    cmd.Parameters.Add("@voziloID", SqlDbType.Int).Value = cbVozilo.SelectedValue;
                    cmd.Parameters.Add("@kupacID", SqlDbType.Int).Value = cbKupac.SelectedValue;
                    cmd.Parameters.Add("@zaposleniID", SqlDbType.Int).Value = cbZaposleni.SelectedValue;
                    cmd.Parameters.Add("@nacinPlacanjaID", SqlDbType.Int).Value = cbNacinPlacanja.SelectedValue;
                    cmd.Parameters.Add("@datum", SqlDbType.Date).Value = datum;
                    //    cmd.Parameters.Add("@prodato", SqlDbType.Bit).Value = chbProdato.IsChecked;

                    if (this.azuriraj)
                    {
                        DataRowView red = this.pomocniRed;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                        cmd.CommandText = @"update tbl_Prodaja set VoziloID = @voziloID,KupacID = @kupacID,ZaposleniID = @zaposleniID,NacinPlacanjaID = @nacinPlacanjaID,
                        Datum = @datum,BrojRata =@brojRata where VoziloID=@id";
                        this.pomocniRed = null;
                    }
                    else
                        cmd.CommandText = @"INSERT INTO tbl_Prodaja(VoziloID,KupacID,ZaposleniID,NacinPlacanjaID,Datum,BrojRata) 
                            VALUES (@voziloID, @kupacID,@zaposleniID, @nacinPlacanjaID, @datum, @brojRata);
                            UPDATE tbl_Vozilo set Prodato = 1 where VoziloID = @voziloID;";

                    cmd.ExecuteNonQuery();
                }


                this.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Unos nije uspešan!\n" + ex.Message, "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Nepravilni podaci!\n" + ex.Message, "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                konekcija?.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
