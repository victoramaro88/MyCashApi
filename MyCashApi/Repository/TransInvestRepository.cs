using MyCashApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace MyCashApi.Repository
{
    public class TransInvestRepository
    {
        string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

        public List<TransInvestModel> ListarTransacaoInvestimento(int transacaoInvestCodi)
        {
            SqlDataReader reader = null;
            List<TransInvestModel> listaTransInvestModel = new List<TransInvestModel>();

            var query = @"SELECT * FROM TransInvest";

            if (transacaoInvestCodi > 0)
            {
                query += " WHERE tiCodi = " + transacaoInvestCodi;
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
                            byte[] binaryString;
                            if (reader[7].ToString().Length > 0)
                            {
                                binaryString = (byte[])reader[7]; //-> Convertendo string novamente em byte[].
                            }
                            else
                            {
                                binaryString = null;
                            }
                            var ret = new TransInvestModel()
                            {
                                tiCodi = int.Parse(reader[0].ToString()),
                                invCodi = int.Parse(reader[1].ToString()),
                                usuCodi = int.Parse(reader[2].ToString()),
                                ttCodi = int.Parse(reader[3].ToString()),
                                tdCodi = int.Parse(reader[4].ToString()),
                                tiData = Convert.ToDateTime(reader[5].ToString()),
                                tiValor = decimal.Parse(reader[6].ToString()),
                                tiComprov = binaryString != null ? Encoding.Default.GetString(binaryString) : ""
                            };

                            listaTransInvestModel.Add(ret);
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

            return listaTransInvestModel;
        }

        public List<TransInvestModel> ListarTransacaoInvestimentoByIdUsuario(int idUsuario)
        {
            SqlDataReader reader = null;
            List<TransInvestModel> listaTransInvestModel = new List<TransInvestModel>();

            var query = @"SELECT * FROM TransInvest WHERE usuCodi = " + idUsuario;

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
                            if (reader[7].ToString().Length > 0)
                            {
                                binaryString = (byte[])reader[7]; //-> Convertendo string novamente em byte[].
                            }
                            else
                            {
                                binaryString = null;
                            }
                            var ret = new TransInvestModel()
                            {
                                tiCodi = int.Parse(reader[0].ToString()),
                                invCodi = int.Parse(reader[1].ToString()),
                                usuCodi = int.Parse(reader[2].ToString()),
                                ttCodi = int.Parse(reader[3].ToString()),
                                tdCodi = int.Parse(reader[4].ToString()),
                                tiData = Convert.ToDateTime(reader[5].ToString()),
                                tiValor = decimal.Parse(reader[6].ToString()),
                                tiComprov = binaryString != null ? Encoding.Default.GetString(binaryString) : ""
                            };

                            listaTransInvestModel.Add(ret);
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

            return listaTransInvestModel;
        }

        public string ManterTransacaoInvestimento(TransInvestModel transInvestModel)
        {
            string resp = "";

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    //-> Convertendo a String do Base64 da imagem em array de bytes para salvar no BD.
                    string file = "";
                    if (transInvestModel.tiComprov.Length > 0)
                    {
                        string imgb64 = (transInvestModel.tiComprov);
                        string ext = imgb64.Split('/')[1];
                        var extn = ext.Split(';')[0];
                        file = imgb64.Split(',')[1];
                        (transInvestModel.tiComprov) = file;
                    }

                    if (transInvestModel.tiCodi > 0) //-> Se vier ID, faz update, senão insere um novo.
                    {
                        command.CommandText =
                            @"UPDATE [dbo].[TransInvest] SET 
                                     [invCodi] = " + transInvestModel.invCodi +
                                    ",[usuCodi] = " + transInvestModel.usuCodi +
                                    ",[ttCodi] = " + transInvestModel.ttCodi +
                                    ",[tdCodi] = " + transInvestModel.tdCodi +
                                    ",[tiData] = '" + transInvestModel.tiData.ToString("yyyy-MM-dd HH:mm:ss") +
                                    "',[tiValor] = " + transInvestModel.tiValor.ToString().Replace(',', '.') +
                                    ",[tiComprov] = " + (file.Length > 0 ? "convert(varbinary(max), '" + file + "')" : "NULL") +
                                " WHERE [tiCodi] = " + transInvestModel.tiCodi;
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText =
                            @"INSERT INTO [dbo].[TransInvest]
                                       ([invCodi],[usuCodi],[ttCodi],[tdCodi],[tiData],[tiValor],[tiComprov])
                                 VALUES
                                       ( " +
                                            transInvestModel.invCodi + ", " +
                                            transInvestModel.usuCodi + ", " +
                                            transInvestModel.ttCodi + ", " +
                                            transInvestModel.tdCodi + ", '" +
                                            transInvestModel.tiData.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                            transInvestModel.tiValor.ToString().Replace(',', '.') + ", " +
                                            (file.Length > 0 ? "convert(varbinary(max), '" + file + "')" : "NULL") +
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