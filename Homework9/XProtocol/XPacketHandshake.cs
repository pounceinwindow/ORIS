using XProtocol.Serializator;

namespace XPackage;

public class XPacketHandshake
{
    [XField(1)]
    public int MagicHandshakeNumber;
}