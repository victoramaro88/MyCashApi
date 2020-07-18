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
    public class TransacoesFixasRepository
    {
        string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

        public List<TransacoesFixasModel> ListarTransacoesFixas(int transacaoFixaCodi, int idUsuario)
        {
            SqlDataReader reader = null;
            List<TransacoesFixasModel> listaTransacoesFixasModel = new List<TransacoesFixasModel>();

            var query = @"SELECT * FROM TransacoesFixas";

            if (transacaoFixaCodi > 0)
            {
                query += " WHERE tfCodi = " + transacaoFixaCodi;
            }

            if (idUsuario > 0)
            {
                query += " AND usuCodi = " + idUsuario;
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
                            if (reader[12].ToString().Length > 0)
                            {
                                binaryString = (byte[])reader[12]; //-> Convertendo string novamente em byte[].
                            }
                            else
                            {
                                binaryString = null;
                            }
                            var ret = new TransacoesFixasModel()
                            {
                                tfCodi = int.Parse(reader[0].ToString()),
                                usuCodi = int.Parse(reader[1].ToString()),
                                ifuCodi = int.Parse(reader[2].ToString()),
                                ttCodi = int.Parse(reader[3].ToString()),
                                tdCodi = int.Parse(reader[4].ToString()),
                                tfDesc = reader[5].ToString(),
                                tfValor = decimal.Parse(reader[6].ToString()),
                                tfDiaVenc = int.Parse(reader[7].ToString()),
                                tfMesDebit = reader[8].ToString().Length > 0 ? DateTime.Parse(reader[8].ToString()) : Convert.ToDateTime("01/01/0001"),
                                tfDataIni = Convert.ToDateTime(reader[9].ToString()),
                                tfDataFim = reader[10].ToString().Length > 0 ? Convert.ToDateTime(reader[10].ToString()) : Convert.ToDateTime("01/01/0001"),
                                tfFlAt = bool.Parse(reader[11].ToString()),
                                tfDoc = binaryString != null ? Encoding.Default.GetString(binaryString) : "",
                            };

                            listaTransacoesFixasModel.Add(ret);
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

            return listaTransacoesFixasModel;
        }

        public List<TransacoesFixasModel> ListarTransacoesFixasByIdUsuario(int idUsuario)
        {
            SqlDataReader reader = null;
            List<TransacoesFixasModel> listaTransacoesFixasModel = new List<TransacoesFixasModel>();

            var query = @"SELECT * FROM TransacoesFixas WHERE usuCodi = " + idUsuario;

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
                            if (reader[10].ToString().Length > 0)
                            {
                                binaryString = (byte[])reader[10]; //-> Convertendo string novamente em byte[].
                            }
                            else
                            {
                                binaryString = null;
                            }
                            var ret = new TransacoesFixasModel()
                            {
                                tfCodi = int.Parse(reader[0].ToString()),
                                usuCodi = int.Parse(reader[1].ToString()),
                                ifuCodi = int.Parse(reader[2].ToString()),
                                ttCodi = int.Parse(reader[3].ToString()),
                                tdCodi = int.Parse(reader[4].ToString()),
                                tfDesc = reader[5].ToString(),
                                tfValor = decimal.Parse(reader[6].ToString()),
                                tfDataIni = Convert.ToDateTime(reader[7].ToString()),
                                tfDataFim = reader[8].ToString().Length > 0 ? Convert.ToDateTime(reader[8].ToString()) : Convert.ToDateTime("01/01/0001"),
                                tfFlAt = bool.Parse(reader[9].ToString()),
                                tfDoc = binaryString != null ? Encoding.Default.GetString(binaryString) : "",
                            };

                            listaTransacoesFixasModel.Add(ret);
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

            return listaTransacoesFixasModel;
        }

        public string ManterTransacoesFixas(TransacoesFixasModel transacoesFixasModel)
        {
            string resp = "";

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                transaction = connection.BeginTransaction("ListTransFixTransaction");
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    //-> Convertendo a String do Base64 da imagem em array de bytes para salvar no BD.
                    string file = "";
                    if (transacoesFixasModel.tfDoc.Length > 0)
                    {
                        string imgb64 = (transacoesFixasModel.tfDoc);
                        string ext = imgb64.Split('/')[1];
                        var extn = ext.Split(';')[0];
                        file = imgb64.Split(',')[1];
                        (transacoesFixasModel.tfDoc) = file;
                    }

                    if (transacoesFixasModel.tfCodi > 0) //-> Se vier ID, faz update, senão insere um novo.
                    {
                        command.CommandText =
                            @"UPDATE [dbo].[TransacoesFixas] SET 
                                   [usuCodi] = " + transacoesFixasModel.usuCodi +
                                  ",[ifuCodi] = " + transacoesFixasModel.ifuCodi +
                                  ",[ttCodi] = " + transacoesFixasModel.ttCodi +
                                  ",[tdCodi] = " + transacoesFixasModel.tdCodi +
                                  ",[tfDesc] = '" + transacoesFixasModel.tfDesc +
                                  "',[tfValor] = " + transacoesFixasModel.tfValor.ToString().Replace(',', '.') +
                                  ",[tfDiaVenc] = " + transacoesFixasModel.tfDiaVenc +
                                  ",[tfMesDebit] = '" + transacoesFixasModel.tfMesDebit.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                  ",[tfDataIni] = '" + transacoesFixasModel.tfDataIni.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                  ",[tfDataFim] = " +
                                  (transacoesFixasModel.tfDataFim.ToString("yyyy-MM-dd HH:mm:ss") == "0001-01-01 00:00:00"
                                        ? "NULL"
                                        : "'" + transacoesFixasModel.tfDataFim.ToString("yyyy-MM-dd HH:mm:ss") + "'") +
                                  ",[tfFlAtivo] = " + (transacoesFixasModel.tfFlAt ? 1 : 0) +
                                  ",[tfDoc] = " + (file.Length > 0 ? "convert(varbinary(max), '" + file + "')" : "NULL") +
                             " WHERE [tfCodi] = " + transacoesFixasModel.tfCodi;
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText =
                            @"INSERT INTO [dbo].[TransacoesFixas]
                                       ([usuCodi],[ifuCodi],[ttCodi],[tdCodi],[tfDesc],[tfValor],[tfDiaVenc],[tfMesDebit],[tfDataIni],[tfDataFim],[tfFlAtivo],[tfDoc])
                                 VALUES
                                       ( " +
                                            transacoesFixasModel.usuCodi + ", " +
                                            transacoesFixasModel.ifuCodi + ", " +
                                            transacoesFixasModel.ttCodi + ", " +
                                            transacoesFixasModel.tdCodi + ", '" +
                                            transacoesFixasModel.tfDesc + "', " +
                                            transacoesFixasModel.tfValor.ToString().Replace(',', '.') + ", " +
                                            transacoesFixasModel.tfDiaVenc + ", '" +
                                            transacoesFixasModel.tfMesDebit.ToString("yyyy-MM-dd HH:mm:ss") + "', '" +
                                            transacoesFixasModel.tfDataIni.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                            (transacoesFixasModel.tfDataFim.ToString("yyyy-MM-dd HH:mm:ss") == "0001-01-01 00:00:00"
                                                ? "NULL, "
                                                : "'" + transacoesFixasModel.tfDataFim.ToString("yyyy-MM-dd HH:mm:ss") + "', ") +
                                            (transacoesFixasModel.tfFlAt ? 1 : 0) + ", " +
                                            (file.Length > 0 ? "convert(varbinary(max), '" + file + "')" : "NULL") +
                                       " )";
                        command.ExecuteNonQuery();

                        if(transacoesFixasModel.tfDataIni.Year == DateTime.Now.Year
                            && transacoesFixasModel.tfDataIni.Month == DateTime.Now.Month
                            && transacoesFixasModel.tfDataIni.Day <= transacoesFixasModel.tfDiaVenc)
                        {
                            // ->Recuperando o último Id
                            int lastId = 0;
                            command.CommandText = "SELECT MAX([tfCodi]) FROM [dbo].[TransacoesFixas]";
                            DataTable dt = new DataTable();
                            SqlDataReader reader;
                            reader = command.ExecuteReader();
                            dt.Load(reader);
                            if (dt.Rows.Count > 0)
                            {
                                lastId = int.Parse(dt.Rows[0].ItemArray[0].ToString().ToLower());
                            }
                            else
                            {
                                return "Não foi possível recuperar o último código da tabela.";
                            }

                            // ->Atualizando o mês de pagamento da Transação Fixa.
                            command.CommandText =
                                @" UPDATE [dbo].[TransacoesFixas]
                                   SET 
                                      [tfMesDebit] = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                                 "' WHERE [tfCodi] = " + lastId;
                            command.ExecuteNonQuery();

                            // ->Alterando o saldo da Inst. Fin. do Usuário, retirando o valor da Transação Fixa.
                            if(transacoesFixasModel.ttCodi == 2)
                            {
                                command.CommandText =
                                @" UPDATE [dbo].[InstFinUsuario] " +
                                 "  SET [ifuSaldo] = (SELECT [ifuSaldo] - " + transacoesFixasModel.tfValor.ToString().Replace(',', '.') + " AS Total FROM [mycash].[dbo].[InstFinUsuario]) " +
                                 " WHERE [ifuCodi] = " + transacoesFixasModel.ifuCodi;
                                command.ExecuteNonQuery();
                            }
                            else if(transacoesFixasModel.ttCodi == 1)
                            {
                                command.CommandText =
                                @" UPDATE [dbo].[InstFinUsuario] " +
                                 "  SET [ifuSaldo] = (SELECT [ifuSaldo] + " + transacoesFixasModel.tfValor.ToString().Replace(',', '.') + " AS Total FROM [mycash].[dbo].[InstFinUsuario]) " +
                                 " WHERE [ifuCodi] = " + transacoesFixasModel.ifuCodi;
                                command.ExecuteNonQuery();
                            }
                            else
                            {
                                transaction.Rollback();
                                connection.Close();
                                return "Tipo de Transação inválida!";
                            }
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

                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        resp = "Erro ao realizar o rollback: " + ex2.GetType() +
                        " | Mensagem: " + ex2.Message;
                    }
                }
            }

            return resp;
        }
    }
}