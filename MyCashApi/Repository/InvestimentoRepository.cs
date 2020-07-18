using MyCashApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyCashApi.Repository
{
    public class InvestimentoRepository
    {
        string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

        public List<InvestimentoModel> ListarInstitFinancByIdUsuario(int idUsuario)
        {
            SqlDataReader reader = null;
            List<InvestimentoModel> listaInstituicaoFinanceiraModel = new List<InvestimentoModel>();

            var query = @"SELECT * FROM Investimento
                            WHERE usuCodi = " + idUsuario;

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
                            var ret = new InvestimentoModel()
                            {
                                invCodi = int.Parse(reader[0].ToString()),
                                usuCodi = int.Parse(reader[1].ToString()),
                                ifuCodi = int.Parse(reader[2].ToString()),
                                invDesc = reader[3].ToString(),
                                invDtIni = Convert.ToDateTime(reader[4].ToString()),
                                invDtFin = reader[5].ToString().Length > 0 ? Convert.ToDateTime(reader[5].ToString()) : Convert.ToDateTime("01/01/0001"),
                                invVlrIni = decimal.Parse(reader[6].ToString()),
                                invTxJur = decimal.Parse(reader[7].ToString()),
                                invObse = reader[8].ToString(),
                                invFlAt = bool.Parse(reader[9].ToString())
                            };

                            listaInstituicaoFinanceiraModel.Add(ret);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    con.Close();
                }
            }

            return listaInstituicaoFinanceiraModel;
        }

        public string ManterInvestimentoUsuario(InvestimentoModel investimentoModel)
        {
            string resp = "";

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    if (investimentoModel.invCodi > 0) //-> Se vier ID, faz update, senão insere um novo.
                    {
                        command.CommandText =
                            @"UPDATE [dbo].[Investimento] SET " + 
                                  "[ifuCodi] = " + investimentoModel.ifuCodi + 
                                  ",[invDesc] = '" + investimentoModel.invDesc +
                                  "',[invDtIni] = '" + investimentoModel.invDtIni.ToString("yyyy-MM-dd HH:mm:ss") +
                                  "',[invDtFin] = " + 
                                  (investimentoModel.invDtFin.ToString("yyyy-MM-dd HH:mm:ss") == "0001-01-01 00:00:00"
                                        ? "NULL"
                                        : "'" + investimentoModel.invDtFin.ToString("yyyy-MM-dd HH:mm:ss") + "'") +
                                  ",[invVlrIni] = " + investimentoModel.invVlrIni.ToString().Replace(',', '.') +
                                  ",[invTXJur] = " + investimentoModel.invTxJur.ToString().Replace(',', '.') +
                                  ",[invObse] = '" + investimentoModel.invObse +
                                  "',[invFlAt] = " + (investimentoModel.invFlAt ? 1 : 0) +
                             " WHERE [invCodi] = " + investimentoModel.invCodi;
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText =
                            @"INSERT INTO [dbo].[Investimento]
                                       ([usuCodi],[ifuCodi],[invDesc],[invDtIni],[invDtFin],[invVlrIni],[invTXJur],[invObse],[invFlAt])
                                 VALUES
                                       ( " +
                                             investimentoModel.usuCodi + ", " +
                                             investimentoModel.ifuCodi + ", '" +
                                             investimentoModel.invDesc + "', '" +
                                             investimentoModel.invDtIni.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                             (investimentoModel.invDtFin.ToString("yyyy-MM-dd HH:mm:ss") == "0001-01-01 00:00:00"
                                                ? "NULL, "
                                                : "'" + investimentoModel.invDtFin.ToString("yyyy-MM-dd HH:mm:ss") + "', ") +
                                             investimentoModel.invVlrIni.ToString().Replace(',', '.') + ", " +
                                             investimentoModel.invTxJur.ToString().Replace(',', '.') + ", '" +
                                             investimentoModel.invObse + "', " +
                                             (investimentoModel.invFlAt ? 1 : 0) +
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