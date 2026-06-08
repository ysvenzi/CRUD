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
        if (string.IsNullOrWhiteSpace(txtProfileName.Text))
        {
            MessageBox.Show("O campo NOME não pode estar vazio.");
            txtProfileName.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(txtProfileEmail.Text))
        {
            MessageBox.Show("O campo EMAIL não pode estar vazio.");
            txtProfileEmail.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(txtProfileUsername.Text))
        {
            MessageBox.Show("O campo USERNAME não pode estar vazio.");
            txtProfileUsername.Focus();
            return;
        }

        var senhaFoiAlterada = !string.IsNullOrWhiteSpace(txtProfilePassword.Password);

        UsuarioAtual.Username = txtProfileUsername.Text;
        UsuarioAtual.Nome = txtProfileName.Text;
        UsuarioAtual.Email = txtProfileEmail.Text;
        if (senhaFoiAlterada) UsuarioAtual.Senha = txtProfilePassword.Password;

        using var conexao = new MySqlConnection(App.stringConexao);
        var query = "UPDATE usuarios SET username = @username, nome = @nome, email = @email";

        if (senhaFoiAlterada) query += ", senha = @senha";

        query += " WHERE id = @id";

        using var comando = new MySqlCommand(query, conexao);

        comando.Parameters.AddWithValue("@username", UsuarioAtual.Username);
        comando.Parameters.AddWithValue("@nome", UsuarioAtual.Nome);
        comando.Parameters.AddWithValue("@email", UsuarioAtual.Email);
        comando.Parameters.AddWithValue("@id", UsuarioAtual.Id);

        if (senhaFoiAlterada) comando.Parameters.AddWithValue("@senha", UsuarioAtual.Senha);
        
        try
        {
            conexao.Open();
            var linhasAfetadas = comando.ExecuteNonQuery();

            if (linhasAfetadas > 0)
                MessageBox.Show("Cadastro atualizado com sucesso!");
            else
                MessageBox.Show("Erro ao atualizar o cadastro!");
        }
        catch (Exception exception)
        {
            MessageBox.Show("Erro de DB.");
        }
    }
}