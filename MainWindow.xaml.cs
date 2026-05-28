using System.Configuration;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace CRUD;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public string stringConexao = Environment.GetEnvironmentVariable("MYSQL_STRING");
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private void BtnCadastrar_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNome.Text) ||
            string.IsNullOrWhiteSpace(txtEmail.Text) ||
            string.IsNullOrWhiteSpace(txtSenha.Password) ||
            string.IsNullOrWhiteSpace(txtUser.Text))
        {
            MessageBox.Show("Todos os campos são obrigatóros",  "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        using (var conexao = new MySqlConnection(stringConexao))
        {
            var query = "INSERT INTO usuarios(nome, email, senha, username) VALUES(@nome, @email, @senha, @username)";

            using (var comando = new MySqlCommand(query, conexao))
            {
                comando.Parameters.AddWithValue("@nome", txtNome.Text);
                comando.Parameters.AddWithValue("@email", txtEmail.Text);
                comando.Parameters.AddWithValue("@senha", txtSenha.Password);
                comando.Parameters.AddWithValue("@username", txtUser.Text);

                try
                {
                    conexao.Open();
                    var linhasAfetadas = comando.ExecuteNonQuery();
                    if (linhasAfetadas > 0)
                    {
                        MessageBox.Show("Cadastro realizado!");
                    }
                }
                catch (Exception exception)
                {
                    if (exception is MySqlException erroSql)
                    {
                        if (erroSql.Number == 1062)
                        {
                            MessageBox.Show("O email ou username já foram ultilizados!");
                            return;
                        }
                    }
                    
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }
    }
}