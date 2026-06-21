using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class Form3 : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString = "Data Source=MSI\\UNKNOWNMEMBER;Initial Catalog=DBAkademikADO; Integrated Security=True";
        ReportMahasiswa rptMhs = new ReportMahasiswa();
        private string prodi { get; set; }
        private DateTime tglmasuk { get; set; }
        public Form3(string Prodi, DateTime TglMasuk)
        {
            InitializeComponent();

            prodi = Prodi;
            tglmasuk = TglMasuk;

            conn = new SqlConnection(connectionString);

            LoadCrystalReport();
        }

        private void LoadCrystalReport()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand("sp_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@inProdi", prodi);
                cmd.Parameters.AddWithValue("@inTglMsuk", tglmasuk.Year);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dtMahasiswa = new DataTable();
                da.Fill(dtMahasiswa);

                conn.Close();

                List<ClassReport> listData = new List<ClassReport>();
                foreach (DataRow row in dtMahasiswa.Rows)
                {
                    listData.Add(new ClassReport
                    {
                        Nama = row["Nama"].ToString(),
                        JenisKelamin = row["JenisKelamin"].ToString(),
                        Alamat = row["Alamat"].ToString(),
                        NamaProdi = row["NamaProdi"].ToString(),
                        TanggalDaftar = Convert.ToDateTime(row["TanggalDaftar"])
                    });
                }

                rptMhs.SetDataSource(listData);

                crystalReportViewer1.ReportSource = rptMhs;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan report: " + ex.Message);
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}