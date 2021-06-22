using System;
using Dominio.Excecoes;

namespace Dominio.Entidades
{
    public class Documento
    {
        public string IdDocumento {get; private set;}
        public string Nome {get; private set;}
        public string Descricao {get; private set;}
        public string CaminhoArquivo { get; private set; }
        public bool IsDocumentoAprovado {get; private set;}
        public string MotivoRecusaAprovacao {get; private set;}

        //Construtor que valida se os dados estão vazios
        public Documento(string nome, string descricao)
        {
            ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(nome),"Nome do Documento é obrigatório");
            ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(descricao),"Descrição do Documento é obrigatório");

            IdDocumento = Guid.NewGuid().ToString();

            Nome = nome;
            Descricao = descricao;

            IsDocumentoAprovado = false;
        }

        //Construtor usado na Fabrica, para construção do objeto vindo do banco de dados
        internal Documento(string idDocumento, string nome, string descricao, string caminhoArquivo, bool isDocumentoAprovado, string motivoRecusaAprovacao)
        {
            IdDocumento = idDocumento;
            Nome = nome;
            Descricao = descricao;
            CaminhoArquivo = caminhoArquivo;
            IsDocumentoAprovado = isDocumentoAprovado;
            MotivoRecusaAprovacao = motivoRecusaAprovacao;
        }

        //método de aprovação de documento
        public void AprovarDocumento()
        {
            if(String.IsNullOrEmpty(CaminhoArquivo)) // Verifica se o caminho do arquivo está vazio
            {
                throw new DocumentoPendenteException(Nome);    
            }
            IsDocumentoAprovado = true;
        }

        //método para carregar caminho do arquivo em um documento
        public void CarregarArquivo(string caminho)
        {
            ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(caminho),"Caminho do arquivo é obrigatório");
            CaminhoArquivo = caminho;
        }

        //método para recusar um documento
        public void RecusarDocumento(string motivorecusa)
        {
            ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(motivorecusa),"Motivo da Recusa do Documento é obrigatório!");
            MotivoRecusaAprovacao = motivorecusa;
            IsDocumentoAprovado = false;
        }

        //método para adicionar um documento
        public void AdicionarDocumento(string nome, string descricao)
        {
            ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(nome),"Nome do Documento é obrigatório");
            ExcecaoDominio.LancarQuando(()=>String.IsNullOrEmpty(descricao),"Descrição do Documento é obrigatório");

            IdDocumento = Guid.NewGuid().ToString();
            
            Nome = nome;
            Descricao = descricao;

            IsDocumentoAprovado = false;
        }

        //método para exibir uma instância de um objeto do tipo Documento
        public override string ToString()
        {
            return
                "\n\tIdDocumento: " + IdDocumento +
                "\n\tNome: " + Nome +
                "\n\tDescrição: " + Descricao +
                "\n\tCaminho Arquivo: " + CaminhoArquivo +
                "\n\tDocumento Aprovado: " + IsDocumentoAprovado +
                "\n\tMotivo Recusa Aprovacao: " + MotivoRecusaAprovacao
            ;
        }
    }
}
