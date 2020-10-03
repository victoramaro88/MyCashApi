using MyCashApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace MyCashApi.Repository
{
    public class CartaoRepository
    {
        string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

        public List<CartaoUsuarioModel> ListarCartoesByIdUsuario(int idUsuario)
        {
            SqlDataReader reader = null;
            List<CartaoUsuarioModel> listaCartoes = new List<CartaoUsuarioModel>();

            var query = @"SELECT 
	                            CART.carCodi, CART.bcCodi, CART.ifCodi, CART.usuCodi, CART.carDesc
	                            , CART.carLimit, CART.carDiaVenc, CART.carSald, CART.carFlAt,
	                            INSTFIN.ifDesc, INSTFIN.ifImg,
	                            BANDCART.bcDesc, BANDCART.bcImg
                            FROM Cartao AS CART WITH (NOLOCK)
                            INNER JOIN InstituicaoFinanceira AS INSTFIN ON INSTFIN.ifCodi = CART.ifCodi
                            INNER JOIN BandeiraCartao AS BANDCART ON BANDCART.bcCodi = CART.bcCodi
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
                            byte[] binaryString;
                            if (reader["ifImg"].ToString().Length > 0)
                            {
                                binaryString = (byte[])reader["ifImg"]; //-> Convertendo string novamente em byte[].
                            }
                            else
                            {
                                binaryString = null;
                            }

                            byte[] binaryString2;
                            if (reader["bcImg"].ToString().Length > 0)
                            {
                                binaryString2 = (byte[])reader["bcImg"]; //-> Convertendo string novamente em byte[].
                            }
                            else
                            {
                                binaryString2 = null;
                            }

                            var ret = new CartaoUsuarioModel()
                            {
                                carCodi = int.Parse(reader["carCodi"].ToString()),
                                bcCodi = int.Parse(reader["bcCodi"].ToString()),
                                ifCodi = int.Parse(reader["ifCodi"].ToString()),
                                usuCodi = int.Parse(reader["usuCodi"].ToString()),
                                carDesc = reader["carDesc"].ToString(),
                                carLimit = decimal.Parse(reader["carLimit"].ToString()),
                                carDiaVenc = int.Parse(reader["carDiaVenc"].ToString()),
                                carSald = decimal.Parse(reader["carSald"].ToString()),
                                carFlAt = Convert.ToBoolean(reader["carFlAt"].ToString()),
                                ifDesc = reader["ifDesc"].ToString(),
                                ifImg = binaryString != null ? Encoding.Default.GetString(binaryString) : "",
                                bcDesc = reader["bcDesc"].ToString(),
                                bcImg = binaryString2 != null ? Encoding.Default.GetString(binaryString2) : ""
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

        public string AlteraStatusCartaoUsr(int carCodi, bool ifuFlAt)
        {
            string resp = "";

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    if (carCodi > 0)
                    {
                        command.CommandText =
                            @"UPDATE [dbo].[Cartao] SET 
                                   [carFlAt] = " + (ifuFlAt ? 1 : 0) +
                             " WHERE [carCodi] = " + carCodi;
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