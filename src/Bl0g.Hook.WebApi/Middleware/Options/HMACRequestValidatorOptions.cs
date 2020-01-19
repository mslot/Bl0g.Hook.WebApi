namespace Bl0g.Hook.WebApi.Middleware.Options
{
    public class HMACRequestValidatorOptions
    {
        public string SecretKey { get; set; }
        public string SignatureHeaderName { get; set; }
    }
}
