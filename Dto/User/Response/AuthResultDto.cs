using System.Collections.Generic;

namespace Server.Dtos
{
    public class LoginResponse : GenericResponse 
    {
        public object Token { get; set; }
        public bool Success { get; set; }
    }
}