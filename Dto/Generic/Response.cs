using System.ComponentModel.DataAnnotations;
namespace Server.Dtos
{
    public class GenericResponse
    {
        [Required]
        public int statusCode {get; set;}
        [Required]
        public string ErrorContent {get;set;}
    }
}