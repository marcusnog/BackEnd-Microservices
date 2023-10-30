using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Ms.Api.Utilities.DTO.Request
{

    public class Giftty_RequestConsultarTrackingAbsulute
    {
        public string codItem { get; set; }
        public string codPedido { get; set; }
        public string projeto { get; set; }
    }

    public class Giftty_RequestConsultarTracking : Giftty_RespostaWebService
    {
        [MaxLength(7, ErrorMessage = "O tamanho maximo do campo 'codItem' é 7 números.")]
        [Description("O código de pedido gerado no sistema.")]
        public string codItem { get; set; }

        [MaxLength(7, ErrorMessage = "O tamanho maximo do campo 'codPedido' é 7 números.")]
        [Description("O código de item do pedido gerado no sistema.")]
        public string codPedido { get; set; }

        [MaxLength(4, ErrorMessage = "O tamanho maximo do campo 'projeto' é 4 caracteres.")]
        [Description("Campo opcional. Parceiros que enviam pedidos com o campo opcional no serviço InserirPedido, devem enviar o mesmo código na hora de recuperar o tracking.")]
        public string projeto { get; set; }

        public Giftty_RespostaWebService Valid()
        {
            List<string> ListaErros = new List<string>();
            Giftty_RespostaWebService respostaApi = new Giftty_RespostaWebService()
            {
                erros = 0,
                detalhesErros = new string[] { }
            };

            bool valido = true;
            bool temCodItem = false;
            if (!string.IsNullOrEmpty(codItem))
            {
                temCodItem = true;
            }

            bool temCodPedido = false;
            if (!string.IsNullOrEmpty(codPedido))
            {
                temCodPedido = true;
            }

            if (!temCodItem && !temCodPedido)
            {
                valido = false;
                return new Giftty_RespostaWebService()
                {
                    erros = 1,
                    statusCode = 400,
                    mensagem = "Ocorreu um ou mais erros de validação.",
                    detalhesErros = new string[]
                    {
                        "Pelo menos um dos campos 'codItem', 'codPedido' deve ser preenchido"
                    }
                };
            }

            if (temCodItem)
            {
                if (!codItem.All(char.IsNumber))
                {
                    valido = false;
                    respostaApi.erros++;
                    respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                    ListaErros.Add("requestConsultarTracking.codItem: este campo é numérico.");
                }
                if (codItem.Length > 7)
                {
                    valido = false;
                    respostaApi.erros++;
                    respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                    ListaErros.Add("requestConsultarTracking.codItem: O tamanho maximo do campo 'codItem' é 7 números.");
                }
            }

            if (temCodPedido)
            {
                if (!codPedido.All(char.IsNumber))
                {
                    valido = false;
                    respostaApi.erros++;
                    respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                    ListaErros.Add("requestConsultarTracking.codPedido: este campo é numérico.");
                }
                if (codPedido.Length > 7)
                {
                    valido = false;
                    respostaApi.erros++;
                    respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                    ListaErros.Add("requestConsultarTracking.codPedido: O tamanho maximo do campo 'codPedido' é 7 números.");
                }
            }

            if (valido)
            {
                respostaApi.statusCode = 200;
            }
            else
            {
                respostaApi.detalhesErros = ListaErros.ToArray();
                respostaApi.statusCode = 400;
            }

            return respostaApi;
        }
    }

    public class Giftty_RequestPedido
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'PedidoParceiro' é obrigatório")]
        [Display(Name = "Pedido Parceiro", Description = "Campo obrigatório. Recebe pedido do parceiro para consulta posterior.")]
        [MaxLength(15, ErrorMessage = "O tamanho maximo do campo 'Pedido Parceiro' é 15 caracteres.")]
        public string PedidoParceiro { get; set; } //>101443</PedidoParceiro>

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Campanha' é obrigatório")]
        [Display(Name = "Campanha", Description = "Campo obrigatório. Recebe campanha do parceiro para consulta posterior.")]
        [MaxLength(15, ErrorMessage = "O tamanho maximo do campo 'Campanha' é 15 caracteres.")]
        public string Campanha { get; set; } //>SUPER PREMIOS</Campanha>

        [Display(Name = "Projeto", Description = "Campo obrigatório. Indica projeto ou campanha. Caso o produto não esteja relacionado a este catálogo específico, o pedido será negado.")]
        [MaxLength(4, ErrorMessage = "O tamanho maximo do campo 'Projeto' é 4 caracteres.")]
        public string Projeto { get; set; } //>I15</Projeto>

        public Giftty_DadosPessoais DadosPessoais { get; set; }

        public Giftty_Enderecos Enderecos { get; set; }

        public Giftty_DadosPagamento DadosPagamento { get; set; }

        public Giftty_Produto[] Produtos { get; set; }

        public Giftty_RespostaWebService Valid()
        {
            List<string> ListaErros = new List<string>();
            Giftty_RespostaWebService respostaApi = new Giftty_RespostaWebService()
            {
                erros = 0,
                detalhesErros = new string[] { }
            };

            try
            {
                bool valido = true;
                if (DadosPessoais == null)
                {
                    valido = false;
                    respostaApi.erros++;
                    respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                    ListaErros.Add("RequestPedido.DadosPessoais: Os dados pessoais são obrigatórios.");
                }
                else
                {
                    if (string.IsNullOrEmpty(DadosPessoais.cpf) && string.IsNullOrEmpty(DadosPessoais.cnpj))
                    {
                        valido = false;
                        respostaApi.erros++;
                        respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                        ListaErros.Add("RequestPedido.DadosPessoais: Um dos campos 'CPF' e 'CNPJ' deve ser preenchido.");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(DadosPessoais.cpf) == false && DadosPessoais.cpf.All(char.IsNumber) == false)
                        {
                            valido = false;
                            respostaApi.erros++;
                            respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                            ListaErros.Add("RequestPedido.DadosPessoais.cpf: No campo 'CPF', devem ser enviados apenas números (sem pontos e traços).");
                        }

                        if (string.IsNullOrEmpty(DadosPessoais.cnpj) == false && DadosPessoais.cnpj.All(char.IsNumber) == false)
                        {
                            valido = false;
                            respostaApi.erros++;
                            respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                            ListaErros.Add("RequestPedido.DadosPessoais.cnpj: No campo 'CNPJ', devem ser enviados apenas números (sem pontos e traços).");
                        }
                    }

                    if (string.IsNullOrEmpty(DadosPessoais.ie) == false && DadosPessoais.ie.All(char.IsNumber) == false)
                    {
                        valido = false;
                        respostaApi.erros++;
                        respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                        ListaErros.Add("RequestPedido.DadosPessoais.ie: No campo 'ie' (Inscrição Estadual do cliente final). Devem ser enviados apenas números (sem pontos e traço).");
                    }

                    if (string.IsNullOrEmpty(DadosPessoais.dataNascimento) == false)
                    {
                        var isValidDate = false;
                        try
                        {
                            var dataN = Convert.ToDateTime(DadosPessoais.dataNascimento, new CultureInfo("pt-BR"));
                            if (dataN != null)
                                isValidDate = true;
                        }
                        catch { isValidDate = false; }

                        if (!isValidDate)
                        {
                            valido = false;
                            respostaApi.erros++;
                            respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                            ListaErros.Add("RequestPedido.DadosPessoais.dataNascimento: Data inválida use o formato dd/mm/aaaa.");
                        }
                    }

                    if (string.IsNullOrEmpty(DadosPessoais.sexo) == false)
                    {
                        if (DadosPessoais.sexo != "F" && DadosPessoais.sexo != "M")
                        {
                            valido = false;
                            respostaApi.erros++;
                            respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                            ListaErros.Add("RequestPedido.DadosPessoais.sexo: Sexo inválido use o 'F' ou 'M'.");
                        }
                    }

                }

                if (Enderecos == null)
                {
                    valido = false;
                    respostaApi.erros++;
                    respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                    ListaErros.Add("RequestPedido.Enderecos: Os Enderecos são obrigatórios (pelo menos o endereço principal).");
                }
                else
                {
                    if (Enderecos.EnderecoPrincipal == null && Enderecos.EnderecoEntrega == null)
                    {
                        valido = false;
                        respostaApi.erros++;
                        respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                        ListaErros.Add("RequestPedido.Enderecos: Os Enderecos são obrigatórios (pelo menos o endereço principal).");
                    }
                    else
                    {
                        if (Enderecos.EnderecoPrincipal != null)
                        {
                            if (string.IsNullOrEmpty(Enderecos.EnderecoPrincipal.cep) == false && Enderecos.EnderecoPrincipal.cep.All(char.IsNumber) == false)
                            {
                                valido = false;
                                respostaApi.erros++;
                                respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                ListaErros.Add("RequestPedido.Enderecos.EnderecoPrincipal.cep: No campo 'cep', devem ser enviados apenas números (sem traço).");
                            }

                            if (string.IsNullOrEmpty(Enderecos.EnderecoPrincipal.ddd) == false && Enderecos.EnderecoPrincipal.ddd.All(char.IsNumber) == false)
                            {
                                valido = false;
                                respostaApi.erros++;
                                respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                ListaErros.Add("RequestPedido.Enderecos.EnderecoPrincipal.ddd: No campo 'ddd', devem ser enviados apenas números (sem traço).");
                            }

                            if (string.IsNullOrEmpty(Enderecos.EnderecoPrincipal.telefone) == false && Enderecos.EnderecoPrincipal.telefone.All(char.IsNumber) == false)
                            {
                                valido = false;
                                respostaApi.erros++;
                                respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                ListaErros.Add("RequestPedido.Enderecos.EnderecoPrincipal.telefone: No campo 'telefone', devem ser enviados apenas números (sem traço).");
                            }
                        }

                        if (Enderecos.EnderecoEntrega != null)
                        {
                            if (string.IsNullOrEmpty(Enderecos.EnderecoEntrega.cep) == false && Enderecos.EnderecoEntrega.cep.All(char.IsNumber) == false)
                            {
                                valido = false;
                                respostaApi.erros++;
                                respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                ListaErros.Add("RequestPedido.Enderecos.EnderecoEntrega.cep: No campo 'cep', devem ser enviados apenas números (sem traço).");
                            }

                            if (Enderecos.EnderecoPrincipal != null)
                            {
                                if (Enderecos.EnderecoPrincipal.cep != Enderecos.EnderecoEntrega.cep
                                    || Enderecos.EnderecoPrincipal.logradouro != Enderecos.EnderecoEntrega.logradouro
                                    || Enderecos.EnderecoPrincipal.bairro != Enderecos.EnderecoEntrega.bairro
                                    || Enderecos.EnderecoPrincipal.estado != Enderecos.EnderecoEntrega.estado
                                    || Enderecos.EnderecoPrincipal.cidade != Enderecos.EnderecoEntrega.cidade
                                    || Enderecos.EnderecoPrincipal.numero != Enderecos.EnderecoEntrega.numero
                                    )
                                {
                                    if (string.IsNullOrEmpty(Enderecos.EnderecoEntrega.ddd) == false)
                                    {
                                        valido = false;
                                        respostaApi.erros++;
                                        respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                        ListaErros.Add("RequestPedido.Enderecos.EnderecoEntrega.ddd: O campo 'ddd' é obrigatório.");
                                    }

                                    if (string.IsNullOrEmpty(Enderecos.EnderecoEntrega.telefone) == false)
                                    {
                                        valido = false;
                                        respostaApi.erros++;
                                        respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                        ListaErros.Add("RequestPedido.Enderecos.EnderecoEntrega.telefone: O campo 'telefone' é obrigatório.");
                                    }
                                }
                            }

                            if (string.IsNullOrEmpty(Enderecos.EnderecoEntrega.ddd) == false && Enderecos.EnderecoEntrega.ddd.All(char.IsNumber) == false)
                            {
                                valido = false;
                                respostaApi.erros++;
                                respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                ListaErros.Add("RequestPedido.Enderecos.EnderecoEntrega.ddd: No campo 'ddd', devem ser enviados apenas números (sem traço).");
                            }

                            if (string.IsNullOrEmpty(Enderecos.EnderecoEntrega.telefone) == false && Enderecos.EnderecoEntrega.telefone.All(char.IsNumber) == false)
                            {
                                valido = false;
                                respostaApi.erros++;
                                respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                ListaErros.Add("RequestPedido.Enderecos.EnderecoEntrega.telefone: No campo 'telefone', devem ser enviados apenas números (sem traço).");
                            }
                        }
                    }
                }

                if (DadosPagamento == null)
                {
                    valido = false;
                    respostaApi.erros++;
                    respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                    ListaErros.Add("RequestPedido.DadosPagamento: Os dados de pagamento são obrigatórios (pelo menos o campo formaPagamento).");
                }
                else
                {
                    if (string.IsNullOrEmpty(DadosPagamento?.formaPagamento) ||
                        (
                        DadosPagamento.formaPagamento?.ToUpper()?.Trim() != "FATURAMENTO"
                        && DadosPagamento.formaPagamento?.ToUpper()?.Trim() != "VISA"
                        && DadosPagamento.formaPagamento?.ToUpper()?.Trim() != "MASTERCARD"
                        && DadosPagamento.formaPagamento?.ToUpper()?.Trim() != "DINERS"
                        && DadosPagamento.formaPagamento?.ToUpper()?.Trim() != "AMEX"
                        && DadosPagamento.formaPagamento?.ToUpper()?.Trim() != "ELO"
                        ))
                    {
                        valido = false;
                        respostaApi.erros++;
                        respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                        ListaErros.Add("RequestPedido.DadosPagamento.formaPagamento: O valor do campo 'formaPagamento' é inválido, use (FATURAMENTO, VISA, MASTERCARD, DINERS, AMEX ou ELO).");
                    }

                    if (DadosPagamento.formaPagamento?.ToUpper()?.Trim() != "FATURAMENTO")
                    {
                        if (string.IsNullOrEmpty(DadosPagamento?.numCartao) || DadosPagamento?.numCartao?.All(char.IsNumber) == false)
                        {
                            valido = false;
                            respostaApi.erros++;
                            respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                            ListaErros.Add("RequestPedido.DadosPagamento.numCartao: No campo 'numCartao', devem ser enviados apenas números (sem pontos e traços).");
                        }

                        if (string.IsNullOrEmpty(DadosPagamento?.validade) || DadosPagamento?.validade?.All(char.IsNumber) == false)
                        {
                            valido = false;
                            respostaApi.erros++;
                            respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                            ListaErros.Add("RequestPedido.DadosPagamento.validade: Data de validade do cartão de crédito. Deve ser enviada no formato mmaa.");
                        }
                        else
                        {
                            try
                            {
                                string mm = DadosPagamento?.validade?.Substring(0, 2);
                                string aa = DadosPagamento?.validade?.Substring(2, 2);
                                int _mm = Convert.ToInt32(mm);
                                int _aa = Convert.ToInt32(aa);

                                string valorMM = mm;
                                string valorAA = aa;

                                if (_mm <= 0 || _mm > 12)
                                {
                                    valido = false;
                                    respostaApi.erros++;
                                    respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                    ListaErros.Add("RequestPedido.DadosPagamento.validade: Data de validade inválido - o mês deve ser entre 01 e 12. A data deve ser enviada no formato mmaa.");
                                }
                                else
                                {
                                    if (_mm > 0 && _mm < 10)
                                    {
                                        valorMM = "0" + _mm.ToString();
                                    }
                                }

                                if (_aa <= 0 || _aa > 99)
                                {
                                    valido = false;
                                    respostaApi.erros++;
                                    respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                    ListaErros.Add("RequestPedido.DadosPagamento.validade: Data de validade inválido - o ano deve ser entre 01 e 99. A data deve ser enviada no formato mmaa.");
                                }
                                else
                                {
                                    if (_aa > 0 && _aa < 10)
                                    {
                                        valorAA = "0" + _aa.ToString();
                                    }
                                }

                                DadosPagamento.validade = valorMM + valorAA;
                            }
                            catch
                            {
                                valido = false;
                                respostaApi.erros++;
                                respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                ListaErros.Add("RequestPedido.DadosPagamento.validade: Data de validade inválido. A data deve ser enviada no formato mmaa.");
                            }
                        }

                        if (string.IsNullOrEmpty(DadosPagamento?.bandeira) ||
                            (DadosPagamento.bandeira?.ToUpper()?.Trim() != "VISA,"
                            && DadosPagamento.bandeira?.ToUpper()?.Trim() != "MASTERCARD"
                            && DadosPagamento.bandeira?.ToUpper()?.Trim() != "DINERS"
                            && DadosPagamento.bandeira?.ToUpper()?.Trim() != "AMEX"
                            && DadosPagamento.bandeira?.ToUpper()?.Trim() != "ELO"
                            ))
                        {
                            valido = false;
                            respostaApi.erros++;
                            respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                            ListaErros.Add("RequestPedido.DadosPagamento.bandeira: O valor do campo 'bandeira' é inválido, use (VISA, MASTERCARD, DINERS, AMEX ou ELO).");
                        }

                        if (string.IsNullOrEmpty(DadosPagamento.nomeTitular))
                        {
                            valido = false;
                            respostaApi.erros++;
                            respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                            ListaErros.Add("RequestPedido.DadosPagamento.nomeTitular: O campo 'nomeTitular' é obrigatório.");
                        }

                        if (string.IsNullOrEmpty(DadosPagamento.cpfTitular) || DadosPagamento.cpfTitular?.All(char.IsNumber) == false)
                        {
                            valido = false;
                            respostaApi.erros++;
                            respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                            ListaErros.Add("RequestPedido.DadosPagamento.cpfTitular: No campo 'cpfTitular', devem ser enviados apenas números (sem pontos e traços).");
                        }

                        if (string.IsNullOrEmpty(DadosPagamento.codSeguranca) || DadosPagamento.codSeguranca?.All(char.IsNumber) == false)
                        {
                            valido = false;
                            respostaApi.erros++;
                            respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                            ListaErros.Add("RequestPedido.DadosPagamento.codSeguranca: No campo 'codSeguranca', devem ser enviados apenas números (sem pontos e traços).");
                        }
                        else
                        {
                            try
                            {
                                var _codSeguranca = Convert.ToInt32(DadosPagamento.codSeguranca);
                                if (_codSeguranca <= 0 || _codSeguranca > 9999)
                                {
                                    valido = false;
                                    respostaApi.erros++;
                                    respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                    ListaErros.Add("RequestPedido.DadosPagamento.codSeguranca: No campo 'codSeguranca' é inválido, o valor deve ser entre 1 e 9999.");
                                }
                            }
                            catch
                            {
                                valido = false;
                                respostaApi.erros++;
                                respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                ListaErros.Add("RequestPedido.DadosPagamento.codSeguranca: No campo 'codSeguranca' é inválido, o valor deve ser entre 1 e 9999.");
                            }
                        }

                        if (!string.IsNullOrEmpty(DadosPagamento.porcentagemPagamento) && DadosPagamento.porcentagemPagamento?.All(char.IsNumber) == false)
                        {
                            valido = false;
                            respostaApi.erros++;
                            respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                            ListaErros.Add("RequestPedido.DadosPagamento.porcentagemPagamento: No campo 'porcentagemPagamento', devem ser enviados apenas números (sem pontos e traços).");
                        }
                        else if (!string.IsNullOrEmpty(DadosPagamento.porcentagemPagamento))
                        {
                            try
                            {
                                var _porcentagemPagamento = Convert.ToInt32(DadosPagamento.porcentagemPagamento);
                                if (_porcentagemPagamento <= 0 || _porcentagemPagamento > 99)
                                {
                                    valido = false;
                                    respostaApi.erros++;
                                    respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                    ListaErros.Add("RequestPedido.DadosPagamento.porcentagemPagamento: No campo 'porcentagemPagamento' é inválido, o valor deve ser entre 0 e 99.");
                                }
                            }
                            catch
                            {
                                valido = false;
                                respostaApi.erros++;
                                respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                ListaErros.Add("RequestPedido.DadosPagamento.porcentagemPagamento: No campo 'porcentagemPagamento' é inválido, o valor deve ser entre 0 e 99.");
                            }
                        }
                    }
                }

                var _produtos = Produtos.ToList();
                _produtos.RemoveAll(x => x == null);
                Produtos = _produtos.ToArray();

                if (Produtos.Length == 0)
                {
                    valido = false;
                    respostaApi.erros++;
                    respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                    ListaErros.Add("RequestPedido.Produtos: Os produtos comprados são obrigatórios.");
                }
                else
                {
                    for (var i = 0; i < Produtos.Length; i++)
                    {
                        if (string.IsNullOrEmpty(Produtos[i]?.ddd) == false && Produtos[i]?.ddd.All(char.IsNumber) == false)
                        {
                            valido = false;
                            respostaApi.erros++;
                            respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                            ListaErros.Add("Produtos[" + i + "].ddd: No campo 'ddd', devem ser enviados apenas números (sem traço).");
                        }

                        if (string.IsNullOrEmpty(Produtos[i]?.telefone) == false && Produtos[i]?.telefone.All(char.IsNumber) == false)
                        {
                            valido = false;
                            respostaApi.erros++;
                            respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                            ListaErros.Add("Produtos[" + i + "].telefone: No campo 'telefone', devem ser enviados apenas números (sem traço).");
                        }

                        if (string.IsNullOrEmpty(Produtos[i].valor) == false)
                        {
                            if (Produtos[i].valor?.IndexOf(",") == -1 && Produtos[i].valor?.IndexOf(".") != -1)
                            {
                                valido = false;
                                respostaApi.erros++;
                                respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                                ListaErros.Add("Produtos[" + i + "].valor: O valor do vale, nos casos em que o vale tiver valor variável. Apenas números. Sem separador de milhares. Vírgula (,) como separador decimal.");
                            }
                        }
                    }
                }

                if (valido)
                {
                    respostaApi.statusCode = 200;
                }
                else
                {
                    respostaApi.detalhesErros = ListaErros.ToArray();
                    respostaApi.statusCode = 400;
                }

                return respostaApi;
            }
            catch
            {
                respostaApi.erros = 1;
                respostaApi.mensagem = "Ocorreu um ou mais erros de validação.";
                ListaErros.Add("RequestPedido: Falha ao concluir a validação, tente novamente.");
                return respostaApi;
            }
        }
    }

    public class Giftty_DadosPessoais
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Nome' é obrigatório")]
        [MaxLength(40, ErrorMessage = "O tamanho maximo do campo 'Nome' é 40 caracteres.")]
        [Display(Name = "Nome", Description = "Nome completo do cliente final. Deve haver pelo menos dois nomes (nome e sobrenome) para aceitação.")]
        public string nome { get; set; } //>JOSE MIRANDA</nome>

        [MaxLength(40, ErrorMessage = "O tamanho maximo do campo 'Razão Social' é 40 caracteres.")]
        [Display(Name = "Razão Social", Description = "Razão Social do cliente (Pessoa Jurídica). É obrigatório apenas quando o cliente for Pessoa Jurídica.")]
        public string razaoSocial { get; set; } //></razaoSocial>

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Tipo Pessoa' é obrigatório")]
        [MaxLength(8, ErrorMessage = "O tamanho maximo do campo 'Tipo Pessoa' é 8 caracteres.")]
        [Display(Name = "Tipo Pessoa", Description = "Informa se o cliente final é Pessoa Física ou jurídic, valores = (FISICA, JURIDICA).")]
        public string tipoPessoa { get; set; } //>FISICA</tipoPessoa>

        [MaxLength(11, ErrorMessage = "O tamanho maximo do campo 'CPF' é 11 números.")]
        [Display(Name = "CPF", Description = "CPF do cliente final. Devem ser enviados apenas números (sem pontos e digito). É obrigatório apenas quando o cliente for Pessoa Física.")]
        public string cpf { get; set; } //>12345678901</cpf>

        [MaxLength(14, ErrorMessage = "O tamanho maximo do campo 'CNPJ' é 14 números.")]
        [Display(Name = "CNPJ", Description = "CNPJ do cliente final. Devem ser enviados apenas números (sem pontos, barra e dígito). É obrigatório apenas quando o cliente for Pessoa Jurídica.")]
        public string cnpj { get; set; } //></cnpj>

        [MaxLength(12, ErrorMessage = "O tamanho maximo do campo 'ie' é 12 números.")]
        [Display(Name = "Inscrição Estadual", Description = "Inscrição Estadual do cliente final. Devem ser enviados apenas números (sem pontos e dígito).")]
        public string ie { get; set; } //></ie>

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Email' é obrigatório")]
        [MaxLength(40, ErrorMessage = "O tamanho maximo do campo 'Email' é 40 caracteres.")]
        [Display(Name = "Email", Description = "Email do cliente final.")]
        public string email { get; set; } //>JOSE.MIRANDA @GMAIL.COM</email> 

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Data Nascimento' é obrigatório")]
        [MaxLength(10, ErrorMessage = "O tamanho maximo do campo 'Data Nascimento' é 10 caracteres.")]
        [Display(Name = "Data Nascimento", Description = "Data de nascimento do cliente final, no formato dd/mm/aaaa.")]
        public string dataNascimento { get; set; } //>JOSE.MIRANDA @GMAIL.COM</email> 

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Sexo' é obrigatório")]
        [MaxLength(1, ErrorMessage = "O tamanho maximo do campo 'Sexo' é 1 caracter 'F' ou 'M'.")]
        [Display(Name = "Sexo", Description = "Sexo do cliente final, valores = (F, M).")]
        public string sexo { get; set; } //>JOSE.MIRANDA @GMAIL.COM</email> 

    }

    public class Giftty_Enderecos
    {
        public Giftty_Endereco EnderecoPrincipal { get; set; }

        public Giftty_EnderecoEntrega EnderecoEntrega { get; set; }
    }

    public class Giftty_Endereco
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Logradouro' é obrigatório")]
        [MaxLength(40, ErrorMessage = "O tamanho maximo do campo 'Logradouro' é 40 caracteres.")]
        [Display(Name = "Logradouro", Description = "Logradouro da residência, ou endereço de cobrança do cliente final. Apenas Logradouro, sem número, complementos etc.")]
        public string logradouro { get; set; } //>AVENIDA BRASIL</logradouro>

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Número' é obrigatório")]
        [MaxLength(6, ErrorMessage = "O tamanho maximo do campo 'Número' é 6 caracteres.")]
        [Display(Name = "Número", Description = "Número da residência, ou endereço de cobrança do cliente final.")]
        public string numero { get; set; } //>743</numero>

        [MaxLength(15, ErrorMessage = "O tamanho maximo do campo 'Complemento' é 15 caracteres.")]
        [Display(Name = "Complemento", Description = "Complemento do endereço residencial ou de cobrança do cliente final.")]
        public string complemento { get; set; } //>APTO 101</complemento>

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Bairro' é obrigatório")]
        [MaxLength(20, ErrorMessage = "O tamanho maximo do campo 'Bairro' é 20 caracteres.")]
        [Display(Name = "Bairro", Description = "Bairro do endereço residencial ou de cobrança do cliente final.")]
        public string bairro { get; set; } //>VILA BRASIL</bairro>

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Cidade' é obrigatório")]
        [MaxLength(25, ErrorMessage = "O tamanho maximo do campo 'Cidade' é 25 caracteres.")]
        [Display(Name = "Cidade", Description = "Cidade do endereço residencial ou de cobrança do cliente final.")]
        public string cidade { get; set; } //>SAO PAULO</cidade>

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Estado (UF)' é obrigatório")]
        [MaxLength(2, ErrorMessage = "O tamanho maximo do campo 'Estado (UF)' é 2 caracteres.")]
        [Display(Name = "Estado (UF)", Description = "UF do endereço residencial ou de cobrança do cliente final.")]
        public string estado { get; set; } //>SP</estado>

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'CEP' é obrigatório")]
        [MaxLength(8, ErrorMessage = "O tamanho maximo do campo 'CEP' é 8 números.")]
        [Display(Name = "CEP", Description = "CEP do endereço residencial ou de cobrança do cliente final. Devem ser enviados apenas números (sem traço).")]
        public string cep { get; set; } //>12345000</cep>

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'DDD' é obrigatório")]
        [MaxLength(2, ErrorMessage = "O tamanho maximo do campo 'DDD' é 2 números.")]
        [Display(Name = "DDD", Description = "DDD do telefone residencial ou de cobrança do cliente final.")]
        public string ddd { get; set; } //>11</ddd>

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Telefone' é obrigatório")]
        [MaxLength(9, ErrorMessage = "O tamanho maximo do campo 'Telefone' é 9 números.")]
        [Display(Name = "Telefone", Description = "Telefone residencial ou de cobrança do cliente final. Devem ser enviados apenas números (sem traço).")]
        public string telefone { get; set; } //>996549126</telefone>

        [MaxLength(2, ErrorMessage = "O tamanho maximo do campo 'DDD Celular' é 2 números.")]
        [Display(Name = "DDD", Description = "DDD do telefone celular do cliente final.")]
        public string dddCel { get; set; } //>11</dddCel>

        [MaxLength(9, ErrorMessage = "O tamanho maximo do campo 'Celular' é 9 números.")]
        [Display(Name = "Telefone", Description = "Telefone celular do cliente final.")]
        public string telefoneCel { get; set; } //>996549126</telefoneCel>
    }

    public class Giftty_EnderecoEntrega
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Logradouro' é obrigatório")]
        [MaxLength(40, ErrorMessage = "O tamanho maximo do campo 'Logradouro' é 40 caracteres.")]
        [Display(Name = "Logradouro", Description = "Logradouro do endereço de entrega do cliente final. Apenas Logradouro, sem número, complementos etc. Obrigatório apenas quando o endereço de entrega for diferente do endereço principal. Se não for enviado, o endereço de entrega considerado será o endereço principal.")]
        public string logradouro { get; set; } //>AVENIDA BRASIL</logradouro>

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Número' é obrigatório")]
        [MaxLength(6, ErrorMessage = "O tamanho maximo do campo 'Número' é 6 caracteres.")]
        [Display(Name = "Número", Description = "Número do endereço de entrega do cliente final. Obrigatório apenas quando o endereço de entrega for diferente do endereço principal. Se não for enviado, o endereço de entrega considerado será o endereço principal.")]
        public string numero { get; set; } //>743</numero>

        [MaxLength(15, ErrorMessage = "O tamanho maximo do campo 'Complemento' é 15 caracteres.")]
        [Display(Name = "Complemento", Description = "Complemento do endereço de entrega do cliente final. ")]
        public string complemento { get; set; } //>APTO 101</complemento>

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Bairro' é obrigatório")]
        [MaxLength(20, ErrorMessage = "O tamanho maximo do campo 'Bairro' é 20 caracteres.")]
        [Display(Name = "Bairro", Description = "Bairro do endereço de entrega do cliente final. Obrigatório apenas quando o endereço de entrega for diferente do endereço principal. Se não for enviado, o endereço de entrega considerado será o endereço principal.")]
        public string bairro { get; set; } //>VILA BRASIL</bairro>

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Cidade' é obrigatório")]
        [MaxLength(25, ErrorMessage = "O tamanho maximo do campo 'Cidade' é 25 caracteres.")]
        [Display(Name = "Cidade", Description = "Cidade do endereço de entrega do cliente final. Obrigatório apenas quando o endereço de entrega for diferente do endereço principal. Se não for enviado, o endereço de entrega considerado será o endereço principal.")]
        public string cidade { get; set; } //>SAO PAULO</cidade>

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Estado (UF)' é obrigatório")]
        [MaxLength(2, ErrorMessage = "O tamanho maximo do campo 'Estado (UF)' é 2 caracteres.")]
        [Display(Name = "Estado (UF)", Description = "UF do endereço de entrega do cliente final. Obrigatório apenas quando o endereço de entrega for diferente do endereço principal. Se não for enviado, o endereço de entrega considerado será o endereço principal.")]
        public string estado { get; set; } //>SP</estado>

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'CEP' é obrigatório")]
        [MaxLength(8, ErrorMessage = "O tamanho maximo do campo 'CEP' é 8 números.")]
        [Display(Name = "CEP", Description = "CEP do endereço de entrega do cliente final. Devem ser enviados apenas números (sem traço). Obrigatório apenas quando o endereço de entrega for diferente do endereço principal. Se não for enviado, o endereço de entrega considerado será o endereço principal.")]
        public string cep { get; set; } //>12345000</cep>

        [MaxLength(2, ErrorMessage = "O tamanho maximo do campo 'DDD' é 2 números.")]
        [Display(Name = "DDD", Description = "DDD do telefone onde será feita a entrega. Obrigatório apenas quando o endereço de entrega for diferente do endereço principal. Se não for enviado, o endereço de entrega considerado será o endereço principal.")]
        public string ddd { get; set; } //>11</ddd>

        [MaxLength(9, ErrorMessage = "O tamanho maximo do campo 'Telefone' é 9 números.")]
        [Display(Name = "Telefone", Description = "Telefone residencial de onde será feita a entrega. Devem ser enviados apenas números (sem traço). Obrigatório apenas quando o endereço de entrega for diferente do endereço principal. Se não for enviado, o endereço de entrega considerado será o endereço principal.")]
        public string telefone { get; set; } //>996549126</telefone>

        [MaxLength(2, ErrorMessage = "O tamanho maximo do campo 'DDD Celular' é 2 números.")]
        [Display(Name = "DDD", Description = "DDD do telefone celular do cliente final.")]
        public string dddCel { get; set; } //>11</dddCel>

        [MaxLength(9, ErrorMessage = "O tamanho maximo do campo 'Celular' é 9 números.")]
        [Display(Name = "Telefone", Description = "Telefone celular do cliente final.")]
        public string telefoneCel { get; set; } //>996549126</telefoneCel>
    }
    public class Giftty_DadosPagamento
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Forma Pagamento' é obrigatório")]
        [MaxLength(10, ErrorMessage = "O tamanho maximo do campo 'Forma Pagamento' é 10 caracteres.")]
        [Display(Name = "Forma Pagamento", Description = "Forma de pagamento desejada, valores = (FATURAMENTO, VISA, MASTERCARD, DINERS, AMEX, ELO).")]
        public string formaPagamento { get; set; }

        [MaxLength(16, ErrorMessage = "O tamanho maximo do campo 'Número' é 16 números.")]
        [Display(Name = "Número", Description = "Número do cartão de crédito. Informação obrigatória apenas quando o pagamento for feito com cartão de crédito.")]
        public string numCartao { get; set; } //>0123456789012345</numCartao>

        [MaxLength(4, ErrorMessage = "O tamanho maximo do campo 'Validade' é 4 números.")]
        [Display(Name = "Validade", Description = "Data de validade do cartão de crédito. Deve ser enviada no formato mmaa. Informação obrigatória apenas quando o pagamento for feito com cartão de crédito.")]
        public string validade { get; set; } //>10/2018</validade>

        [MaxLength(10, ErrorMessage = "O tamanho maximo do campo 'Bandeira' é 10 caracteres.")]
        [Display(Name = "Bandeira", Description = "Bandeira do cartão de crédito. Informação obrigatória apenas quando o pagamento for feito com cartão de crédito, valores = (VISA, MASTERCARD, DINERS, AMEX, ELO).")]
        public string bandeira { get; set; } //>mastercard</bandeira>

        [MaxLength(24, ErrorMessage = "O tamanho maximo do campo 'Nome do titular' é 24 caracteres.")]
        [Display(Name = "Nome do titular", Description = "Nome do titular do cartão de crédito. Informação obrigatória apenas quando o pagamento for feito com cartão de crédito.")]
        public string nomeTitular { get; set; } //>John Doe</nomeTitular>

        [MaxLength(11, ErrorMessage = "O tamanho maximo do campo 'CPF do titular' é 11 números.")]
        [Display(Name = "CPF do titular", Description = "CPF do titular do cartão de crédito. Informação obrigatória apenas quando o pagamento for feito com cartão de crédito.")]
        public string cpfTitular { get; set; } //>32403980870</cpfTitular>

        [MaxLength(4, ErrorMessage = "O tamanho maximo do campo 'Digito verificador (segurança)' é 11 números.")]
        [Display(Name = "Digito verificador (segurança)", Description = "Digito verificador (segurança) do cartão de crédito. Informação obrigatória apenas quando o pagamento for feito com cartão de crédito.")]
        public string codSeguranca { get; set; } //>123</codSeguranca>

        [MaxLength(2, ErrorMessage = "O tamanho maximo do campo 'Porcentagem a ser cobrada do cartão' é 2 números.")]
        [Display(Name = "Porcentagem a ser cobrada do cartão", Description = "Porcentagem a ser cobrada do cartão de crédito. Este campo não é obrigatório. Utilização deste campo mediante negociação comercial.")]
        public string porcentagemPagamento { get; set; } //>50</porcentagemPagamento>

        [Range(1, 99, ErrorMessage = "O campo 'numParcelas' deve estar entre 1 e 99.")]
        [Display(Name = "Número de parcelas", Description = "Número de parcelas desejada para pagamento da compra. Informação obrigatória apenas quando o pagamento for feito com cartão de crédito. Em outras formas de pagamento, essa informação será ignorada.")]
        public int? numParcelas { get; set; } //>50</porcentagemPagamento>

    }
    public class Giftty_Produto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Código' é obrigatório")]
        [MaxLength(6, ErrorMessage = "O tamanho maximo do campo 'Código' é 6 caracteres.")]
        [Display(Name = "Código", Description = "Código (SKU) do produto desejado.")]
        public string codigo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Quantidade' é obrigatório")]
        [Range(1, 99, ErrorMessage = "O campo 'Quantidade' deve estar entre 1 e 99.")]
        [Display(Name = "Quantidade", Description = "Quantidade desejada do produto.")]
        public int? quantidade { get; set; }

        [MaxLength(2, ErrorMessage = "O tamanho maximo do campo 'DDD' é 2 números.")]
        [Display(Name = "DDD", Description = "O DDD do telefone que irá receber a recarga. Informação obrigatória apenas quando o produto for do departamento Recarga de Celular.")]
        public string ddd { get; set; } //>11</ddd>

        [MaxLength(9, ErrorMessage = "O tamanho maximo do campo 'Telefone' é 9 números.")]
        [Display(Name = "Telefone", Description = "O Número do telefone que irá receber a recarga. Informação obrigatória apenas quando o produto for do departamento Recarga de Celular.")]
        public string telefone { get; set; } //>996549126</telefone>

        [MaxLength(10, ErrorMessage = "O tamanho maximo do campo 'Valor' é 10 caracteres.")]
        [Display(Name = "Valor", Description = "O valor do vale, nos casos em que o vale tiver valor variável. Apenas números. Sem separador de milhares. Vírgula (,) como separador decimal.")]
        public string valor { get; set; }

        [MaxLength(30, ErrorMessage = "O tamanho maximo do campo 'Código Cartão' é 30 caracteres.")]
        [Display(Name = "Código Cartão", Description = "Vale a ser recarregado (para produtos de tipo recarga). Inserir apenas o código (letras e números), sem espaços, traços, pontos ou qualquer outro caracter.")]
        public string codigoCartao { get; set; }
    }

    public class Giftty_RespostaWebService
    {
        public Giftty_RespostaWebService()
        {
            this.erros = 0;
            this.statusCode = 200;
        }

        public Giftty_RespostaWebService(Exception ex)
        {
            this.statusCode = 500;
            this.erros = 1;
            this.mensagem = GetErros(ex);
            this.detalhesErros = new string[] { };
            this.excecao = ex;
        }

        public Giftty_RespostaWebService(Giftty_RespostaWebService model)
        {
            this.statusCode = model.statusCode;
            this.erros = model.erros;
            this.mensagem = model.mensagem;
            this.detalhesErros = model.detalhesErros;
            this.excecao = model.excecao;
        }


        public int statusCode { get; set; }
        public int? erros { get; set; }
        public string mensagem { get; set; }
        public string[] detalhesErros { get; set; }
        public Exception excecao { get; set; }

        public string GetErros(Exception ex)
        {
            var mensagem = ex.Message.ToLower().Contains("inner exception") || ex.Message.ToLower().Contains("one or more errors occurred") ?
                        (ex.InnerException.Message.ToLower().Contains("inner exception") || ex.InnerException.Message.ToLower().Contains("an error occurred while sending the request") ?
                        ex.InnerException.InnerException.Message : ex.InnerException.Message)
                        : ex.Message;

            return mensagem;
        }

        public Giftty_RespostaWebService ToRespostaApi()
        {
            return this;
        }
    }

}
