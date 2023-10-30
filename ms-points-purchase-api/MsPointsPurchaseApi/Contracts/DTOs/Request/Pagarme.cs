using Newtonsoft.Json.Linq;
using MsPointsPurchaseApi.Contracts.DTOs.Response;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace MsPointsPurchaseApi.Contracts.DTOs.Request
{
    public class PagarMeReverseOrderRequest
    {
        public string charge_id { get; set; }
        public int amount { get; set; }
    }

    public struct ED_Retorno_API_Pagamento
    {
        public Boolean SUCESSO;
        public String MENSAGEM_ERRO;
        public PagarMeCreateOrderRequest oED_Request;
        public PagarMeCreateOrderResponse oED_Response;
        public Decimal Valor;
        public Decimal ValorFrete;
        public PagarMeReverseOrderRequest oED_Reverse_Request;
        public PagarMeReverseOrderResponse oED_Reverse_Response;
        //public Int32 amount { get; set; }
        public Int32 VALOR_TRANSACAO;

        #region Pedido

        public Boolean Tem_ID_TRANSACAO
        {
            get
            {
                Boolean retorno = false;

                if (SUCESSO)
                {
                    if (oED_Response != null && oED_Response.charges != null && oED_Response.charges.Count > 0 && !String.IsNullOrEmpty(oED_Response.charges[0].id))
                    {
                        retorno = true;
                    }
                }

                return retorno;
            }
        }

        public String ID_TRANSACAO
        {
            get
            {
                String retorno = String.Empty;

                if (SUCESSO)
                {
                    if (oED_Response != null && oED_Response.charges != null && oED_Response.charges.Count > 0 && !String.IsNullOrEmpty(oED_Response.charges[0].id))
                    {
                        retorno = oED_Response.charges[0].id;
                    }
                }

                return retorno;
            }
        }

        private Boolean TemErro(PagarMeLastTransactionResponse oPagarMeLastTransactionResponse)
        {
            Boolean retorno = true;

            if (oPagarMeLastTransactionResponse.gateway_response.code.Equals("200"))
            {
                if (oPagarMeLastTransactionResponse.success)
                {
                    if (!String.IsNullOrEmpty(oPagarMeLastTransactionResponse.antifraud_response.status) && oPagarMeLastTransactionResponse.antifraud_response.status.Equals("approved"))
                    {
                        retorno = false;
                    }
                }
            }

            return retorno;
        }

        private Boolean VerificaResponse_TemErro_Charges()
        {
            Boolean retorno = false;

            if (oED_Response != null)
            {
                if (oED_Response.charges != null && oED_Response.charges.Count > 0)
                {
                    if (oED_Response.charges[0] != null)
                    {
                        retorno = true;
                    }
                }
            }

            return retorno;
        }

        private ED_Retorno_API_Pagamento_Erro_Generico VerificaResponse_TemErro_Errors()
        {
            ED_Retorno_API_Pagamento_Erro_Generico oRetorno = new ED_Retorno_API_Pagamento_Erro_Generico();
            oRetorno.ERRO = false;
            oRetorno.MENSAGEM = String.Empty;

            if (oED_Response != null)
            {
                if (oED_Response.errors != null)
                {
                    ArrayList listaErro = new ArrayList();

                    JObject jObject = JObject.Parse(oED_Response.errors.ToString());

                    if (jObject.Count > 0)
                    {
                        foreach (var x in jObject)
                        {
                            foreach (String valor in x.Value)
                            {
                                listaErro.Add(valor);
                            }
                        }
                    }

                    if (listaErro.Count > 0)
                    {
                        oRetorno.ERRO = true;
                        oRetorno.MENSAGEM = Convert.ToString(listaErro[0]);
                    }
                }
            }

            return oRetorno;
        }

        public Boolean Tem_Erro_API_Pedido
        {
            get
            {
                Boolean retorno = false;

                if (VerificaResponse_TemErro_Charges())
                {
                    retorno = TemErro(oED_Response.charges[0].last_transaction);
                }
                else
                {
                    ED_Retorno_API_Pagamento_Erro_Generico oED_Retorno_API_Pagamento_Erro_Generico = VerificaResponse_TemErro_Errors();
                    retorno = oED_Retorno_API_Pagamento_Erro_Generico.ERRO;
                }

                return retorno;
            }
        }

        public String Erro_API_Pedido
        {
            get
            {
                String retorno = String.Empty;

                if (VerificaResponse_TemErro_Charges())
                {
                    PagarMeLastTransactionResponse oPagarMeLastTransactionResponse = oED_Response.charges[0].last_transaction;
                    if (TemErro(oPagarMeLastTransactionResponse))
                    {
                        if (oPagarMeLastTransactionResponse.gateway_response.errors != null && oPagarMeLastTransactionResponse.gateway_response.errors.Count > 0)
                        {
                            String retornoTemp = String.Empty;

                            foreach (PagarMeGatewayErrorResponse oED in oPagarMeLastTransactionResponse.gateway_response.errors)
                            {
                                if (!String.IsNullOrEmpty(oED.message))
                                {
                                    retornoTemp = String.Concat(retornoTemp, oED.message, " | ");
                                }
                            }

                            if (!String.IsNullOrEmpty(retornoTemp))
                            {
                                retornoTemp = retornoTemp.Substring(0, retornoTemp.Length - 3);
                                retorno = retornoTemp;
                            }

                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(oPagarMeLastTransactionResponse.status))
                                retorno = oPagarMeLastTransactionResponse.status;
                        }
                    }
                }
                else
                {
                    ED_Retorno_API_Pagamento_Erro_Generico oED_Retorno_API_Pagamento_Erro_Generico = VerificaResponse_TemErro_Errors();
                    retorno = oED_Retorno_API_Pagamento_Erro_Generico.MENSAGEM;
                }

                retorno = TrataTraducaoErro(retorno);

                return retorno;
            }
        }

        private String TrataTraducaoErro(String valor)
        {
            String retorno = valor;

            if (!String.IsNullOrEmpty(valor))
            {
                //TraducaoErroBusiness oTraducaoErroBusiness = new TraducaoErroBusiness();
                //retorno = oTraducaoErroBusiness.Processa(valor);
                //oTraducaoErroBusiness = null;
            }

            return retorno;
        }

        #endregion Pedido

        #region Estorno

        public String ID_TRANSACAO_ESTORNO
        {
            get
            {
                String retorno = String.Empty;

                if (SUCESSO)
                {
                    if (oED_Reverse_Response != null && !String.IsNullOrEmpty(oED_Reverse_Response.id))
                    {
                        retorno = oED_Reverse_Response.id;
                    }
                }

                return retorno;
            }
        }

        public Boolean Tem_Erro_API_Estorno
        {
            get
            {
                Boolean retorno = false;

                if (oED_Reverse_Response != null)
                {
                    retorno = TemErro(oED_Reverse_Response.last_transaction);
                }

                return retorno;
            }
        }

        public String Erro_API_Estorno
        {
            get
            {
                String retorno = String.Empty;

                if (oED_Reverse_Response != null)
                {
                    PagarMeLastTransactionResponse oPagarMeLastTransactionResponse = oED_Response.charges[0].last_transaction;
                    if (TemErro(oPagarMeLastTransactionResponse))
                    {
                        if (oPagarMeLastTransactionResponse.gateway_response.errors != null && oPagarMeLastTransactionResponse.gateway_response.errors.Count > 0)
                        {
                            String retornoTemp = String.Empty;

                            foreach (PagarMeGatewayErrorResponse oED in oPagarMeLastTransactionResponse.gateway_response.errors)
                            {
                                if (!String.IsNullOrEmpty(oED.message))
                                {
                                    retornoTemp = String.Concat(retornoTemp, oED.message, " | ");
                                }
                            }

                            if (!String.IsNullOrEmpty(retornoTemp))
                            {
                                retornoTemp = retornoTemp.Substring(0, retornoTemp.Length - 3);
                                retorno = retornoTemp;
                            }

                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(oPagarMeLastTransactionResponse.status))
                                retorno = oPagarMeLastTransactionResponse.status;
                        }
                    }
                }

                return retorno;
            }
        }

        #endregion Estorno

        #region Auxiliar

        public struct ED_Retorno_API_Pagamento_Erro_Generico
        {
            public Boolean ERRO;
            public String MENSAGEM;
        }

        #endregion Auxiliar
    }

    public class PagarMeCreateOrderRequest
    {
        [Required(ErrorMessage = "Informe os dados do cliente.")]
        public PagarMeCustomerOrder customer { get; set; }
        [Required(ErrorMessage = "Informe os itens do pedido.")]
        public List<PagarMeProductsOrder> items { get; set; }
        [Required(ErrorMessage = "Informe os dados do pagamento.")]
        public List<PagarMePaymentsOrder> payments { get; set; }
        public bool closed { get; set; }
    }

    public class PagarMeCustomerOrder
    {
        [Required(ErrorMessage = "Informe o nome do cliente.")]
        public string name { get; set; }
        public string type { get; set; }
        public string email { get; set; }
        public string document { get; set; }
        public string document_type { get; set; }
        public string gender { get; set; }
        public PagarMeAddressOrder address { get; set; }
        public PagarMePhonesOrder phones { get; set; }
        public DateTime birthdate { get; set; }
    }

    public class PagarMeAddressOrder
    {
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string zip_code { get; set; }
        public string line_1 { get; set; }
        public string line_2 { get; set; }
    }

    public class PagarMePhonesOrder
    {
        public PagarMeHomePhoneOrder home_phone { get; set; }
        public PagarMeMobilePhoneOrder mobile_phone { get; set; }
    }

    public class PagarMeHomePhoneOrder
    {
        public string country_code { get; set; }
        public string area_code { get; set; }
        public string number { get; set; }
    }

    public class PagarMeMobilePhoneOrder
    {
        public string country_code { get; set; }
        public string area_code { get; set; }
        public string number { get; set; }
    }

    public class PagarMeProductsOrder
    {
        public Int32 amount { get; set; }
        public string description { get; set; }
        public Int32 quantity { get; set; }
        public string code { get; set; }
    }

    public class PagarMeShippingOrder
    {
        public Int32 amount { get; set; }
        public string description { get; set; }
        public string recipient_name { get; set; }
        public string recipient_phone { get; set; }
        public PagarMeAddressOrder address { get; set; }
    }

    public class PagarMePaymentsOrder
    {
        public string payment_method { get; set; }
        public PagarMeCreditCardOrder credit_card { get; set; }
    }

    public class PagarMeCreditCardOrder
    {
        public int installments { get; set; }
        public string statement_descriptor { get; set; }
        public PagarMeCardOrder card { get; set; }
    }

    public class PagarMeCardOrder
    {
        [Required(ErrorMessage = "Informe o número do cartão.")]
        public string number { get; set; }
        public string holder_name { get; set; }
        [Required(ErrorMessage = "Informe o mês da validade do cartão.")]
        public Int32 exp_month { get; set; }
        [Required(ErrorMessage = "Informe o ano da validade do cartão.")]
        public Int32 exp_year { get; set; }
        public string cvv { get; set; }
        public PagarMeAddressOrder billing_address { get; set; }
    }
}
