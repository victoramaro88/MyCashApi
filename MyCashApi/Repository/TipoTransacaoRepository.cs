using MyCashApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyCashApi.Repository
{
    public class TipoTransacaoRepository
    {
        private readonly string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

        public List<TipoTransacaoModel> ListarTipoTransacao(int codigoTipoTransacao)
        {
            List<TipoTransacaoModel> listaTipoTransacao = new List<TipoTransacaoModel>();

            SqlDataReader reader = null;

            var query = @"SELECT * FROM TipoTransacao";

            if (codigoTipoTransacao > 0)
            {
                query += " WHERE ttCodi = " + codigoTipoTransacao;
            }

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
                            var ret = new TipoTransacaoModel()
                            {
                                ttCodi = int.Parse(reader[0].ToString()),
                                ttDesc = reader[1].ToString()
                            };

                            listaTipoTransacao.Add(ret);
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
            
            return listaTipoTransacao;
        }
    }
}