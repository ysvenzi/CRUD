using System.Windows;
using CRUD.Modelos;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class MeuPerfil : Window
{
    private Usuario UsuarioAtual;
    public MeuPerfil(Usuario usuario) 
    {
        InitializeComponent();
        UsuarioAtual = usuario;
        txtProfileName.Text = UsuarioAtual.Nome;
        txtProfileEmail.Text = UsuarioAtual.Email;
        txtProfileUsername.Text = UsuarioAtual.Username;
    }

    private void BtnSalvar_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtProfileName.Text) ||
            string.IsNullOrWhiteSpace(txtProfileEmail.Text) ||
            string.IsNullOrWhiteSpace(txtProfileUsername.Text))
        {
            MessageBox.Show("Campos de Nome, Email, Senha e Username são obrigatórios!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        using var conexao = new MySqlConnection(App.stringConexao);
        const string query = "";
    }
}