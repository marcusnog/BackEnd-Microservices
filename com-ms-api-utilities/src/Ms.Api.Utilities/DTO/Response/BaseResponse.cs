using Ms.Api.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms.Api.Utilities.DTO.Response
{
//    public class BaseResponse
//    {
//    }

    #region Order Generico

    public class BaseResponse
    {
        public BaseResponse()
        {
            this.ProtocoloCatalogoService = Guid.NewGuid().ToString();
        }

        public bool Sucesso { get; set; }
        public bool Warning { get; set; }
        //public Enums.TipoDeErro? TipoDeErro { get; set; }
        //public Enums.CodigoErro? CodigoDoErro { get; set; }
        public string MensagemDeErro { get; set; }
        public string MensagemDeErroEmPontos { get; set; }


        /// <summary>
        /// Gerado pelo serviço
        /// </summary>
        public string ProtocoloCatalogoService { get; set; }

        /// <summary>
        /// Vem da NovaPontoCom
        /// </summary>
        public string Protocolo { get; set; }

        /// <summary>
        /// Vem do cliente que utiliza esse serviço
        /// </summary>
        public string ProtocoloCliente { get; set; }
    }


    #endregion Order Generico
}
