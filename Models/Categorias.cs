using System.ComponentModel.DataAnnotations;

namespace NotiX.Models
{
    public class Categorias
    {
        /// <summary>
        /// Classe para descrever as Categorias existentes na redação
        /// </summary>
        public Categorias()
        {
            ListaNoticias = new HashSet<Noticias>();
        }

        /// <summary>
        /// Chave Primária (PK)
        /// </summary>
        [Key]
        public int Id { get; set; }
		/// <summary>
		/// Nome da categoria
		/// </summary>
		[Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
		public string Categoria { get; set; }
        /// <summary>
        /// Lista de noticias associadas à categoria
        /// </summary>
        public ICollection<Noticias> ListaNoticias { get; set; }
    }
}

