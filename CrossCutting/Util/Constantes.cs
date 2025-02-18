namespace CrossCutting.Util
{
    public class Constantes
    {
        /*
        Constantes de TipoTrabalho
        */
        public const int TIPO_TRABALHO_CERTIFICACAO = 1;
        public const int TIPO_TRABALHO_RELATORIO_DIRETO = 2;

        /**
         * Constantes de TipoSituacaoPropostaAcao
         */
        public const int TIPO_SITUACAO_PROPOSTA_ACAO_EM_ELABORACAO = 1;
        public const int TIPO_SITUACAO_PROPOSTA_ACAO_SUBMETIDA_ANALISE = 2;
        public const int TIPO_SITUACAO_PROPOSTA_ACAO_DEVOLVIDA_CORRECAO = 3;
        public const int TIPO_SITUACAO_PROPOSTA_ACAO_INCLUIDA_PFA = 4;
        public const int TIPO_SITUACAO_PROPOSTA_ACAO_INCLUIDA_BANCO_PROPOSTAS = 5;

        /**
        * Constantes de TipoSituacaoVersao
        */
        public const byte TIPO_SITUACAO_VERSAO_EM_ELABORACAO = 1;
        public const byte TIPO_SITUACAO_VERSAO_CONCLUIDA = 2;
        public const byte TIPO_SITUACAO_VERSAO_APROVADA = 3;

        /**
        * Constantes de TipoSituacaoPropostaTMS
        */
        public const int TIPO_SITUACAO_PROPOSTA_TMS_CADASTRADA = 1;
        public const int TIPO_SITUACAO_PROPOSTA_TMS_AGRUPADA = 2;
        public const int TIPO_SITUACAO_PROPOSTA_TMS_SEPARADA = 3;
        public const int TIPO_SITUACAO_PROPOSTA_TMS_PRIORIZADA = 5;
        public const int TIPO_SITUACAO_PROPOSTA_TMS_CADASTRADA_POR_AGRUPAMENTO = 6;
        public const int TIPO_SITUACAO_PROPOSTA_TMS_CADASTRADA_POR_SEPARAÇÃO = 7;

        /**
       * Constantes de TipoSituacaoAcao
       */
        public const int TIPO_SITUACAO_ACAO_CRIADA = 1;
        public const int TIPO_SITUACAO_ACAO_RETORNADA_PARA_ALTERACAO = 2;
        public const int TIPO_SITUACAO_ACAO_ALTERADA = 3;

    }
}
