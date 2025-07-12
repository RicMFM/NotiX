﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotiX.Models
{
    public class Noticias
    {
        /// <summary>
        /// Classe para descrever as Noticias existentes na redação
        /// </summary>
        public Noticias()
        {
            this.DataEscrita = DateTime.Now;
            ListaFotos = new HashSet<Fotos>();
        }
        /// <summary>
        /// Chave Primária (PK)
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Titulo da noticia
        /// </summary>
        [Display(Name="Título")]
        public string Titulo { get; set; }
        /// <summary>
        /// Texto da noticia
        /// </summary>
        public string Texto { get; set; }

		/// <summary>
		/// Data de Edição da noticia
		/// </summary>
		[Display(Name = "Data de Edição")]
		public DateTime? DataEdicao { get; set; }
        /// <summary>
        /// Data em que a noticia foi publicada
        /// </summary>
        [Display(Name="Data de Criação")]
        public DateTime DataEscrita { get; set; }

        /// <summary>
        /// Chave forasteira para categoria associada à noticia
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "Categoria")]
        [ForeignKey(nameof(Categoria))]
        public int CategoriaFK { get; set; }
        public Categorias Categoria { get; set; }
        /// <summary>
        /// Lista de fotos associadas a uma noticia
        /// </summary>
        public ICollection<Fotos> ListaFotos { get; set; }
    }
}
