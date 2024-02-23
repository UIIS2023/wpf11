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
    /// Interaction logic for FrmVozilo.xaml
    /// </summary>
    public partial class FrmVozilo : Window
    {
        Konekcija con = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        DataRowView pomocniRed;
        bool azuriraj;
        public FrmVozilo()
        {
            InitializeComponent();
            konekcija = con.KreirajKonekciju();
            PopuniPadajuceListe();
            txtBrojSasije.Focus();
        }
        public FrmVozilo(bool azuriraj,DataRowView red) : this()
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
                string vratiModel = @"select ModelID, Naziv from tbl_Model";
                FillComboBox(cbModel, vratiModel);
                string vratiKategoriju = @"select KategorijaID, Naziv from tbl_Kategorija";
                FillComboBox(cbKategorija, vratiKategoriju);
                string vratiBoju = @"select BojaID, Naziv from tbl_Boja";
                FillComboBox(cbBoja, vratiBoju);
                string vratiGorivo = @"select GorivoID,Naziv from tbl_Gorivo";
                FillComboBox(cbGorivo, vratiGorivo);
            }
            catch (SqlException)
            {
                MessageBox.Show("Padajuce liste nisu popunjene", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                konekcija?.Close();
            }
        }

        private void cbProizvodjac_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
               if (cbProizvodjac.SelectedItem != null)
                {
                    int proizvodjacID = Convert.ToInt32(((DataRowView)cbProizvodjac.SelectedItem)["ProizvodjacID"]);
                    string vratiModel = @"select ModelID, Naziv from tbl_Model WHERE ProizvodjacID="+ proizvodjacID;
                    FillComboBox(cbModel, vratiModel);
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
                    if (string.IsNullOrWhiteSpace(txtBrojSasije.Text) || string.IsNullOrWhiteSpace(txtGodinaProizvodnje.Text) ||
                      string.IsNullOrWhiteSpace(txtCena.Text) || cbModel.SelectedValue == null ||
                      string.IsNullOrWhiteSpace(txtPredjenoKM.Text) ||cbKategorija.SelectedValue == null ||
                      cbBoja.SelectedValue == null || cbGorivo.SelectedValue == null)
                    {
                        throw new FormatException("Polja ne smeju biti prazna!");
                    }
                    cmd.Parameters.Add("@brojSasije", SqlDbType.NVarChar).Value = txtBrojSasije.Text;
                    cmd.Parameters.Add("@godina", SqlDbType.Int).Value = int.Parse(txtGodinaProizvodnje.Text);
                    cmd.Parameters.Add("@cena", SqlDbType.Money).Value = int.Parse(txtCena.Text);
                    cmd.Parameters.Add("@modelID", SqlDbType.Int).Value = cbModel.SelectedValue;
                    cmd.Parameters.Add("@predjenoKM", SqlDbType.Int).Value = int.Parse(txtPredjenoKM.Text);
                    cmd.Parameters.Add("@kategorijaID", SqlDbType.Int).Value = cbKategorija.SelectedValue;
                    cmd.Parameters.Add("@bojaID", SqlDbType.Int).Value = cbBoja.SelectedValue;
                    cmd.Parameters.Add("@gorivoID", SqlDbType.Int).Value = cbGorivo.SelectedValue;
                    cmd.Parameters.Add("@prodato", SqlDbType.Bit).Value = chbProdato.IsChecked;
                    if(this.azuriraj)
                    {
                        DataRowView red = this.pomocniRed;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                        cmd.CommandText = @"update tbl_Vozilo set BrojSasije=@brojSasije, GodinaProizvodnje = @godina, 
                        Cena = @cena,Prodato = @prodato, ModelID=@modelID, PredjenoKM = @predjenoKM, KategorijaID = @kategorijaID, BojaID = @bojaID,
                        GorivoID = @gorivoID where VoziloID=@id";
                        this.pomocniRed = null;
                    }
                    else
                        cmd.CommandText = @"INSERT INTO tbl_Vozilo(BrojSasije, GodinaProizvodnje, PredjenoKM, Cena,Prodato, ModelID, KategorijaID, BojaID, GorivoID) 
                            VALUES (@brojSasije, @godina,@predjenoKM,@cena,@prodato, @modelID, @kategorijaID, @bojaID, @gorivoID)";
                    cmd.ExecuteNonQuery();
                }
                this.Close();
            }
            catch (SqlException ex){ MessageBox.Show("Unos nije uspešan!\n" + ex.Message, "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);}
            catch (FormatException ex){ MessageBox.Show("Nepravilni podaci!\n" + ex.Message, "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);}
            finally {konekcija?.Close();}
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
