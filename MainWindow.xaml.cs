using System.Windows;
using MySql.Data.MySqlClient;
using Mysqlx.Cursor;

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

        using (var conexao = new MySqlConnection(App.stringConexao))
        {
            var query = "SELECT * FROM usuarios WHERE username = @username AND senha = @senha";

            using (var comando = new MySqlCommand(query, conexao))
            {
                comando.Parameters.AddWithValue("@username", txtUsername.Text);
                comando.Parameters.AddWithValue("@senha", txtPassword.Password);
                
                try
                {
                    conexao.Open();
                    using (var leitor = comando.ExecuteReader())
                    {
                        if (!leitor.HasRows)
                        {
                            MessageBox.Show("Usuário e/ou Senha estão errados!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        while (leitor.Read())
                        {
                            MessageBox.Show(leitor.GetString(1));
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    return;
                }
            }
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