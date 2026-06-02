using System.Windows;
using CRUD.Modelos;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void BtnLogin_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUsername.Text))
        {
            MessageBox.Show("Preencha o Nome de usuário!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            txtUsername.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(txtPassword.Password))
        {
            MessageBox.Show("Preencha a Senha!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            txtPassword.Focus();
            return;
        }

        using var conexao = new MySqlConnection(App.stringConexao);
        const string query = "SELECT * FROM usuarios WHERE username = @username AND senha = @senha";

        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("@username", txtUsername.Text);
        comando.Parameters.AddWithValue("@senha", txtPassword.Password);
                
        try
        {
            conexao.Open();
            using var leitor = comando.ExecuteReader();
            if (!leitor.HasRows)
            {
                MessageBox.Show("Usuário e/ou Senha estão errados!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            while (leitor.Read())
            {
                var usuarioBanco = new Usuario();
                
                usuarioBanco.Id = leitor.GetInt32(0);
                usuarioBanco.Nome = leitor.GetString(1);
                usuarioBanco.Email = leitor.GetString(2);
                usuarioBanco.Senha = leitor.GetString(3);
                usuarioBanco.Username = leitor.GetString(4);
                
                new MeuPerfil(usuarioBanco).Show();
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return;
        }
    }

    private void BtnRegister_OnClick(object sender, RoutedEventArgs e)
    {
        Cadastro cadastro = new Cadastro();
        Hide();
        cadastro.ShowDialog();
        Show();
    }
}