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
    /// Interaction logic for FrmAuto_Oprema.xaml
    /// </summary>
    public partial class FrmAuto_Oprema : Window
    {
        Konekcija con = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        DataRowView pomocniRed;
        bool azuriraj;
        public FrmAuto_Oprema()
        {
            InitializeComponent();
            konekcija = con.KreirajKonekciju();
            PopuniPadajuceListe();
        }
        public FrmAuto_Oprema(bool azuriraj, DataRowView red) : this()
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
                string vratiVozilo = @"select VoziloID, BrojSasije from tbl_Vozilo";
                FillComboBox(cbBrojSasije, vratiVozilo);

                string vratiOpremu = @"select OpremaID, Naziv from tbl_Oprema";
                FillComboBox(cbOprema, vratiOpremu);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Padajuce liste nisu popunjene.\n" + ex.Message, "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
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

                    if (cbBrojSasije.SelectedValue == null || cbOprema.SelectedValue == null)
                    {
                        throw new FormatException("Morate izabrati vrednosti za vozilo i opremu!");
                    }

                    cmd.Parameters.Add("@voziloID", SqlDbType.Int).Value = cbBrojSasije.SelectedValue;
                    cmd.Parameters.Add("@opremaID", SqlDbType.Int).Value = cbOprema.SelectedValue;

                    if (this.azuriraj)
                    {
                        DataRowView red = this.pomocniRed;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                        cmd.CommandText = @"update tbl_Auto_Oprema set VoziloID=@voziloID, OpremaID=@opremaID where Auto_OpremaID=@id";
                        this.pomocniRed = null;
                    }
                    else
                        cmd.CommandText = @"INSERT INTO tbl_Auto_Oprema(VoziloID, OpremaID)
                            VALUES (@voziloID,@opremaID)";

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
