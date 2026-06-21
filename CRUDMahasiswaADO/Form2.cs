using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CRUDMahasiswaADO
{
    public partial class Form2 : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString = "Data Source=MSI\\UNKNOWNMEMBER;Initial Catalog=DBAkademikADO; Integrated Security=True";
        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DataTable dtProdi;

        public Form2()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            dTP.Format = DateTimePickerFormat.Custom;
            dTP.CustomFormat = "yyyy";
            dTP.ShowUpDown = true;
            dTP.MinDate = new DateTime(2000, 1, 1);
            dTP.MaxDate = DateTime.Now;

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            btnCetak.Enabled = false;

            try
            { 
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("select namaprodi from programstudi", conn);
                cmd.CommandType = CommandType.Text;
                dtProdi = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dtProdi);
                comboBox1.DataSource = dtProdi;
                comboBox1.DisplayMember = "namaprodi";
                comboBox1.ValueMember = "namaprodi";
            }

            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }

            // TODO: This line of code loads data into the 'dBAkademikADODataSet.Mahasiswa' table. You can move, or remove it, as needed.
            this.mahasiswaTableAdapter.Fill(this.dBAkademikADODataSet.Mahasiswa);

        }

        private void BtnReport_Click(object sender, EventArgs e)
        {
            try
            { 
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("sp_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@inProdi", SqlDbType.VarChar, 50).Value = comboBox1.Text.ToString();
                cmd.Parameters.Add("@inTglMsuk", SqlDbType.VarChar, 4).Value = dTP.Value.Year.ToString(); ;

                da = new SqlDataAdapter(cmd);

                dtMahasiswa = new DataTable();
                da.Fill(dtMahasiswa);

                dataGridView1.DataSource = dtMahasiswa;

                if (dtMahasiswa.Rows.Count > 0)
                {
                    btnCetak.Enabled = true;
                }
                else
                {
                    btnCetak.Enabled = false;
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
            catch (Exception ex)

            {
                MessageBox.Show("Gagal load data: " + ex.Message);

            }
            
        }

        private void btnCetak_Click(object sender, EventArgs e)
        {
            Form3 frm3 = new Form3(comboBox1.SelectedValue.ToString(), dTP.Value);
            frm3.Show();
            this.Hide();
        }
    }
}
