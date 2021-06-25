using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entidades;
using Dominio.Entidades;
using Dominio.Excecoes;

namespace Dominio.RaizAgregacao
{
    public class Proposta
    {
        public const int QUANTIDADE_MAXIMA_PROPONENTES = 4;
        public const int QUANTIDADE_MINIMA_PROPONENTES = 1;
        public string IdProposta {get; private set;}
        public Imovel Imovel {get ; private set;}
        public List<Proponente> Proponentes {get; private set;}
        public decimal ValorEntrada{ get; private set; }
        public int PrazoFinanciamento { get; private set; }  
        public decimal TaxaJurosAnual {get; private set;}
        public bool IsPropostaAprovada{get; private set;}
        public decimal ValorPrimeiraParcela {get; private set;}
        public decimal RendaBrutaMinima {get; private set;}
        public decimal RendaTotalProponentes {get; private set;}
        public DateTime? DataAnaliseProposta{get; private set;}
        public String MotivoRecusaProposta{get;private set;}

        //Construtor que valida se os dados estão vazios ou nullos
        public Proposta(Imovel imovel, List<Proponente> proponentes, decimal valorEntrada, int prazoFinanciamento, decimal taxaJurosAnual)
        {

            ExcecaoDominio.LancarQuando(()=>imovel==null, "Imóvel é obrigatório!");
            ExcecaoDominio.LancarQuando(()=>proponentes==null, "Proponente é obrigatório!");
            ExcecaoDominio.LancarQuando(()=>proponentes.Count>=QUANTIDADE_MAXIMA_PROPONENTES, "A Quantidade máxima de proponentes é "+QUANTIDADE_MAXIMA_PROPONENTES);
            ExcecaoDominio.LancarQuando(()=>valorEntrada == 0,"Valor de Entrada é obrigatório"); 
            ExcecaoDominio.LancarQuando(()=>prazoFinanciamento == 0,"Prazo de Financiamento é obrigatório");

            IdProposta = Guid.NewGuid().ToString();
            
            Imovel = imovel;
            Proponentes = proponentes;
            ValorEntrada = valorEntrada;
            PrazoFinanciamento = prazoFinanciamento;
            IsPropostaAprovada = false;
            TaxaJurosAnual = taxaJurosAnual;


            //Procedimento para cálculo do valor da Proposta (RendaMinima/ValorDaPrimeiraParcela)
            foreach (var prop in Proponentes)
            {
                RendaTotalProponentes += prop.RendaBruta; 
            }

            decimal valorImovel = Imovel.ValorImovel;
            const decimal taxaRendaMinima = 30.0M /100; // CONST * Taxa para calculo da renda bruta minima 
            decimal taxaMes = (taxaJurosAnual/12)/100;

            decimal valorFinanciado = valorImovel - ValorEntrada;  // valor financiado      

            decimal amortizacao = Math.Round(valorFinanciado / PrazoFinanciamento, 2); // calculo da primeira parcela
            decimal jurosSaldoDevedor = Math.Round(taxaMes * valorFinanciado, 2);
            decimal parcela = amortizacao + jurosSaldoDevedor; 

            decimal rendaMinima = Math.Round(parcela / taxaRendaMinima);

            ValorPrimeiraParcela = parcela;
            RendaBrutaMinima = rendaMinima;
        }

        //Construtor utilizado pela Fábrica, para instanciação do Objeto vindo do Banco de Dados.
        internal Proposta(string idProposta, Imovel imovel, List<Proponente> proponentes, decimal valorEntrada, int prazoFinanciamento, decimal taxaJurosAnual, bool isPropostaAprovada, decimal valorPrimeiraParcela, decimal rendaBrutaMinima, decimal rendaTotalProponentes, DateTime? dataAnaliseProposta, string motivoRecusaProposta)
        {
            IdProposta = idProposta;
            Imovel = imovel;
            Proponentes = proponentes;
            ValorEntrada = valorEntrada;
            PrazoFinanciamento = prazoFinanciamento;
            TaxaJurosAnual = taxaJurosAnual;
            IsPropostaAprovada = isPropostaAprovada;
            ValorPrimeiraParcela = valorPrimeiraParcela;
            RendaBrutaMinima = rendaBrutaMinima;
            RendaTotalProponentes = rendaTotalProponentes;
            DataAnaliseProposta = dataAnaliseProposta;
            MotivoRecusaProposta = motivoRecusaProposta;
        }

        //Método para Adicionar Proponente na Proposta
        public void AdicionarProponente (Proponente prop)
        {   
            ExcecaoDominio.LancarQuando(()=>prop==null,"Proponente é obrigatório!");

            if(Proponentes.Count >= QUANTIDADE_MAXIMA_PROPONENTES)
            {
                throw new PropostaNumeroMaximoProponentesException("Quantidade máxima de Proponentes atingido, impossível adicionar proponente!");
            }else
            {
                Proponentes.Add(prop);
            }  
        }

        //Método para Remover Proponente da Proposta
        public void RemoverProponente(Proponente prop)
        {
            ExcecaoDominio.LancarQuando(()=>prop==null, "Necessário informar um proponente para remoção! ");
            
            if(Proponentes.Count <= QUANTIDADE_MINIMA_PROPONENTES)
            {
                throw new PropostaNumeroMinimoProponentesException("Quantidade mínima de Proponentes atingido, impossível remover proponente!");
            }else
                {
                    var proponente = Proponentes.FirstOrDefault(item=>item.IdProponente == prop.IdProponente);
                    if(proponente!=null)
                    {
                        Proponentes.Remove(proponente);
                    }
                }
        }

        //Método para Alterar o Valor de Entrada da Proposta
        public void AlterarValorEntrada(decimal novoValor)
        {
            ExcecaoDominio.LancarQuando(()=>novoValor==0,"Novo valor de Entrada é obrigatório!");
            ValorEntrada = novoValor;
        }

        //Método para alterar Prazo de Financiamento da Proposta
        public void AlterarPrazoFinanciamento(int prazoFinanciamento)
        {
            ExcecaoDominio.LancarQuando(()=>prazoFinanciamento == 0,"Prazo de Financiamento é obrigatório");
            PrazoFinanciamento = prazoFinanciamento;
        }

        //Método para Alterar Taxa de Juros Anual da Proposta
        public void AlterarTaxaJurosAnual(decimal taxaJuros)
        {
            ExcecaoDominio.LancarQuando(()=>taxaJuros == 0,"Taxa de Juros é Obrigatória! ");
            TaxaJurosAnual = taxaJuros;
        }

        //Método para Aprovavar Proposta com suas verificações
        public void AprovarProposta()
        {
            foreach(var proponente in Proponentes)
            {
                foreach(var documento in proponente.Documentos)
                {
                    //Algum documento não foi carregado para o sistema
                    if(String.IsNullOrEmpty(documento.CaminhoArquivo))
                        throw new DocumentoPendenteException("Documento: " + documento.Nome + " do " + proponente.NomeCompleto + " não foi carregado no sistema!");

                    //Algum documento não foi aprovado
                    if(documento.IsDocumentoAprovado==false)
                        throw new DocumentoPendenteAprovacaoException("Documento: " + documento.Nome + " do " + proponente.NomeCompleto + " não foi aprovado");
                }

                if(proponente.PossuiRestricao == true)
                {
                    throw new ProponenteComRestricaoException("Proponente: " + proponente.NomeCompleto + " com restrição!");
                }
            }            

            if(RendaTotalProponentes < RendaBrutaMinima)
            {
                throw new RendaInsuficienteException("Renda Total dos Proponentes é insuficiente!");
            }

            IsPropostaAprovada=true; 
            MotivoRecusaProposta = null;
            DataAnaliseProposta = DateTime.Now;
        }

        //Método para Rejeitar a Proposta, passando o motivo da rejeição
        public void RejeitarProposta(String motivo)
        {
            IsPropostaAprovada = false;
            MotivoRecusaProposta = motivo;
            DataAnaliseProposta = DateTime.Now;
        }

        //Método para exibir
        public void ImprimirProposta()
        {
            System.Console.WriteLine(Imovel.ToString());

            foreach(var prop in Proponentes)
            {
                System.Console.WriteLine("\nProponente: " + prop.ToString());        
            }
            
            System.Console.WriteLine("Valor de Entrada: " + ValorEntrada);
            System.Console.WriteLine("Prazo de Financiamento: " + PrazoFinanciamento);
            System.Console.WriteLine("Taxa de Juros Anual: " + TaxaJurosAnual);
            System.Console.WriteLine("Situação da Proposta: " + IsPropostaAprovada);
            System.Console.WriteLine("Data Análise da Proposta: " + DataAnaliseProposta);
            System.Console.WriteLine("1ª Parcela: " + ValorPrimeiraParcela);
            System.Console.WriteLine("Renda Minima: " + RendaBrutaMinima);
            System.Console.WriteLine("Renda Total: " + RendaTotalProponentes);
        }        
    }
}
