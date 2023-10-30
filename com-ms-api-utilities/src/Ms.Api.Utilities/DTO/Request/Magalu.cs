using System.Net;
using System.Text;

namespace Ms.Api.Utilities.DTO.Request
{
    //public class Magalu_ConsultStatusOrderRequest : BaseRequest_Pedido
    //{
    //    public string? CPF { get; set; }
    //    public string? CodigoPedido { get; set; }
    //    public string? CodigoCampanha { get; set; }
    //}

    public class Magalu_CreateOrderRequest : BaseRequest_Pedido
    {
        //public long PedidoParceiro { get; set; }
        //public decimal? ValorFrete { get; set; }
        public Magalu_RecipientOrder Destinatario { get; set; }
        public Magalu_DeliveryAddress EnderecoEntrega { get; set; }
        public List<Magalu_ProductCreateOrder> Produtos { get; set; }
        //public int? OperadorTeleMarketingId { get; set; }
        //public string? CodigoResgateAqui { get; set; }
        //public string? TipoFrete { get; set; }
        public string? Parceiro { get; set; }

        public string CodigoCampanhaFornecedor { get; set; }
    }

    public class Magalu_RecipientOrder
    {
        public string? CPFCNPJ { get; set; }
        //public string? Email { get; set; }
        //public string? Nome { get; set; }
        //public string? InscricaoEstadual { get; set; }
        //public string? CodigoClienteCitibank { get; set; }
        public string? Telefone { get; set; }
        public string? Telefone1 { get; set; }
        //public bool? AceitaReceberContatoUnicef { get; set; }
    }

    public class Magalu_DeliveryAddress : BaseRequest_EnderecoPedido
    {
        //public string? Bairro { get; set; }
        //public string? Cidade { get; set; }
        //public string? CEP { get; set; }
        //public string? Complemento { get; set; }
        //public string? Estado { get; set; }
        //public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        //public string? Referencia { get; set; }
        //public string? Telefone { get; set; }
        public string? Telefone1 { get; set; }
        //public string? Telefone2 { get; set; }
        //public string? Telefone3 { get; set; }
        public string? NomeResponsavel { get; set; }
    }

    public class Magalu_ProductCreateOrder
    {
        /// <summary>
        /// SKU
        /// </summary>
        public string? Codigo { get; set; }

        /// <summary>
        /// Quantidade do produto a ser pedido
        /// </summary>
        public int? Quantidade { get; set; }

        /// <summary>
        /// Valor do preço de venda do produto a ser conferido. Valida o valor e abortará o pedido caso haja diferença.
        /// </summary>
        public decimal? PrecoVenda { get; set; }

        /// <summary>
        /// Nome do produto
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Imagem principal do produto
        /// </summary>
        public string? UrlImagem { get; set; }

        public decimal? ValorSubsidio { get; set; }
    }

    public class Magalu_ModelosMagaLu
    {
        public class ProdutoMagaLu
        {
            //Variação do produto (CODIGO + MODELO = SKU do Produto)
            public string CODIGO { get; set; }
            public string MODELO { get; set; }
            public string CATEGORIA { get; set; }
            public string DESC_CATEGORIA { get; set; }
            public string SUBCATEGORIA { get; set; }
            public string DESC_SUBCATEGORIA { get; set; }
            public string REFERENCIA { get; set; }

            //Título do produto.
            //Importante: O nome comercial do produto no site do Magazine Luiza é composto de <DESCRICAO> - <REFERENCIA>. Sugerimos a mesma implementação em seu site
            public string DESCRICAO { get; set; }

            //0 = Bivolt
            //1 = 110 Volts
            //2 = 220 Volts
            //3 = Não utiliza energia elétrica
            //4 = 380 volts
            //5 = Bateria Recarregável
            //6 = Pilha
            //7 = Energia Solar
            public string VOLTAGEM { get; set; }
            public string MARCA { get; set; }

            //Link de imagem do produto. Tamanho preferencial: 210x210 px.
            public string IMAGEM { get; set; }
            //Link de imagem de vitrine do produto. Tamanho preferencial: 210x210 px.	
            public string IMAGEM_VITRINE { get; set; }
            //Link de imagem grande de vitrine do produto. Tamanho preferencial: 1500x1500 px.	
            public string IMAGEM_VITRINE_GRANDE { get; set; }
            //Link de imagem de categoria do produto. Tamanho preferencial: 210x210 px.	
            public string IMAGEM_CATEGORIA { get; set; }
            //Link de imagem PPI* do produto. Tamanho prefencial: 210x210 px.	
            public string IMAGEM_PRODUTO_PPI { get; set; }
            //Link de imagem de detalhe do produto. Tamanho prefencial: 410x308 px.	
            public string IMAGEM_PRODUTO_DETALHE { get; set; }
            //Link de imagem ampliada do produto. Tamanho prefencial: 1500x1500 px.	
            public string IMAGEM_PRODUTO_GRANDE { get; set; }
            //Quantidade de detalhes do produto para ser utilizada para visualização das imagens. Se a imagem tiver 5 detalhes, além da imagem principal enviada nos campos acima, haverá imagens de A à E (010502100a.jpg, ... 010502100e.jpg).	
            public int? QTDE_DETALHES { get; set; }
            //Valor de tabela do produto (Preço de) em R$	
            public decimal? VALOR { get; set; }
            //Valor de venda do produto (Preço por) em R$	
            public decimal? VALOR_VENDA { get; set; }
            public bool? ATIVO { get; set; }
            public string CLASSIFICACAO_FISCAL { get; set; }
            public string ACAO { get; set; }
            public string DATA_ALTERACAO { get; set; }

            //Indica se o produto necessita de montagem. 0=Não / 1=Sim	
            public string TEM_MONTAGEM { get; set; }

            //Indica qual é o produto mestre deste. Usado para produtos com mais de uma cor, ou voltagem, etc. Quando um produto não tem um mestre isto quer dizer que ele é o mestre, então o valor preenchido é o código do próprio produto (campo CODIGO).
            public string MESTRE { get; set; }

            //Atualmente exitem dois tipos: produto ou bundle. Bundle é a composição de 2 ou 3 produtos selecionados pelo Magazine Luiza.	
            public string TIPO { get; set; }
            public string VIDEO { get; set; }
            public string ESPECIFICACAO { get; set; }

        }

        public class CategoriaMagaLu
        {
            public string strLinha { get; set; }
            public string Descricaolinha { get; set; }
            public string strSetor { get; set; }
            public string strDescricao { get; set; }
        }

        public class CorMagaLu
        {
            public string strCor { get; set; }
            public string strDescricao { get; set; }
        }

        #region WebService

        //Requests
        public class BaseRequestMagaLu
        {
            public RequestEstoqueMagaLu ConsultaProduto { get; set; }
            public RequestConsultaPrecoProdutoMagaLu ConsultaPrecoProduto { get; set; }
            public RequestConsultaCepEntregaMagaLu ConsultaCep { get; set; }
            public RequestConsultaPrazoEntregaMagaLu ConsultaEntrega { get; set; }
            public RequestCalculaCarrinhoFreteMagaLu CalculaCarrinhoFrete { get; set; }
            public RequestFinalizarPedidoMagaLu Carrinho { get; set; }
            public RequestTrackingFullMagaLu Pedidos { get; set; }
            public RequestAprovaPedidoMagaLu AprovaPedido { get; set; }

        }

        public class RequestEstoqueMagaLu
        {
            public string Codigo { get; set; }
            public string Modelo { get; set; }
            public string Quantidade { get; set; }
            public string IdResgateCampanha { get; set; }
        }

        public class RequestConsultaPrecoProdutoMagaLu
        {
            public string Codigo { get; set; }
            public string Modelo { get; set; }
            public string IdResgateCampanha { get; set; }
        }

        public class RequestConsultaCepEntregaMagaLu
        {
            public string IdResgateCampanha { get; set; }
            public string CEP { get; set; }
        }

        public class RequestConsultaPrazoEntregaMagaLu
        {
            public string CPF { get; set; }
            public string IdResgateCampanha { get; set; }
            public string CEP { get; set; }
            public List<RequestProdutoMagaLu> ListaProdutos { get; set; }
        }

        public class CalcularFrete_Filtro
        {
            public string CPF { get; set; }
            public string CodigoCampanha { get; set; }
            public string CEP { get; set; }
            public string ChaveSessaoCarrinhoParceiro { get; set; }

            public List<RequestProdutoMagaLu> ListaProdutos { get; set; }
        }

        public class RequestProdutoMagaLu
        {
            public string Codigo { get; set; }
            public string Modelo { get; set; }
            public int Quantidade { get; set; }
        }

        public class RequestCalculaCarrinhoFreteMagaLu
        {
            public string Chave_sessao { get; set; }
            public string CPF { get; set; }
            public string IdResgateCampanha { get; set; }
            public string CEP { get; set; }
            public List<RequestProdutoMagaLu> ListaProdutos { get; set; }
        }

        public class RequestFinalizarPedidoMagaLu
        {
            public string Chave_sessao { get; set; }
            public string CPF { get; set; }
            public string IdResgateCampanha { get; set; }
            public string CEP { get; set; }
            public string Endereco { get; set; }
            public string Numero { get; set; }
            public string Complemento { get; set; }
            public string Bairro { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
            public string DDD { get; set; }
            public string Telefone { get; set; }
            public string Nome_Premiado { get; set; }
            public string Inscricao { get; set; }
            public string PedidoParceiro { get; set; }
        }

        public class RequestTrackingFullMagaLu
        {
            public string CPF { get; set; }
            public string IdResgateCampanha { get; set; }
            public string Pedido { get; set; }
        }

        public class RequestAprovaPedidoMagaLu
        {
            public string Pedido { get; set; }
            public string CPF { get; set; }
            public string IdResgateCampanha { get; set; }
            public string Aprovado { get; set; }
        }


        //Rspostas
        public class BaseRespostaMagaLu
        {
            public List<ResponseEstoqueMagaLu> ConsultaProduto { get; set; }
            public List<ResponseConsultaPrecoProdutoMagaLu> ConsultaPrecoProduto { get; set; }
            public List<ResponseConsultaCepEntregaMagaLu> ConsultaCEP { get; set; }
            public List<ResponseTrackingFullMagaLu> Pedidos { get; set; }
            public ResponseEntregaMagaLu Entrega { get; set; }
            public ResponseCarrinhoFreteMagaLu Carrinho { get; set; }
            public ResponseAprovaPedidoMagaLu AprovaPedido { get; set; }

        }

        //public class RespostaEntregaMagaLu
        //{
        //    public List<ResponseEntregaMagaLu> Entrega { get; set; }
        //    public List<ResponseProdutoEntregaMagaLu> Produto { get; set; }
        //}

        //public class RespostaCarrinhoFreteMagaLu
        //{
        //    public List<ResponseCarrinhoFreteMagaLu> Carrinho { get; set; }
        //    public List<ResponseProdutoEntregaMagaLu> Produto { get; set; }
        //}

        public class ResponseEstoqueMagaLu
        {
            public string Codigo { get; set; }
            public string Modelo { get; set; }
            public string Quantidade { get; set; }
            public string Liberado { get; set; }
            public string strLiberado
            {
                get
                {
                    switch (Liberado)
                    {
                        case "0": return "Produto indisponível para venda";
                        case "1": return "Produto liberado para venda";
                        default: return Liberado;
                    }
                }
            }

            public string IdResgateCampanha { get; set; }
            public string idStatus { get; set; }
            public string strStatus
            {
                get
                {
                    switch (idStatus)
                    {
                        case "-1": return "Falha na execução do WebMétodo";
                        case "0": return "Operação realizada com sucesso";
                        case "2": return "Campanha inexistente ou expirada";
                        case "3": return "Produto e/ou Modelo Inexistente";
                        default: return idStatus;
                    }
                }
            }

            public string Mensagem { get; set; }
            public string tem_montagem { get; set; }
        }

        public class ResponseConsultaPrecoProdutoMagaLu
        {
            public string Codigo { get; set; }
            public string Modelo { get; set; }
            public string Valor_Reais { get; set; }
            public decimal? Valor_ReaisDecimal { get { try { return Convert.ToDecimal(Valor_Reais, new System.Globalization.CultureInfo("en-US")); } catch { return null; } } }
            public string Valor_Moeda { get; set; }
            public decimal? Valor_MoedaDecimal { get { try { return Convert.ToDecimal(Valor_Moeda, new System.Globalization.CultureInfo("en-US")); } catch { return null; } } }
            public string IdResgateCampanha { get; set; }
            public string idStatus { get; set; }
            public string strStatus
            {
                get
                {
                    switch (idStatus)
                    {
                        case "-1": return "Falha na execução do WebMétodo";
                        case "0": return "Operação realizada com sucesso";
                        case "2": return "Campanha inexistente ou expirada";
                        case "3": return "Produto e/ou Modelo Inexistente";
                        default: return idStatus;
                    }
                }
            }

            public string Mensagem { get; set; }
        }

        public class ResponseConsultaCepEntregaMagaLu
        {
            public string CEP { get; set; }
            public string IdResgateCampanha { get; set; }
            public string idStatus { get; set; }
            public string strStatus
            {
                get
                {
                    switch (idStatus)
                    {
                        case "-1": return "Falha na execução do WebMétodo";
                        case "0": return "Operação realizada com sucesso";
                        case "1": return "Formato de CEP inválido (o tamanho do campo CEP deve ser de 8 dígitos, sem a utilização de \" - \" ou espaços.)";
                        case "2": return "CEP inexistente na base de dados do site ou CEP Inválido";
                        default: return idStatus;
                    }
                }
            }

            public string Mensagem { get; set; }
        }

        public class ResponseEntregaMagaLu
        {
            public string IdResgateCampanha { get; set; }
            public string CEP { get; set; }
            public string Prazo { get; set; }
            public string Data_Entrega { get; set; }
            public string strPrazo { get { try { return Convert.ToDecimal(Prazo, new System.Globalization.CultureInfo("en-US")) <= 1 ? $"{Prazo} dia útil - (" + Data_Entrega + ")" : $"{Prazo} dias úteis (" + Data_Entrega + ")"; } catch { return Prazo + " dia(s) úteis - (" + Data_Entrega + ")"; } } }
            public string idStatus { get; set; }
            public string strStatus
            {
                get
                {
                    switch (idStatus)
                    {
                        case "-1": return "Falha na execução do WebMétodo";
                        case "0": return "Operação realizada com sucesso";
                        case "1": return "Existe(m) produto(s) do carrinho que está(ão) sem estoque";
                        case "2": return "Campanha inexistente ou expirada";
                        case "3": return "Produto e/ou modelo Inexistente";
                        case "4": return "Cep de entrega não cadastrado na base";


                        default: return idStatus;
                    }
                }
            }
            public string Mensagem { get; set; }
            public List<ResponseProdutoEntregaMagaLu> ListaProdutos { get; set; }
        }

        public class ResponseProdutoEntregaMagaLu
        {
            public string Codigo { get; set; }
            public string Modelo { get; set; }
            public string Quantidade { get; set; }
            public string Liberado { get; set; }
            public string strLiberado
            {
                get
                {
                    switch (Liberado)
                    {
                        case "0": return "Produto indisponível";
                        case "1": return "Produto disponível para compra";
                        default: return Liberado;
                    }
                }
            }
            public string Estoque { get; set; }
        }

        public class ResponseCarrinhoFreteMagaLu
        {
            public string Chave_sessao { get; set; }
            public string IdResgateCampanha { get; set; }
            public string CEP { get; set; }
            public string Valor_Frete { get; set; }
            public decimal? ValorFreteDecimal { get { try { return Convert.ToDecimal(Valor_Frete, new System.Globalization.CultureInfo("en-US")); } catch { return null; } } }
            public string Valor_Frete_Moeda { get; set; }
            public decimal? ValorFreteMoedaDecimal { get { try { return Convert.ToDecimal(Valor_Frete_Moeda, new System.Globalization.CultureInfo("en-US")); } catch { return null; } } }
            public string Prazo { get; set; }
            public string Data_Entrega { get; set; }
            public string strPrazo { get { try { return Convert.ToDecimal(Prazo, new System.Globalization.CultureInfo("en-US")) <= 1 ? $"{Prazo} dia útil - (" + (string.IsNullOrEmpty(Data_Entrega) ? "data não informada" : Data_Entrega) + ")" : $"{Prazo} dias úteis (" + (string.IsNullOrEmpty(Data_Entrega) ? "data não informada" : Data_Entrega) + ")"; } catch { return Prazo + " dia(s) úteis - (" + (string.IsNullOrEmpty(Data_Entrega) ? "data não informada" : Data_Entrega) + ")"; } } }
            public string idStatus { get; set; }
            public string strStatus
            {
                get
                {
                    switch (idStatus)
                    {
                        case "-1": return "Falha na execução do WebMétodo";
                        case "0": return "Operação realizada com sucesso";
                        case "1": return "Existe(m) produto(s) do carrinho que está(ão) sem estoque";
                        case "2": return "Campanha inexistente ou expirada";
                        case "3": return "Produto e/ou modelo Inexistente";
                        case "4": return "Cep de entrega não cadastrado na base";
                        case "5": return "CPF Inválido";
                        default: return idStatus;
                    }
                }
            }
            public string Mensagem { get; set; }
            public List<ResponseProdutoEntregaMagaLu> ListaProdutos { get; set; }
        }

        public class ResponseFinalizarPedidoMagaLu
        {
            public string Chave_sessao { get; set; }
            public string CPF { get; set; }
            public string IdResgateCampanha { get; set; }
            public string Pedido { get; set; }
            public string Quantidade_itens { get; set; }
            public string Valor_Reais { get; set; }
            public string Valor_Moeda { get; set; }
            public string CEP { get; set; }

            public string Endereco { get; set; }
            public string Numero { get; set; }
            public string Complemento { get; set; }
            public string Bairro { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
            public string DDD { get; set; }
            public string Telefone { get; set; }

            public string Previsao { get; set; }
            public string Valor_Tot_Produto { get; set; }
            public string Valor_Tot_Produto_Moeda { get; set; }
            public string Valor_Frete { get; set; }
            public string Valor_Frete_Moeda { get; set; }
            public string idStatus { get; set; }
            public string strStatus
            {
                get
                {
                    switch (idStatus)
                    {
                        case "-1": return "Falha na execução do WebMétodo";
                        case "0": return "Operação realizada com sucesso";
                        case "1": return "Existe(m) produto(s) do carrinho que está(ão) sem estoque";
                        case "2": return "Campanha inexistente ou expirada";
                        case "3": return "Produto e/ou modelo Inexistente";
                        case "4": return "Cep de entrega não cadastrado na base";
                        case "5": return "CPF Inválido";
                        case "6": return "Dado(s) de Endereço de Entrega Inconsistente(s)";

                        default: return idStatus;
                    }
                }
            }

            public string Mensagem { get; set; }

            public List<ResponseProdutoEntregaMagaLu> ListaProdutos { get; set; }
        }

        public class ResponseTrackingFullMagaLu
        {
            public string CPF { get; set; }
            public string IdResgateCampanha { get; set; }

            public string Pedido { get; set; }
            public string PedidoParceiro { get; set; }
            public string DataPedido { get; set; }
            public string EntregaPrevista { get; set; }
            public string PedidoStatusId { get; set; }
            public string PedidoStatusDesc { get; set; }
            public string idStatus { get; set; }
            public string strStatus
            {
                get
                {
                    switch (idStatus)
                    {
                        case "-1": return "Falha na execução do WebMétodo";
                        case "0": return "Operação realizada com sucesso";
                        case "1": return "Pedido ou CPF inexistente ou divergentes";
                        default: return idStatus;
                    }
                }
            }
            public string Mensagem { get; set; }

            public ResponseEnderecoEntregaMagaLu EnderecoEntrega { get; set; }
            public List<ResponseItemEntregaMagaLu> ListaEntrega { get; set; }
        }

        public class ResponseEnderecoEntregaMagaLu
        {
            public string CEP { get; set; }
            public string Endereco { get; set; }
            public string Numero { get; set; }
            public string Complemento { get; set; }
            public string Bairro { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
            public string DDD { get; set; }
            public string Telefone { get; set; }
        }

        public class ResponseItemEntregaMagaLu
        {
            public string NotaFiscalNumero { get; set; }
            public string NotaFiscalChave { get; set; }
            public string EntregaPrevista { get; set; }
            public string UrlRastreamento { get; set; }
            public List<ResponseItemStatusEntrega> ListaStatus { get; set; }
            public List<ResponseItemHistoricoEntrega> ListaHistorico { get; set; }
            public List<ResponseProdutoEntrega> ListaProdutos { get; set; }
        }

        public class ResponseItemStatusEntrega
        {
            public string StatusId { get; set; }
            public string DataStatus { get; set; }
            public string DescStatus { get; set; }
        }
        public class ResponseItemHistoricoEntrega
        {
            public string DataHistorico { get; set; }
            public string DescHistorico { get; set; }
        }
        public class ResponseProdutoEntrega
        {
            public string ProdutoId { get; set; }
            public string DescProduto { get; set; }
        }

        public class ResponseAprovaPedidoMagaLu
        {
            public string CPF { get; set; }
            public string IdResgateCampanha { get; set; }

            public string Pedido { get; set; }
            public string idStatus { get; set; }
            public string strStatus
            {
                get
                {
                    switch (idStatus)
                    {
                        case "-1": return "Falha na execução do WebMétodo";
                        case "0": return "Operação realizada com sucesso";
                        case "1": return "Pedido ou CPF inexistente ou divergentes";
                        case "2": return "Falha na campanha passada";
                        case "4": return "Cpf ou cnpj inválido";
                        case "14": return "Pedido não encontrado";
                        case "15": return "Pedido já aprovado";
                        case "16": return "Pedido já cancelado";
                        case "17": return "Falha no envio da aprovação";
                        default: return idStatus;
                    }
                }
            }
            public string Mensagem { get; set; }
        }


        #endregion

        #region Tools
        public static string Decode(string valor)
        {
            try
            {
                //var _valor = valor.ToLower();
                //if (!_valor.Contains('£') && !_valor.Contains(';')
                //    && !_valor.Contains('§') && !_valor.Contains('&') && !_valor.Contains('³') && !_valor.Contains('©')
                //    && !_valor.Contains('ª') && !_valor.Contains('¡') && !_valor.Contains("ã?") && !_valor.Contains("ã´") && !_valor.Contains("ã'") && !_valor.Contains("ã")
                //    && (_valor.Contains('á') || _valor.Contains('à') || _valor.Contains('â') || _valor.Contains('ã')
                //    || _valor.Contains('ó') || _valor.Contains('ò') || _valor.Contains('õ') || _valor.Contains('ô')
                //    || _valor.Contains('é') || _valor.Contains('è') || _valor.Contains('ê')
                //    || _valor.Contains('ç')
                //    || _valor.Contains('í') || _valor.Contains('ì')
                //    || _valor.Contains('ú') || _valor.Contains('ù')))
                //    return valor;
                byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(valor);

                var resultado = WebUtility.HtmlDecode(Encoding.UTF8.GetString(bytes, 0, bytes.Length));

                return WebUtility.HtmlDecode(Encoding.UTF8.GetString(bytes, 0, bytes.Length));
            }
            catch
            {
                return valor;
            }
        }

        public static string Decode(string valor, Encoding _From_enconding = null, Encoding To_encoding = null)
        {
            try
            {
                if (_From_enconding == null)
                    _From_enconding = Encoding.UTF8;

                if (To_encoding == null)
                    To_encoding = Encoding.GetEncoding("ISO-8859-1");

                byte[] bytes = _From_enconding.GetBytes(valor);
                var resultado = WebUtility.HtmlDecode(To_encoding.GetString(bytes, 0, bytes.Length));

                return resultado;
            }
            catch
            {
                return valor;
            }
        }

        public static string GetVoltagm(int? VoltagemId)
        {
            switch (VoltagemId)
            {
                case 0: return "Bivolt";
                case 1: return "110 Volts";
                case 2: return "220 Volts";
                case 3: return "Não utiliza energia elétrica";
                case 4: return "380 volts";
                case 5: return "Bateria Recarregável";
                case 6: return "Pilha";
                case 7: return "Energia Solar";
                default: return VoltagemId.ToString();
            }
        }
        #endregion
    }

}
