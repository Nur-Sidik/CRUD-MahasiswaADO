using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class Form1 : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString = "Data Source=MSI\\UNKNOWNMEMBER;Initial Catalog=DBAkademikADO; Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

       private void ConnectDatabase()
       {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                MessageBox.Show("Koneksi Berhasil");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi Gagal: " + ex.Message);
            }
       }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectDatabase();
        }

        private void buttonload_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                { 
                    conn.Open();
                }

                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                dataGridView1.Columns.Add("NIM", "NIM");
                dataGridView1.Columns.Add("Nama", "Nama");
                dataGridView1.Columns.Add("JenisKelamin", "Jenis Kelamin");
                dataGridView1.Columns.Add("TanggalLahir", "Tanggal Lahir");
                dataGridView1.Columns.Add("Alamat", "Alamat");
                dataGridView1.Columns.Add("KodeProdi", "Kode Prodi");

                string query = "SELECT * FROM Mahasiswa";

                SqlCommand cnd = new SqlCommand(query, conn);
                SqlDataReader reader = cnd.ExecuteReader();

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["NIM"].ToString(),
                        reader["Nama"].ToString(),
                        reader["JenisKelamin"].ToString(),
                        Convert.ToDateTime(reader["TanggalLahir"]).ToShortDateString(),
                        reader["Alamat"].ToString(),
                        reader["KodeProdi"].ToString()
                    );
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (textNIM.Text == "")
                {
                    MessageBox.Show("NIM harus diisi");
                    textNIM.Focus();
                    return;
                }

                if (textNama.Text == "")
                {
                    MessageBox.Show("Nama harus diisi");
                    textNama.Focus();
                    return;
                }

                if (comboBoxJK.Text == "")
                {
                    MessageBox.Show("Jenis Kelamin harus dipilih");
                    comboBoxJK.Focus();
                    return;
                }

                if (textKP.Text == "")
                {
                    MessageBox.Show("Kode Prodi harus diisi");
                    textKP.Focus();
                    return;
                }

                string query = @"INSERT INTO Mahasiswa
                        (NIM, Nama, JenisKelamin, TanggalLahir, Alamat, KodeProdi, TanggalDaftar)
                        VALUES
                        (@NIM, @Nama, @JK, @TanggalLahir, @Alamat, @KodeProdi, @TanggalDaftar)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@NIM", textNIM.Text);
                cmd.Parameters.AddWithValue("@Nama", textNama.Text);
                cmd.Parameters.AddWithValue("@JK", comboBoxJK.Text);
                cmd.Parameters.AddWithValue("@TanggalLahir", dtpTL.Value.Date);
                cmd.Parameters.AddWithValue("@Alamat", textAlamat.Text);
                cmd.Parameters.AddWithValue("@KodeProdi", textKP.Text);
                cmd.Parameters.AddWithValue("@TanggalDaftar", DateTime.Now);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Data mahasiswa berhasil ditambahkan");
                    ClearForm();
                    btnLoad.PerformClick();
                }
                else
                {
                    MessageBox.Show("Data gagal ditambahkan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textNIM_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
