namespace NotiX.ViewModels
{
    public class FotoComNoticiaDTO
    {
        /// <summary>
        /// Chave Primária (PK)
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nome do ficheiro que contém a Foto
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Lista de Noticias associadas a cada foto
        /// </summary>
        public List<NoticiaDTO> Noticias { get; set; } = [];
    }
}
