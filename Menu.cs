using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Banco
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btnPizza_Click(object sender, EventArgs e)
        {
            var pizza = new CadastroPizza();
            pizza.Show();
            Hide();
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            var cliente = new CadastroCliente();
            cliente.Show();
            Hide();
        }

        private void btnPedido_Click(object sender, EventArgs e)
        {
            var pedido = new CadastroPedido();
            pedido.Show();
            Hide();
        }
    }
}
