using Microsoft.AspNetCore.Http.HttpResults;
using MvcCoreCrudDoctores.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;

namespace MvcCoreCrudDoctores.Repositories
{
    #region PROCEDURES
//    CREATE PROCEDURE SP_INSERTDOCTOR
//(@APELLIDO NVARCHAR(50),@SALARIO INT, @ESPECIALIDAD NVARCHAR(50),@HOSPITAL_COD INT)
//AS
//    declare @nextId int
//    select @nextId=max(doctor_no) from doctor


//    insert into doctor values(@HOSPITAL_COD, @nextId, @APELLIDO, @ESPECIALIDAD, @SALARIO);
//    GO
    #endregion
    public class DoctorRepository
    {
        private SqlCommand com;
        private SqlConnection cn;
        private SqlDataReader reader;

        public DoctorRepository()
        {
            string connectonString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2023";
            cn = new SqlConnection(connectonString);
            com = new SqlCommand();
            com.Connection = cn;
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            List<Doctor> doctors = new List<Doctor>();
            string sql = "Select * from Doctor";
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            this.reader= await this.com.ExecuteReaderAsync();
            while (await this.reader.ReadAsync())
            {
                Doctor doc = new();
                doc.DoctorNo = int.Parse(this.reader["DOCTOR_NO"].ToString());
                doc.HospitalCod = int.Parse(this.reader["HOSPITAL_COD"].ToString());
                doc.Salario = int.Parse(this.reader["SALARIO"].ToString());
                doc.Apellido = this.reader["APELLIDO"].ToString();
                doc.Especialidad = this.reader["Especialidad"].ToString();
                doctors.Add(doc);
            }
            await this.cn.CloseAsync();
            await this.reader.CloseAsync();
            return doctors;
        }

        public async Task<Doctor> FindDocotrAsync(int id)
        {
            string sql = "Select * from Doctor where doctor_no=@id";
            this.com.Parameters.AddWithValue("@id", id);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();
            Doctor doc = null;
            if (await this.reader.ReadAsync())
            {
                doc = new();
                doc.DoctorNo = int.Parse(this.reader["DOCTOR_NO"].ToString());
                doc.HospitalCod = int.Parse(this.reader["HOSPITAL_COD"].ToString());
                doc.Salario = int.Parse(this.reader["SALARIO"].ToString());
                doc.Apellido = this.reader["APELLIDO"].ToString();
                doc.Especialidad = this.reader["Especialidad"].ToString();
            }
            await this.cn.CloseAsync();
            await this.reader.CloseAsync();
            this.com.Parameters.Clear();
            return doc;
        }

        public async Task UpdateDoctorAsync(string apellido,int salario, string especialidad,int cod_hos,int iddoc)
        {
            string sql = "Update Doctor set apellido=@apellido,especialidad=@especialidad,salario=@salario, hospital_cod=@cod_hos where doctor_no=@id";
            this.com.Parameters.AddWithValue("@id", iddoc);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@especialidad", especialidad);
            this.com.Parameters.AddWithValue("@cod_hos", cod_hos);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task DeleteDoctorAsync(int iddoc)
        {
            string sql = "delete Doctor where doctor_no=@id";
            this.com.Parameters.AddWithValue("@id", iddoc);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task InsertDoctorAsync(string apellido,int salario,string especialidad,int hosCod)
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERTDOCTOR";
            this.com.Parameters.AddWithValue("@APELLIDO", apellido);
            this.com.Parameters.AddWithValue("@SALARIO", salario);
            this.com.Parameters.AddWithValue("@ESPECIALIDAD", especialidad);
            this.com.Parameters.AddWithValue("@HOSPITAL_COD", hosCod);
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
    }
}
