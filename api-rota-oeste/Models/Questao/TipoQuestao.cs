namespace api_rota_oeste.Models.Questao;

/// <summary>
/// Representa o enum <see cref="TipoQuestao"/> que define os diferentes tipos de questões que podem ser associadas a um checklist.
/// </summary>
/// <remarks>
/// Este enum é utilizado para especificar o tipo de uma questão, como objetiva, múltipla escolha ou upload de imagem.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="TipoQuestao"/>:
/// <code>
/// var tipo = TipoQuestao.QUESTAO_MULTIPLA_ESCOLHA;
/// </code>
/// </example>
public enum TipoQuestao
{
    QUESTAO_OBJETIVA = 1,
    
    QUESTAO_MULTIPLA_ESCOLHA = 2,
    
    QUESTAO_UPLOAD_DE_IMAGEM = 3
    
}