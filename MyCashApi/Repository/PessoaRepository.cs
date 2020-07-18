using MyCashApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyCashApi.Repository
{
    public class PessoaRepository
    {
        string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

        public List<PessoaModel> ListarPessoas()
        {
            SqlDataReader reader = null;
            List<PessoaModel> lstPessoas = new List<PessoaModel>();

            var query = @"SELECT * FROM Pessoa";

            using (SqlConnection con = new SqlConnection(strConn.ToString()))
            {
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                try
                {
                    reader = com.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var ret = new PessoaModel()
                            {
                                idPessoa = int.Parse(reader[0].ToString()),
                                nomePessoa = reader[1].ToString(),
                                nascimentoPessoa = Convert.ToDateTime(reader[2].ToString())
                            };

                            lstPessoas.Add(ret);
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    con.Close();
                }
            }

            return lstPessoas;
        }
    }
}