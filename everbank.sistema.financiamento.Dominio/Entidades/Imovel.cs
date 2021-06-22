using System;
using Dominio.Excecoes;
using Dominio.ObjetosValor;

namespace Domain.Entidades
{
    public class Imovel
    {
        public string IdImovel {get; private set;}
        public Endereco Endereco {get; private set;} 
        public int InscricaoMunicipal {get; private set;}
        public decimal ValorImovel {get; private set;}

        //Construtor que valida se os dados estão vazios
        public Imovel(Endereco endereco, int inscricaoMunicipal, decimal valorImovel)
        {
            ExcecaoDominio.LancarQuando(()=>endereco==null,"Endereço do Imóvel é inválido");
            ExcecaoDominio.LancarQuando(()=>inscricaoMunicipal == 0,"Inscrição Municipal do Imóvel é obrigatório");
            ExcecaoDominio.LancarQuando(()=>valorImovel == 0,"Valor do Imóvel é obrigatório");

            IdImovel = Guid.NewGuid().ToString();
            
            Endereco = endereco;
            InscricaoMunicipal = inscricaoMunicipal;
            ValorImovel = valorImovel;
        }

        //Construtor usado na Fábrica, para criação do Objeto vindo do Banco de Dados
        internal Imovel(string idImovel, Endereco endereco, int inscricaoMunicipal, decimal valorImovel)
        {
            IdImovel = idImovel;
            Endereco = endereco;
            InscricaoMunicipal = inscricaoMunicipal;
            ValorImovel = valorImovel;
        }

        //metodo para exibir uma instância de um objeto do tipo Imovel
        public override string ToString()
        {
            return  "\nEndereco: " + Endereco +
                    "\nInscrição Municipal: " + InscricaoMunicipal +
                    "\nValor Imovel: " + ValorImovel 
            ;
        }
    }
}
