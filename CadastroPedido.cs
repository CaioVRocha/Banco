using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Banco
{
    public partial class CadastroPedido : Form
    {
        MySqlConnection conexao;
        string strConexao = new Conexao().ObterConexao();
        string tabela = "Pedidos";
        string id = "pedidoId";

        public CadastroPedido()
        {
            InitializeComponent();
        }

        private void Limpar()
        {
            Codigo.Clear();
            listCliente.SelectedIndex = -1;
            listPizzas.SelectedIndex = -1;
            Quantidade.Text = "0";
            ValorFinal.Text = "0";

            btnExcluir.Enabled = false;
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            conexao = new MySqlConnection(strConexao);
            try
            {
                conexao.Open();
                var adpPedido = new MySqlDataAdapter();
                var tbPedidos = new DataTable();

                var cliente = listCliente.Text == "" ? throw new Exception("Selecione o cliente") : listCliente.Text;
                var pizza = listPizzas.Text == "" ? throw new Exception("Selecione a pizza") : listPizzas.Text;
                var quantidade = Quantidade.Text == "" || Quantidade.Text == "0" ? throw new Exception("Preencha o campo quantidade") : Convert.ToInt32(Quantidade.Text);

                #region Pizza

                var cmdPizza = new MySqlCommand($"SELECT * FROM pizzas WHERE sabor = '{pizza}'", conexao);
                var dp = new MySqlDataAdapter();
                dp.SelectCommand = cmdPizza;
                var pizzas = new DataTable();
                dp.Fill(pizzas);

                var codigoPizza = "";
                var precoPizza = 0.0;

                for (int i = 0; i < pizzas.Rows.Count; i++)
                {
                    var item = pizzas.Rows[i].ItemArray;
                    codigoPizza = item[0].ToString();
                    precoPizza = Convert.ToDouble(item[3].ToString());
                }
                #endregion Pizza

                ValorFinal.Text = (precoPizza * quantidade).ToString();
                var valorFinal = ValorFinal.Text == "" ? 0.0 : Convert.ToDouble(ValorFinal.Text.Replace(".", ","));

                #region Cliente

                var cmdCliente = new MySqlCommand($"SELECT * FROM clientes WHERE nome = '{cliente}'", conexao);
                var dc = new MySqlDataAdapter();
                dc.SelectCommand = cmdCliente;
                var clientes = new DataTable();
                dc.Fill(clientes);

                var codigoCliente = "";

                for (int i = 0; i < clientes.Rows.Count; i++)
                {
                    var item = clientes.Rows[i].ItemArray;
                    codigoCliente = item[0].ToString();
                }
                #endregion Cliente

                var sql = $"INSERT INTO {tabela} VALUES (0, '{codigoCliente}', '{codigoPizza}', '{quantidade}', '{valorFinal.ToString().Replace(",", ".")}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
                var adiciona = new MySqlCommand(sql, conexao);
                adpPedido.SelectCommand = adiciona;
                adpPedido.Fill(tbPedidos);
                MessageBox.Show("Registro Salvo com Sucesso!", "SALVANDO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var cmdPedido = new MySqlCommand($"SELECT * FROM {tabela} ORDER BY {id}", conexao);
                var da = new MySqlDataAdapter();
                da.SelectCommand = cmdPedido;
                da.Fill(tbPedidos);
                dataGridView1.DataSource = tbPedidos;

                Limpar();
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            Codigo.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            #region Pizza

            var cmdPizza = new MySqlCommand($"SELECT * FROM pizzas WHERE pizzaId = '{dataGridView1.CurrentRow.Cells[2].Value}'", conexao);
            var dp = new MySqlDataAdapter();
            dp.SelectCommand = cmdPizza;
            var pizzas = new DataTable();
            dp.Fill(pizzas);

            var saborPizza = "";

            for (int i = 0; i < pizzas.Rows.Count; i++)
            {
                var item = pizzas.Rows[i].ItemArray;
                saborPizza = item[1].ToString();
            }
            #endregion Pizza

            #region Cliente

            var cmdCliente = new MySqlCommand($"SELECT * FROM clientes WHERE clienteId = '{dataGridView1.CurrentRow.Cells[1].Value}'", conexao);
            var dc = new MySqlDataAdapter();
            dc.SelectCommand = cmdCliente;
            var clientes = new DataTable();
            dc.Fill(clientes);

            var nomeCliente = "";

            for (int i = 0; i < clientes.Rows.Count; i++)
            {
                var item = clientes.Rows[i].ItemArray;
                nomeCliente = item[1].ToString();
            }
            #endregion Cliente

            listPizzas.Text = saborPizza;
            listCliente.Text = nomeCliente;

            Quantidade.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            ValorFinal.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();

            btnExcluir.Enabled = true;
        }

        private void CadastroPedido_Load(object sender, EventArgs e)
        {
            conexao = new MySqlConnection(strConexao);
            try
            {
                #region Cliente

                var cmdCliente = new MySqlCommand("SELECT * FROM clientes ORDER BY nome", conexao);
                var dc = new MySqlDataAdapter();
                dc.SelectCommand = cmdCliente;
                var clientes = new DataTable();
                dc.Fill(clientes);

                for (int i = 0; i < clientes.Rows.Count; i++)
                {
                    var item = clientes.Rows[i].ItemArray;
                    listCliente.Items.Add(item[1].ToString());
                }
                #endregion Cliente

                #region Pizzas

                var cmdPizza = new MySqlCommand("SELECT * FROM pizzas ORDER BY sabor", conexao);
                var dpz = new MySqlDataAdapter();
                dpz.SelectCommand = cmdPizza;
                var pizzas = new DataTable();
                dpz.Fill(pizzas);

                for (int i = 0; i < pizzas.Rows.Count; i++)
                {
                    var item = pizzas.Rows[i].ItemArray;
                    listPizzas.Items.Add(item[1].ToString());
                }
                #endregion Pizzas

                conexao.Open();
                var cmdPedido = new MySqlCommand($"SELECT * FROM {tabela} ORDER BY {id}", conexao);
                var dp = new MySqlDataAdapter();
                dp.SelectCommand = cmdPedido;
                var tbPedidos = new DataTable();
                dp.Fill(tbPedidos);

                Codigo.DataBindings.Add("Text", tbPedidos, "pizzaId");
                //listCliente.DataBindings.Add("Text", tbPedidos, "clienteId");
                //listPizzas.DataBindings.Add("Text", tbPedidos, "pizzaId");
                Quantidade.DataBindings.Add("Text", tbPedidos, "quantidade");
                ValorFinal.DataBindings.Add("Text", tbPedidos, "valorFinal");

                dataGridView1.DataSource = tbPedidos;

                dataGridView1.Columns[0].HeaderText = "Código";
                dataGridView1.Columns[1].HeaderText = "Cliente";
                dataGridView1.Columns[2].HeaderText = "Pizza";
                dataGridView1.Columns[3].HeaderText = "Quantidade";
                dataGridView1.Columns[4].HeaderText = "Preço Final";
                dataGridView1.Columns[5].HeaderText = "Data Pedido";

                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[5].ReadOnly = true;
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

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            var menu = new Menu();
            menu.Show();
            Hide();
        }

        private void btnExcluir_Click_1(object sender, EventArgs e)
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
    }
}
