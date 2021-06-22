using System;
using Dominio.Excecoes;

namespace Dominio.ObjetosValor
{
    public class Endereco
    {
        public string Logradouro {get; private set;}
        public string Numero {get; private set;}
        public string Complemento {get; private set;}
        public string Cep {get; private set;}
        public string Bairro {get; private set;}
        public string Cidade {get; private set;}
        public string Estado {get; private set;}

        //Construtor que valida se os dados estão vazios ou nullos
        public Endereco(string logradouro, string numero, string complemento, string cep, string bairro, string cidade, string estado)
        {
            ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(logradouro),"Logradouro do endereço é obrigatório");
            ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(numero),"Número do Logradouro é obrigatório");
            ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(cep),"CEP do logradouro é obrigatório");
            ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(bairro),"Bairro do logradouro é obrigatório");
            ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(cidade),"Cidade do logradouro é obrigatório");
            ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(estado),"Estado do logradouro é obrigatório");

            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Cep = cep;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado; 
        }

        //Método para exibir uma instância de um objeto do tipo Endereco
        public override string ToString()
        {
            return  "\n\tLogradouro: " + Logradouro + 
                    "\n\tNúmero: " + Numero +
                    "\n\tComplemento: " + Complemento +
                    "\n\tCEP: " + Cep +
                    "\n\tBairro: " + Bairro +
                    "\n\tCidade: " + Cidade +
                    "\n\tEstado: " + Estado;
        }
    }
}
