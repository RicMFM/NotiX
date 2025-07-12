namespace NotiX.ViewModels
{
    public class NoticiasDetalhadasDTO
    {
        /// <summary>
        /// Chave Primária (PK)
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Titulo da noticia
        /// </summary>
        public string Titulo { get; set; }
        /// <summary>
        /// Texto da noticia
        /// </summary>
        public string Texto { get; set; }
        /// <summary>
        /// Data de Edição da noticia
        /// </summary>
        public DateTime? DataEdicao { get; set; }
        /// <summary>
        /// Data em que a noticia foi publicada
        /// </summary>
        public DateTime DataEscrita { get; set; }
    }
}
