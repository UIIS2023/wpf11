using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace AutoPlac.Forms
{
    /// <summary>
    /// Interaction logic for FrmNabavka.xaml
    /// </summary>
    public partial class FrmNabavka : Window
    {
        Konekcija con = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        DataRowView pomocniRed;
        bool azuriraj;
        public FrmNabavka()
        {
            InitializeComponent();
            konekcija = con.KreirajKonekciju();
            PopuniPadajuceListe();
            txtCenaNabavke.Focus();
        }
        public FrmNabavka(bool azuriraj, DataRowView red) : this()
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
                string vratiVozilo= @"select VoziloID, BrojSasije from tbl_Vozilo";
                FillComboBox(cbVozilo, vratiVozilo);

                string vratiDobavljaca = @"select DobavljacID, Naziv from tbl_Dobavljac";
                FillComboBox(cbDobavljac, vratiDobavljaca);

                string vratiZaposlenog = @"select ZaposleniID, Ime from tbl_Zaposleni";
                FillComboBox(cbZaposleni, vratiZaposlenog);


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

                    if (string.IsNullOrWhiteSpace(txtCenaNabavke.Text) || dpDatum.SelectedDate == null ||
                        cbVozilo.SelectedValue == null || cbDobavljac.SelectedValue == null || cbZaposleni.SelectedValue == null)
                    {
                        throw new FormatException("Polja ne smeju biti prazna!");
                    }

                    DateTime date = (DateTime)dpDatum.SelectedDate;
                    string datum = date.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add("@cenaNabavke", SqlDbType.Int).Value = int.Parse(txtCenaNabavke.Text);
                    cmd.Parameters.Add("@voziloID", SqlDbType.Int).Value = cbVozilo.SelectedValue;
                    cmd.Parameters.Add("@dobavljacID", SqlDbType.Int).Value = cbDobavljac.SelectedValue;
                    cmd.Parameters.Add("@zaposleniID", SqlDbType.Int).Value = cbZaposleni.SelectedValue;
                    cmd.Parameters.Add("@datum", SqlDbType.DateTime).Value = datum;

                    if (this.azuriraj)
                    {
                        DataRowView red = this.pomocniRed;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                        cmd.CommandText = @"update tbl_Nabavka set VoziloID = @voziloID,ZaposleniID = @zaposleniID,DobavljacID = @dobavljacID,Datum = @datum,Cena =@cenaNabavke where NabavkaID=@id";
                        this.pomocniRed = null;
                    }
                    else
                        cmd.CommandText = @"INSERT INTO tbl_Nabavka(VoziloID,DobavljacID,ZaposleniID,Cena,Datum) 
                            VALUES (@voziloID, @dobavljacID,@zaposleniID, @cenaNabavke, @datum)";

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
