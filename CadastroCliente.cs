using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Banco
{
    public partial class CadastroCliente : Form
    {
        MySqlConnection conexao;
        string strConexao = new Conexao().ObterConexao();
        string tabela = "Clientes";
        string id = "clienteId";

        public CadastroCliente()
        {
            InitializeComponent();
        }

        private void Limpar()
        {
            Codigo.Clear();
            Nome.Clear();
            CPF.Clear();
            Endereco.Clear();
            Celular.Clear();
            Nome.Focus();

            btnAtualizar.Enabled = false;
            btnExcluir.Enabled = false;
            CPF.Enabled = true;
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            conexao = new MySqlConnection(strConexao);
            try
            {
                conexao.Open();
                var adpCliente = new MySqlDataAdapter();
                var tbClientes = new DataTable();

                var nome = Nome.Text == "" ? throw new Exception("Nome não pode ficar em branco") : Nome.Text;
                var cpf = CPF.Text == "" ? throw new Exception("CPF não pode ficar em branco") : CPF.Text;
                var endereco = Endereco.Text == "" ? null : Endereco.Text;
                var celular = Celular.Text == "" ? null : Celular.Text;


                var sql = $"INSERT INTO {tabela} VALUES (0, '{nome}', '{cpf}', '{endereco}', '{celular}')";
                var adiciona = new MySqlCommand(sql, conexao);
                adpCliente.SelectCommand = adiciona;
                adpCliente.Fill(tbClientes);
                MessageBox.Show("Registro Salvo com Sucesso!", "SALVANDO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var cmdCliente = new MySqlCommand($"SELECT * FROM Clientes ORDER BY {id}", conexao);
                var da = new MySqlDataAdapter();
                da.SelectCommand = cmdCliente;
                da.Fill(tbClientes);
                dataGridView1.DataSource = tbClientes;

                Limpar();
            }
            catch (Exception E)
            {
                var msg = E.Message;
                if (msg.Contains("Duplicate"))
                    msg = "Cliente já cadastrado";

                MessageBox.Show(msg);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            Codigo.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            Nome.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            CPF.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            Endereco.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            Celular.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();

            btnAtualizar.Enabled = true;
            btnExcluir.Enabled = true;
            CPF.Enabled = false;
        }

        private void CadastroCliente_Load(object sender, EventArgs e)
        {
            conexao = new MySqlConnection(strConexao);
            try
            {
                conexao.Open();
                var cmdCliente = new MySqlCommand($"SELECT * FROM {tabela} ORDER BY {id}", conexao);
                var da = new MySqlDataAdapter();
                da.SelectCommand = cmdCliente;
                var tbCliente = new DataTable();
                da.Fill(tbCliente);

                Codigo.DataBindings.Add("Text", tbCliente, "clienteId");
                Nome.DataBindings.Add("Text", tbCliente, "nome");
                CPF.DataBindings.Add("Text", tbCliente, "cpf");
                Endereco.DataBindings.Add("Text", tbCliente, "endereco");
                Celular.DataBindings.Add("Text", tbCliente, "celular");

                dataGridView1.DataSource = tbCliente;

                dataGridView1.Columns[0].HeaderText = "Código";
                dataGridView1.Columns[1].HeaderText = "Nome";
                dataGridView1.Columns[2].HeaderText = "CPF";
                dataGridView1.Columns[3].HeaderText = "Endereço";
                dataGridView1.Columns[4].HeaderText = "Celular";

                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = true;

            }

            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }

            finally
            {
                conexao.Close();
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            conexao = new MySqlConnection(strConexao);
            if (MessageBox.Show("Tem certeza que deseja excluir esse registro?", "Cuidado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                MessageBox.Show("Operação cancelada");
            }
            else
            {
                try
                {
                    conexao.Open();
                    var adpPizza = new MySqlDataAdapter();
                    var tbCliente = new DataTable();
                    var exclui = new MySqlCommand($"Delete From {tabela} Where {id} =" + Convert.ToInt16(Codigo.Text), conexao);
                    adpPizza.SelectCommand = exclui;
                    adpPizza.Fill(tbCliente);
                    MessageBox.Show("Registro excluído com sucesso!", "EXCLUINDO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var cmdCliente = new MySqlCommand($"SELECT * FROM {tabela} ORDER BY {id}", conexao);
                    var da = new MySqlDataAdapter();
                    da.SelectCommand = cmdCliente;
                    da.Fill(tbCliente);
                    dataGridView1.DataSource = tbCliente;
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message);
                }
                finally
                {
                    conexao.Close();
                }
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            conexao = new MySqlConnection(strConexao);
            try
            {
                conexao.Open();
                var adpPizza = new MySqlDataAdapter();
                var tbCliente = new DataTable();
                
                var codigo = Convert.ToInt32(Codigo.Text);
                var nome = Nome.Text == "" ? throw new Exception("Nome não pode ficar em branco") : Nome.Text;
                var endereco = Endereco.Text == "" ? null : Endereco.Text;
                var celular = Celular.Text == "" ? null : Celular.Text;

                var sqlpizza = $"UPDATE {tabela} SET nome = '{nome}', endereco = '{endereco}', celular = '{celular}' WHERE {id} = {codigo}";
                var updateFornecedor = new MySqlCommand(sqlpizza, conexao);
                adpPizza.SelectCommand = updateFornecedor;
                adpPizza.Fill(tbCliente);
                MessageBox.Show("Registro Alterado com sucesso!", "ALTERANDO", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                var cmdCliente = new MySqlCommand($"SELECT * FROM {tabela} ORDER BY {id}", conexao);
                var da = new MySqlDataAdapter();
                da.SelectCommand = cmdCliente;
                da.Fill(tbCliente);
                dataGridView1.DataSource = tbCliente;
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            var menu = new Menu();
            menu.Show();
            Hide();
        }
    }
}
