namespace ShieldJWTLib.Abstract
{
    public abstract class ShieldJWTAbstract
    {
        protected readonly string _issuer;
        protected readonly string _key;
        public string Audience { get; set; }

        public ShieldJWTAbstract(string issuer, string key)
        {
            _issuer = "ShieldJWT";
            _key = "a890597f580476d70368bd4c40081dc1bd6f6fb76512318f0fe92929f8cb2720";
        }
    }
}
