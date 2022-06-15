using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banco
{
    public class Conexao
    {
        public string ObterConexao()
        {
            return "server=localhost; database=Pizzaria; uid=root; password=C&v1608R;";
        }
    }
}
