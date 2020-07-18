using MyCashApi.Controllers;
using MyCashApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyCashApi.Repository
{
    public class UsuarioRepository
    {
        private readonly string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;
        UtilitariosController utilitarios = new UtilitariosController();

        public string AutenticarUsuario(string userName, string password)
        {
            string senhaCripto = utilitarios.GerarHashString(password);

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();

                command.CommandText =
                    @"SELECT * FROM [mycash].[dbo].[Usuario] AS Usr
                        INNER JOIN [mycash].[dbo].[PerfilUsuario] AS PerfUsr ON Usr.usuCodi = PerfUsr.usuCodi
                        WHERE Usr.usuEmail = '" + userName + "' AND Usr.usuSenha = '" + senhaCripto + "'";

                DataTable dt = new DataTable();
                SqlDataReader reader;
                reader = command.ExecuteReader();
                dt.Load(reader);

                if (dt.Rows.Count > 0)
                {
                    return "OK";
                }
                else
                {
                    return null;
                }
            }
        }

        public List<DadosUsuarioModel> ListarUsuarios(int codigoUsuario)
        {
            SqlDataReader reader = null;
            List<DadosUsuarioModel> lstPessoas = new List<DadosUsuarioModel>();

            var query = @"SELECT * FROM [mycash].[dbo].[Usuario] AS Usr
                        INNER JOIN [mycash].[dbo].[Endereco] AS Ende WITH(NOLOCK) ON Ende.usuCodi = Usr.usuCodi
                        INNER JOIN [mycash].[dbo].[PerfilUsuario] AS PerfUsu WITH(NOLOCK) ON PerfUsu.usuCodi = Usr.usuCodi";

            if (codigoUsuario > 0)
            {
                query += " WHERE Usr.usuCodi = " + codigoUsuario;
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
                            var retUsuario = new UsuarioModel()
                            {
                                usuCodi = int.Parse(reader[0].ToString()),
                                usuNome = reader[1].ToString(),
                                usuNCPF = reader[2].ToString(),
                                usuNasc = Convert.ToDateTime(reader[3].ToString()),
                                usuEmail = reader[4].ToString(),
                                usuSenha = reader[5].ToString(),
                                usuDtCad = Convert.ToDateTime(reader[6].ToString()),
                                usuFlAt = Convert.ToBoolean(reader[7].ToString()),
                                usuValido = Convert.ToBoolean(reader[8].ToString()),
                                usuSexo = reader[9].ToString(),
                                usuCttEma = Convert.ToBoolean(reader[10].ToString())
                            };

                            var retEndereco = new EnderecoModel()
                            {
                                endCodi = int.Parse(reader[11].ToString()),
                                usuCodi = int.Parse(reader[12].ToString()),
                                endNCEP = reader[13].ToString(),
                                endLogr = reader[14].ToString(),
                                endNume = reader[15].ToString(),
                                endCompl = reader[16].ToString(),
                                endBairr = reader[17].ToString(),
                                endCidad = reader[18].ToString(),
                                endEsta = reader[19].ToString()
                            };

                            var retPerfilUsuario = new PerfilUsuarioModel()
                            {
                                perCodi = int.Parse(reader[20].ToString()),
                                usuCodi = int.Parse(reader[21].ToString()),
                            };

                            DadosUsuarioModel dadosUsuarioModel = new DadosUsuarioModel();
                            dadosUsuarioModel.usuarioModel = retUsuario;
                            dadosUsuarioModel.enderecoModel = retEndereco;
                            dadosUsuarioModel.perfilUsuarioModel = retPerfilUsuario;

                            lstPessoas.Add(dadosUsuarioModel);
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

        public List<DadosUsuEmailModel> ListarUsuarioByEmail(string emailUsuario)
        {
            SqlDataReader reader = null;
            List<DadosUsuEmailModel> lstPessoas = new List<DadosUsuEmailModel>();

            var query = @"SELECT
	                    Usuario.usuCodi,
	                    Usuario.usuNome,
	                    Usuario.usuNCPF,
	                    Usuario.usuNasc,
	                    Usuario.usuEmail,
	                    Usuario.usuDtCad,
	                    Usuario.usuSexo,
	                    PerfilUsuario.perCodi,
	                    Usuario.usuValido
                    FROM [mycash].[dbo].[Usuario] AS Usuario
                    INNER JOIN [mycash].[dbo].[PerfilUsuario] AS PerfilUsuario ON Usuario.usuCodi = PerfilUsuario.usuCodi";

            if (emailUsuario.Length > 0)
            {
                query += " WHERE usuEmail = '" + emailUsuario + "'";
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
                            var ret = new DadosUsuEmailModel()
                            {
                                usuCodi = int.Parse(reader[0].ToString()),
                                usuNome = reader[1].ToString(),
                                usuNCPF = reader[2].ToString(),
                                usuNasc = Convert.ToDateTime(reader[3].ToString()),
                                usuEmail = reader[4].ToString(),
                                usuDtCad = Convert.ToDateTime(reader[5].ToString()),
                                usuSexo = reader[6].ToString(),
                                perCodi = int.Parse(reader[7].ToString()),
                                usuValido = Convert.ToBoolean(reader[8].ToString())
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

        public List<UsuarioModel> ConsultaUsuarioByEmailCPF(string emailCPF)
        {
            SqlDataReader reader = null;
            List<UsuarioModel> lstPessoas = new List<UsuarioModel>();

            var query = @"SELECT * FROM Usuario";

            if (emailCPF.Length > 0)
            {
                if (emailCPF.Contains("@"))
                {
                    query += " WHERE usuEmail = '" + emailCPF + "'";
                }
                else
                {
                    query += " WHERE usuNCPF = '" + emailCPF + "'";
                }
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
                            var ret = new UsuarioModel()
                            {
                                usuCodi = int.Parse(reader[0].ToString()),
                                usuNome = reader[1].ToString(),
                                usuNCPF = reader[2].ToString(),
                                usuNasc = Convert.ToDateTime(reader[3].ToString()),
                                usuEmail = reader[4].ToString(),
                                usuSenha = reader[5].ToString(),
                                usuDtCad = Convert.ToDateTime(reader[6].ToString()),
                                usuFlAt = Convert.ToBoolean(reader[7].ToString()),
                                usuValido = Convert.ToBoolean(reader[8].ToString()),
                                usuSexo = reader[9].ToString(),
                                usuCttEma = Convert.ToBoolean(reader[10].ToString())
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

        //Não estou usando este serviço.
        public string InserirUsuario(UsuarioModel usuarioModel)
        {
            string resp = "";
            int lastId = 0;

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                transaction = connection.BeginTransaction("UsuarioTransaction");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    //Inserindo na tabela Usuario.
                    command.CommandText =
                        @"INSERT INTO [dbo].[Usuario]
                                   ([usuNome],[usuNCPF],[usuNasc],[usuEmail],[usuSenha],[usuDtCad],[usuFlAt],[usuValido])
                             VALUES
                                   ( '" +
                                        usuarioModel.usuNome + "', '" +
                                        usuarioModel.usuNCPF + "', '" +
                                        usuarioModel.usuNasc.ToString("yyyy-MM-dd HH:mm:ss") + "', '" +
                                        usuarioModel.usuEmail + "', '" +
                                        usuarioModel.usuSenha + "', '" +
                                        usuarioModel.usuDtCad.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                        "1, " + //status do usuário sempre será true, pois está sendo criado.
                                        "0 " + //status da validação do usuário sempre será false, até fazer a ativação.
                                  ")";
                    command.ExecuteNonQuery();

                    //Recuperando o último Id
                    command.CommandText = "SELECT MAX([usuCodi]) FROM [dbo].[Usuario]";
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

                    //Inserindo na tabela PerfilUsuario.
                    command.CommandText =
                        @"INSERT INTO [dbo].[PerfilUsuario]
                                   ([perCodi]
                                   ,[usuCodi])
                             VALUES
                                   (" +
                                        "2" + ", " + //Sempre o cadastro será como usuário comum (Usuário Sistema).
                                        lastId +
                                   ")";
                    command.ExecuteNonQuery();

                    transaction.Commit();
                    resp = "OK";
                }
                catch (Exception ex)
                {
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

        public string ManterUsuario(UsuarioModel usuarioModel, EnderecoModel enderecoModel, PerfilUsuarioModel perfilUsuarioModel)
        {
            int lastId = 0;
            string resp = "";
            string senhaCripto = utilitarios.GerarHashString(usuarioModel.usuSenha);

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                transaction = connection.BeginTransaction("UsuarioTransaction");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    if (usuarioModel.usuCodi > 0)
                    {
                        //UPDATE Usuario.
                        command.CommandText =
                            @"UPDATE [dbo].[Usuario] SET 
                               [usuNome] = '" + usuarioModel.usuNome +
                                   "', [usuNasc] = '" + usuarioModel.usuNasc.ToString("yyyy-MM-dd HH:mm:ss") +
                                   "', [usuEmail] = '" + usuarioModel.usuEmail +
                                   (usuarioModel.usuSenha.Length > 0 ? 
                                   "', [usuSenha] = '" + senhaCripto : "")+
                                   "', [usuSexo] = '" + usuarioModel.usuSexo +
                                   "', [usuCttEma] = " + (usuarioModel.usuCttEma ? '1' : '0') +
                             " WHERE [usuCodi] = " + usuarioModel.usuCodi;

                        command.ExecuteNonQuery();

                        //UPDATE Endereço
                        command.CommandText =
                        @"UPDATE [dbo].[Endereco] SET
                              [endNCEP] = '" + enderecoModel.endNCEP +
                              "',[endLogr] = '" + enderecoModel.endLogr +
                              "',[endNume] = '" + enderecoModel.endNume +
                              "',[endCompl] = '" + enderecoModel.endCompl +
                              "',[endBairr] = '" + enderecoModel.endBairr +
                              "',[endCida] = '" + enderecoModel.endCidad +
                              "',[endEsta] = '" + enderecoModel.endEsta +
                         "' WHERE [endCodi] = " + enderecoModel.endCodi;

                        command.ExecuteNonQuery();

                        //UPDATE PerfilUsuario
                        command.CommandText =
                        @"UPDATE [dbo].[PerfilUsuario]
                           SET [perCodi] = " + perfilUsuarioModel.perCodi +
                         " WHERE [usuCodi] = " + usuarioModel.usuCodi;

                        command.ExecuteNonQuery();

                        resp = "OK";
                    }
                    else
                    {
                        //INSERT Usuario.
                        command.CommandText =
                            @"INSERT INTO [dbo].[Usuario]
                                   ([usuNome],[usuNCPF],[usuNasc],[usuEmail],[usuSenha],[usuDtCad],[usuFlAt],[usuValido],[usuSexo],[usuCttEma])
                             VALUES
                                   ( '" +
                                            usuarioModel.usuNome + "', '" +
                                            usuarioModel.usuNCPF + "', '" +
                                            usuarioModel.usuNasc.ToString("yyyy-MM-dd HH:mm:ss") + "', '" +
                                            usuarioModel.usuEmail + "', '" +
                                            senhaCripto + "', '" +
                                            usuarioModel.usuDtCad.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                            "1, " + //status do usuário sempre será true, pois está sendo criado.
                                            "0, '" + //status da validação do usuário sempre será false, até fazer a ativação. 
                                            usuarioModel.usuSexo + "', " +
                                            (usuarioModel.usuCttEma ? '1' : '0') +
                                      ")";
                        command.ExecuteNonQuery();

                        //Recuperando o último Id
                        command.CommandText = "SELECT MAX([usuCodi]) FROM [dbo].[Usuario]";
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

                        //INSERT Endereço
                        command.CommandText =
                        @"INSERT INTO [dbo].[Endereco]
                                   ([usuCodi],[endNCEP],[endLogr],[endNume],[endCompl],[endBairr],[endCida],[endEsta])
                             VALUES
                                   (" +
                                        lastId +
                                        ", '" + enderecoModel.endNCEP +
                                        "', '" + enderecoModel.endLogr +
                                        "', '" + enderecoModel.endNume +
                                        "', '" + enderecoModel.endCompl +
                                        "', '" + enderecoModel.endBairr +
                                        "', '" + enderecoModel.endCidad +
                                        "', '" + enderecoModel.endEsta +
                                        "')";
                        command.ExecuteNonQuery();

                        //Inserindo na tabela PerfilUsuario.
                        command.CommandText =
                            @"INSERT INTO [dbo].[PerfilUsuario]
                                   ([perCodi]
                                   ,[usuCodi])
                             VALUES
                                   (" +
                                            "2" + ", " + //Sempre o cadastro será como usuário comum (Usuário Sistema).
                                            lastId +
                                       ")";
                        command.ExecuteNonQuery();

                        // ->Enviando e-mail para validar o cadastro.
                        string nomeUsuario = usuarioModel.usuNome;
                        int indice = 0;
                        indice = nomeUsuario.IndexOf(" ");
                        nomeUsuario = nomeUsuario.Substring(0, indice);

                        string destinatário = usuarioModel.usuEmail;
                        string assunto = "Ativação de conta MyCash";
                        string mensagem = "Olá " + nomeUsuario + ", \n\n Clique no link http://mycashapp.gearhostpreview.com/api/Usuario/AtivaUsuario/" + lastId + " para ativar sua conta. \n\n\n Caso não tenha realizado o cadastro no site, desconsidere este e-mail.";

                        UtilitariosController utilitariosController = new UtilitariosController();
                        resp = utilitariosController.EnviarEmail(destinatário, assunto, mensagem);
                    }

                    transaction.Commit();
                    connection.Close();
                }
                catch (Exception ex)
                {
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

        public string AtivaUsuario(int idUsuario)
        {
            string resp = "";

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();

                command.Connection = connection;

                try
                {
                    if (idUsuario > 0)
                    {
                        //UPDATE do Status do Usuario.
                        command.CommandText =
                            @"UPDATE [dbo].[Usuario] SET 
                               [usuValido] = 1
                              WHERE [usuCodi] = " + idUsuario;

                        command.ExecuteNonQuery();
                    }

                    resp = "OK";
                    connection.Close();
                }
                catch (Exception ex)
                {
                    connection.Close();
                    resp = "Erro ao inserir no banco de dados: " + ex.GetType() +
                        " | Mensagem: " + ex.Message;
                }
            }

            return resp;
        }
    }
}