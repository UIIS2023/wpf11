using AutoPlac.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoPlac
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string ucitanaTabela;
        bool azuriraj;
        private Konekcija con = new Konekcija();
        private SqlConnection konekcija = new SqlConnection();

        #region select upiti
        static string placanjaSelect = @"select NacinPlacanjaID as ID, Naziv, Opis from tbl_NacinPlacanja";
        static string gorivoSelect = @"select GorivoID as ID, Naziv, Opis from tbl_Gorivo";
        static string bojaSelect = @"select BojaID as ID, Naziv, Opis from tbl_Boja";
        static string kupacSelect = @"select KupacID as ID, Ime, Prezime,JMBG,Kontakt,Adresa, BrojRacuna as 'Broj racuna' from tbl_Kupac";
        static string dobavljacSelect = @"select DobavljacID as ID, Naziv, Kontakt,Adresa,BrojRacuna as 'Broj racuna' from tbl_Dobavljac";
        static string kategorijaSelect = @"select KategorijaID as ID, Naziv from tbl_Kategorija";
        static string proizvodjacSelect = @"select ProizvodjacID as ID, Naziv from tbl_Proizvodjac";
        static string opremaSelect = @"select OpremaID as ID, Naziv, Opis from tbl_Oprema";
        static string zaposleniSelect = @"select ZaposleniID as ID, Ime, Prezime,DatumZaposljavanja as 'Datum zaposljavanja',Plata,Kontakt,BrojRacuna as 'Broj racuna',
                                         RadnoMesto as 'Radno mesto',JMBG,Adresa from tbl_Zaposleni";
        static string nabavkaSelect = @"select tbl_Nabavka.NabavkaID as ID,tbl_Nabavka.Datum,tbl_Zaposleni.Ime as Zaposleni, tbl_Dobavljac.Naziv as Dobavljac,
                                            tbl_Model.Naziv as Model, tbl_Proizvodjac.Naziv as Proizvodjac, tbl_Nabavka.Cena
                                            from tbl_Nabavka
                                            join tbl_Dobavljac on tbl_Nabavka.DobavljacID = tbl_Dobavljac.DobavljacID
                                            join tbl_Zaposleni on tbl_Nabavka.ZaposleniID = tbl_Zaposleni.ZaposleniID
                                            join tbl_Vozilo on tbl_Nabavka.VoziloID = tbl_Vozilo.VoziloID
                                            join tbl_Model on tbl_Vozilo.ModelID = tbl_Model.ModelID
                                            join tbl_Proizvodjac on tbl_Model.ProizvodjacID = tbl_Proizvodjac.ProizvodjacID;";
        static string prodajaSelect = @"select tbl_Prodaja.ProdajaID as ID,tbl_Prodaja.Datum ,tbl_Zaposleni.Ime as Zaposleni, tbl_Kupac.Ime as Kupac,
                            tbl_Model.Naziv as Model, tbl_Proizvodjac.Naziv as Proizvodjac, tbl_Vozilo.Cena , tbl_NacinPlacanja.Naziv as NacinPlacanja, BrojRata
                                            from tbl_Prodaja
                                            join tbl_Kupac on tbl_Prodaja.KupacID = tbl_Kupac.KupacID
                                            join tbl_Zaposleni on tbl_Prodaja.ZaposleniID = tbl_Zaposleni.ZaposleniID
                                            join tbl_Vozilo on tbl_Prodaja.VoziloID = tbl_Vozilo.VoziloID
                                            join tbl_Model on tbl_Vozilo.ModelID = tbl_Model.ModelID
                                            join tbl_Proizvodjac on tbl_Model.ProizvodjacID = tbl_Proizvodjac.ProizvodjacID
                                            join tbl_NacinPlacanja on tbl_Prodaja.NacinPlacanjaID = tbl_NacinPlacanja.NacinPlacanjaID";
        static string auto_opremaSelect = @"select Auto_OpremaID as ID,tbl_Model.Naziv as Model, tbl_Proizvodjac.Naziv as Proizvodjac, 
                                            tbl_Vozilo.BrojSasije as BrojSasije, tbl_Oprema.Naziv as Oprema 
                                            from tbl_Auto_Oprema
                                            join tbl_Vozilo on tbl_Auto_Oprema.VoziloID = tbl_Vozilo.VoziloID
                                            join tbl_Model on tbl_Vozilo.ModelID = tbl_Model.ModelID
                                            join tbl_Proizvodjac on tbl_Model.ProizvodjacID = tbl_Proizvodjac.ProizvodjacID
                                            join tbl_Oprema on tbl_Auto_Oprema.OpremaID = tbl_Oprema.OpremaID;";
        static string voziloSelect = @"SELECT tbl_Vozilo.VoziloID as ID,tbl_Vozilo.GodinaProizvodnje as 'Godina proizvodnje',BrojSasije as 'Broj sasije',
        tbl_Vozilo.PredjenoKM as 'Kilometraza',tbl_Proizvodjac.Naziv as Proizvodjac,tbl_Model.Naziv AS Model,tbl_Boja.Naziv AS Boja,tbl_Gorivo.Naziv AS Gorivo, Prodato,
                                            tbl_Vozilo.Cena,
                                            tbl_Kategorija.Naziv AS Kategorija
                                            FROM tbl_Vozilo
                                            INNER JOIN tbl_Model ON tbl_Vozilo.ModelID = tbl_Model.ModelID
                                            INNER JOIN tbl_Boja ON tbl_Vozilo.BojaID = tbl_Boja.BojaID
                                            INNER JOIN tbl_Gorivo ON tbl_Vozilo.GorivoID = tbl_Gorivo.GorivoID
                                            INNER JOIN tbl_Kategorija ON tbl_Vozilo.KategorijaID = tbl_Kategorija.KategorijaID
                                            INNER JOIN tbl_Proizvodjac ON tbl_Model.ProizvodjacID = tbl_Proizvodjac.ProizvodjacID";
        static string modelSelect = @"SELECT tbl_Model.ModelID as ID,tbl_Model.Naziv AS Model,tbl_Proizvodjac.Naziv AS Proizvodjac
                                            FROM tbl_Model
                                            INNER JOIN tbl_Proizvodjac ON tbl_Model.ProizvodjacID = tbl_Proizvodjac.ProizvodjacID;";

        #endregion

        #region select sa USLOVOM
        string selectUslovVozila = @"SELECT * FROM tbl_Vozilo 
                                            INNER JOIN tbl_Model ON tbl_Vozilo.ModelID = tbl_Model.ModelID 
                                            INNER JOIN tbl_Proizvodjac ON tbl_Model.ProizvodjacID = tbl_Proizvodjac.ProizvodjacID
										    where VoziloID=";
        string selectUslovGorivo = @"select * from tbl_Gorivo where GorivoID=";
        string selectUslovPlacanje = @"select * from tbl_NacinPlacanja where NacinPlacanjaID=";
        string selectUslovBoja = @"select * from tbl_Boja where BojaID=";
        string selectUslovKategorija = @"select * from tbl_Kategorija where KategorijaID=";
        string selectUslovProizvodjac = @"select * from tbl_Proizvodjac where ProizvodjacID=";
        string selectUslovOprema = @"select * from tbl_Oprema where OpremaID=";
        string selectUslovModel = @"select * from tbl_Model where ModelID=";
        string selectUslovAuto_Oprema = @"select * from tbl_Auto_Oprema where Auto_OpremaID=";
        string selectUslovKupac = @"select * from tbl_Kupac where KupacID=";
        string selectUslovDobavljac = @"select * from tbl_Dobavljac  where DobavljacID=";
        string selectUslovZaposleni = @"select * from tbl_Zaposleni  where ZaposleniID=";
        string selectUslovNabavka = @"select * from tbl_Nabavka  where NabavkaID=";
        string selectUslovProdaja = @"select * from tbl_Prodaja  where ProdajaID=";

        #endregion

        #region delete upiti
        static string deleteNacinPlacanja = "DELETE FROM tbl_NacinPlacanja WHERE NacinPlacanjaID=";
        static string deleteGorivo = "DELETE FROM tbl_Gorivo WHERE GorivoID=";
        static string deleteBoja = "DELETE FROM tbl_Boja WHERE BojaID=";
        static string deleteKupac = "DELETE FROM tbl_Kupac WHERE KupacID=";
        static string deleteDobavljac = "DELETE FROM tbl_Dobavljac WHERE DobavljacID=";
        static string deleteKategorija = "DELETE FROM tbl_Kategorija WHERE KategorijaID=";
        static string deleteProizvodjac = "DELETE FROM tbl_Proizvodjac WHERE ProizvodjacID=";
        static string deleteOprema = "DELETE FROM tbl_Oprema WHERE OpremaID=";
        static string deleteZaposleni = "DELETE FROM tbl_Zaposleni WHERE ZaposleniID=";
        static string deleteNabavka = "DELETE FROM tbl_Nabavka WHERE NabavkaID=";
        static string deleteProdaja = "DELETE FROM tbl_Prodaja WHERE ProdajaID=";
        static string deleteAutoOprema = "DELETE FROM tbl_Auto_Oprema WHERE Auto_OpremaID=";
        static string deleteVozilo = "DELETE FROM tbl_Vozilo WHERE VoziloID=";
        static string deleteModel = "DELETE FROM tbl_Model WHERE ModelID=";

        #endregion
        public MainWindow()
        {
            InitializeComponent();
            konekcija = con.KreirajKonekciju();
            UcitajPodatke(dataGridCentralni, voziloSelect);
            lblTabela.Content = "Vozila";
        }
        private void UcitajPodatke(DataGrid grid, string selectUpit)
        {
            try
            {
                konekcija.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectUpit, konekcija);
                DataTable dt = new DataTable();
                sqlDataAdapter.Fill(dt);
                if (grid != null)
                {
                    grid.ItemsSource = dt.DefaultView;
                    ApplyDateFormatting(grid, "Datum");
                    ApplyDateFormatting(grid, "Datum zaposljavanja");
                }
                ucitanaTabela = selectUpit;
                dt.Dispose();
                sqlDataAdapter.Dispose();
            }
            catch (SqlException)
            {
                MessageBox.Show("Neuspesno ucitani podaci", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                    konekcija?.Close();
            }
        }
        private void ApplyDateFormatting(DataGrid grid, string columnDate)
        {
            var datumColumn = grid.Columns.FirstOrDefault(column => (string)column.Header == columnDate);

            if (datumColumn != null)
            {
                // Format the date column
                if (datumColumn is DataGridTextColumn textColumn)
                {
                    textColumn.Binding.StringFormat = "yyyy-MM-dd";
                }
            }
        }
        private void btnVozila_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, voziloSelect);
            lblTabela.Content = "Vozila";
        }

        private void btnOprema_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, opremaSelect);
            lblTabela.Content = "Oprema";
        }

        private void btnAuto_Oprema_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, auto_opremaSelect);
            lblTabela.Content = "Auto oprema";
        }

        private void btnProdaja_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, prodajaSelect);
            lblTabela.Content = "Prodaja";
        }

        private void btnNacinPlacanja_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, placanjaSelect);
            lblTabela.Content = "Nacini placanja";
        }

        private void btnNabavka_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, nabavkaSelect);
            lblTabela.Content = "Nabavka";
        }

        private void btnDobavljac_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, dobavljacSelect);
            lblTabela.Content = "Dobavljaci";
        }

        private void btnZaposleni_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, zaposleniSelect);
            lblTabela.Content = "Zaposleni";
        }

        private void btnKupci_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, kupacSelect);
            lblTabela.Content = "Kupci";
        }

        private void btnProizvodjac_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, proizvodjacSelect);
            lblTabela.Content = "Proizvodjaci";
        }

        private void btnModeli_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, modelSelect);
            lblTabela.Content = "Modeli";
        }

        private void btnKategorija_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, kategorijaSelect);
            lblTabela.Content = "Kategorije";
        }

        private void btnGorivo_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, gorivoSelect);
            lblTabela.Content = "Goriva";
        }

        private void btnBoja_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, bojaSelect);
            lblTabela.Content = "Boje";
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window prozor;
            if (ucitanaTabela.Equals(voziloSelect))
            {
                prozor = new FrmVozilo();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, voziloSelect);
            }
            else if (ucitanaTabela.Equals(proizvodjacSelect))
            {
                prozor = new FrmProizvodjac();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, proizvodjacSelect);
            }
            else if (ucitanaTabela.Equals(modelSelect))
            {
                prozor = new FrmModel();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, modelSelect);
            }
            else if (ucitanaTabela.Equals(opremaSelect))
            {
                prozor = new FrmOprema();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, opremaSelect);
            }
            else if (ucitanaTabela.Equals(placanjaSelect))
            {
                prozor = new FrmNacinPlacanja();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, placanjaSelect);
            }
            else if (ucitanaTabela.Equals(bojaSelect))
            {
                prozor = new FrmBoja();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, bojaSelect);
            }
            else if (ucitanaTabela.Equals(kategorijaSelect))
            {
                prozor = new FrmKategorija();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, kategorijaSelect);
            }
            else if (ucitanaTabela.Equals(gorivoSelect))
            {
                prozor = new FrmGorivo();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, gorivoSelect);
            }
            else if (ucitanaTabela.Equals(auto_opremaSelect))
            {
                prozor = new FrmAuto_Oprema();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, auto_opremaSelect);
            }
            else if (ucitanaTabela.Equals(kupacSelect))
            {
                prozor = new FrmKupac();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, kupacSelect);
            }
            else if (ucitanaTabela.Equals(dobavljacSelect))
            {
                prozor = new FrmDobavljac();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, dobavljacSelect);
            }
            else if (ucitanaTabela.Equals(prodajaSelect))
            {
                prozor = new FrmProdaja();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, prodajaSelect);
            }
            else if (ucitanaTabela.Equals(nabavkaSelect))
            {
                prozor = new FrmNabavka();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, nabavkaSelect);
            }
            else if (ucitanaTabela.Equals(zaposleniSelect))
            {
                prozor = new FrmZaposleni();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, zaposleniSelect);
            }

        }

        void PopuniFormu(DataGrid grid, string selectUslov)
        {
            try
            {
                konekcija.Open();
                azuriraj = true;
                DataRowView red = (DataRowView)grid.SelectedItems[0];
                SqlCommand cmd = new SqlCommand { Connection = konekcija };
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                cmd.CommandText = selectUslov + "@id";
                SqlDataReader citac = cmd.ExecuteReader();
                cmd.Dispose();
                if (citac.Read())
                {
                    if (ucitanaTabela.Equals(voziloSelect))
                    {
                        FrmVozilo prozorVozilo = new FrmVozilo(azuriraj, red);
                        prozorVozilo.txtBrojSasije.Text = citac["BrojSasije"].ToString();
                        prozorVozilo.txtPredjenoKM.Text = citac["PredjenoKM"].ToString();
                        prozorVozilo.txtGodinaProizvodnje.Text = citac["GodinaProizvodnje"].ToString();
                        prozorVozilo.txtCena.Text = citac["Cena"].ToString();
                        prozorVozilo.cbBoja.SelectedValue = citac["BojaID"].ToString();
                        prozorVozilo.cbGorivo.SelectedValue = citac["GorivoID"].ToString();
                        prozorVozilo.cbKategorija.SelectedValue = citac["KategorijaID"].ToString();
                        prozorVozilo.cbProizvodjac.SelectedValue = citac["ProizvodjacID"].ToString();
                        prozorVozilo.chbProdato.IsChecked = (bool)citac["Prodato"];
                        prozorVozilo.cbModel.SelectedValue = citac["ModelID"].ToString();
                        prozorVozilo.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(proizvodjacSelect))
                    {
                        FrmProizvodjac prozorMarka = new FrmProizvodjac(azuriraj, red);
                        prozorMarka.txtNazivProizvodjaca.Text = citac["Naziv"].ToString();
                        prozorMarka.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(modelSelect))
                    {
                        FrmModel prozorModel = new FrmModel(azuriraj, red);
                        prozorModel.txtNazivModela.Text = citac["Naziv"].ToString();
                        prozorModel.cbProizvodjac.SelectedValue = citac["ProizvodjacID"].ToString();
                        prozorModel.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(opremaSelect))
                    {
                        FrmOprema prozorOprema = new FrmOprema(azuriraj, red);
                        prozorOprema.txtNazivOpreme.Text = citac["Naziv"].ToString();
                        prozorOprema.txtOpisOpreme.Text = citac["Opis"].ToString();
                        prozorOprema.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(placanjaSelect))
                    {
                        FrmNacinPlacanja prozorNacinPlacanja = new FrmNacinPlacanja(azuriraj, red);
                        prozorNacinPlacanja.txtNazivPlacanja.Text = citac["Naziv"].ToString();
                        prozorNacinPlacanja.txtOpisPlacanja.Text = citac["Opis"].ToString();
                        prozorNacinPlacanja.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(bojaSelect))
                    {
                        FrmBoja prozorBoja = new FrmBoja(azuriraj, red);
                        prozorBoja.txtNazivBoje.Text = citac["Naziv"].ToString();
                        prozorBoja.txtOpisBoje.Text = citac["Opis"].ToString();
                        prozorBoja.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(kategorijaSelect))
                    {
                        FrmKategorija prozorKategorija = new FrmKategorija(azuriraj, red);
                        prozorKategorija.txtNazivKategorije.Text = citac["Naziv"].ToString();
                        prozorKategorija.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(gorivoSelect))
                    {
                        FrmGorivo prozorGorivo = new FrmGorivo(azuriraj, red);
                        prozorGorivo.txtNazivGoriva.Text = citac["Naziv"].ToString();
                        prozorGorivo.txtOpisGoriva.Text = citac["Opis"].ToString();

                        prozorGorivo.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(auto_opremaSelect))
                    {
                        FrmAuto_Oprema prozorAuto_Oprema = new FrmAuto_Oprema(azuriraj, red);
                        prozorAuto_Oprema.cbBrojSasije.SelectedValue = citac["VoziloID"];
                        prozorAuto_Oprema.cbOprema.SelectedValue = citac["OpremaID"];

                        prozorAuto_Oprema.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(kupacSelect))
                    {
                        FrmKupac prozorKupac = new FrmKupac(azuriraj, red);
                        prozorKupac.txtIme.Text = citac["Ime"].ToString();
                        prozorKupac.txtPrezime.Text = citac["Prezime"].ToString();
                        prozorKupac.txtJMBG.Text = citac["JMBG"].ToString();
                        prozorKupac.txtKontakt.Text = citac["Kontakt"].ToString();
                        prozorKupac.txtAdresa.Text = citac["Adresa"].ToString();
                        prozorKupac.txtBrojRacuna.Text = citac["BrojRacuna"].ToString();

                        prozorKupac.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(dobavljacSelect))
                    {
                        FrmDobavljac prozorDobavljac = new FrmDobavljac(azuriraj, red);
                        prozorDobavljac.txtNaziv.Text = citac["Naziv"].ToString();
                        prozorDobavljac.txtKontakt.Text = citac["Kontakt"].ToString();
                        prozorDobavljac.txtAdresa.Text = citac["Adresa"].ToString();
                        prozorDobavljac.txtBrojRacuna.Text = citac["BrojRacuna"].ToString();

                        prozorDobavljac.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(prodajaSelect))
                    {
                        FrmProdaja prozorProdaja = new FrmProdaja(azuriraj, red);
                        prozorProdaja.txtBrojRata.Text = citac["BrojRata"].ToString();
                        prozorProdaja.dpDatum.SelectedDate = (DateTime)citac["Datum"];
                        prozorProdaja.cbVozilo.SelectedValue = citac["VoziloID"].ToString();
                        prozorProdaja.cbKupac.SelectedValue = citac["KupacID"].ToString();
                        prozorProdaja.cbZaposleni.SelectedValue = citac["ZaposleniID"].ToString();
                        prozorProdaja.cbNacinPlacanja.SelectedValue = citac["NacinPlacanjaID"].ToString();

                        prozorProdaja.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(nabavkaSelect))
                    {
                        FrmNabavka prozorNabavka = new FrmNabavka(azuriraj, red);
                        prozorNabavka.txtCenaNabavke.Text = citac["Cena"].ToString();
                        prozorNabavka.dpDatum.SelectedDate = (DateTime)citac["Datum"];
                        prozorNabavka.cbVozilo.SelectedValue = citac["VoziloID"].ToString();
                        prozorNabavka.cbDobavljac.SelectedValue = citac["DobavljacID"].ToString();
                        prozorNabavka.cbZaposleni.SelectedValue = citac["ZaposleniID"].ToString();
                        prozorNabavka.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(zaposleniSelect))
                    {
                        FrmZaposleni prozorZaposleni = new FrmZaposleni(azuriraj, red);
                        prozorZaposleni.txtIme.Text = citac["Ime"].ToString();
                        prozorZaposleni.txtPrezime.Text = citac["Prezime"].ToString();
                        prozorZaposleni.txtJMBG.Text = citac["JMBG"].ToString();
                        prozorZaposleni.txtKontakt.Text = citac["Kontakt"].ToString();
                        prozorZaposleni.txtAdresa.Text = citac["Adresa"].ToString();
                        prozorZaposleni.txtBrojRacuna.Text = citac["BrojRacuna"].ToString();
                        prozorZaposleni.txtRadnoMesto.Text = citac["RadnoMesto"].ToString();
                        prozorZaposleni.txtPlata.Text = citac["Plata"].ToString();
                        prozorZaposleni.dpDatumZaposljavanja.SelectedDate = (DateTime)citac["DatumZaposljavanja"];
                        prozorZaposleni.ShowDialog();

                    }


                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Nije selektovan red!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
                this.azuriraj = false;
            }
        }
        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(voziloSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovVozila);
                UcitajPodatke(dataGridCentralni, voziloSelect);
            }
            else if (ucitanaTabela.Equals(proizvodjacSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovProizvodjac);
                UcitajPodatke(dataGridCentralni, proizvodjacSelect);
            }
            else if (ucitanaTabela.Equals(modelSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovModel);
                UcitajPodatke(dataGridCentralni, modelSelect);
            }
            else if (ucitanaTabela.Equals(opremaSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovOprema);
                UcitajPodatke(dataGridCentralni, opremaSelect);
            }
            else if (ucitanaTabela.Equals(placanjaSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovPlacanje);
                UcitajPodatke(dataGridCentralni, placanjaSelect);
            }
            else if (ucitanaTabela.Equals(bojaSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovBoja);
                UcitajPodatke(dataGridCentralni, bojaSelect);
            }
            else if (ucitanaTabela.Equals(kategorijaSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovKategorija);
                UcitajPodatke(dataGridCentralni, kategorijaSelect);
            }
            else if (ucitanaTabela.Equals(gorivoSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovGorivo);
                UcitajPodatke(dataGridCentralni, gorivoSelect);
            }
            else if (ucitanaTabela.Equals(auto_opremaSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovAuto_Oprema);
                UcitajPodatke(dataGridCentralni, auto_opremaSelect);
            }
            else if (ucitanaTabela.Equals(kupacSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovKupac);
                UcitajPodatke(dataGridCentralni, kupacSelect);
            }
            else if (ucitanaTabela.Equals(dobavljacSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovDobavljac);
                UcitajPodatke(dataGridCentralni, dobavljacSelect);
            }
            else if (ucitanaTabela.Equals(prodajaSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovProdaja);
                UcitajPodatke(dataGridCentralni, prodajaSelect);
            }
            else if (ucitanaTabela.Equals(nabavkaSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovNabavka);
                UcitajPodatke(dataGridCentralni, nabavkaSelect);
            }
            else if (ucitanaTabela.Equals(zaposleniSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovZaposleni);
                UcitajPodatke(dataGridCentralni, zaposleniSelect);
            }
        }
        private void ObrisiZapis(DataGrid grid, string deleteSelect)
        {
            try
            {
                konekcija.Open();
                DataRowView red = (DataRowView)grid.SelectedItems[0];
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result.Equals(MessageBoxResult.Yes))
                {
                    SqlCommand cmd = new SqlCommand { Connection = konekcija };
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = deleteSelect + "@id";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Postoje povezani podaci u drugim tabelama!\n" + ex.Message, "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                konekcija?.Close();
            }
        }
        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(voziloSelect))
            {
                ObrisiZapis(dataGridCentralni, deleteVozilo);
                UcitajPodatke(dataGridCentralni, voziloSelect);
            }
            else if (ucitanaTabela.Equals(proizvodjacSelect))
            {
                ObrisiZapis(dataGridCentralni, deleteProizvodjac);
                UcitajPodatke(dataGridCentralni, proizvodjacSelect);
            }
            else if (ucitanaTabela.Equals(modelSelect))
            {
                ObrisiZapis(dataGridCentralni, deleteModel);
                UcitajPodatke(dataGridCentralni, modelSelect);
            }
            else if (ucitanaTabela.Equals(opremaSelect))
            {
                ObrisiZapis(dataGridCentralni, deleteOprema);
                UcitajPodatke(dataGridCentralni, opremaSelect);
            }
            else if (ucitanaTabela.Equals(placanjaSelect))
            {
                ObrisiZapis(dataGridCentralni, deleteNacinPlacanja);
                UcitajPodatke(dataGridCentralni, placanjaSelect);
            }
            else if (ucitanaTabela.Equals(bojaSelect))
            {
                ObrisiZapis(dataGridCentralni, deleteBoja);
                UcitajPodatke(dataGridCentralni, bojaSelect);
            }
            else if (ucitanaTabela.Equals(kategorijaSelect))
            {
                ObrisiZapis(dataGridCentralni, deleteKategorija);
                UcitajPodatke(dataGridCentralni, kategorijaSelect);
            }
            else if (ucitanaTabela.Equals(gorivoSelect))
            {
                ObrisiZapis(dataGridCentralni, deleteGorivo);
                UcitajPodatke(dataGridCentralni, gorivoSelect);
            }
            else if (ucitanaTabela.Equals(auto_opremaSelect))
            {
                ObrisiZapis(dataGridCentralni, deleteAutoOprema);
                UcitajPodatke(dataGridCentralni, auto_opremaSelect);
            }
            else if (ucitanaTabela.Equals(kupacSelect))
            {
                ObrisiZapis(dataGridCentralni, deleteKupac);
                UcitajPodatke(dataGridCentralni, kupacSelect);
            }
            else if (ucitanaTabela.Equals(dobavljacSelect))
            {
                ObrisiZapis(dataGridCentralni, deleteDobavljac);
                UcitajPodatke(dataGridCentralni, dobavljacSelect);
            }
            else if (ucitanaTabela.Equals(prodajaSelect))
            {
                ObrisiZapis(dataGridCentralni, deleteProdaja);
                UcitajPodatke(dataGridCentralni, dobavljacSelect);
            }
            else if (ucitanaTabela.Equals(nabavkaSelect))
            {
                ObrisiZapis(dataGridCentralni, deleteNabavka);
                UcitajPodatke(dataGridCentralni, nabavkaSelect);
            }
            else if (ucitanaTabela.Equals(zaposleniSelect))
            {
                ObrisiZapis(dataGridCentralni, deleteZaposleni);
                UcitajPodatke(dataGridCentralni, zaposleniSelect);
            }
        }

    }
}
