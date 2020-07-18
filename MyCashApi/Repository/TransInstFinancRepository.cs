using MyCashApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace MyCashApi.Repository
{
    public class TransInstFinancRepository
    {
        string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

        public List<TransInstFinancModel> ListarTransacaoInstFinanceira(int transacaoInstFinancCodi)
        {
            SqlDataReader reader = null;
            List<TransInstFinancModel> listaTransInstFinancModel = new List<TransInstFinancModel>();

            var query = @"SELECT * FROM TransInstFinanc";

            if (transacaoInstFinancCodi > 0)
            {
                query += " WHERE tifCodi = " + transacaoInstFinancCodi;
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
                            var ret = new TransInstFinancModel()
                            {
                                tifCodi = int.Parse(reader[0].ToString()),
                                ifuCodi = int.Parse(reader[1].ToString()),
                                usuCodi = int.Parse(reader[2].ToString()),
                                ttCodi = int.Parse(reader[3].ToString()),
                                tdCodi = int.Parse(reader[4].ToString()),
                                tifData = Convert.ToDateTime(reader[5].ToString()),
                                tifValor = decimal.Parse(reader[6].ToString()),
                                tifComprov = binaryString != null ? Encoding.Default.GetString(binaryString) : ""
                            };

                            listaTransInstFinancModel.Add(ret);
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

            return listaTransInstFinancModel;
        }

        public List<TransInstFinancModel> ListarTransacaoInstFinanceiraByIdUsuario(int idUsuario)
        {
            SqlDataReader reader = null;
            List<TransInstFinancModel> listaTransInstFinancModel = new List<TransInstFinancModel>();

            var query = @"SELECT * FROM TransInstFinanc WHERE usuCodi = " + idUsuario;

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
                            var ret = new TransInstFinancModel()
                            {
                                tifCodi = int.Parse(reader[0].ToString()),
                                ifuCodi = int.Parse(reader[1].ToString()),
                                usuCodi = int.Parse(reader[2].ToString()),
                                ttCodi = int.Parse(reader[3].ToString()),
                                tdCodi = int.Parse(reader[4].ToString()),
                                tifData = Convert.ToDateTime(reader[5].ToString()),
                                tifValor = decimal.Parse(reader[6].ToString()),
                                tifComprov = binaryString != null ? Encoding.Default.GetString(binaryString) : ""
                            };

                            listaTransInstFinancModel.Add(ret);
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

            return listaTransInstFinancModel;
        }

        public string ManterTransacaoInstFinanceira(TransInstFinancModel transInstFinancModel)
        {
            string resp = "";

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("TransInstFinTransaction");
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    //-> Convertendo a String do Base64 da imagem em array de bytes para salvar no BD.
                    string file = "";
                    if (transInstFinancModel.tifComprov.Length > 0)
                    {
                        string imgb64 = (transInstFinancModel.tifComprov);
                        string ext = imgb64.Split('/')[1];
                        var extn = ext.Split(';')[0];
                        file = imgb64.Split(',')[1];
                        (transInstFinancModel.tifComprov) = file;
                    }

                    if (transInstFinancModel.tifCodi > 0) //-> Se vier ID, faz update, senão insere um novo.
                    {
                        // ->Recuperando o valor antigo e removendo-o do saldo do cartão
                        decimal valorAntigo = 0;
                        command.CommandText = "SELECT [tifValor] FROM [mycash].[dbo].[TransInstFinanc] WHERE [tifCodi] = " + transInstFinancModel.tifCodi;
                        DataTable dt = new DataTable();
                        SqlDataReader reader;
                        reader = command.ExecuteReader();
                        dt.Load(reader);
                        if (dt.Rows.Count > 0)
                        {
                            valorAntigo = decimal.Parse(dt.Rows[0].ItemArray[0].ToString().ToLower());
                        }
                        else
                        {
                            transaction.Rollback();
                            connection.Close();
                            return "Não foi possível recuperar o último valor da transação.";
                        }

                        // ->Alterando o saldo do cartão, removendo o valor antigo da transação.
                        command.CommandText =
                            transInstFinancModel.ttCodi == 2 ? // -> Se for 2 (Despesa) adiciona ao saldo, se for 1 (Receita), retira do saldo.
                            @"UPDATE [mycash].[dbo].[InstFinUsuario]
                                SET [ifuSaldo] += " + valorAntigo.ToString().Replace(',', '.') +
                                " WHERE [ifuCodi] = " + transInstFinancModel.ifuCodi
                        :
                            @"UPDATE [mycash].[dbo].[InstFinUsuario]
                                SET [ifuSaldo] -= " + valorAntigo.ToString().Replace(',', '.') +
                                " WHERE [ifuCodi] = " + transInstFinancModel.ifuCodi;
                        command.ExecuteNonQuery();

                        // ->Alterando o registro da transação.
                        command.CommandText =
                            @"UPDATE [dbo].[TransInstFinanc] SET 
	                                   [ifuCodi] = " + transInstFinancModel.ifuCodi +
                                      ",[usuCodi] = " + transInstFinancModel.usuCodi +
                                      ",[ttCodi] = " + transInstFinancModel.ttCodi +
                                      ",[tdCodi] = " + transInstFinancModel.tdCodi +
                                      ",[tifData] = '" + transInstFinancModel.tifData.ToString("yyyy-MM-dd HH:mm:ss") +
                                      "',[tifValor] = " + transInstFinancModel.tifValor.ToString().Replace(',', '.') +
                                      ",[tifComprov] = " + (file.Length > 0 ? "convert(varbinary(max), '" + file + "')" : "NULL") +
                                 " WHERE [tifCodi] =" + transInstFinancModel.tifCodi;
                        command.ExecuteNonQuery();

                        // ->Alterando o saldo da Instituição Financeira, conforme o tipo de despesa.
                        if (transInstFinancModel.ttCodi == 2) // -> Se for 2 (Despesa) retira do saldo, se for 1 (Receita), adiciona no saldo.
                        {
                            command.CommandText =
                                @"UPDATE [mycash].[dbo].[InstFinUsuario]
                                    SET [ifuSaldo] -= " + transInstFinancModel.tifValor.ToString().Replace(',', '.') +
                                    " WHERE [ifuCodi] = " + transInstFinancModel.ifuCodi;
                            command.ExecuteNonQuery();
                        }
                        else if (transInstFinancModel.ttCodi == 1)
                        {
                            command.CommandText =
                                @"UPDATE [mycash].[dbo].[InstFinUsuario]
                                    SET [ifuSaldo] += " + transInstFinancModel.tifValor.ToString().Replace(',', '.') +
                                    " WHERE [ifuCodi] = " + transInstFinancModel.ifuCodi;
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            transaction.Rollback();
                            connection.Close();
                            return "Tipo de Transação inválida!";
                        }
                    }
                    else
                    {
                        command.CommandText =
                            @"INSERT INTO [dbo].[TransInstFinanc]
                                       ([ifuCodi],[usuCodi],[ttCodi],[tdCodi],[tifData],[tifValor],[tifComprov])
                                 VALUES
                                       ( " +
                                            transInstFinancModel.ifuCodi + ", " +
                                            transInstFinancModel.usuCodi + ", " +
                                            transInstFinancModel.ttCodi + ", " +
                                            transInstFinancModel.tdCodi + ", '" +
                                            transInstFinancModel.tifData.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                            transInstFinancModel.tifValor.ToString().Replace(',', '.') + ", " +
                                            (file.Length > 0 ? "convert(varbinary(max), '" + file + "')" : "NULL") +
                                       " )";
                        command.ExecuteNonQuery();

                        // ->Alterando o Saldo da Instituição Financeira do Usuário.
                        if (transInstFinancModel.ttCodi == 2) // -> Se for 2 (Despesa) retira do saldo, se for 1 (Receita), adiciona no saldo.
                        {
                            command.CommandText =
                                @"UPDATE [mycash].[dbo].[InstFinUsuario]
                                    SET [ifuSaldo] -= " + transInstFinancModel.tifValor.ToString().Replace(',', '.') +
                                    " WHERE [ifuCodi] = " + transInstFinancModel.ifuCodi;
                            command.ExecuteNonQuery();
                        }
                        else if (transInstFinancModel.ttCodi == 1)
                        {
                            command.CommandText =
                                @"UPDATE [mycash].[dbo].[InstFinUsuario]
                                    SET [ifuSaldo] += " + transInstFinancModel.tifValor.ToString().Replace(',', '.') +
                                    " WHERE [ifuCodi] = " + transInstFinancModel.ifuCodi;
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            transaction.Rollback();
                            connection.Close();
                            return "Tipo de Transação inválida!";
                        }
                    }

                    transaction.Commit();
                    resp = "OK";
                    connection.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    connection.Close();
                    resp = "Erro ao inserir no banco de dados: " + ex.GetType() +
                        " | Mensagem: " + ex.Message;
                }
            }

            return resp;
        }
    }
}