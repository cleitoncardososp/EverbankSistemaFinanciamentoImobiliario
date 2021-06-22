using System.IO;
using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Aplicacao.CasosDeUso.PropostaCase;
using System.Threading.Tasks;
using Aplicacao.CasosDeUso.DocumentoCase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Dominio.Excecoes;
using Aplicacao.CasosDeUso.ProponenteCase;
using Newtonsoft.Json;

namespace Servico.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PropostaController : ControllerBase
    {
        public static IWebHostEnvironment _enviroment;

        public IMediator Mediator{get;set;}

        private readonly ILogger<PropostaController> _logger;

        public PropostaController(ILogger<PropostaController> logger, IMediator mediator, IWebHostEnvironment environment)
        {
            _logger = logger;
            Mediator = mediator;
            _enviroment = environment;
        }
        
        //Criar Proposta ✅
        [HttpPost()]
        public async Task<ActionResult> Post([FromBody]CriarPropostaRequest request)
        {
            _logger.LogInformation("Recebido Post de criação de proposta",request);
           if(ModelState.IsValid)
           {
                _logger.LogInformation("Modelo válido, chamando Caso de Uso");
               try
               {
                   CriarPropostaResponse response = await Mediator.Send(request);
                   _logger.LogInformation("Caso de uso retornado com sucesso. Retorno do caso de uso:",response);
                   return Ok(response);
               }
               catch(ExcecaoDominio ex)
               {
                    _logger.LogWarning("Exceção de domínio ao rodar o caso de uso", ex);
                   return BadRequest(ex.Message);
               }
               catch(Exception ex)
               {
                    _logger.LogWarning("Erro não tratado",ex);
                   return BadRequest(ex.Message);
               }
           }
           else
           {
               _logger.LogInformation("Modelo inválido",ModelState);
               return BadRequest(ModelState);
           }
        }

        //Carregar Arquivo 
        [HttpPost("{IdProposta}/proponente/{IdProponente}/documento/{IdDocumento}/upload")]   
        public async Task<ActionResult> CarregarArquivo([FromRoute]string IdProposta, [FromRoute]string IdProponente, [FromRoute]string IdDocumento, [FromForm]IFormFile arquivo)
        {
            _logger.LogInformation("Recebido Post de upload de arquivo");
                if(arquivo.Length > 0)
                {
                    try
                    {
                        _logger.LogInformation("Início do processo de request");
                        CarregarArquivoRequest carregarArquivoRequest = new CarregarArquivoRequest();
                        carregarArquivoRequest.IdProposta = IdProposta;
                        carregarArquivoRequest.IdProponente = IdProponente;
                        carregarArquivoRequest.IdDocumento = IdDocumento;

                        MemoryStream memoryStream = new MemoryStream();
                        arquivo.CopyTo(memoryStream);
                        memoryStream.Dispose();

                        carregarArquivoRequest.NomeArquivo = arquivo.FileName; 
                        carregarArquivoRequest.ConteudoArquivo = memoryStream.ToArray();

                        CarregarArquivoResponse response = await Mediator.Send(carregarArquivoRequest);
                        _logger.LogInformation("Caso de uso retornado com sucesso. Retorno do caso de uso Carregar Arquivo", response);
                        return Ok(response);
                    }
                    catch (ExcecaoDominio ex)
                    {
                        _logger.LogInformation("Exceção de domínio ao rodar o caso de uso Carregar Arquivo");
                        return BadRequest(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Erro não tratado", ex);
                        return BadRequest(ex.Message);
                    }
                }else
                    {
                        _logger.LogWarning("Arquivo não recebido");
                        return BadRequest();
                    }                
        }

        //Aprovar Documento
        [HttpPost("{IdProposta}/proponente/{IdProponente}/documento/{IdDocumento}/aprovardocumento")]
        public async Task<ActionResult> AprovarDocumento([FromRoute]AprovarDocumentoRequest request)
        {
            _logger.LogInformation("Recebido Post de Aprovação de Documento", request);
            if(ModelState.IsValid)
            {
                _logger.LogInformation("Modelo válido, chamando Caso de Uso Aprovar Documento.");
                try
                {
                    AprovarDocumentoResponse response = await Mediator.Send(request);
                    _logger.LogInformation("Caso de uso retornado com sucesso. Retorno do caso de uso Aprovar Documento", response);
                    return Ok(response);
                }
                catch (ExcecaoDominio ex)
                {
                    _logger.LogWarning("Exceção de domínio ao rodar o caso de uso Aprovar Documento", ex);
                    return BadRequest(ex.Message);
                }
                catch(Exception ex)
                {
                    _logger.LogWarning("Erro não tratado Aprovar Documento", ex);
                    return BadRequest(ex.Message);
                }
            }else
                {
                    _logger.LogInformation("Modelo Recebido Inválido Aprovar Documento", ModelState);
                    return BadRequest(ModelState);
                }
        }

        //Recusar Documento
        [HttpPost("{IdProposta}/proponente/{IdProponente}/documento/{IdDocumento}/recusardocumento")]
        public async Task<ActionResult> RecusarDocumento([FromBody]RecusarDocumentoRequest request)
        {
            _logger.LogInformation("Recebido Post de Recusar Documento", request);
            if(ModelState.IsValid)
            {
                _logger.LogInformation("Modelo válido, chamando Caso de Uso Recusar Documento.");
                try
                {
                    RecusarDocumentoResponse response = await Mediator.Send(request);
                    _logger.LogInformation("Caso de uso retornado com sucesso. Retorno do caso de uso Recusar Documento", response);
                    return Ok(response);
                }
                catch (ExcecaoDominio ex)
                {
                    _logger.LogWarning("Exceção de domínio ao rodar o caso de uso Recusar Documento", ex);
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Erro não tratado ao Recusar Documento", ex);
                    return BadRequest(ex.Message);
                }
            }else
                {
                    _logger.LogInformation("Modelo Recebido Inválido Recusar Documento", ModelState);
                    return BadRequest(ModelState);
                }
        }

        //Incluir Restrição
        [HttpPost("{IdProposta}/proponente/{IdProponente}/incluirrestricaoproponente")]
        public async Task<ActionResult> IncluirRestricao([FromBody]IncluirRestricaoProponenteRequest request)
        {
            _logger.LogInformation("Modelo válido, chamando Caso de Uso Incluir Restricao Proponente");
            if(ModelState.IsValid)
            {
                try
                {
                    IncluirRestricaoProponenteResponse response = await Mediator.Send(request);
                    _logger.LogInformation("Caso de uso retornado com sucesso. Retorno do caso de uso Incluir Restricao Proponente", response);
                    return Ok(response);
                }
                catch(ExcecaoDominio ex)
                {
                    _logger.LogWarning("Exceção de domínio ao rodar o caso de uso Incluir Restrição Proponente", ex);
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Erro não tratado ao Incluir Restrição Proponente", ex);
                    return BadRequest(ex.Message);
                }
            }else
                {
                    _logger.LogInformation("Modelo Recebido Inválido Recusar Documento", ModelState);
                    return BadRequest(ModelState);
                }
        }

        //Remover Restricao Proponente
        [HttpPost("{IdProposta}/proponente/{IdProponente}/removerrestricaoproponente")]
        public async Task<ActionResult> RemoverRestricao([FromRoute]RemoverRestricaoProponenteRequest request)
        {
            _logger.LogInformation("Modelo válido, chamando Caso de Uso Remover Restricao Proponente");
            if(ModelState.IsValid)
            {
                try
                {
                    RemoverRestricaoProponenteResponse response = await Mediator.Send(request);
                    _logger.LogInformation("Caso de uso retornado com sucesso. Retorno do caso de uso Remover Restrição Proponente", response);
                    return Ok(response);
                }
                catch (ExcecaoDominio ex)
                {
                    _logger.LogWarning("Exceção de domínio ao rodar o caso de uso Remover Restrição Proponente", ex);
                    return BadRequest(ex.Message);
                }
                catch(Exception ex)
                {
                    _logger.LogWarning("Erro não tratado ao Remover Restricao Proponente", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                _logger.LogInformation("Modelo Recebido Inválido Remover Restricao Proponente", ModelState);
                return BadRequest(ModelState);                    
            }
        }
        
        //Adicionar Proponente
        [HttpPost("{IdProposta}/adicionarproponente")]
        public async Task<ActionResult> AdicionarProponente([FromRoute]AdicionarProponenteRequest request)
        {
            _logger.LogInformation("Modelo válido, chamando Caso de Uso Adicionar Proponente");
            if(ModelState.IsValid)
            {
                try
                {
                    AdicionarProponenteResponse response = await Mediator.Send(request);
                    _logger.LogInformation("Caso de uso retornado com sucesso. Retorno do caso de uso Adicionar Proponente", response);
                    return Ok(response);
                }
                catch (ExcecaoDominio ex)
                {
                    _logger.LogWarning("Exceção de domínio ao rodar o caso de uso Adicionar Proponente", ex);
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Erro não tratado ao Adicionar Proponente", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                _logger.LogInformation("Modelo recebido Inválido Adicionar Proponente", ModelState);
                return BadRequest(ModelState);
            }
        }

        //Alterar Prazo de Financiamento ✅
        [HttpPost("{IdProposta}/alterarprazofinanciamento")]
        public async Task<ActionResult> AlterarPrazoFinanciamento([FromRoute]string IdProposta, [FromBody]AlterarPrazoFinanciamentoRequest alterarPrazoFinanciamentoRequest)
        {
                try
                {   
                    _logger.LogInformation("Chamando Caso de Uso Alterar Prazo de Financiamento");
                    alterarPrazoFinanciamentoRequest.IdProposta = IdProposta;

                    AlterarPrazoFinanciamentoResponse response = await Mediator.Send(alterarPrazoFinanciamentoRequest);
                    _logger.LogInformation("Caso de Uso retornado com sucesso. Retorno do caso de uso Alterar Prazo de Financiamento", response);
                    return Ok(response);
                }
                catch(ExcecaoDominio ex)
                {
                    _logger.LogWarning("Exceção de domínio ao rodar o caso de uso Alterar Prazo de Financiamento", ex);
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning ("Erro não tratado ao Alterar Prazo de Financiamento", ex);
                    return BadRequest(ex.Message);
                }
        }

        //Alterar Taxa de Juros ✅
        [HttpPost("{IdProposta}/alterartaxadejuros")]
        public async Task<ActionResult> AlterarTaxaDeJuros([FromRoute]string IdProposta, [FromBody]AlterarTaxaDeJurosRequest alterarTaxaDeJurosRequest)
        {
            try
            {
                _logger.LogInformation("Chamando Caso de Uso Alterar Taxa de Juros");
                alterarTaxaDeJurosRequest.IdProposta = IdProposta; 

                AlterarTaxaDeJurosResponse response = await Mediator.Send(alterarTaxaDeJurosRequest);
                _logger.LogInformation("Caso de uso retornado com sucesso. Retorno do caso de uso Alterar Taxa de Juros", response);
                return Ok(response);
            }
            catch(ExcecaoDominio ex)
            {
                _logger.LogWarning("Exceção de domínio ao rodar o caso de uso Alterar Taxa de Juros", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning ("Erro não tratado ao Alterar Prazo de Financiamento", ex);
                return BadRequest(ex.Message);
            }
        }

        //Alterar Valor de Entrada
        [HttpPost("{IdProposta}/alterarvalorentrada")]
        public async Task<ActionResult> AlterarOValorDeEntrada([FromRoute]string IdProposta, [FromBody]AlterarValorEntradaRequest alterarValorEntradaRequest)
        {
            try
            {
                _logger.LogInformation("Chamando caso de uso Alterar Valor de Entrada");    
                alterarValorEntradaRequest.IdProposta = IdProposta;
                
                AlterarValorEntradaResponse response = await Mediator.Send(alterarValorEntradaRequest);
                _logger.LogInformation("Caso de uso retornado com sucesso. Retorno do caso de uso Alterar Valor de Entrada", response);
                return Ok(response);
            }
            catch(ExcecaoDominio ex)
            {
                _logger.LogInformation("Exceção de domínio ao rodar o caso de uso Alterar Valor de Entrada", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning ("Erro não tratado ao Alterar o Valor de Entrada", ex);
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        //Aprovar Proposta
        [HttpPost("{IdProposta}/aprovarproposta")]
        public async Task<ActionResult> AprovarProposta([FromRoute]AprovarPropostaRequest request)
        {
            _logger.LogInformation("Modelo válido, chamando caso de uso Aprovar Proposta");
            if(ModelState.IsValid)
            {
                try
                {
                    AprovarPropostaResponse response = await Mediator.Send(request);
                    _logger.LogInformation("Caso de uso retornado com sucesso. Retorno do caso de uso Aprovar Proposta", response);
                    return Ok(response);
                }
                catch (ExcecaoDominio ex)
                {
                    _logger.LogInformation("Exceção de domínio ao rodar o caso de uso Aprovar Proposta", ex);
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Erro não tratado ao Aprovar Proposta", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                _logger.LogInformation("Modelo recebido inválido Aprovar Proposta", ModelState);
                return BadRequest(ModelState);
            }
        }


        //Consultar Proposta
        [HttpPost("{IdProposta}/consultarproposta")]
        public async Task<ActionResult> ConsultarProposta([FromRoute]ConsultarPropostaRequest request)
        {
            try
            {
                _logger.LogInformation("Chamando Caso de Uso Consultar Proposta");
                ConsultarPropostaResponse response = await Mediator.Send(request);
                _logger.LogInformation("Caso de uso retornado com sucesso. Retornado do caso de uso Consultar Proposta", response);
                return Ok(response);
            }
            catch(ExcecaoDominio ex)
            {
                _logger.LogInformation("Exceção de domínio ao rodar o Caso de Uso Consultar Proposta", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Erro não tratado ao Consultar Proposta", ex);
                return BadRequest(ex.Message);
            }
        }


        //Remover Proponente
        [HttpPost("{IdProposta}/proponente/{IdProponente}/removerproponente")]
        public async Task<ActionResult> RemoverProponente ([FromRoute]string IdProposta, [FromRoute]string IdProponente)
        {
            try
            {
                RemoverProponenteRequest removerProponenteRequest = new RemoverProponenteRequest();
                removerProponenteRequest.IdProposta = IdProposta;
                removerProponenteRequest.IdProponente = IdProponente;

                _logger.LogInformation("Chamando caso de uso Remover Proponente");
                RemoverProponenteResponse response = await Mediator.Send(removerProponenteRequest);
                _logger.LogInformation("Caso de uso retornado com sucesso. Retornado do caso de uso Remover Proponente", response);
                return Ok(response);
            }
            catch(ExcecaoDominio ex)
            {
                _logger.LogInformation("Exceção de domínio ao rodar o Caso de Uso Remover Proponente", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Erro não tratado ao Remover Proponente", ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
