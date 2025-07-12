namespace NotiX.ViewModels
{
    public class NoticiaDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// Titulo da noticia
        /// </summary>
        public string Titulo { get; set; } = "";
        /// <summary>
        /// Texto da noticia
        /// </summary>
        public string Texto { get; set; }
        public DateTime DataEscrita { get; set; }
    }
}
