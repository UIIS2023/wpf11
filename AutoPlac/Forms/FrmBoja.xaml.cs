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
    /// Interaction logic for FrmBoja.xaml
    /// </summary>
    public partial class FrmBoja : Window
    {
        bool azuriraj;
        DataRowView pomocniRed;
        Konekcija con = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        public FrmBoja()
        {
            InitializeComponent();
            konekcija = con.KreirajKonekciju();
            txtNazivBoje.Focus();
        }
        public FrmBoja(bool azuriraj, DataRowView pomocniRed) : this()
        {
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = konekcija;

                    if (string.IsNullOrWhiteSpace(txtNazivBoje.Text))
                    {
                        throw new FormatException("Naziv ne sme biti prazan!");
                    }

                    cmd.Parameters.Add("@nazivBoje", SqlDbType.NVarChar).Value = txtNazivBoje.Text;
                    cmd.Parameters.Add("@opisBoje", SqlDbType.NVarChar).Value = txtOpisBoje.Text;

                    if (this.azuriraj)
                    {
                        DataRowView red = this.pomocniRed;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                        cmd.CommandText = @"update tbl_Boja set Naziv=@nazivBoje, Opis=@opisBoje where BojaID=@id";
                        this.pomocniRed = null;
                    }
                    else
                        cmd.CommandText = @"INSERT INTO tbl_Boja(Naziv,Opis) VALUES (@nazivBoje,@opisBoje)";

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
