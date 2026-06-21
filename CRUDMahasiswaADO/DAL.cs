using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDMahasiswaADO
{
    internal class DAL
    {
        // Samakan dengan connection string yang sudah kamu pakai di Form1/Form2/Form3
        private static string connectionString = "Data Source=MSI\\UNKNOWNMEMBER;Initial Catalog=DBAkademikADO; Integrated Security=True";

        public string GetConnectionString()
        {
            return connectionString;
        }

        SqlConnection conn = new SqlConnection(connectionString);

        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DataTable dtProdi;

        // ================= MAHASISWA - READ =================

        public int CountMhs()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_CountMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter outputParam = new SqlParameter("@Total", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputParam);

            cmd.ExecuteNonQuery();

            return Convert.ToInt32(outputParam.Value);
        }

        public DataTable GetMhs()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_GetMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);

            return dtMahasiswa;
        }

        public DataTable GetMhsByNIM(string nim)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_GetMahasiswaByNIM", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@NIM", nim);

            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);

            return dtMahasiswa;
        }

        // ================= MAHASISWA - INSERT / UPDATE / DELETE =================

        public void InsertMhs(string nim, string nama, string alamat, string jenisKelamin,
            DateTime tanggalLahir, string kodeProdi, byte[] foto)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                SqlCommand command = new SqlCommand("sp_InsertMahasiswa", conn, trans);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@NIM", nim);
                command.Parameters.AddWithValue("@Nama", nama);
                command.Parameters.AddWithValue("@Alamat", alamat);
                command.Parameters.AddWithValue("@JenisKelamin", jenisKelamin);
                command.Parameters.AddWithValue("@TanggalLahir", tanggalLahir);
                command.Parameters.AddWithValue("@KodeProdi", kodeProdi);
                command.Parameters.AddWithValue("@TanggalDaftar", DateTime.Now);
                command.Parameters.AddWithValue("@Foto", (object)foto ?? DBNull.Value);

                command.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateMhs(string nim, string nama, string alamat, string jenisKelamin,
            DateTime tanggalLahir, string kodeProdi, byte[] foto)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand command = new SqlCommand("sp_UpdateMahasiswa", conn);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@NIM", nim);
            command.Parameters.AddWithValue("@Nama", nama);
            command.Parameters.AddWithValue("@Alamat", alamat);
            command.Parameters.AddWithValue("@JenisKelamin", jenisKelamin);
            command.Parameters.AddWithValue("@TanggalLahir", tanggalLahir);
            command.Parameters.AddWithValue("@KodeProdi", kodeProdi);
            command.Parameters.AddWithValue("@Foto", (object)foto ?? DBNull.Value);

            command.ExecuteNonQuery();
        }

        public void DeleteMhs(string nim)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_DeleteMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NIM", nim);

            cmd.ExecuteNonQuery();
        }

        // ================= PROGRAM STUDI =================

        public DataTable GetProdi()
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

            return dtProdi;
        }

        // ================= REPORT/REKAP =================

        public DataTable GetDataRekap(string prodi, DateTime tanggalMasuk)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_Report", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inProdi", prodi);
            cmd.Parameters.AddWithValue("@inTglMsuk", tanggalMasuk.Year.ToString());

            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);

            return dtMahasiswa;
        }

        // ================= CHART / DASHBOARD =================

        public DataTable GetAllDataChart()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_DashBoard", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);

            return dtMahasiswa;
        }

        public DataTable GetDataChartByTahun(DateTime thMasuk)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_DashBoardByTahun", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inTglMsuk", thMasuk.Year.ToString());

            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);

            return dtMahasiswa;
        }
    }
}