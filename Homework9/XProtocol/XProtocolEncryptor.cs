namespace XProtocol
{
    public static class XProtocolEncryptor
    {
        // тот же ключ, что и в статье
        private static string Key { get; } = "2e985f930853919313c96d001cb5701f";

        public static byte[] Encrypt(byte[] data)
        {
            return RijndaelHandler.Encrypt(data, Key);
        }

        public static byte[] Decrypt(byte[] data)
        {
            return RijndaelHandler.Decrypt(data, Key);
        }
    }
}