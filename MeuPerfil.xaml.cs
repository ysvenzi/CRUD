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

    private void BtnDeleteProfile_OnClick(object sender, RoutedEventArgs e)
    {
        var resultadoMessageBox = MessageBox.Show("Você tem certeza que deseja apagar o seu perfil?",
            "Confirmação de Exclusão", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (resultadoMessageBox == MessageBoxResult.No) return;

        // Criar uma query
        const string query = "DELETE FROM usuarios WHERE id = @id";
        // Criar a conexao
        using var conexao = new MySqlConnection(App.stringConexao);
        // Criar o comando
        using var comando = new MySqlCommand(query, conexao);
        // Adicionar os parametros
        comando.Parameters.AddWithValue("@id", UsuarioAtual.Id);
        try
        {
            // Abrir conexao
            conexao.Open();
            // Executar o comando
            var linhasAfetadas = comando.ExecuteNonQuery();
            // Verificar se o comando foi executado
            if (linhasAfetadas > 0)
            {
                MessageBox.Show("Perfil deletado com sucesso!");
                // Se ele foi executado, fechar a janela MeuPerfil
                Close();
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}
    

