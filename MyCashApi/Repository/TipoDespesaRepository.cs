using MyCashApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyCashApi.Repository
{
    public class TipoDespesaRepository
    {
        string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

        public List<TipoDespesaModel> ListarTipoDespesa(int codigoTipoDespesa)
        {
            SqlDataReader reader = null;
            List<TipoDespesaModel> lstTipoDespesa = new List<TipoDespesaModel>();

            var query = @"SELECT * FROM TipoDespesa";

            if (codigoTipoDespesa > 0)
            {
                query += " WHERE tdCodi = " + codigoTipoDespesa;
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
                            var ret = new TipoDespesaModel()
                            {
                                tdCodi = int.Parse(reader[0].ToString()),
                                tdDesc = reader[1].ToString(),
                                tdFlAt = Convert.ToBoolean(reader[2].ToString())
                            };

                            lstTipoDespesa.Add(ret);
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

            return lstTipoDespesa;
        }

        public string ManterTipoDespesa(TipoDespesaModel tipoDespesaModel)
        {
            string resp = "";

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    if (tipoDespesaModel.tdCodi > 0) //-> Se vier ID, faz update, senão insere um novo.
                    {
                        command.CommandText =
                             @"UPDATE [dbo].[TipoDespesa] SET 
                                       [tdDesc] = '" + tipoDespesaModel.tdDesc +
                                      "',[tdFlAt] = " + (tipoDespesaModel.tdFlAt ? 1 : 0) + 
                                 " WHERE [tdCodi] = " + tipoDespesaModel.tdCodi;
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText =
                             @"INSERT INTO [dbo].[TipoDespesa]
                                       ([tdDesc],[tdFlAt])
                                 VALUES
                                       ( '" +
                                            tipoDespesaModel.tdDesc + "', " +
                                            (tipoDespesaModel.tdFlAt ? 1 : 0) +
                                       " )";
                        command.ExecuteNonQuery();
                    }

                    resp = "OK";
                }
                catch (Exception ex)
                {
                    resp = "Erro ao inserir no banco de dados: " + ex.GetType() +
                        " | Mensagem: " + ex.Message;
                }
            }

            return resp;
        }
    }
}