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
    /// Interaction logic for FrmKategorija.xaml
    /// </summary>
    public partial class FrmKategorija : Window
    {
        bool azuriraj;
        DataRowView pomocniRed;
        Konekcija con = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        public FrmKategorija()
        {
            InitializeComponent();
            konekcija = con.KreirajKonekciju();
            txtNazivKategorije.Focus();
        }
        public FrmKategorija(bool azuriraj, DataRowView red) : this()
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

                    if (string.IsNullOrWhiteSpace(txtNazivKategorije.Text))
                    {
                        throw new FormatException("Naziv kategorije ne sme biti prazan!");
                    }
                    cmd.Parameters.Add("@nazivKategorije", SqlDbType.NVarChar).Value = txtNazivKategorije.Text;

                    if (this.azuriraj)
                    {
                        DataRowView red = this.pomocniRed;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                        cmd.CommandText = @"update tbl_Kategorija set Naziv=@nazivKategorije where KategorijaID=@id";
                        this.pomocniRed = null;
                    }
                    else
                        cmd.CommandText = @"INSERT INTO tbl_Kategorija(Naziv) VALUES (@nazivKategorije)";

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
