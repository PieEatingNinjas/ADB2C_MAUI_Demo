namespace MyB2CApp
{
    internal class PlatformConfig
    {
        public static PlatformConfig Instance { get; } = new PlatformConfig();

        public object ParentWindow { get; set; }

        private PlatformConfig()
        { }
    }
}
