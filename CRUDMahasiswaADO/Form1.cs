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
using System.IO;

namespace CRUDMahasiswaADO
{
    public partial class Form1 : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString = "Data Source=MSI\\UNKNOWNMEMBER;Initial Catalog=DBAkademikADO; Integrated Security=True";
        DAL dbLogic = new DAL();

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
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                DataTable dt = dbLogic.GetMhs();
                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns["Foto"] != null)
                {
                    DataGridViewImageColumn fotoColumn = (DataGridViewImageColumn)dataGridView1.Columns["Foto"];
                    fotoColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                }

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
        }

        private byte[] ConvertImageToBytes(PictureBox pb)
        {
            if (pb.Image == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                pb.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
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

                byte[] imgBytes = ConvertImageToBytes(fotoMHS);

                dbLogic.InsertMhs(textNIM.Text, textNama.Text, textAlamat.Text,
                    comboBoxJK.Text, dtpTL.Value.Date, textKP.Text, imgBytes);

                MessageBox.Show("Data mahasiswa berhasil ditambahkan");
                ClearForm();
                buttonload.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] imgBytes = ConvertImageToBytes(fotoMHS);

                dbLogic.UpdateMhs(textNIM.Text, textNama.Text, textAlamat.Text,
                    comboBoxJK.Text, dtpTL.Value.Date, textKP.Text, imgBytes);

                MessageBox.Show("Data berhasil diupdate");
                ClearForm();
                buttonload.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultConfirm = MessageBox.Show(
                    "Yakin ingin menghapus data?",
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultConfirm == DialogResult.Yes)
                {
                    dbLogic.DeleteMhs(textNIM.Text);

                    MessageBox.Show("Data berhasil dihapus");
                    ClearForm();
                    buttonload.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textNIM.Text = row.Cells["NIM"].Value.ToString();
                textNama.Text = row.Cells["Nama"].Value.ToString();
                comboBoxJK.Text = row.Cells["JenisKelamin"].Value.ToString();
                dtpTL.Value = Convert.ToDateTime(row.Cells["TanggalLahir"].Value);
                textAlamat.Text = row.Cells["Alamat"].Value.ToString();
                textKP.Text = row.Cells["KodeProdi"].Value.ToString();

                if (row.Cells["Foto"].Value != DBNull.Value && row.Cells["Foto"].Value != null)
                {
                    byte[] imgBytes = (byte[])row.Cells["Foto"].Value;
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        fotoMHS.Image = Image.FromStream(ms);
                        fotoMHS.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                else
                {
                    fotoMHS.Image = null;
                }
            }
        }

        private void ClearForm()
        {
            textNIM.Clear();
            textNIM.Clear();
            textNama.Clear();
            comboBoxJK.SelectedIndex = -1;
            dtpTL.Value = DateTime.Now;
            textAlamat.Clear();
            textKP.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxJK.Items.Clear();
            comboBoxJK.Items.Add("L");
            comboBoxJK.Items.Add("P");

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.CellClick += dataGridView1_CellContentClick;

            LoadData();
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

        private void textNIM_TextChanged(object sender, EventArgs e)
        {

        }

        private void btndata_Click(object sender, EventArgs e)
        {
            Form2 fm3 = new Form2();
            fm3.Show();
            this.Hide();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
