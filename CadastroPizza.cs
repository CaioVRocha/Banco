using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Banco
{
    public partial class CadastroPizza : Form
    {
        MySqlConnection conexao;
        string strConexao = new Conexao().ObterConexao();
        string tabela = "Pizzas";
        string id = "pizzaId";

        public CadastroPizza()
        {
            InitializeComponent();
        }

        private void Limpar()
        {
            Codigo.Clear();
            Sabor.Clear();
            Descricao.Clear();
            Preco.Clear();
            Sabor.Focus();

            btnAtualizar.Enabled = false;
            btnExcluir.Enabled = false;
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            conexao = new MySqlConnection(strConexao);
            try
            {
                conexao.Open();
                var adpPizza = new MySqlDataAdapter();
                var tbPizzas = new DataTable();

                var sabor = Sabor.Text == "" ? throw new Exception("Sabor não pode ficar em branco") : Sabor.Text;
                var descricao = Descricao.Text == "" ? throw new Exception("Descrição não pode ficar em branco") : Descricao.Text;
                var preco = Preco.Text == "" ? 0.0 : Convert.ToDouble(Preco.Text.Replace(".", ","));


                var sql = $"INSERT INTO {tabela} VALUES (0, '{sabor}', '{descricao}', {preco.ToString().Replace(",", ".")})";
                var adiciona = new MySqlCommand(sql, conexao);
                adpPizza.SelectCommand = adiciona;
                adpPizza.Fill(tbPizzas);
                MessageBox.Show("Registro Salvo com Sucesso!", "SALVANDO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var cmdPizza = new MySqlCommand($"SELECT * FROM {tabela} ORDER BY {id}", conexao);
                var da = new MySqlDataAdapter();
                da.SelectCommand = cmdPizza;
                da.Fill(tbPizzas);
                dataGridView1.DataSource = tbPizzas;

                Limpar();
            }
            catch (Exception E)
            {
                var msg = E.Message;
                if (msg.Contains("Duplicate"))
                    msg = "Sabor de pizza já cadastrado";

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
            Sabor.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            Descricao.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            Preco.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();

            btnAtualizar.Enabled = true;
            btnExcluir.Enabled = true;
        }

        private void CadastroPizza_Load(object sender, EventArgs e)
        {
            conexao = new MySqlConnection(strConexao);
            try
            {
                conexao.Open();
                var cmdPizza = new MySqlCommand($"SELECT * FROM {tabela} ORDER BY {id}", conexao);
                var da = new MySqlDataAdapter();
                da.SelectCommand = cmdPizza;
                var tbPizza = new DataTable();
                da.Fill(tbPizza);

                Codigo.DataBindings.Add("Text", tbPizza, "pizzaId");
                Sabor.DataBindings.Add("Text", tbPizza, "sabor");
                Descricao.DataBindings.Add("Text", tbPizza, "descricao");
                Preco.DataBindings.Add("Text", tbPizza, "preco");

                dataGridView1.DataSource = tbPizza;

                dataGridView1.Columns[0].HeaderText = "Código";
                dataGridView1.Columns[1].HeaderText = "Sabor";
                dataGridView1.Columns[2].HeaderText = "Descrição";
                dataGridView1.Columns[3].HeaderText = "Preço";

                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].ReadOnly = true;

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
                    var tbPizza = new DataTable();
                    var exclui = new MySqlCommand($"Delete From {tabela} Where {id} =" + Convert.ToInt16(Codigo.Text), conexao);
                    adpPizza.SelectCommand = exclui;
                    adpPizza.Fill(tbPizza);
                    MessageBox.Show("Registro excluído com sucesso!", "EXCLUINDO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var cmdPizza = new MySqlCommand($"SELECT * FROM {tabela} ORDER BY {id}", conexao);
                    var da = new MySqlDataAdapter();
                    da.SelectCommand = cmdPizza;
                    da.Fill(tbPizza);
                    dataGridView1.DataSource = tbPizza;
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
                var tbPizza = new DataTable();
                
                var codigo = Convert.ToInt32(Codigo.Text);
                var sabor = Sabor.Text == "" ? throw new Exception("Sabor não pode ficar em branco") : Sabor.Text;
                var descricao = Descricao.Text == "" ? throw new Exception("Descrição não pode ficar em branco") : Descricao.Text;
                var preco = Preco.Text == "" ? 0.0 : Convert.ToDouble(Preco.Text.Replace(".", ","));

                var sqlpizza = $"UPDATE {tabela} SET sabor = '{sabor}', descricao = '{descricao}', preco = '{preco.ToString().Replace(",", ".")}' WHERE {id} = {codigo}";
                var updateFornecedor = new MySqlCommand(sqlpizza, conexao);
                adpPizza.SelectCommand = updateFornecedor;
                adpPizza.Fill(tbPizza);
                MessageBox.Show("Registro Alterado com sucesso!", "ALTERANDO", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                var cmdPizza = new MySqlCommand($"SELECT * FROM {tabela} ORDER BY {id}", conexao);
                var da = new MySqlDataAdapter();
                da.SelectCommand = cmdPizza;
                da.Fill(tbPizza);
                dataGridView1.DataSource = tbPizza;
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
