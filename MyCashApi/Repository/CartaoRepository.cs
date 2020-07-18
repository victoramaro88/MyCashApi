using MyCashApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyCashApi.Repository
{
    public class CartaoRepository
    {
        string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

        public List<CartaoModel> ListarCartoesByIdUsuario(int idUsuario)
        {
            SqlDataReader reader = null;
            List<CartaoModel> listaCartoes = new List<CartaoModel>();

            var query = @"SELECT * FROM Cartao
                            WHERE [usuCodi] = " + idUsuario;

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
                            var ret = new CartaoModel()
                            {
                                carCodi = int.Parse(reader[0].ToString()),
                                bcCodi = int.Parse(reader[1].ToString()),
                                ifCodi = int.Parse(reader[2].ToString()),
                                usuCodi = int.Parse(reader[3].ToString()),
                                carDesc = reader[4].ToString(),
                                carLimit = decimal.Parse(reader[5].ToString()),
                                carDiaVenc = int.Parse(reader[6].ToString()),
                                carSald = decimal.Parse(reader[7].ToString()),
                                carFlAt = Convert.ToBoolean(reader[8].ToString())
                            };

                            listaCartoes.Add(ret);
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

            return listaCartoes;
        }

        public string ManterCartaoUsuario(CartaoModel cartaoModel)
        {
            string resp = "";

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    if (cartaoModel.carCodi > 0) //-> Se vier ID, faz update, senão insere um novo.
                    {
                        command.CommandText =
                            @"UPDATE [dbo].[Cartao] SET 
                                   [bcCodi] = " + cartaoModel.bcCodi +
                                  ",[ifCodi] = " + cartaoModel.ifCodi +
                                  ",[usuCodi] = " + cartaoModel.usuCodi +
                                  ",[carDesc] = '" + cartaoModel.carDesc +
                                  "',[carLimit] = " + cartaoModel.carLimit.ToString().Replace(',', '.') +
                                  ",[carDiaVenc] = " + cartaoModel.carDiaVenc + 
                                  ",[carSald] = " + cartaoModel.carSald.ToString().Replace(',', '.') +
                                  ",[carFlAt] = " + (cartaoModel.carFlAt ? 1 : 0) +
                             " WHERE [carCodi] = " + cartaoModel.carCodi;
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText =
                            @"INSERT INTO [dbo].[Cartao]
                                       ([bcCodi],[ifCodi],[usuCodi],[carDesc],[carLimit],[carDiaVenc],[carSald],[carFlAt])
                                 VALUES
                                       ( " +
                                            cartaoModel.bcCodi + ", " +
                                            cartaoModel.ifCodi + ", " +
                                            cartaoModel.usuCodi + ", '" +
                                            cartaoModel.carDesc + "', " +
                                            cartaoModel.carLimit.ToString().Replace(',', '.') + ", " +
                                            cartaoModel.carDiaVenc + ", " +
                                            cartaoModel.carSald.ToString().Replace(',', '.') + ", " +
                                            (cartaoModel.carFlAt ? 1 : 0) +
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