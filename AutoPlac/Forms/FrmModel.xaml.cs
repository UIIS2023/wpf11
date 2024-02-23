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
using System.Threading;

namespace AutoPlac.Forms
{
    /// <summary>
    /// Interaction logic for FrmModel.xaml
    /// </summary>
    public partial class FrmModel : Window
    {
        Konekcija con = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmModel()
        {
            InitializeComponent();
            konekcija = con.KreirajKonekciju();
            PopuniPadajuceListe();
            txtNazivModela.Focus();
        }
        public FrmModel(bool azuriraj, DataRowView red) : this()
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
                string vratiProizvodjac = @"select ProizvodjacID, Naziv from tbl_Proizvodjac";
                FillComboBox(cbProizvodjac, vratiProizvodjac);
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

                    if (cbProizvodjac.SelectedValue == null || string.IsNullOrWhiteSpace(txtNazivModela.Text))
                        throw new FormatException("Polja ne smeju biti prazna!");


                        cmd.Parameters.Add("@nazivModela", SqlDbType.NVarChar).Value = txtNazivModela.Text;
                     cmd.Parameters.Add("@proizvodjacID", SqlDbType.Int).Value = cbProizvodjac.SelectedValue;

                    if (this.azuriraj)
                    {
                        DataRowView red = this.pomocniRed;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                        cmd.CommandText = @"update tbl_Model set Naziv=@nazivModela, ProizvodjacID=@proizvodjacID where ModelID=@id";
                        this.pomocniRed = null;
                    }
                    else
                        cmd.CommandText = @"INSERT INTO tbl_Model(Naziv,ProizvodjacID) VALUES (@nazivModela,@proizvodjacID)";

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
