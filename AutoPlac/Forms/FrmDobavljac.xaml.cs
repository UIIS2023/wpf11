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
    /// Interaction logic for FrmDobavljac.xaml
    /// </summary>
    public partial class FrmDobavljac : Window
    {
        bool azuriraj;
        DataRowView pomocniRed;
        Konekcija con = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        public FrmDobavljac()
        {
            InitializeComponent();
            konekcija = con.KreirajKonekciju();
            txtNaziv.Focus();
        }
        public FrmDobavljac(bool azuriraj, DataRowView red) : this()
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

                    if (string.IsNullOrWhiteSpace(txtNaziv.Text) || string.IsNullOrWhiteSpace(txtKontakt.Text) ||
                        string.IsNullOrWhiteSpace(txtKontakt.Text) || string.IsNullOrWhiteSpace(txtBrojRacuna.Text))
                    {
                        throw new FormatException("Polja ne smeju biti prazna!");
                    }
                    cmd.Parameters.Add("@naziv", SqlDbType.NVarChar).Value = txtNaziv.Text;
                    cmd.Parameters.Add("@kontakt", SqlDbType.NVarChar).Value = txtKontakt.Text;
                    cmd.Parameters.Add("@adresa", SqlDbType.NVarChar).Value = txtAdresa.Text;
                    cmd.Parameters.Add("@brojRacuna", SqlDbType.NVarChar).Value = txtBrojRacuna.Text;

                    if (this.azuriraj)
                    {
                        DataRowView red = this.pomocniRed;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                        cmd.CommandText = @"update tbl_Dobavljac set Naziv=@naziv,Kontakt=@kontakt,Adresa=@adresa,
                                            BrojRacuna=@brojRacuna where DobavljacID=@id";
                        this.pomocniRed = null;
                    }
                    else
                        cmd.CommandText = @"INSERT INTO tbl_Dobavljac(Naziv,Kontakt,Adresa,BrojRacuna) VALUES (@naziv,@kontakt,@adresa,@brojRacuna)";

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
