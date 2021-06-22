using System;
using System.Collections.Generic;
using Dominio.Excecoes;

namespace Dominio.Entidades
{
    public class Proponente
    {
        public string IdProponente {get; private set;}
        public List<Documento> Documentos {get; private set;}
        public string NomeCompleto {get; private set;}
        public string Cpf {get; private set;}
        public DateTime DataNascimento {get; private set;}
        public string EstadoCivil { get; private set;}
        public decimal RendaBruta {get; private set;}
        public bool PossuiAlgumaDoencaGrave {get; private set;}
        public bool PossuiRestricao{get; private set;}
        public string Restricao{get; private set;}
    
    //Construtor que valida se os dados estão vazios ou nulos
    public Proponente(List<Documento> documentos, string nomeCompleto, string cpf, DateTime dataNascimento, string estadoCivil, decimal rendaBruta)
    {
        ExcecaoDominio.LancarQuando(()=>documentos==null || documentos.Count==0,"Documentos do proponente são obrigatórios");
        ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(nomeCompleto),"Nome Completo do proponente é obrigatório");
        ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(cpf),"CPF é obrigatório");
        ExcecaoDominio.LancarQuando(()=>dataNascimento == null, "Data de Nascimento é obrigatório");
        ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(estadoCivil),"Estado Civil é obrigatório"); 
        ExcecaoDominio.LancarQuando(()=>rendaBruta == 0,"Renda Bruta é obrigatório");

        IdProponente = Guid.NewGuid().ToString();;
        Documentos = documentos;
            
        NomeCompleto = nomeCompleto;
        Cpf = cpf;
        DataNascimento = dataNascimento;
        EstadoCivil = estadoCivil;
        RendaBruta = rendaBruta;

        PossuiAlgumaDoencaGrave = false;
        PossuiRestricao = false;   
    }

    //Construtor usado pela Fábrica, para instanciação do objeto proponente vindo do banco de dados.
    internal Proponente(string idProponente, List<Documento> documentos, string nomeCompleto, string cpf, DateTime dataNascimento, string estadoCivil, decimal rendaBruta, bool possuiAlgumaDoencaGrave, bool possuiRestricao, string restricao)
    {
        IdProponente = idProponente;
        Documentos = documentos;
        NomeCompleto = nomeCompleto;
        Cpf = cpf;
        DataNascimento = dataNascimento;
        EstadoCivil = estadoCivil;
        RendaBruta = rendaBruta;
        PossuiAlgumaDoencaGrave = possuiAlgumaDoencaGrave;
        PossuiRestricao = possuiRestricao;
        Restricao = restricao;
    }
    
    //Método para Incluir Restrição em um proponente recebendo uma string "Motivo" como parâmetro
    public void IncluirRestricao(String motivo)
    {
        ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(motivo),"Motivo da Restrição é obrigatório! ");

        PossuiRestricao = true;
        Restricao = motivo;
    }

    //Método para Remover uma restrição de um proponente
    public void RemoverRestricao()
    {
        PossuiRestricao=false;
    }

    //Método para exibir uma instância de um objeto do tipo Proponente
    public override string ToString()
        {
            return 
                "\n\tNome: " + NomeCompleto +
                "\n\tCPF: " + Cpf +
                "\n\tData de Nascimento: " + DataNascimento +
                "\n\tEstado Civil: " + EstadoCivil +
                "\n\tRenda Bruta: " + RendaBruta +
                "\n\tPossui Alguma Doença Grave: " + PossuiAlgumaDoencaGrave +
                "\n\tPossui Alguma Restrição: " + PossuiRestricao;
        }

    }
}
