using ETicaretAPI.Application.Enums;

namespace ETicaretAPI.Application.CustomAttribues
{
    public class AuthorizeDefinitionAttribute : Attribute
    {
        public string Menu { get; set; }
        public string Definition { get; set; }
        public ActionType ActionType { get; set; }
    }
}
