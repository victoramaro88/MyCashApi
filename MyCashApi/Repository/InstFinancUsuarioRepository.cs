using MyCashApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyCashApi.Repository
{
    public class InstFinancUsuarioRepository
    {
        string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

        public List<InstFinancUsuarioModel> ListarInstitFinancUsuario(int instFinUsuCod)
        {
            SqlDataReader reader = null;
            List<InstFinancUsuarioModel> listaInstituicaoFinanceiraModel = new List<InstFinancUsuarioModel>();

            var query = @"SELECT * FROM InstFinUsuario";

            if (instFinUsuCod > 0)
            {
                query += " WHERE ifuCodi = " + instFinUsuCod;
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
                            var ret = new InstFinancUsuarioModel()
                            {
                                ifuCodi = int.Parse(reader[0].ToString()),
                                ifCodi = int.Parse(reader[1].ToString()),
                                usuCodi = int.Parse(reader[2].ToString()),
                                ifuNAgen = reader[3].ToString(),
                                ifuNConta = reader[4].ToString(),
                                ifuLimit = decimal.Parse(reader[5].ToString()),
                                ifuSaldo = decimal.Parse(reader[6].ToString()),
                                ifuFlAt = Convert.ToBoolean(reader[7].ToString())
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

        public string ManterInstitFinancUsuario(InstFinancUsuarioModel instFinancUsuarioModel)
        {
            string resp = "";

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    if (instFinancUsuarioModel.ifuCodi > 0) //-> Se vier ID, faz update, senão insere um novo.
                    {
                        command.CommandText =
                            @"UPDATE [dbo].[InstFinUsuario] SET 
                                   [ifCodi] = " + instFinancUsuarioModel.ifCodi +
                                  ",[ifuNAgen] = '" + instFinancUsuarioModel.ifuNAgen +
                                  "',[ifuNConta] = '" + instFinancUsuarioModel.ifuNConta +
                                  "',[ifuLimit] = " + instFinancUsuarioModel.ifuLimit.ToString().Replace(',', '.') +
                                  ",[ifuSaldo] = " + instFinancUsuarioModel.ifuSaldo.ToString().Replace(',', '.') +
                                  ",[ifuFlAt] = " + (instFinancUsuarioModel.ifuFlAt ? 1 : 0) +
                             " WHERE [ifuCodi] = " + instFinancUsuarioModel.ifuCodi;
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText =
                            @"INSERT INTO [dbo].[InstFinUsuario]
                                       ([ifCodi],[usuCodi],[ifuNAgen],[ifuNConta],[ifuLimit],[ifuSaldo],[ifuFlAt])
                                 VALUES
                                       ( " +
                                            instFinancUsuarioModel.ifCodi + ", " +
                                            instFinancUsuarioModel.usuCodi + ", '" +
                                            instFinancUsuarioModel.ifuNAgen + "', '" +
                                            instFinancUsuarioModel.ifuNConta + "', " +
                                            instFinancUsuarioModel.ifuLimit.ToString().Replace(',', '.') + ", " +
                                            instFinancUsuarioModel.ifuSaldo.ToString().Replace(',', '.') + ", " +
                                            (instFinancUsuarioModel.ifuFlAt ? 1 : 0) +
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