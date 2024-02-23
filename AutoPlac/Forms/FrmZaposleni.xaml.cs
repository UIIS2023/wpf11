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
    /// Interaction logic for FrmZaposleni.xaml
    /// </summary>
    public partial class FrmZaposleni : Window
    {
        bool azuriraj;
        DataRowView pomocniRed;
        Konekcija con = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        public FrmZaposleni()
        {
            InitializeComponent();
            konekcija = con.KreirajKonekciju();
            txtIme.Focus();
        }
        public FrmZaposleni(bool azuriraj, DataRowView red) : this()
        {
            this.azuriraj = azuriraj;
            this.pomocniRed = red;
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = konekcija;
                    if (string.IsNullOrWhiteSpace(txtIme.Text) || string.IsNullOrWhiteSpace(txtPrezime.Text) || string.IsNullOrWhiteSpace(txtJMBG.Text) || 
                        string.IsNullOrWhiteSpace(txtBrojRacuna.Text) || string.IsNullOrWhiteSpace(txtKontakt.Text) || string.IsNullOrWhiteSpace(txtAdresa.Text) ||
                        string.IsNullOrWhiteSpace(txtBrojRacuna.Text) || string.IsNullOrWhiteSpace(txtRadnoMesto.Text) ||dpDatumZaposljavanja.SelectedDate == null || 
                        string.IsNullOrWhiteSpace(txtPlata.Text))
                    {
                        throw new FormatException("Polja ne smeju biti prazna!");
                    }
                    DateTime date = (DateTime)dpDatumZaposljavanja.SelectedDate;
                    string datum = date.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add("@ime", SqlDbType.NVarChar).Value = txtIme.Text;
                    cmd.Parameters.Add("@prezime", SqlDbType.NVarChar).Value = txtPrezime.Text;
                    cmd.Parameters.Add("@jmbg", SqlDbType.NVarChar).Value = txtJMBG.Text;
                    cmd.Parameters.Add("@kontakt", SqlDbType.NVarChar).Value = txtKontakt.Text;
                    cmd.Parameters.Add("@adresa", SqlDbType.NVarChar).Value = txtAdresa.Text;
                    cmd.Parameters.Add("@brojRacuna", SqlDbType.NVarChar).Value = txtBrojRacuna.Text;
                    cmd.Parameters.Add("@radnoMesto", SqlDbType.NVarChar).Value = txtRadnoMesto.Text;
                    cmd.Parameters.Add("@datumZaposljavanja", SqlDbType.Date).Value = datum;
                    cmd.Parameters.Add("@plata", SqlDbType.Int).Value = int.Parse(txtPlata.Text);
                    if (this.azuriraj)
                    {
                        DataRowView red = this.pomocniRed;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                        cmd.CommandText = @"update tbl_Zaposleni set Ime=@ime,Prezime=@prezime,JMBG=@jmbg,Kontakt=@kontakt,Adresa=@adresa,
                                            BrojRacuna=@brojRacuna, DatumZaposljavanja=@datumZaposljavanja, RadnoMesto=@radnoMesto, Plata=@plata where ZaposleniID=@id";
                        this.pomocniRed = null;
                    }
                    else
                        cmd.CommandText = @"INSERT INTO tbl_Zaposleni(Ime,Prezime,JMBG,Kontakt,Adresa,BrojRacuna,DatumZaposljavanja,
                                            RadnoMesto,Plata) VALUES (@ime,@prezime,@jmbg,@kontakt,@adresa,@brojRacuna, @datumZaposljavanja,@radnoMesto,@plata)";
                    cmd.ExecuteNonQuery();}
                this.Close();
            }
            catch (SqlException ex){ MessageBox.Show("Unos nije uspešan!\n" + ex.Message, "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);}
            catch (FormatException ex){ MessageBox.Show("Nepravilni podaci!\n" + ex.Message, "Greška!", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { konekcija?.Close();}
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
