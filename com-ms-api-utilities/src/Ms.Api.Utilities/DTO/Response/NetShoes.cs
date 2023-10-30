using Ms.Api.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms.Api.Utilities.DTO.Response
{
    public class NetShoes_ResultPedido
    {

        private int CodigoPedidoField;

        private string MensagemField;

        private bool SucessoField;

        public int CodigoPedido
        {
            get
            {
                return this.CodigoPedidoField;
            }
            set
            {
                this.CodigoPedidoField = value;
            }
        }

        public string Mensagem
        {
            get
            {
                return this.MensagemField;
            }
            set
            {
                this.MensagemField = value;
            }
        }
        public bool Sucesso
        {
            get
            {
                return this.SucessoField;
            }
            set
            {
                this.SucessoField = value;
            }
        }
    }
}
